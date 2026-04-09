# Algorithms and Data Structures — Project Context

**Course:** Algorithms and Data Structures (КАСД)  
**Semester:** 3rd semester, 2nd year  
**Language:** C# 12 / .NET 8.0  
**Author:** Smeryse

---

## Project Overview

This is an educational repository for a university course on Algorithms and Data Structures. The project contains:

- **Contests (CT1–CT9):** Competitive programming tasks with solutions, sample I/O, and detailed explanations
- **Tasks (01–18+):** Individual programming assignments implementing data structures and algorithms
- **Labs:** Database labs (db/) and OOP labs (oop/)
- **Exams:** Exam preparation materials, theory answers, and task solutions
- **Docs:** Lecture PDFs, theory notes, cheatsheets, and literature

The codebase focuses on C# implementations of classic algorithms and data structures, with emphasis on:
- Sorting algorithms (quick sort, count sort, merge sort)
- Data structures (heaps, stacks, queues, segment trees, treaps, AVL trees)
- String algorithms (hashing, Z-function, prefix function, Aho-Corasick)
- Dynamic programming
- Graph algorithms (disjoint sets, shortest paths)

---

## Repository Structure

```
.
├── contests/              # Competitive programming contests
│   ├── CT1/               # Contest 1: Sorting, heaps, binary search
│   ├── CT2/               # Contest 2: Stacks, queues, disjoint sets
│   ├── CT3/               # Contest 3: Dynamic programming
│   ├── CT4/               # Contest 4: Segment trees
│   ├── CT5/               # Contest 5: Trees (AVL, Treap)
│   ├── CT6/               # Contest 6: TBD
│   ├── CT7/               # Contest 7: TBD
│   ├── CT8/               # Contest 8: TBD
│   └── CT9/               # Contest 9: String algorithms
│
├── tasks/                 # Individual programming tasks
│   ├── 01-vector-norm/
│   ├── 02-complex-calculator/
│   ├── 03-sorting-algorithms/
│   ├── 05-max-heap/
│   ├── 06-heap-priority-queue/
│   ├── 08-my-array-list/
│   ├── 10-my-vector/
│   ├── 12-my-stack/
│   ├── 18-my-tree-map/
│   └── Tasks.md           # Task tracker
│
├── labs/                  # Laboratory works
│   ├── db/                # Database labs (12 labs)
│   └── oop/               # OOP labs (5 labs)
│       ├── 01-Hierarchy/
│       ├── 02-Polymorphism/
│       ├── 03-04-FileDirectoryIO-DelegatesAndInterfaces/
│       └── Labs.md
│
├── exams/                 # Exam preparation
│   └── Attestations/
│       ├── TaskAnswers/   # T01.md – T40.md (task solutions)
│       ├── TheoryAnswers/ # Q01.md – Q51.md (theory answers)
│       └── ExamPrep.md
│
├── docs/                  # Course materials
│   ├── 01-lectures/       # Lecture PDFs (ЛК1–ЛК21)
│   ├── 02-lectures/
│   ├── cheatsheets/
│   └── literature/
│
├── assets/                # Images and resources
├── meta/                  # Course meta-information
├── .venv/                 # Python virtual environment
└── .vscode/               # VS Code settings
```

---

## Contest Structure

Each contest folder (CT1–CT9) follows a standard structure:

```
CTn/
├── Tasks/           # Task solutions (A-TaskName.cs, B-TaskName.cs, ...)
├── Samples/         # Test cases (A.in, A.out, B.in, B.out, ...)
├── Explanations/    # Detailed solution explanations (A.md, B.md, ...)
├── Program.cs       # Main entry point with task routing
├── CTn.csproj       # Project file
├── CTn.sln          # Solution file (optional)
└── CTn.md           # Contest index with task list
```

### Running a Contest Task

From within a contest folder:

```bash
# Run task A with sample input
dotnet run -- A sample

# Run task A and compare with expected output
dotnet run -- A sample check

# Run task A with custom input file
dotnet run -- A path/to/input.txt

# Run task A and compare with custom output
dotnet run -- A input.txt output.txt
```

### Available Contests

| Contest | Topic | Tasks | Status |
|---------|-------|-------|--------|
| CT1 | Sorting, heaps, binary search | A–N (14) | ✅ |
| CT2 | Stacks, queues, disjoint sets | A–N (14) | ✅ |
| CT3 | Dynamic programming | A–N (14) | ✅ |
| CT4 | Segment trees | A–N (14) | 🟡 |
| CT5 | Trees (AVL, Treap) | A–N (14) | ⬜ |
| CT6 | TBD | A–N (14) | ⬜ |
| CT7 | TBD | A–N (14) | ⬜ |
| CT8 | TBD | A–N (14) | ⬜ |
| CT9 | String algorithms | A–G (7) | ✅ |

---

## Tech Stack

### Core Technologies

| Component | Technology |
|-----------|------------|
| Language | C# 12 |
| Framework | .NET 8.0 |
| Package Manager | NuGet |
| IDE | Visual Studio Code / VS 2022 |

### Project Configuration

Standard `.csproj` format for tasks:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <RootNamespace>CT9</RootNamespace>
    <AssemblyName>CT9</AssemblyName>
    <ImplicitUsings>disable</ImplicitUsings>
    <Nullable>disable</Nullable>
  </PropertyGroup>
