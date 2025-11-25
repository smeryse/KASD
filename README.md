Образовательный репозиторий с лабораторными работами по программированию на C#.
## Трекер задач
### Лабораторные работы
- [x] ЛР1 
- [x] ЛР2
- [x] ЛР3 (сделаю позже)
- [x] ЛР4 (сделаю позже)
- [x] ЛР5 (Рефактор, сдать)

### Вариант 4 — markdown таблица

| Task | Name                     | Status | Depends on |
| ---- | ------------------------ | ------ | ---------- |
| 1    | NDVectorNorm             | ✅      | —          |
| 2    | ComplexNumberCalculator  | ✅      | —          |
| 3    | IntSortingAlgorithms     | ⬜      | —          |
| 4    | GenericSortingAlgorithms | ⬜      | 3          |
| 5    | GenericMaxHeap           | ✅      | —          |
| 6    | HeapPriorityQueue        | ✅      | 5          |


### Задачи
[x] Task 1: NDVectorNorm
[x] Task 2: ComplexNumberCalculator
[-] Task 3: IntSortingAlgorithms
[-] Task 4: GenericSortingAlgorithms ← Task 3
[x] Task 5: GenericMaxHeap
[x] Task 6: HeapPriorityQueue ← Task 5
- [] Task 7:
- [] Task 8:
- [] Task 9:
- [] Task 10:
- [] Task 11:
- [] Task 12:
- [] Task 13:
- [] Task 14:
- [] Task 15:
- [] Task 16:
- [] Task 17:

### Контесты

## Технологический стек

- **Язык**: C# 12
- **Платформа**: .NET 8.0
- **Управление пакетами**: NuGet
- **CI/CD**: GitHub Actions

### Основные зависимости

- `Newtonsoft.Json` 13.0.4 - Сериализация JSON
- `System.Text.Json` 9.0.9 - Встроенная JSON сериализация

## Требования

- .NET 8.0 SDK или выше
- Visual Studio Code (рекомендуется) или Visual Studio 2022

## Установка и запуск

### Восстановление зависимостей

```bash
dotnet restore
```

### Сборка проекта

```bash
dotnet build
```

### Запуск конкретной лабораторной работы

```bash
cd Практики/ЛР1
dotnet run
```

### Запуск тестов

```bash
dotnet test
```

## Автоматизация

Проект использует GitHub Actions для автоматической сборки и тестирования при каждом push'е на ветку `main`. 

Workflow файл: `.github/workflows/dotnet.yml`

## История миграции

Проект был мигрирован с .NET Framework 4.8 на .NET 8.0 SDK-style format для современной разработки:

- Все файлы `App.config` и `packages.config` удалены
- Свойства проекта перенесены в SDK-стиль `.csproj`
- Автоматическое управление версиями и зависимостями

## Автор

smeryse
