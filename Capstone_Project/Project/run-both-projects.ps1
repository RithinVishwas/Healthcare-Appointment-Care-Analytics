# Detailed script comments explain how the project is launched. No commands were changed.
# File: run-both-projects.ps1
# Layer: Project support layer
# Purpose: This file is the helper script for running the API and MVC projects during local development/demo.
# Best-practice note: comments explain intent only; no business logic has been changed.
# Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

# Starts the API and MVC projects in two PowerShell windows.
# Update paths only if you rename the project folders.
# Starts the selected ASP.NET Core project from the command line.
# Opens a separate terminal/process so API and MVC can run together.
Start-Process powershell -ArgumentList "-NoExit", "dotnet run --project src/Healthcare.API"
# Starts the selected ASP.NET Core project from the command line.
# Opens a separate terminal/process so API and MVC can run together.
Start-Process powershell -ArgumentList "-NoExit", "dotnet run --project src/Healthcare.MVC"
