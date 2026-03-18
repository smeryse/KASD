# Лабораторна робота №2
## Дата-логічне моделювання БД

---

## 1. ЗАВДАННЯ

1.  Побудувати **інфологічну модель** предметної області (виконано в ЛР №1).
2.  Розробити **дата-логічну модель** на основі інфологічної.
3.  Визначити **типи даних** для кожного атрибута.
4.  Визначити **первинні ключі** (PK) та **зовнішні ключі** (FK).
5.  Нормалізувати відношення до **3НФ** (третьої нормальної форми).
6.  Побудувати **ER-діаграму** дата-логічного рівня.

---

## 2. ОПИС ПРЕДМЕТНОЇ ОБЛАСТІ

**Предметна область:** Система управління музичною бібліотекою DJ

**Основні сутності:**
- **Track (Трек)** — музична композиція
- **Artist (Виконавець)** — виконавець треку
- **Genre (Жанр)** — жанр музики
- **Collection (Колекція/Плейлист)** — підбірка треків
- **Event (Подія/Виступ)** — виступ DJ

---

## 3. ТАБЛИЧНЕ ОПИСАНИЕ СУТНОСТЕЙ

### 3.1. Таблиця **Artist** (Виконавці)

| Атрибут       | Тип даних    | Обмеження | Опис              |
| ------------- | ------------ | --------- | ----------------- |
| artist_id     | INT          | PK        | ID виконавця      |
| name          | VARCHAR(255) | NOT NULL  | Ім'я              |
| country       | VARCHAR(100) | NULL      | Країна            |
| style         | VARCHAR(100) | NULL      | Стиль             |
| active_years  | VARCHAR(50)  | NULL      | Роки активності   |
| bio           | TEXT         | NULL      | Біографія         |

---

### 3.2. Таблиця **Genre** (Жанри)

| Атрибут         | Тип даних    | Обмеження    | Опис                |
| --------------- | ------------ | ------------ | ------------------- |
| genre_id        | INT          | PK           | ID жанру            |
| name            | VARCHAR(100) | NOT NULL     | Назва жанру         |
| parent_genre_id | INT          | FK → Genre   |Parent-жанр (ієрархія) |
| bpm_range       | VARCHAR(50)  | NULL         | Діапазон BPM        |
| description     | TEXT         | NULL         | Опис                |

---

### 3.3. Таблиця **Track** (Треки)

| Атрибут      | Тип даних    | Обмеження         | Опис                  |
| ------------ | ------------ | ----------------- | --------------------- |
| track_id     | INT          | PK                | ID треку              |
| title        | VARCHAR(255) | NOT NULL          | Назва треку           |
| artist_id    | INT          | FK → Artist       | Виконавець            |
| genre_id     | INT          | FK → Genre        | Жанр                  |
| bpm          | DECIMAL(5,2) | NULL              | Темп                  |
| key          | VARCHAR(10)  | NULL              | Тональність           |
| duration     | INT          | NULL              | Тривалість (сек)      |
| file_format  | VARCHAR(20)  | NULL              | Формат файлу          |
| file_path    | VARCHAR(500) | NULL              | Шлях до файлу         |
| rating       | DECIMAL(3,2) | CHECK (0-5)       | Оцінка DJ             |
| play_count   | INT          | DEFAULT 0         | Кількість програвань  |
| date_added   | DATE         | DEFAULT NOW()     | Дата додавання        |
| comments     | TEXT         | NULL              | Коментарі             |

---

### 3.4. Таблиця **Collection** (Колекції/Плейлисти)

| Атрибут          | Тип даних    | Обмеження | Опис                  |
| ---------------- | ------------ | --------- | --------------------- |
| collection_id    | INT          | PK        | ID колекції           |
| name             | VARCHAR(255) | NOT NULL  | Назва                 |
| type             | VARCHAR(50)  | NULL      | Тип (сет, підбірка)   |
| description      | TEXT         | NULL      | Опис                  |
| style            | VARCHAR(100) | NULL      | Стиль                 |
| planned_duration | INT          | NULL      | Планова тривалість    |
| created_at       | DATE         | DEFAULT NOW() | Дата створення    |
| notes            | TEXT         | NULL      | Нотатки               |
| total_duration   | INT          | NULL      | Загальна тривалість   |

