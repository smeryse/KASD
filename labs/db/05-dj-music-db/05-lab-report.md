## Тема: Разработка БД для заданной предметной области

---
## Цель работы
Закрепление способов применения DDL-команд SQL и средств СУБД PostgreSQL для создания и структурной модификации БД.

---

## 1. Листинги DDL-команд

### 1.1. Команда создания базы данных

```sql
CREATE DATABASE dj_db
    WITH ENCODING = 'UTF8'
    LC_COLLATE = 'ru_RU.UTF-8'
    LC_CTYPE = 'ru_RU.UTF-8'
    TEMPLATE = template0;
```
![[Pasted image 20260318161918.png]]
### 1.2. Команды создания таблиц (CREATE TABLE)

```sql
CREATE TABLE Artist (
    artist_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    country VARCHAR(100),
    style VARCHAR(100),
    active_years VARCHAR(50),
    bio TEXT
);

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

CREATE TABLE CollectionTrack (
    collection_id INT REFERENCES Collection(collection_id),
    track_id INT REFERENCES Track(track_id),
    position INT NOT NULL,
    transition_notes TEXT,
    PRIMARY KEY (collection_id, track_id)
);

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
![[Pasted image 20260318162147.png]]
![[Pasted image 20260318162213.png]]
### 1.3. Команды создания ограничений целостности (ALTER TABLE)
#### 1. Artist
```sql
ALTER TABLE Artist
ALTER COLUMN name SET NOT NULL;
```
![[Pasted image 20260318163732.png]]
#### 2. Genre
```sql
ALTER TABLE Genre
ADD CONSTRAINT fk_genre_parent
FOREIGN KEY (parent_genre_id) REFERENCES Genre(genre_id);

ALTER TABLE Genre
ALTER COLUMN name SET NOT NULL;

ALTER TABLE Genre
ADD CONSTRAINT uq_genre_name UNIQUE (name);
```
![[Pasted image 20260318163910.png]]
#### 3. Track
```sql
ALTER TABLE Track
ADD CONSTRAINT fk_track_artist
FOREIGN KEY (artist_id) REFERENCES Artist(artist_id);

ALTER TABLE Track
ADD CONSTRAINT fk_track_genre
FOREIGN KEY (genre_id) REFERENCES Genre(genre_id);

ALTER TABLE Track
ADD CONSTRAINT chk_track_rating
CHECK (rating >= 0 AND rating <= 5);

ALTER TABLE Track
ALTER COLUMN title SET NOT NULL;

ALTER TABLE Track
ALTER COLUMN date_added SET DEFAULT CURRENT_DATE;

ALTER TABLE Track
ADD CONSTRAINT uq_track_title_artist UNIQUE (title, artist_id);
```
![[Pasted image 20260318163957.png]]
#### 4. Collection
```sql
ALTER TABLE Collection
ALTER COLUMN name SET NOT NULL;

ALTER TABLE Collection
ALTER COLUMN created_at SET DEFAULT CURRENT_DATE;
```
![[Pasted image 20260318164042.png]]
#### 5. CollectionTrack

```sql
ALTER TABLE CollectionTrack
ADD CONSTRAINT fk_collectiontrack_collection
FOREIGN KEY (collection_id) REFERENCES Collection(collection_id);

ALTER TABLE CollectionTrack
ADD CONSTRAINT fk_collectiontrack_track
FOREIGN KEY (track_id) REFERENCES Track(track_id);
```
![[Pasted image 20260318164108.png]]
#### 6. Event
```sql
ALTER TABLE Event
ADD CONSTRAINT fk_event_collection
FOREIGN KEY (collection_id) REFERENCES Collection(collection_id);

ALTER TABLE Event
ALTER COLUMN venue SET NOT NULL;

