# Algorithms and Data Structures

Учебный репозиторий на C#/.NET. Основная рабочая структура:

- `contests/` — контесты по алгоритмам (CT1–CT12)
- `tasks/` — отдельные учебные задачи (1–31)
- `labs/` — лабораторные работы (OOP + базы данных)
- `docs/` — лекции, теория, литература
- `exams/` — подготовка к экзаменам и коллоквиуму
- `meta/` — мета-информация курса
- `assets/` — изображения, конфиги, документация

## Требования

- .NET 8.0 SDK+
- C# 12

## Запуск задач

```bash
# Запуск контестной задачи
dotnet run --project contests/CT9/CT9.csproj -- A sample

# Запуск учебной задачи
dotnet run --project tasks/01-vector-norm/VectorNorm.csproj

# Проверка по эталонному выводу
dotnet run --project contests/CT9/CT9.csproj -- A sample check
```

Подробнее: [assets/README.md](assets/README.md) и [assets/AGENTS.md](assets/AGENTS.md).
