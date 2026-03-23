# Лабораторная работа №2
## Дата-логическое моделирование БД

---

## 1. ЗАДАНИЕ

1.  Построить **инфологическую модель** предметной области (выполнено в ЛР №1).
2.  Разработать **дата-логическую модель** на основе инфологической.
3.  Определить **типы данных** для каждого атрибута.
4.  Определить **первичные ключи** (PK) и **внешние ключи** (FK).
5.  Нормализовать отношения до **3НФ** (третьей нормальной формы).
6.  Построить **ER-диаграмму** дата-логического уровня.

---

## 2. ОПИСАНИЕ ПРЕДМЕТНОЙ ОБЛАСТИ

**Предметная область:** Система управления музыкальной библиотекой DJ

**Основные сущности:**
- **Track (Трек)** — музыкальная композиция
- **Artist (Исполнитель)** — исполнитель трека
- **Genre (Жанр)** — жанр музыки
- **Collection (Коллекция/Плейлист)** — подборка треков
- **Event (Событие/Выступление)** — выступление DJ

---

## 3. ТАБЛИЧНОЕ ОПИСАНИЕ СУЩНОСТЕЙ

### 3.1. Таблица **Artist** (Исполнители)

| Атрибут       | Тип данных   | Ограничения | Описание            |
| ------------- | ------------ | ----------- | ------------------- |
| artist_id     | INT          | PK          | ID исполнителя      |
| name          | VARCHAR(255) | NOT NULL    | Имя                 |
| country       | VARCHAR(100) | NULL        | Страна              |
| style         | VARCHAR(100) | NULL        | Стиль               |
| active_years  | VARCHAR(50)  | NULL        | Годы активности     |
| bio           | TEXT         | NULL        | Биография           |

---

### 3.2. Таблица **Genre** (Жанры)

| Атрибут         | Тип данных   | Ограничения      | Описание                  |
| --------------- | ------------ | ---------------- | ------------------------- |
| genre_id        | INT          | PK               | ID жанра                  |
| name            | VARCHAR(100) | NOT NULL         | Название жанра            |
| parent_genre_id | INT          | FK → Genre       | Родительский жанр         |
| bpm_range       | VARCHAR(50)  | NULL             | Диапазон BPM              |
| description     | TEXT         | NULL             | Описание                  |

---

### 3.3. Таблица **Track** (Треки)

| Атрибут     | Тип данных   | Ограничения   | Описание                |
| ----------- | ------------ | ------------- | ----------------------- |
| track_id    | INT          | PK            | ID трека                |
| title       | VARCHAR(255) | NOT NULL      | Название трека          |
| artist_id   | INT          | FK → Artist   | Исполнитель             |
| genre_id    | INT          | FK → Genre    | Жанр                    |
| bpm         | DECIMAL(5,2) | NULL          | Темп                    |
| key         | VARCHAR(10)  | NULL          | Тональность             |
| duration    | INT          | NULL          | Длительность (сек)      |
| file_format | VARCHAR(20)  | NULL          | Формат файла            |
| file_path   | VARCHAR(500) | NULL          | Путь к файлу            |
| rating      | DECIMAL(3,2) | CHECK (0-5)   | Оценка DJ               |
| play_count  | INT          | DEFAULT 0     | Количество проигрываний |
| date_added  | DATE         | DEFAULT NOW() | Дата добавления         |
| comments    | TEXT         | NULL          | Комментарии             |

---

### 3.4. Таблица **Collection** (Коллекции/Плейлисты)

| Атрибут          | Тип данных   | Ограничения | Описание              |
| ---------------- | ------------ | ----------- | --------------------- |
| collection_id    | INT          | PK          | ID коллекции          |
| name             | VARCHAR(255) | NOT NULL    | Название              |
| type             | VARCHAR(50)  | NULL        | Тип (сет, подборка)   |
| description      | TEXT         | NULL        | Описание              |
| style            | VARCHAR(100) | NULL        | Стиль                 |
| planned_duration | INT          | NULL        | Плановая длительность |
| created_at       | DATE         | DEFAULT NOW() | Дата создания       |
| notes            | TEXT         | NULL        | Заметки               |
| total_duration   | INT          | NULL        | Общая длительность    |

---

### 3.5. Таблица **CollectionTrack** (Треки в коллекции)

| Атрибут            | Тип данных | Ограничения           | Описание             |
| ------------------ | ---------- | --------------------- | -------------------- |
| collection_id      | INT        | PK, FK → Collection   | ID коллекции         |
| track_id           | INT        | PK, FK → Track        | ID трека             |
| position           | INT        | NOT NULL              | Позиция в списке     |
| transition_notes   | TEXT       | NULL                  | Заметки перехода     |

---

### 3.6. Таблица **Event** (События/Выступления)

| Атрибут         | Тип данных   | Ограничения        | Описание             |
| --------------- | ------------ | ------------------ | -------------------- |
| event_id        | INT          | PK                 | ID события           |
| venue           | VARCHAR(255) | NOT NULL           | Место проведения     |
| city            | VARCHAR(100) | NULL               | Город                |
| date            | DATE         | NOT NULL           | Дата                 |
| audience_size   | INT          | NULL               | Количество зрителей  |
| event_type      | VARCHAR(50)  | NULL               | Тип события          |
| collection_id   | INT          | FK → Collection    | Использованный сет   |
| feedback        | TEXT         | NULL               | Отзывы               |
| earnings        | DECIMAL(10,2)| NULL               | Гонорар              |

