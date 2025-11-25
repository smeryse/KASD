# Tasks
| # | Name | Description | Status | Based on |
|---|------|-------------|--------|-----------|
| 1 | [VectorNorm](Tasks/01-VectorNorm) | Computing norms of N-dimensional vectors | ✅ |  |
| 2 | [ComplexCalculator](Tasks/02-ComplexCalculator) | Arithmetic operations on complex numbers | ✅ |  |
| 3 | [SortingAlgorithms](Tasks/03-SortingAlgorithms) | Implementing sorting algorithms for integers | ⏳  |  |
| 4 | [GenericSorting](Tasks/04-GenericSorting) | Universal sorting with generics (Generic<T>) | ⏳ | 3 |
| 5 | [MaxHeap](Tasks/05-MaxHeap) | Generic max heap data structure | ✅ |  |
| 6 | [HeapPriorityQueue](Tasks/06-HeapPriorityQueue) | Generic priority queue using a heap | ✅ | 5 |
| 7 |  |  |  |  |
| 8 |  |  |  |  |
| 9 |  |  |  |  |
| 10 |  |  |  |  |
| 11 |  |  |  |  |
| 12 |  |  |  |  |
| 13 |  |  |  |  |
| 14 |  |  |  |  |
| 15 |  |  |  |  |
| 16 |  |  |  |  |
| 17 |  |  |  |  |

## Build & Run

### Build all tasks

```bash
cd /KASD/Tasks
dotnet build Tasks.sln
```

### Run a specific task

```bash
# Navigate to task folder and run
cd 01-VectorNorm && dotnet run
cd 02-ComplexCalculator && dotnet run
cd 03-SortingAlgorithms && dotnet run
cd 04-GenericSorting && dotnet run
cd 05-MaxHeap && dotnet run
cd 06-HeapPriorityQueue && dotnet run
```

## Requirements

- .NET 8.0 SDK or higher
- C# 12
- Visual Studio Code / Visual Studio 2022

## Notes

- The `Algorithms` folder (inside Task 3) contains shared utility algorithms
- Task 3 is **not implemented yet** - it serves as a placeholder for sorting algorithm implementations
- Task numbering corresponds to the main README.md in the repository root
- Add a missing overwrite constructors on [HeapPriorityQueue](06-HeapPriorityQueue/MyPriorityQueue.cs)
