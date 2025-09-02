# API Prueba IA (.NET 8 · Arquitectura Hexagonal)

API REST para gestión de **Productos, Categorías y Usuarios** con **autenticación JWT** (access + refresh token).  
Stack: **.NET 8**, **ASP.NET Core**, **EF Core (SQL Server)**, **MediatR**, **FluentValidation**, **Swagger**.

---

## ✨ Características
- Arquitectura **Hexagonal** (Ports & Adapters): `Domain`, `Application`, `Infrastructure`, `Presentation`.
- **JWT** de acceso (15 min) + **Refresh token** (7 días, cookie httpOnly) y **roles**: `Admin`, `Editor`.
- **ProblemDetails** para errores, **CORS**, **Swagger** con esquema **Bearer**.
- CRUD de **Productos** (con imágenes), **Categorías** (árbol/plano) y **Usuarios** (solo Admin).
- Catálogo público (solo activos) con **paginación, filtro y orden**.

---

## 🧭 Estructura de carpetas

```
/Api_test_ia.Domain
  └─ Entities/ (User, Category, Product, ProductImage, UserRefreshToken, ...)

/Api_test_ia.Application
  ├─ Abstractions/        (Ports: repos, seguridad, UoW)
  ├─ Dtos/                (contratos internos)
  ├─ UseCases/            (Commands/Queries + Handlers con MediatR)
  └─ DependencyInjection.cs  ← registro de Application

/Api_test_ia.Infrastructure
  ├─ Persistence/Context  (AppDbContext)
  ├─ Persistence/Uow      (EfUnitOfWork)
  ├─ Repositories/        (adaptadores EF a los Ports)
  ├─ Security/            (BcryptPasswordHasher, JwtProvider)
  ├─ Auth/                (EfUserRepository, EfRefreshTokenStore, JwtOptions)
  └─ DependencyInjection.cs ← registro de Infrastructure

/Api_test_ia.Presentation
  ├─ Controllers/         (controladores delgados → MediatR)
  ├─ Contracts/           (requests/responses HTTP)
  ├─ Mappings/            (Request → Command/Query)
  ├─ Program.cs           (mínimo)
  └─ DependencyInjection.cs ← Swagger, CORS, ProblemDetails, pipeline
```

> **Referencias entre proyectos**
>
> - `Presentation` → referencia a `Application` y `Infrastructure`  
> - `Infrastructure` → referencia a `Application` y `Domain`  
> - `Application` → referencia a `Domain`  
> - `Domain` → **no** referencia a nadie

---

## ✅ Requisitos
- .NET 8 SDK
- SQL Server (local o en contenedor)
- (Opcional) Docker Desktop para usar `docker compose`
- (Opcional) VS/VS Code para ejecutar `.http`

---

## ⚙️ Configuración

### `appsettings.json` (en **Api_test_ia.Presentation**)
```json
{
  "ConnectionStrings": {
    "SqlServer": "Server=localhost;Database=ApiPruebaIa;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Issuer": "ApiPruebaIa",
    "Audience": "ApiPruebaIaClient",
    "Key": "REEMPLAZA-ESTA-CLAVE-LARGA-32+CARACTERES",
    "AccessTokenMinutes": 15,
    "RefreshTokenDays": 7
  },
  "Cors": { "Origins": [ "http://localhost:4200", "https://localhost:4200" ] },
  "Logging": { "LogLevel": { "Default": "Information" } }
}
```
> ⚠️ Cambia **`Jwt:Key`** por una clave larga y secreta.

---

## 🗄️ Base de datos

Puedes usar **el script** SQL incluido en el reto (crea `User`, `Category`, `Product`, `ProductImage` y siembra el admin), y **añadir** la tabla de refresh tokens si no la tienes aún:

```sql
CREATE TABLE [dbo].[UserRefreshToken](
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [UserId] INT NOT NULL,
  [TokenHash] NVARCHAR(256) NOT NULL,
  [ExpiresAt] DATETIME2 NOT NULL,
  [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_UserRefreshToken_CreatedAt DEFAULT SYSUTCDATETIME(),
  [RevokedAt] DATETIME2 NULL,
  CONSTRAINT FK_UserRefreshToken_User FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id]) ON DELETE CASCADE
);
CREATE INDEX IX_UserRefreshToken_User_Valid ON [dbo].[UserRefreshToken] ([UserId],[ExpiresAt],[RevokedAt]);
```

> Alternativa: crear esta tabla mediante **EF Core Migrations** en el proyecto `Infrastructure` y ejecutar `dotnet ef database update` con **startup** `Presentation`.

---

## ▶️ Ejecutar en local

```bash
dotnet restore
dotnet build
dotnet run --project Api_test_ia.Presentation
```

- Swagger: `https://localhost:7133/swagger`
- Health (si lo habilitaste): `GET /health`

---

## 🔐 Login y autorización

1) `POST /api/v1/auth/login` con:
```json
{"email":"admin@demo.com","password":"admin123"}
```
Obtendrás `accessToken` (JWT). También se emite un **refresh token** y se guarda hasheado en la BD.

2) En Swagger, click **Authorize** → pega **solo** el `accessToken` (sin “Bearer ”).