---

## 4. ER-ДИАГРАММА (Дата-логический уровень)

```
┌─────────────┐       ┌─────────────┐
│   Artist    │ 1   N │    Track    │
├─────────────┤       ├─────────────┤
│ artist_id PK│───────│ artist_id FK│
│ name        │       │ track_id PK │
│ country     │       │ title       │
│ style       │       │ genre_id FK │
└─────────────┘       └──────┬──────┘
                             │
                       N     │     1
                    ┌────────┴────────┐
                    │                 │
              ┌─────┴─────┐     ┌─────┴─────┐
              │   Genre   │     │Collection│
              ├───────────┤     ├───────────┤
              │genre_id PK│     │collection│
              │name       │     │_id PK    │
              │parent_    │     │name      │
              │genre_id FK│     │type      │
              └───────────┘     └─────┬─────┘
                                      │
                                1     │     N
                                ┌─────┴─────┐
                                │Collection│
                                │  Track    │
                                ├───────────┤
                                │collection│
                                │_id FK    │
                                │track_id  │
                                │FK        │
                                └───────────┘

┌─────────────┐       ┌─────────────┐
│ Collection  │ 1   1 │    Event    │
├─────────────┤       ├─────────────┤
│collection_id│───────│collection_  │
└─────────────┘       │id FK        │
                      │event_id PK  │
                      │venue        │
                      │city         │
                      │date         │
                      └─────────────┘
```

---

## 5. НОРМАЛИЗАЦИЯ

### 5.1. Первая нормальная форма (1НФ)

✅ Все атрибуты атомарны
✅ Нет повторяющихся групп
✅ Каждая строка уникальна (есть PK)

---

### 5.2. Вторая нормальная форма (2НФ)

✅ Все неключевые атрибуты полностью зависят от PK
✅ Частичные зависимости отсутствуют

**Пример:**
- В таблице **Track**: `title`, `bpm`, `duration` зависят от `track_id`
- В таблице **CollectionTrack**: `position`, `transition_notes` зависят от составного ключа `(collection_id, track_id)`

---

### 5.3. Третья нормальная форма (3НФ)

✅ Отсутствуют транзитивные зависимости

**Пример устранения транзитивной зависимости:**

❌ **Было бы (нарушение 3НФ):**
```
Track(track_id, title, artist_name, artist_country, bpm, genre_name)
```

✅ **Стало (3НФ):**
```
Track(track_id, title, artist_id FK, genre_id, bpm)
Artist(artist_id, name, country)
Genre(genre_id, name)
```

---

## 6. ВЫВОДЫ

В ходе выполнения лабораторной работы:

1.  ✅ Разработана **дата-логическая модель** БД на основе инфологической модели из ЛР №1.
2.  ✅ Определены **типы данных** для всех атрибутов (INT, VARCHAR, TEXT, DATE, DECIMAL).
3.  ✅ Определены **первичные ключи** (PK) для каждой таблицы.
4.  ✅ Определены **внешние ключи** (FK) для связей между таблицами.
5.  ✅ Проведена **нормализация до 3НФ**:
    - Устранены повторяющиеся данные
    - Устранены частичные зависимости
    - Устранены транзитивные зависимости
6.  ✅ Построена **ER-диаграмма** дата-логического уровня.

**Результат:** 6 таблиц, нормализованных до 3НФ, готовых к физической реализации в СУБД PostgreSQL.

---

## ПРИЛОЖЕНИЕ А. SQL-скрипт создания таблиц

```sql
-- Таблица Artist
CREATE TABLE Artist (
    artist_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    country VARCHAR(100),
    style VARCHAR(100),
    active_years VARCHAR(50),
    bio TEXT
);

-- Таблица Genre
CREATE TABLE Genre (
    genre_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    parent_genre_id INT REFERENCES Genre(genre_id),
    bpm_range VARCHAR(50),
    description TEXT
);

-- Таблица Track
CREATE TABLE Track (
    track_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    artist_id INT REFERENCES Artist(artist_id),
    genre_id INT REFERENCES Genre(genre_id),
    bpm DECIMAL(5,2),
    key VARCHAR(10),
    duration INT,
    file_format VARCHAR(20),
    file_path VARCHAR(500),
    rating DECIMAL(3,2) CHECK (rating >= 0 AND rating <= 5),
    play_count INT DEFAULT 0,
    date_added DATE DEFAULT CURRENT_DATE,
    comments TEXT
);

-- Таблица Collection
CREATE TABLE Collection (
    collection_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    type VARCHAR(50),
    description TEXT,
    style VARCHAR(100),
    planned_duration INT,
    created_at DATE DEFAULT CURRENT_DATE,
    notes TEXT,
    total_duration INT
);

-- Таблица CollectionTrack
CREATE TABLE CollectionTrack (
    collection_id INT REFERENCES Collection(collection_id),
    track_id INT REFERENCES Track(track_id),
    position INT NOT NULL,
    transition_notes TEXT,
    PRIMARY KEY (collection_id, track_id)
);

-- Таблица Event
CREATE TABLE Event (
    event_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    venue VARCHAR(255) NOT NULL,
    city VARCHAR(100),
    date DATE NOT NULL,
    audience_size INT,
    event_type VARCHAR(50),
    collection_id INT REFERENCES Collection(collection_id),
    feedback TEXT,
    earnings DECIMAL(10,2)
);
```
