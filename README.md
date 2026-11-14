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

## Garden map (Home) and world pages

- `Components/Pages/Home.razor` - the garden adventure map. Milo walks along the path, locked worlds are hidden in fog.
- `Components/Shared/GardenScene.razor` - the drawn landscape (SVG). Positions of the worlds are in `Services/GardenLayout.cs`.
- `Components/Pages/ModuleDetails.razor` - lessons, quick check, stars, celebration.
- `wwwroot/js/bodyquest-worlds.js` - sounds (generated in the browser, no audio files), confetti, music, Milo's walk, read-aloud.
- Progress and stars are saved in the browser (localStorage). "Start over" on the map erases them.