ALTER TABLE Event
ALTER COLUMN date SET NOT NULL;
```
![[Pasted image 20260318164140.png]]

## 2. Модификации структуры БД

Изменений структуры не было

---

## 3. Индексы базы данных

### 3.1. Кластерные индексы (по первичным ключам)
> В PostgreSQL кластерные индексы создаются автоматически при создании `PRIMARY KEY`, и таблицы физически упорядочиваются по этим ключам. Мы не создаем их вручную, но перечислим их для полноты.

| №   | Таблица         | Поле                      | Имя индекса          |
| --- | --------------- | ------------------------- | -------------------- |
| 1   | Artist          | artist_id                 | artist_pkey          |
| 2   | Genre           | genre_id                  | genre_pkey           |
| 3   | Track           | track_id                  | track_pkey           |
| 4   | Collection      | collection_id             | collection_pkey      |
| 5   | CollectionTrack | (collection_id, track_id) | collectiontrack_pkey |
| 6   | Event           | event_id                  | event_pkey           |
### 3.2. Некластерные индексы (вручную созданные для ускорения запросов)

#### Индексы для таблицы Artist
```sql
CREATE INDEX idx_artist_name ON Artist (name);
CREATE INDEX idx_artist_country_style ON Artist (country, style);
```

| №   | Имя индекса              | Таблица | Поля           | Тип    | Комментарий                |
| --- | ------------------------ | ------- | -------------- | ------ | -------------------------- |
| 1   | idx_artist_name          | Artist  | name           | B-Tree | Поиск по имени артиста     |
| 2   | idx_artist_country_style | Artist  | country, style | B-Tree | Фильтрация по стране/стилю |
####  Индексы для таблицы Genre
```sql
CREATE INDEX idx_genre_name ON Genre (name);
CREATE INDEX idx_genre_parent ON Genre (parent_genre_id);
CREATE INDEX idx_genre_bpm_range ON Genre (bpm_range);
```

| № | Имя индекса         | Таблица  | Поля             | Тип     | Комментарий               |
|---|---------------------|----------|------------------|---------|---------------------------|
| 1 | idx_genre_name      | Genre    | name             | B-Tree  | Поиск по названию жанра   |
| 2 | idx_genre_parent    | Genre    | parent_genre_id  | B-Tree  | Фильтрация по родителю    |
| 3 | idx_genre_bpm_range | Genre    | bpm_range        | B-Tree  | Поиск по BPM-диапазону    |
#### Индексы для таблицы Track

```sql
CREATE INDEX idx_track_title ON Track (title);
CREATE INDEX idx_track_artist_title ON Track (artist_id, title);
CREATE INDEX idx_track_genre ON Track (genre_id);
CREATE INDEX idx_track_bpm ON Track (bpm);
CREATE INDEX idx_track_key ON Track (key);
CREATE INDEX idx_track_date_added ON Track (date_added);
CREATE INDEX idx_track_rating ON Track (rating);
```

|№|Имя индекса|Таблица|Поля|Тип|Комментарий|
|---|---|---|---|---|---|
|1|idx_track_title|Track|title|B-Tree|Поиск по названию трека|
|2|idx_track_artist_title|Track|artist_id, title|B-Tree|Фильтрация по артисту+названию|
|3|idx_track_genre|Track|genre_id|B-Tree|Поиск по жанру|
|4|idx_track_bpm|Track|bpm|B-Tree|Поиск по BPM|
|5|idx_track_key|Track|key|B-Tree|Поиск по музыкальному ключу|
|6|idx_track_date_added|Track|date_added|B-Tree|Поиск по дате добавления|
|7|idx_track_rating|Track|rating|B-Tree|Поиск по рейтингу|

#### Индексы для таблицы Collection
```sql
CREATE INDEX idx_collection_name ON Collection (name);
CREATE INDEX idx_collection_type_style ON Collection (type, style);
CREATE INDEX idx_collection_created_at ON Collection (created_at);
```

| №   | Имя индекса               | Таблица    | Поля        | Тип    | Комментарий              |
| --- | ------------------------- | ---------- | ----------- | ------ | ------------------------ |
| 1   | idx_collection_name       | Collection | name        | B-Tree | Поиск по названию        |
| 2   | idx_collection_type_style | Collection | type, style | B-Tree | Фильтрация по типу/стилю |
| 3   | idx_collection_created_at | Collection | created_at  | B-Tree | Поиск по дате создания   |
#### Индексы для таблицы CollectionTrack
```sql
CREATE INDEX idx_collectiontrack_collection ON CollectionTrack (collection_id);
CREATE INDEX idx_collectiontrack_track ON CollectionTrack (track_id);
```

|№|Имя индекса|Таблица|Поля|Тип|Комментарий|
|---|---|---|---|---|---|
|1|idx_collectiontrack_collection|CollectionTrack|collection_id|B-Tree|Поиск по сборке|
|2|idx_collectiontrack_track|CollectionTrack|track_id|B-Tree|Поиск по треку|

#### Индексы для таблицы Event
```sql
CREATE INDEX idx_event_venue_date ON Event (venue, date);
CREATE INDEX idx_event_date ON Event (date);
CREATE INDEX idx_event_collection ON Event (collection_id);
CREATE INDEX idx_event_event_type ON Event (event_type);
```

|№|Имя индекса|Таблица|Поля|Тип|Комментарий|
|---|---|---|---|---|---|
|1|idx_event_venue_date|Event|venue, date|B-Tree|Фильтрация по месту+дате|
|2|idx_event_date|Event|date|B-Tree|Поиск по дате|
|3|idx_event_collection|Event|collection_id|B-Tree|Поиск по сборке|
|4|idx_event_event_type|Event|event_type|B-Tree|Поиск по типу события|
## 4. Диаграмма базы данных

![[Pasted image 20260318184814.png]]

## 5. Перечень разработанных таблиц БД

**Таблица 5.1.** Перечень разработанных таблиц БД

| № пп | Имя таблицы     | Описание                                                                                                                         |
| ---- | --------------- | -------------------------------------------------------------------------------------------------------------------------------- |
| 1    | Artist          | Хранит информацию об артистах (диджеях/музыкантах): имя, страна, стиль, период активности, биография                             |
| 2    | Genre           | Содержит жанры музыки с поддержкой иерархии (родительские жанры), BPM-диапазон и описание                                        |
| 3    | Track           | Основная таблица с треками: название, ссылки на артиста и жанр, BPM, тональность, длительность, рейтинг, счётчик воспроизведений |
| 4    | Collection      | Информация о сборках/сетаках: название, тип, описание, стиль, плановая длительность, заметки                                     |
| 5    | CollectionTrack | Таблица-связка для отношения многие-ко-многим между Collection и Track; определяет порядок треков в сборке                       |
| 6    | Event           | Сведения о мероприятиях/выступлениях: площадка, город, дата, тип события, аудитория, обратная связь, заработок                   |

---

## 6. Данные в таблицах (скриншоты)
![[Pasted image 20260318200508.png]]
![[Pasted image 20260318200515.png]]
![[Pasted image 20260318200525.png]]
![[Pasted image 20260318202449.png]]
![[Pasted image 20260318202456.png]]
![[Pasted image 20260318202521.png]]

---

## Выводы

В ходе выполнения лабораторной работы:
1. Была разработана база данных для предметной области «DJ Music Library» (библиотека музыки для диджеинга)
2. Созданы 6 таблиц с ограничениями целостности (PRIMARY KEY, FOREIGN KEY, UNIQUE, CHECK, NOT NULL)
3. Реализованы кластерные индексы для первичных ключей (создаются автоматически в PostgreSQL)
4. Созданы 22 некластерных индекса для оптимизации запросов (по полям поиска, фильтрации и соединения таблиц)
5. Введены тестовые данные из файла `luxiut_3.txt` с автоматическим парсингом и распределением по жанрам
6. Сформирована диаграмма базы данных, отображающая связи между таблицами

В результате была спроектирована нормализованная база данных, поддерживающая:
- иерархическую структуру жанров (рекурсивная связь в таблице `Genre`);
- связь многие-ко-многим между сборками и треками через таблицу-связку `CollectionTrack`;
- отслеживание статистики воспроизведений и рейтинга треков;
- привязку мероприятий (событий) к коллекциям треков.

Применение индексов обеспечивает эффективное выполнение типовых запросов: поиск по названию трека/артиста, фильтрация по BPM, жанру, рейтингу, дате добавления.

---

## Приложения

### Приложение А. Полный SQL-скрипт создания БД

```sql
-- Создание базы данных
CREATE DATABASE dj_db
    WITH ENCODING = 'UTF8'
    LC_COLLATE = 'ru_RU.UTF-8'
    LC_CTYPE = 'ru_RU.UTF-8'
    TEMPLATE = template0;

