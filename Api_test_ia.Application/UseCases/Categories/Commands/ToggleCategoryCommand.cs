using MediatR;

namespace Api_test_ia.Application.UseCases.Categories.Commands
{
    public sealed record ToggleCategoryCommand(int Id) : IRequest;
}
