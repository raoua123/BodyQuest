# BodyQuest – Task 1

Blazor Web App (.NET 8) + EF Core + SQL Server.

## Run

```bash
dotnet tool install --global dotnet-ef      # once
dotnet restore
dotnet ef migrations add InitialCreate
dotnet run
```

The app applies the migration and seeds the 6 modules on first start.
The default connection string uses SQL Server LocalDB (Windows). On Mac/Linux, point
`ConnectionStrings:BodyQuestDb` in `appsettings.json` to a SQL Server Docker container.
