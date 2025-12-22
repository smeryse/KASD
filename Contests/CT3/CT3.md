# Contests 3

| # | Name | Description | Status |
| -- | -- | -- | -- |
| A | [GrasshopperCollect](Tasks/A-GrasshopperCollect.cs) |  |  |
| B | [TurtleAndCoins](Tasks/B-TurtleAndCoins.cs) |  |  |
| B-G | [TurtleAndCoins (Greedy)](Tasks/B-TurtleAndCoins-Greedy.cs) |  |  |
| C | [LargestSubseq](Tasks/C-LargestSubseq.cs) |  |  |
| D | [KnightMove](Tasks/D-KnightMove.cs) |  |  |
| E | [LevenshteinDist](Tasks/E-LevenshteinDist.cs) |  |  |
| F | [Cafe](Tasks/F-Cafe.cs) |  |  |
| G | [DeleteStaples](Tasks/G-DeleteStaples.cs) |  |  |
| H | [AquariumSeller](Tasks/H-AquariumSeller.cs) |  |  |
| I | [ReplaceDomino](Tasks/I-ReplaceDomino.cs) |  |  |
| J | [CutePatterns](Tasks/J-CutePatterns.cs) |  |  |
| K | [Multybacpack](Tasks/K-Multybacpack.cs) |  |  |

## Запуск с тестами

Тесты размещай в `Contests/CT3/Tests`:
- `A.txt` — основной тест для задачи A
- `A-custom.txt` — дополнительный тест для задачи A

Примеры:

```bash
cd Contests/CT3

# Стандартный ввод
dotnet run -- A

# Автотест из файла Contests/CT3/Tests/A.txt
dotnet run -- A sample

# Файл по имени Contests/CT3/Tests/A-custom.txt
dotnet run -- A custom

# Явный путь к файлу
dotnet run -- A ./Tests/A-custom.txt
```
