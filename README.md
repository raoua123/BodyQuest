# 🧬 BodyQuest – Interactive Human Body Learning Adventure

**BodyQuest** is an interactive educational web application built with **Blazor and .NET 8**, designed to help children explore the human body through an engaging, game-inspired learning experience.

Children can discover different body systems, explore educational content, answer quizzes, earn stars, and track their learning progress in an interactive virtual garden.

## ✨ Features

- **Interactive learning worlds:** Explore different parts and systems of the human body through an illustrated garden map.
- **Educational content:** Discover lessons, interesting facts, explanations, and quizzes.
- **Gamification:** Earn stars, unlock worlds, complete challenges, and celebrate achievements.
- **User accounts:** Authentication and account management using ASP.NET Core Identity.
- **Multilingual interface:** Switch between English, French, and Arabic.
- **Responsive design:** A child-friendly interface with interactive visual elements and animations.
- **Progress tracking:** Keep track of learning activities and achievements.

## 🛠️ Technology Stack

| Category | Technologies |
|---|---|
| Framework | Blazor Web App (.NET 8), ASP.NET Core |
| Programming language | C# |
| Frontend | Razor Components, HTML, CSS, JavaScript |
| Database | MySQL |
| ORM | Entity Framework Core |
| Authentication | ASP.NET Core Identity |
| Interactive elements | JavaScript, SVG, CSS animations |

## 📁 Project Structure

- `Components/Pages/` – Main application pages, including the home page, learning worlds, and profile.
- `Components/Shared/` – Reusable interface components and the interactive garden scene.
- `Data/` – Database context, migrations, and initial data.
- `Models/` – Application entities and data models.
- `Services/` – Application logic, localization, quizzes, profiles, and learning progress.
- `Auth/` – Authentication-related components and functionality.
- `wwwroot/` – Static assets, stylesheets, JavaScript, and illustrations.

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- MySQL Server
- Entity Framework Core CLI

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/raoua123/BodyQuest.git
   cd BodyQuest
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Configure your MySQL connection string in `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "BodyQuestDb": "Server=localhost;Database=bodyquest_db;User=root;Password=YOUR_PASSWORD;"
     }
   }
   ```

4. Install the Entity Framework Core CLI if needed:

   ```bash
   dotnet tool install --global dotnet-ef --version 8.0.34
   ```

5. Apply the database migrations:

   ```bash
   dotnet ef database update
   ```

6. Run the application:

   ```bash
   dotnet run
   ```

Open the local URL displayed in the terminal to access BodyQuest.

> **Note:** If the database already exists, use the existing migrations rather than generating a new `InitialCreate` migration. Keep database credentials out of public repositories.

## 🌱 The Learning Garden

The home page features an interactive garden adventure map where children can explore learning worlds. The landscape is illustrated using SVG, while animations and interactive effects bring the experience to life.

Key files:

- `Components/Pages/Home.razor` – Main garden adventure map.
- `Components/Shared/GardenScene.razor` – Illustrated garden landscape.
- `Services/GardenLayout.cs` – World positions and garden layout.
- `Components/Pages/ModuleDetails.razor` – Lessons, quizzes, stars, and celebrations.
- `wwwroot/js/bodyquest-worlds.js` – Interactive effects, sounds, animations, and read-aloud functionality.

---

**BodyQuest – Explore your body, discover how it works, and learn through adventure!** 🧠💚
