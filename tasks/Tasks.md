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
cd 07-PriorityQueueSimulation && dotnet run
cd 08-MyArrayList && dotnet run
cd 09-MyArrayTagParser && dotnet run
cd 10-MyVector && dotnet run
cd 11-IPParser && dotnet run
cd 12-MyStack && dotnet run
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
