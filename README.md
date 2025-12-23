# Educational C# Labs Repository

Repository for educational lab | tasks | contests exercises in C# programming.

## Task Tracker

* [Tasks](Tasks/Tasks.md)
* [Labs](Labs/Labs.md)
* [Contests](Contests/Contests.md)

## Contests Structure

Each contest folder `CT1`..`CT8` contains:

* `CTn.md` — contest index
* `Tasks/` — solutions per task
* `Samples/` — `A.in` / `A.out` pairs
* `Explanations/` — `A.md`..`N.md` notes

Run a task from its contest folder:

```bash
dotnet run -- A sample
dotnet run -- A Samples/A.in
```

Compare output with `Samples/A.out`:

```bash
dotnet run -- A sample check
```

## Tech Stack

* **Language**: C# 12
* **Platform**: .NET 8.0
* **Package Management**: NuGet
* **CI/CD**: GitHub Actions

### Key Dependencies

* `Newtonsoft.Json` 13.0.4 – JSON serialization
* `System.Text.Json` 9.0.9 – Built-in JSON serialization

## Requirements

* .NET 8.0 SDK or higher
* Visual Studio Code (recommended) or Visual Studio 2022

## Installation & Running

### Restore Dependencies

```bash
dotnet restore
```

## Migration History

This project was migrated from .NET Framework 4.8 to the modern .NET 8.0 SDK-style format:

* All `App.config` and `packages.config` files were removed
* Project properties converted to SDK-style `.csproj` format
* Automatic versioning and dependency management enabled

## Author

Smeryse
