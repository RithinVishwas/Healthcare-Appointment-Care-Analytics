# Detailed script comments explain how the project is launched. No commands were changed.
#!/usr/bin/env bash
# File: run-both-projects.sh
# Layer: Project support layer
# Purpose: This file is the helper script for running the API and MVC projects during local development/demo.
# Best-practice note: comments explain intent only; no business logic has been changed.
# Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

set -e
# Starts the selected ASP.NET Core project from the command line.
(dotnet run --project src/Healthcare.API) &
# Starts the selected ASP.NET Core project from the command line.
(dotnet run --project src/Healthcare.MVC) &
wait
