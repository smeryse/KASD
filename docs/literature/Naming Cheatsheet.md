
| Сущность                            | Стиль                  | Пример                          |
| ----------------------------------- | ---------------------- | ------------------------------- |
| **Класс/Структура**                 | PascalCase             | `CustomerAccount`, `Point3D`    |
| **Интерфейс**                       | PascalCase с `I`       | `ILogger`, `ISerializable`      |
| **Метод**                           | PascalCase             | `CalculateTotal()`, `GetName()` |
| **Свойство**                        | PascalCase             | `FirstName`, `IsActive`         |
| **Приватное поле**                  | _camelCase             | `_count`, `_customerName`       |
| **Константа**                       | PascalCase             | `MaxItems`, `Pi`                |
| **Локальная переменная / параметр** | camelCase              | `itemCount`, `recipientAddress` |
| **Пространство имён**               | PascalCase             | `MyCompany.Project.Module`      |
| **Событие**                         | PascalCase             | `OrderCompleted`                |
| **Делегат**                         | PascalCase с `Handler` | `OrderPlacedHandler`            |

**Правила:**

- Ясные имена, отражающие смысл.
- Избегать неочевидных сокращений.
- Методы → глаголы, свойства → существительные/прилагательные.
- Приватные поля → с `_` или camelCase.