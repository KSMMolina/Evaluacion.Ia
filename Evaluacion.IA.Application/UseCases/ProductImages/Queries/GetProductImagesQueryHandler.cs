using MediatR;
using Evaluacion.IA.Application.Common;
using Evaluacion.IA.Application.DTOs;
using Evaluacion.IA.Application.Interfaces;

namespace Evaluacion.IA.Application.UseCases.ProductImages.Queries;

public sealed class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQuery, ApiResponse<List<ProductImageDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductImagesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<List<ProductImageDto>>> Handle(GetProductImagesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Validaciones de entrada
            if (request.ProductId <= 0)
            {
                return ApiResponse<List<ProductImageDto>>.Failure("El ID del producto debe ser válido");
            }

            // Verificar que el producto exista
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product is null)
            {
                return ApiResponse<List<ProductImageDto>>.Failure($"No se encontró el producto con ID {request.ProductId}");
            }

            // Obtener las imágenes del producto
            var images = await _unitOfWork.ProductImages.FindAsync(pi => pi.ProductId == request.ProductId);

            // Filtrar solo la primaria si se solicita
            if (request.OnlyPrimary)
            {
                images = images.Where(pi => pi.IsPrimary);
            }

            // Crear DTOs ordenados
            var imageDtos = images
                .OrderBy(pi => pi.Order)
                .ThenBy(pi => pi.Id)
                .Select(pi => new ProductImageDto(
                    pi.Id,
                    pi.ImageUrl.Value,
                    pi.Alt.Value,
                    pi.Order,
                    pi.IsPrimary
                ))
                .ToList();

            return ApiResponse<List<ProductImageDto>>.Success(imageDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<ProductImageDto>>.Failure($"Error al obtener las imágenes del producto: {ex.Message}");
        }
    }
}
