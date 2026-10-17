# Calligraphy Backend

ASP.NET Core Web API (.NET 10), organized into API, Application, Domain, and Infrastructure projects.

## Run locally

1. Install the .NET 10 SDK and SQL Server / SQL Server Express.
2. Update `Calligraphy.Api/appsettings.json` connection string for your SQL Server.
3. Configure secrets using user-secrets or environment variables; do not commit real credentials.
   - `CloudinarySettings:CloudName`, `CloudinarySettings:ApiKey`, `CloudinarySettings:ApiSecret`
   - `Razorpay:KeyId`, `Razorpay:KeySecret`
   - JWT signing key under the configuration key expected by `JwtExtension`.
4. From this directory run:

   ```powershell
   dotnet restore .\Calligraphy.Api\Calligraphy.Api.csproj
   dotnet build .\Calligraphy.Api\Calligraphy.Api.csproj
   dotnet run --project .\Calligraphy.Api\Calligraphy.Api.csproj --launch-profile https
   ```

5. Open the Swagger URL printed in the terminal. The current HTTPS profile uses `https://localhost:7284/swagger`; the HTTP profile uses `http://localhost:5210/swagger`.

## Database migration

This source adds `PaymentMethod` to the `Payments` entity and includes the `AddPaymentMethod` migration. Before using the updated model against an existing database, apply migrations from this directory:

```powershell
dotnet ef database update --project .\Calligraphy.Infrastructure\Calligraphy.Infrastructure.csproj --startup-project .\Calligraphy.Api\Calligraphy.Api.csproj
```

Review the connection string before applying migrations. Back up any database containing important data.

## Notes

- `appsettings.json` intentionally contains blank Cloudinary and Razorpay credentials. Set real values through User Secrets or environment variables.
- Do not commit API keys, passwords, or other secrets.
- Build output folders (`bin` and `obj`) are excluded from source control.
