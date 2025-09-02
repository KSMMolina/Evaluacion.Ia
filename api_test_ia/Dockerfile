# ====== Build ======
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia sólo los .csproj para aprovechar la cache
COPY ["Api_test_ia.Domain/Api_test_ia.Domain.csproj", "Api_test_ia.Domain/"]
COPY ["Api_test_ia.Application/Api_test_ia.Application.csproj", "Api_test_ia.Application/"]
COPY ["Api_test_ia.Infrastructure/Api_test_ia.Infrastructure.csproj", "Api_test_ia.Infrastructure/"]
COPY ["Api_test_ia.Presentation/Api_test_ia.Presentation.csproj", "Api_test_ia.Presentation/"]

RUN dotnet restore "Api_test_ia.Presentation/Api_test_ia.Presentation.csproj"

# Copia el resto del código
COPY . .
WORKDIR /src/Api_test_ia.Presentation

ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api_test_ia.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ====== Runtime ======
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Puerto HTTP interno del contenedor
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# (Opcional) Define la connection string por variable de entorno
# ENV ConnectionStrings__SqlServer="Server=sql;Database=ApiPruebaIa;User Id=sa;Password=Your_p@ssw0rd!;TrustServerCertificate=True"

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api_test_ia.Presentation.dll"]# ====== Build ======
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia sólo los .csproj para aprovechar la cache
COPY ["Api_test_ia.Domain/Api_test_ia.Domain.csproj", "Api_test_ia.Domain/"]
COPY ["Api_test_ia.Application/Api_test_ia.Application.csproj", "Api_test_ia.Application/"]
COPY ["Api_test_ia.Infrastructure/Api_test_ia.Infrastructure.csproj", "Api_test_ia.Infrastructure/"]
COPY ["Api_test_ia.Presentation/Api_test_ia.Presentation.csproj", "Api_test_ia.Presentation/"]

RUN dotnet restore "Api_test_ia.Presentation/Api_test_ia.Presentation.csproj"

# Copia el resto del código
COPY . .
WORKDIR /src/Api_test_ia.Presentation

ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api_test_ia.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ====== Runtime ======
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Puerto HTTP interno del contenedor
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# (Opcional) Define la connection string por variable de entorno
# ENV ConnectionStrings__SqlServer="Server=sql;Database=ApiPruebaIa;User Id=sa;Password=Your_p@ssw0rd!;TrustServerCertificate=True"

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api_test_ia.Presentation.dll"]