</Project>
```

### Common Patterns

**FastScanner:** Custom buffered input reader for competitive programming:

```csharp
internal sealed class FastScanner
{
    private readonly Stream stream;
    private readonly byte[] buffer;
    
    public int NextInt() { /* ... */ }
    public string NextString() { /* ... */ }
    public long NextLong() { /* ... */ }
}
```

**Task Structure:** Each task is a static class with a `Solve()` method:

```csharp
internal static class TaskName
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        // Read input, solve, write output
    }
}
```

---

## Building and Running

### Prerequisites

- .NET 8.0 SDK or higher
- Git (for version control)

### Build Commands

```bash
# Restore dependencies (if needed)
dotnet restore

# Build a specific contest
cd contests/CT9 && dotnet build

# Build all tasks
cd tasks && dotnet build Tasks.sln

# Run tests (if available)
dotnet test
```

### Running Tasks

```bash
# Individual task
cd tasks/01-vector-norm && dotnet run

# Contest task with sample
cd contests/CT9 && dotnet run -- A sample

# Contest task with verification
cd contests/CT9 && dotnet run -- A sample check
```

---

## Development Conventions

### Naming Conventions

- **Directories:** kebab-case (`01-vector-norm`, `string-algorithms`)
- **Classes:** PascalCase (`SubstringCompare`, `FastScanner`)
- **Task files:** `{Letter}-{TaskName}.cs` (`A-SubstringCompare.cs`)
- **Contest folders:** `CTn` format (`CT1`, `CT9`)

### Code Style

- **Namespace:** Match project name (`CT9`, `CT9.Tasks`)
- **Access modifiers:** Explicit (`internal`, `private`, `public`)
- **Documentation:** XML comments for public APIs
- **Comments:** Russian language for explanations

### File Organization

Each contest task file should contain:
1. XML documentation header with task description
2. Static class with `Solve()` method
3. Helper classes (algorithms, data structures)
4. `FastScanner` class for input (or use shared infrastructure)

### Git Practices

- **Commit messages:** Descriptive, Russian or English
- **Branch naming:** `feature/task-name`, `fix/issue-description`
- **Ignore:** `bin/`, `obj/`, `.vscode/`, `.obsidian/`, `*.user`

---

## Key Algorithms Implemented

### Sorting
- Simple Sort (bubble/selection)
- Count Sort
- Quick Sort
- Generic sorting with `IComparable<T>`

### Data Structures
- **Heap:** Max heap, priority queue
- **Stack:** `MyStack<T>`, stack with min tracking
- **Queue:** Array-based deque
- **List:** `MyArrayList<T>`, `MyVector<T>`
- **Tree:** `MyTreeMap<K,V>`, AVL tree, Treap
- **Segment Tree:** Sum, min, max, k-th one, lazy propagation

### String Algorithms
- **Hashing:** Polynomial rolling hash, double hashing
- **Prefix Function:** KMP algorithm
- **Z-Function:** Z-algorithm
- **Aho-Corasick:** Multiple pattern matching

### Dynamic Programming
- Knapsack problem
- Longest increasing subsequence
- Edit distance (Levenshtein)
- Path counting (turtle, grasshopper)

---

## Testing Practices

### Sample Testing

Each contest task has sample I/O files:
- `Samples/A.in` — input data
- `Samples/A.out` — expected output

Run verification:
```bash
dotnet run -- A sample check
```

### Manual Testing

```csharp
// In Program.cs or task file
static void Main()
{
    // Test with hardcoded data
    Test();
    
    // Or run from stdin
    Solve();
}
```

---

## Known Issues & Technical Debt

### Current Issues

1. **Namespace mismatch in CT9:** `CT9.csproj` has `RootNamespace>CT1</RootNamespace>`
2. **Code duplication:** `FastScanner` is duplicated in every task file
3. **Missing tests:** No unit tests for algorithms
4. **Incomplete contests:** CT5–CT8 are placeholders
5. **Assets cleanup:** `assets/images/lectures/` contains many `Pasted image *.png` files

### TODO Items

From `TODO.md`:
- [ ] Add input/output files for CT2, CT3
- [ ] Solve half of CT3 + I/O files
- [ ] Refactor Tasks folders in CT1, CT2
- [ ] Clean up Attestations folder

---

## Useful Commands

```bash
# Find all .cs files in contests
find contests -name "*.cs" -type f

# Count lines of code
find contests tasks -name "*.cs" | xargs wc -l

# Build all contests
for d in contests/CT*/; do (cd "$d" && dotnet build); done

# Run all task tests in CT9
for task in A B C D E F G; do
    echo "=== Task $task ==="
    dotnet run -- $task sample check
done
```

---

## Resources

- [Contests Index](contests/Contests.md)
- [Tasks Index](Учеба%20ВУЗ/kasd/tasks/Tasks.md)
- [Labs Index](labs/oop/Labs.md)
- [Repo Rules](REPO_RULES.md)
- [Tree Structure](tree.txt)

---

## Author Notes

This repository was migrated from .NET Framework 4.8 to .NET 8.0 SDK-style format. All `App.config` and `packages.config` files were removed in favor of modern `.csproj` configuration.

For questions or contributions, refer to `REPO_RULES.md` for structure and naming conventions.
