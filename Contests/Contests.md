# Contests

| # | Name | Description | Status |
| -- | -- | -- | -- |
| 1 | [CT1](CT1/CT1.md) | Contest tasks: sorting, heaps, binary search | ✅ |
| 2 | [CT2](CT2/CT2.md) | Contest tasks: stacks, queues, disjoint sets | ✅ |
| 3 | [CT3](CT3/CT3.md) | Contest tasks: dynamic programming | ✅ |
| 4 | [CT4](CT4/CT4.md) | Contest tasks: segment tree basics | 🟡 |
| 5 | [CT5](CT5/CT5.md) | Contest tasks: TBD | ⬜ |
| 6 | [CT6](CT6/CT6.md) | Contest tasks: TBD | ⬜ |
| 7 | [CT7](CT7/CT7.md) | Contest tasks: TBD | ⬜ |
| 8 | [CT8](CT8/CT8.md) | Contest tasks: TBD | ⬜ |

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
- `sample` (optional) — use input from `Samples/<TASK_ID>.in`

**Important:** The double dash `--` is required to pass arguments to the program (separates `dotnet run` arguments from program arguments).

#### Running examples

**CT1 (tasks A-N):**
```bash
cd CT1

# Run task A with standard input
dotnet run -- A

# Run task A with sample input file
dotnet run -- A sample

# Run task N
dotnet run -- N
```

**CT2 (tasks A-N):**
```bash
cd CT2

# Run task A with standard input
dotnet run -- A

# Run task A with sample input file
dotnet run -- A sample

# Run task I
dotnet run -- I
```

#### Available tasks

**CT1:**
- A-N (14 tasks): sorting, heaps, binary search

**CT2:**
- A-N (14 tasks): stacks, queues, disjoint sets

**CT3:**
- A-N (14 tasks): dynamic programming

**CT4:**
- A-N (14 tasks): segment tree basics

#### What happens when running

1. **No arguments:** program displays a hint with available tasks
2. **With task letter:** program runs the corresponding task and waits for input data from standard input
3. **With task letter and `sample`:** program uses input from `Samples/<TASK_ID>.in`

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

- [Contest Links](links.md)