-- Подключение к БД
\c dj_db

-- ============================================================================
-- Создание таблиц
-- ============================================================================

CREATE TABLE Artist (
    artist_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    country VARCHAR(100),
    style VARCHAR(100),
    active_years VARCHAR(50),
    bio TEXT
);

CREATE TABLE Genre (
    genre_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    parent_genre_id INT REFERENCES Genre(genre_id),
    bpm_range VARCHAR(50),
    description TEXT
);

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

CREATE TABLE CollectionTrack (
    collection_id INT REFERENCES Collection(collection_id),
    track_id INT REFERENCES Track(track_id),
    position INT NOT NULL,
    transition_notes TEXT,
    PRIMARY KEY (collection_id, track_id)
);

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

-- ============================================================================
-- Ограничения целостности (ALTER TABLE)
-- ============================================================================

-- Artist
ALTER TABLE Artist ALTER COLUMN name SET NOT NULL;

-- Genre
ALTER TABLE Genre
ADD CONSTRAINT fk_genre_parent
FOREIGN KEY (parent_genre_id) REFERENCES Genre(genre_id);
ALTER TABLE Genre ALTER COLUMN name SET NOT NULL;
ALTER TABLE Genre ADD CONSTRAINT uq_genre_name UNIQUE (name);

-- Track
ALTER TABLE Track
ADD CONSTRAINT fk_track_artist
FOREIGN KEY (artist_id) REFERENCES Artist(artist_id);
ALTER TABLE Track
ADD CONSTRAINT fk_track_genre
FOREIGN KEY (genre_id) REFERENCES Genre(genre_id);
ALTER TABLE Track
ADD CONSTRAINT chk_track_rating
CHECK (rating >= 0 AND rating <= 5);
ALTER TABLE Track ALTER COLUMN title SET NOT NULL;
ALTER TABLE Track ALTER COLUMN date_added SET DEFAULT CURRENT_DATE;
ALTER TABLE Track
ADD CONSTRAINT uq_track_title_artist UNIQUE (title, artist_id);

