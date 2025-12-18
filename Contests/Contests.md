# Contests

| # | Name | Description | Status | Based on |
| -- | -- | -- | -- | -- |
| 1 | [CT1](Contests/CT1/CT1.md) | Contest tasks: sorting, heaps, binary search | ✅ |
| 2 | [CT2](Contests/CT2)/CT2.md | Contest tasks: stacks, queues, disjoint sets | ✅ |

## Requirements

- .NET 8.0 SDK or higher
- C# 12
- Visual Studio Code / Visual Studio 2022

## Build & Run

### Build a specific contest

```bash
# Navigate to contest folder and build
cd CT1 && dotnet build
# or
cd CT2 && dotnet build
```

### Run a specific task

Each contest supports running tasks via command line. Command format:

```bash
dotnet run -- <TASK_ID> [sample]
```

**Parameters:**
- `<TASK_ID>` — task letter (A, B, C, ...)
- `sample` (optional) — use built-in sample input data instead of standard input

**Important:** The double dash `--` is required to pass arguments to the program (separates `dotnet run` arguments from program arguments).

#### Running examples

**CT1 (tasks A-M):**
```bash
cd CT1

# Run task A with standard input
dotnet run -- A

# Run task A with built-in sample
dotnet run -- A sample

# Run task M
dotnet run -- M

# Run task M with sample
dotnet run -- M sample
```

**CT2 (tasks A-I):**
```bash
cd CT2

# Run task A with standard input
dotnet run -- A

# Run task A with built-in sample
dotnet run -- A sample

# Run task I
dotnet run -- I
```

#### Available tasks

**CT1:**
- A-M (13 tasks): sorting, heaps, binary search

**CT2:**
- A-I (9 tasks): stacks, queues, disjoint sets

#### What happens when running

1. **No arguments:** program displays a hint with available tasks
2. **With task letter:** program runs the corresponding task and waits for input data from standard input
3. **With task letter and `sample`:** program uses built-in sample input data (convenient for quick testing)

#### Complete workflow example

```bash
# 1. Navigate to contest folder
cd CT1

# 2. Build project (optional, dotnet run will build automatically)
dotnet build

# 3. Run task A with sample
dotnet run -- A sample

# 4. Or run task A and enter data manually
dotnet run -- A
# (now you can enter data)
```

## Notes

- [Contest Links](Contests/Ссылки.md)
