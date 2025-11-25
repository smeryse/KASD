
# Labs
| # | Name | Description | Status | Based on |
| -- | -- | -- | -- | -- |
| 1 | Hierarchy | Class hierarchy and encapsulation in C# | ✅ |  |
| 2 | Polymorphism | Class hierarchy with interfaces and polymorphism | ✅ | 1 |
| 3 | FileDirectoryIO | File and directory operations using File, Directory, FileInfo, DirectoryInfo, and FileStream | ✅ |  |
| 4 | DelegatesAndInterfaces | Interfaces and delegates in C# | ✅ | 3 |
| 5 | DelegatesAndEvents | Event-driven programming and delegates in C# | ⏳ |  |

## Build & Run

### Build all tasks

```bash
cd /KASD/Labs
dotnet build Labs.sln
```

### Run a specific lab

```bash
# Navigate to task folder and run
cd 01-Hierarchy && dotnet run
cd 02-Polymorphism && dotnet run
cd 03-FileDirectoryIO && dotnet run
cd 04-DelegatesAndInterfaces && dotnet run
cd 05-DelegatesAndEvents && dotnet run
```

## Requirements

- .NET 8.0 SDK or higher
- C# 12
- Visual Studio Code / Visual Studio 2022

## Notes
- [GitCommitGuidelines](GitCommitGuidelines.md)