-- Collection
ALTER TABLE Collection ALTER COLUMN name SET NOT NULL;
ALTER TABLE Collection ALTER COLUMN created_at SET DEFAULT CURRENT_DATE;

-- CollectionTrack
ALTER TABLE CollectionTrack
ADD CONSTRAINT fk_collectiontrack_collection
FOREIGN KEY (collection_id) REFERENCES Collection(collection_id);
ALTER TABLE CollectionTrack
ADD CONSTRAINT fk_collectiontrack_track
FOREIGN KEY (track_id) REFERENCES Track(track_id);

-- Event
ALTER TABLE Event
ADD CONSTRAINT fk_event_collection
FOREIGN KEY (collection_id) REFERENCES Collection(collection_id);
ALTER TABLE Event ALTER COLUMN venue SET NOT NULL;
ALTER TABLE Event ALTER COLUMN date SET NOT NULL;
```

### Приложение Б. Скрипт создания индексов

```sql
-- ============================================================================
-- Индексы для таблицы Artist
-- ============================================================================
CREATE INDEX idx_artist_name ON Artist (name);
CREATE INDEX idx_artist_country_style ON Artist (country, style);

-- ============================================================================
-- Индексы для таблицы Genre
-- ============================================================================
CREATE INDEX idx_genre_name ON Genre (name);
CREATE INDEX idx_genre_parent ON Genre (parent_genre_id);
CREATE INDEX idx_genre_bpm_range ON Genre (bpm_range);

-- ============================================================================
-- Индексы для таблицы Track
-- ============================================================================
CREATE INDEX idx_track_title ON Track (title);
CREATE INDEX idx_track_artist_title ON Track (artist_id, title);
CREATE INDEX idx_track_genre ON Track (genre_id);
CREATE INDEX idx_track_bpm ON Track (bpm);
CREATE INDEX idx_track_key ON Track (key);
CREATE INDEX idx_track_date_added ON Track (date_added);
CREATE INDEX idx_track_rating ON Track (rating);

-- ============================================================================
-- Индексы для таблицы Collection
-- ============================================================================
CREATE INDEX idx_collection_name ON Collection (name);
CREATE INDEX idx_collection_type_style ON Collection (type, style);
CREATE INDEX idx_collection_created_at ON Collection (created_at);

-- ============================================================================
-- Индексы для таблицы CollectionTrack
-- ============================================================================
CREATE INDEX idx_collectiontrack_collection ON CollectionTrack (collection_id);
CREATE INDEX idx_collectiontrack_track ON CollectionTrack (track_id);

-- ============================================================================
-- Индексы для таблицы Event
-- ============================================================================
CREATE INDEX idx_event_venue_date ON Event (venue, date);
CREATE INDEX idx_event_date ON Event (date);
CREATE INDEX idx_event_collection ON Event (collection_id);
CREATE INDEX idx_event_event_type ON Event (event_type);
```
