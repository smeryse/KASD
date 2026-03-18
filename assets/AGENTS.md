# Repository Guidelines

## Project Structure & Module Organization
- `Tasks/` holds numbered assignments (e.g., `Tasks/01-VectorNorm/`) with a dedicated `.csproj` per task.
- `Labs/` contains lab exercises, each in its own folder and usually paired with a `.sln`/`.csproj`.
- `Contests/` stores contest sets (`CT1`..`CT8`) with `Tasks/`, `Samples/`, and `Explanations/` subfolders.
- `Images/`, `Theory/`, and `Attestations/` are supporting materials; treat them as read-only unless instructed.

## Build, Test, and Development Commands
- Restore dependencies for any project:
  - `dotnet restore Tasks/Tasks.sln`
- Build a specific project:
  - `dotnet build Tasks/03-SortingAlgorithms/SortingAlgorithms.csproj`
- Run a contest task (from a contest folder):
  - `dotnet run -- A sample`
  - `dotnet run -- A Samples/A.in`
- Compare output with the provided sample:
  - `dotnet run -- A sample check`

## Coding Style & Naming Conventions
- C# 12, .NET 8.0 projects; use 4-space indentation and standard C# formatting.
- Types and public members use `PascalCase`; locals/parameters use `camelCase`.
- Keep file and folder names aligned with task identifiers (e.g., `01-VectorNorm`).

## Testing Guidelines
- No centralized test framework is present.
- Validate logic using contest samples in `Contests/*/Samples` and any task-specific input/output conventions.

## Commit & Pull Request Guidelines
- Use short, imperative commit messages (e.g., "Update CT4", "Add output comparison").
- PRs should include a concise description, the affected task/lab/contest paths, and sample output notes when applicable.

## Configuration Tips
- Target .NET 8.0 SDK or later; Visual Studio Code or Visual Studio 2022 works well.