3) Policies disponibles:
- `AdminOnly` → requiere `role = Admin`
- `AdminOrEditor` → requiere `role = Admin` o `Editor`

4) Renovación y cierre de sesión:
- `POST /api/v1/auth/refresh` (usa la cookie httpOnly con el refresh)
- `POST /api/v1/auth/logout` (revoca el refresh actual)

---

## 📚 Endpoints principales

### Auth
- `POST /api/v1/auth/login` → devuelve `AuthTokens` (access token por 15 min).
- `POST /api/v1/auth/refresh` → rota refresh y entrega nuevo access token.
- `POST /api/v1/auth/logout` → revoca el refresh vigente.

### Admin · Productos (Admin/Editor)
- `GET /api/v1/admin/products` (paginado)
- `GET /api/v1/admin/products/{id}`
- `POST /api/v1/admin/products`
- `PUT /api/v1/admin/products/{id}`
- `PATCH /api/v1/admin/products/{id}/toggle`
- `POST /api/v1/admin/products/{id}/images`
- `PUT /api/v1/admin/products/{id}/images/{imgId}`
- `DELETE /api/v1/admin/products/{id}/images/{imgId}`

### Admin · Categorías (Admin/Editor)
- `GET /api/v1/admin/categories?flat=false`
- `POST /api/v1/admin/categories`
- `PUT /api/v1/admin/categories/{id}`
- `PATCH /api/v1/admin/categories/{id}/toggle`

### Admin · Usuarios (Admin)
- `GET /api/v1/admin/users` (paginado, filtros `search`, `role`, `sort`, `dir`)
- `GET /api/v1/admin/users/{id}`
- `POST /api/v1/admin/users`
- `PUT /api/v1/admin/users/{id}`
- `DELETE /api/v1/admin/users/{id}`

### Catálogo público
- `GET /api/v1/catalog/products?search=&categoryId=&sort=price|createdAt&dir=asc|desc&page=1&pageSize=12`
- `GET /api/v1/catalog/products/{id}`
- `GET /api/v1/catalog/categories?flat=false`

> Las listas devuelven `{ items, page, pageSize, total }` y header `X-Total-Count`.

---

## 🧪 Pruebas rápidas

Incluye `Api_test_ia.Presentation/api_test_ia.http` con requests listos para Visual Studio/VS Code.  
Pasos en Swagger:
1. Login (endpoint de `auth/login`).
2. Click **Authorize** y pega el token.
3. Ejecuta endpoints de Admin/Catálogo.

---

## 🐳 Docker

### Dockerfile
Ubicado en `Api_test_ia.Presentation/Dockerfile`. Expone puerto **8080** dentro del contenedor.

### docker-compose (API + SQL)
Coloca este archivo en la **raíz** (junto a la `.sln`).

```yaml
version: "3.9"

services:
  sql:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: api-sql
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Your_p@ssw0rd!
    ports:
      - "1433:1433"

  api:
    build:
      context: .
      dockerfile: Api_test_ia.Presentation/Dockerfile
    container_name: api-net8
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__SqlServer: "Server=sql;Database=ApiPruebaIa;User Id=sa;Password=Your_p@ssw0rd!;TrustServerCertificate=True"
    ports:
      - "7133:8080"
    depends_on:
      - sql
```

**Comandos**:
```bash
# API + SQL
docker compose up --build

# Solo API (usando SQL del host)
docker build -f Api_test_ia.Presentation/Dockerfile -t api-test-ia .
docker run --rm -p 7133:8080 \
  -e ConnectionStrings__SqlServer="Server=host.docker.internal,1433;Database=ApiPruebaIa;User Id=sa;Password=Your_p@ssw0rd!;TrustServerCertificate=True" \
  api-test-ia
```

**.dockerignore** (en la **raíz**):
```
**/bin/
**/obj/
**/.vs/
**/.vscode/
**/node_modules/
**/*.user
**/*.suo
**/*.swp
**/logs/
.git
.gitignore
Dockerfile
docker-compose*.yml
```

---

## 🧩 Decisiones de diseño (Hexagonal)

- **Domain**: Entidades y reglas puras; sin dependencias.
- **Application**: **Ports** (interfaces), **DTOs** y **UseCases** (MediatR + FluentValidation).
- **Infrastructure**: Adaptadores a los ports (EF Core, JWT, BCrypt), `AppDbContext`, UoW.
- **Presentation**: Controllers delgados; **Contracts** HTTP y **Mappings** → Commands/Queries.
- `Program.cs` minimalista:
  ```csharp
  builder.Services.AddApplication()
                  .AddInfrastructure(builder.Configuration)
                  .AddPresentation(builder.Configuration);
  app.UsePresentation();
  ```

---

## 🧰 Troubleshooting

- **403 en Admin** → el token no tiene `role=Admin`. Loguéate con `admin@demo.com`.
- **409/400 al crear** → SKU/Email duplicado (validación de negocio).
- **`Invalid object name 'dbo.UserRefreshToken'`** → crea la tabla anterior o aplica migración.
- **`AddValidatorsFromAssembly` no existe** → instala `FluentValidation.DependencyInjectionExtensions` en **Application**.
- **CORS** desde otro puerto → agrega el origen a `Cors:Origins` en `appsettings.json`.

---

## 📄 Licencia
Uso interno para evaluación técnica.
