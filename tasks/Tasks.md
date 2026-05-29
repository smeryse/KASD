# Tasks
| # | Name | Description | Status | Based on |
| -- | -- | -- | -- | -- |
| 1 | [VectorNorm](Tasks/01-VectorNorm) | Computing norms of N-dimensional vectors | ✅ |  |
| 2 | [ComplexCalculator](Tasks/02-ComplexCalculator) | Arithmetic operations on complex numbers | ✅ |  |
| 3 | [SortingAlgorithms](Tasks/03-SortingAlgorithms) | Implementing sorting algorithms for integers | ⏳  |  |
| 4 | [GenericSorting](Tasks/04-GenericSorting) | Universal sorting with generics (Generic<T>) | ⏳ | 3 |
| 5 | [MaxHeap](Tasks/05-MaxHeap) | Generic max heap data structure | ✅ |  |
| 6 | [HeapPriorityQueue](Tasks/06-HeapPriorityQueue) | Generic priority queue using a heap | ✅ | 5 |
| 7 | [PriorityQueueSimulation](Tasks/07-PriorityQueueSimulation) | Simulating step by step | ✅ | 6 |
| 8 | [MyArrayList](Tasks/08-MyArrayList) | Generic array list data structure | ✅ |  |
| 9 | [MyArrayTagParser](Tasks/09-MyArrayTagParser) | Parsing htmp tags from input.txt (html) | ✅ | 8 |
| 10 | [MyVector](Tasks/10-MyVector) | Generic vector data structure | ✅ |  |
| 11 | [IPParser](Tasks/11-IPParser) | Parsing IP program by MyVector class | ✅ | 10 |
| 12 | [MyStack](Tasks/12-MyStack) | Generic stack data structure | ✅ | 10 |
| 13 | [ReversePolishEntry](Tasks/13-ReversePolishEntry) | Evaluating expressions in reverse Polish notation | ✅ | 12 |
| 14 | [MyArrayDeque](14-my-array-deque) | Generic array deque (broken/stub) | ❌ |  |
| 15 | (empty) | Not implemented | ❌ |  |
| 16 | (empty) | Not implemented | ❌ |  |
| 17 | (empty) | Not implemented | ❌ |  |
| 18 | [MyTreeMap](18-my-tree-map) | Generic tree map (BST with comparator) | ✅ |  |
| 19 | [MyTreeSet](19-my-tree-set) | Red-black tree set | ✅ | 18 |
| 20 | [GraphAlgorithms](20-graph-algorithms) | Graph algorithms (SCC, max flow, max clique) | ✅ |  |
| 21 | [MyHashMap](21-my-hash-map) | Hash map with separate chaining | ✅ |  |
| 22 | [MyHtmlTagParser](22-my-html-tag-parser) | HTML tag counter via MyHashMap | ✅ | 21 |
| 23 | [VariableFileParser](23-variable-file-parser) | Variable definition parser with MyHashMap | ✅ | 21 |
| 24 | [MapComparison](24-map-comparison) | Performance comparison: HashMap vs TreeMap | ✅ | 18, 21 |
| 25 | [MyHashSet](25-my-tree-set) | Set backed by MyTreeMap | ✅ | 18 |
| 26 | [StringSetComparison](26-string-set-comparison) | String set with word-length comparer | ✅ | 25 |
| 27 | [UniqueWords](27-unique-words) | Unique words via MyHashSet | ✅ | 25 |
| 28 | [ExpandMyHashSet](28-expand-my-hash-set) | Iterators: IMyIterator, IMyListIterator + error hierarchy | ✅ | 25, 19, 6, 8, 10, 29 |
| 29 | [InterfaceHierarchy](29-word-frequency-counter) | Interface hierarchy (IMyCollection, IMyList, etc.) + MyLinkedList | ✅ |  |
| 30 | [MyString](30-text-analyzer) | Custom string class on char[] | ✅ |  |
| 31 | [CrissCross](31-criss-cross) | Criss-cross puzzle generator (backtracking) | ✅ |  |

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
cd 07-PriorityQueueSimulation && dotnet run
cd 08-MyArrayList && dotnet run
cd 09-MyArrayTagParser && dotnet run
cd 10-MyVector && dotnet run
cd 11-IPParser && dotnet run
cd 12-MyStack && dotnet run
cd 13-ReversePolishEntry && dotnet run
cd 14-MyArrayDeque && dotnet run
cd 18-MyTreeMap && dotnet run
cd 19-MyTreeSet && dotnet run
cd 21-MyHashMap && dotnet run
cd 22-MyHtmlTagParser && dotnet run
cd 23-VariableFileParser && dotnet run
cd 24-MapComparison && dotnet run
cd 25-MyHashSet && dotnet run
cd 26-StringSetComparison && dotnet run
cd 27-UniqueWords && dotnet run
cd 28-expand-my-hash-set && dotnet run
cd 29-word-frequency-counter && dotnet run
cd 30-text-analyzer && dotnet run
cd 31-criss-cross && dotnet run
```

## Requirements

- .NET 8.0 SDK or higher
- C# 12
- Visual Studio Code / Visual Studio 2022

## csproj template

```cs
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <RootNamespace>Task13</RootNamespace>
    <AssemblyName>ReversePolishEntry</AssemblyName>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\12-MyStack\MyStack.csproj" />
  </ItemGroup>

</Project>
```

## add to project

➜  05_tasks git:(main) ✗ dotnet sln Tasks.sln add 14-MyArrayDeque/MyArrayDeque.csproj
Проект "14-MyArrayDeque/MyArrayDeque.csproj" добавлен в решение.


Для кажддой задачи должен быть  рпавильно указао csproj
А также минимум два cs файла

program.cs - здесь тестовые данные и запуск MyArrayList.cs для демо версии

```cs
namespace Task13
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
```

и файл, в котором хранится реализация задачи MyArrayList.cs

```cs
namespace Task13
{
    public class MyStack<T> : MyVector<T>
        // Релизация
}
```

csproj должен иметь неймспейт текущей задачи наприме Task13