---

### 3.5. Таблиця **CollectionTrack** (Треки в колекції)

| Атрибут            | Тип даних | Обмеження                | Опис                |
| ------------------ | --------- | ------------------------ | ------------------- |
| collection_id      | INT       | PK, FK → Collection      | ID колекції         |
| track_id           | INT       | PK, FK → Track           | ID треку            |
| position           | INT       | NOT NULL                 | Позиція в списку    |
| transition_notes   | TEXT      | NULL                     | Нотатки переходу    |

---

### 3.6. Таблиця **Event** (Події/Виступи)

| Атрибут         | Тип даних    | Обмеження         | Опис                |
| --------------- | ------------ | ----------------- | ------------------- |
| event_id        | INT          | PK                | ID події            |
| venue           | VARCHAR(255) | NOT NULL          | Місце проведення    |
| city            | VARCHAR(100) | NULL              | Місто               |
| date            | DATE         | NOT NULL          | Дата                |
| audience_size   | INT          | NULL              | Кількість глядачів  |
| event_type      | VARCHAR(50)  | NULL              | Тип події           |
| collection_id   | INT          | FK → Collection   | Використаний сет    |
| feedback        | TEXT         | NULL              | Відгуки             |
| earnings        | DECIMAL(10,2)| NULL              | Гонорар             |

---

## 4. ER-ДІАГРАМА (Дата-логічний рівень)

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

## 5. НОРМАЛІЗАЦІЯ

### 5.1. Перша нормальна форма (1НФ)

✅ Всі атрибути атомарні
✅ Немає повторюваних груп
✅ Кожен рядок унікальний (є PK)

---

### 5.2. Друга нормальна форма (2НФ)

✅ Всі неключові атрибути повністю залежать від PK
✅ Часткові залежності відсутні

**Приклад:**
- У таблиці **Track**: `title`, `bpm`, `duration` залежать від `track_id`
- У таблиці **CollectionTrack**: `position`, `transition_notes` залежать від складного ключа `(collection_id, track_id)`

---

### 5.3. Третя нормальна форма (3НФ)

✅ Відсутні транзитивні залежності

**Приклад усунення транзитивної залежності:**

❌ **Було б (порушення 3НФ):**
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

## 6. ВИСНОВКИ

У ході виконання лабораторної роботи:

1.  ✅ Розроблено **дата-логічну модель** БД на основі інфологічної моделі з ЛР №1.
2.  ✅ Визначено **типи даних** для всіх атрибутів (INT, VARCHAR, TEXT, DATE, DECIMAL).
3.  ✅ Визначено **первинні ключі** (PK) для кожної таблиці.
4.  ✅ Визначено **зовнішні ключі** (FK) для зв'язків між таблицями.
5.  ✅ Проведено **нормалізацію до 3НФ**:
    - Усунуто повторювані дані
    - Усунуто часткові залежності
    - Усунуто транзитивні залежності
6.  ✅ Побудовано **ER-діаграму** дата-логічного рівня.

**Результат:** 6 таблиць, нормалізованих до 3НФ, готових до фізичної реалізації в СУБД PostgreSQL.

---

## ДОДАТОК А. SQL-скрипт створення таблиць

```sql
-- Таблиця Artist
CREATE TABLE Artist (
    artist_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    country VARCHAR(100),
    style VARCHAR(100),
    active_years VARCHAR(50),
    bio TEXT
);

-- Таблиця Genre
CREATE TABLE Genre (
    genre_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    parent_genre_id INT REFERENCES Genre(genre_id),
    bpm_range VARCHAR(50),
    description TEXT
);

-- Таблиця Track
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

-- Таблиця Collection
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

-- Таблиця CollectionTrack
CREATE TABLE CollectionTrack (
    collection_id INT REFERENCES Collection(collection_id),
    track_id INT REFERENCES Track(track_id),
    position INT NOT NULL,
    transition_notes TEXT,
    PRIMARY KEY (collection_id, track_id)
);

-- Таблиця Event
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
