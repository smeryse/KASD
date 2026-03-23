# Лабораторная работа №6

**Тема:** Разработка области запросов к БД заданной предметной области

**Вариант:** Индивидуальный (на основе БД из лабораторной работы №5)

## 1. ЗАДАНИЕ

Разработка области запросов к БД заданной предметной области с использованием DML-команд SQL и средств СУБД PostgreSQL.

**Таблица 6.1 – Задание на разработку запросов:**
1. **Написать и протестировать DML-команды для управления данными БД:**
   - добавление записи;
   - изменение всех полей данных записи, идентифицированной значением первичного ключа;
   - удаление записи, идентифицированной значением первичного ключа.
2. **Ввести в таблицы тестовые наборы данных** при помощи разработанных команд добавления записей.
3. **Разработать не менее трех запросов** с использованием команды SELECT, предусматривающих соединение, выборку и проекцию отношений БД, в том числе запросы с использованием групповых операций, а также операций переименования атрибутов результатного отношения.
4. **Разработать представления** для формирования выходных данных согласно модели «Черный ящик», разработанной при выполнении лабораторной работы №1.

---

## 2. ХОД РАБОТЫ

### 2.1. Разработка DML-команд для управления данными

Для каждой таблицы базы данных `dj_db` были разработаны команды INSERT, UPDATE, DELETE.

**Статистика базы данных:**
- **artist**: 1921 запись
- **genre**: 23 записи
- **track**: 2790 записей
- **collection**: 10 записей
- **event**: 5 записей
- **collectiontrack**: связи между коллекциями и треками

#### Таблица Artist
```sql
INSERT INTO artist (name, country, style, active_years, bio) VALUES
    ('Deadmau5', 'Canada', 'Progressive House', '1998-present', 'Canadian electronic music producer and DJ');

-- Изменение записи по первичному ключу
UPDATE artist
SET name = 'Deadmau5 Official',
    country = 'Canada',
    style = 'Progressive House',
    active_years = '1998-present',
    bio = 'Canadian electronic music producer and DJ (updated)'
WHERE artist_id = 1922;

DELETE FROM track WHERE artist_id = 1922;
DELETE FROM artist WHERE artist_id = 1922;
```

![[Pasted image 20260318213549.png]]
#### Таблица Genre
```sql
INSERT INTO genre (name, parent_genre_id, bpm_range, description) VALUES
    ('Deep Techno', 5, '120-128', 'A deeper, more atmospheric subgenre of techno');

UPDATE genre
SET name = 'Deep Techno',
    parent_genre_id = 5,
    bpm_range = '120-130',
    description = 'A deeper, more atmospheric subgenre of techno (updated)'
WHERE genre_id = 46;

DELETE FROM track WHERE genre_id = 46;
DELETE FROM genre WHERE parent_genre_id = 46;
DELETE FROM genre WHERE genre_id = 46;
```
![[Pasted image 20260318213814.png]]
#### Таблица Track
```sql
INSERT INTO track (title, artist_id, genre_id, bpm, key, duration, file_format, file_path, rating, play_count, date_added, comments) VALUES
    ('Strobe', 2, 1, 128.0, 'Am', 634, 'FLAC', '/music/strobe.flac', 4.8, 0, CURRENT_DATE, 'Classic progressive house track');

UPDATE track
SET title = 'Strobe (Remastered)',
    artist_id = 2,
    genre_id = 1,
    bpm = 128.5,
    key = 'Am',
    duration = 634,
    file_format = 'FLAC',
    file_path = '/music/strobe.flac',
    rating = 5.0,
    play_count = 1,
    date_added = CURRENT_DATE,
    comments = 'Classic progressive house track (updated)'
WHERE track_id = 2913;

DELETE FROM track WHERE track_id = 2913;
```
![[Pasted image 20260318214122.png]]
#### Таблица Collection
```sql
INSERT INTO collection (name, type, description, style, planned_duration, created_at, notes, total_duration) VALUES
    ('Summer Vibes 2026', 'DJ Set', 'Summer party collection', 'House', 3600, CURRENT_DATE, 'Ready for summer events', NULL);

UPDATE collection
SET name = 'Summer Vibes 2026',
    type = 'DJ Set',
    description = 'Summer party collection (updated)',
    style = 'House',
    planned_duration = 3600,
    created_at = CURRENT_DATE,
    notes = 'Ready for summer events',
    total_duration = 3540
WHERE collection_id = 11;

DELETE FROM collectiontrack WHERE collection_id = 11;
DELETE FROM event WHERE collection_id = 11;
DELETE FROM collection WHERE collection_id = 11;
```

![[Pasted image 20260318214243.png]]

#### Таблица CollectionTrack
```sql
INSERT INTO collectiontrack (collection_id, track_id, position, transition_notes) VALUES
    (1, 100, 10, 'Smooth transition from track 100');

UPDATE collectiontrack
SET collection_id = 1,
    track_id = 100,
    position = 10,
    transition_notes = 'Smooth transition from track 100 (updated)'
WHERE collection_id = 1 AND track_id = 100;

DELETE FROM collectiontrack WHERE collection_id = 1 AND track_id = 100;
```
![[Pasted image 20260318214352.png]]
#### Таблица Event
```sql
INSERT INTO event (venue, city, date, audience_size, event_type, collection_id, feedback, earnings) VALUES
    ('Club Paradise', 'Ibiza', '2026-07-15', 500, 'Club Night', 1, 'Amazing crowd energy!', 5000.00);

UPDATE event
SET venue = 'Club Paradise',
    city = 'Ibiza',
    date = '2026-07-15',
    audience_size = 550,
    event_type = 'Club Night',
    collection_id = 1,
    feedback = 'Amazing crowd energy! (updated)',
    earnings = 5500.00
WHERE event_id = 11;

DELETE FROM event WHERE event_id = 11;
```
![[Pasted image 20260318214514.png]]
### 2.2. Ввод тестовых данных

При помощи разработанных INSERT-команд введены тестовые данные для демонстрации работы запросов.

**Листинг SQL-команд:**
```sql
INSERT INTO artist (name, country, style, active_years, bio) VALUES
    ('Martin Garrix', 'Netherlands', 'Big Room', '2012-present', 'Dutch DJ and record producer'),
    ('Tiësto', 'Netherlands', 'Trance', '1994-present', 'Legendary Dutch DJ'),
    ('Armin van Buuren', 'Netherlands', 'Trance', '1995-present', 'Dutch DJ and producer');

INSERT INTO genre (name, parent_genre_id, bpm_range, description) VALUES
    ('Big Room', 1, '126-132', 'Festival house music'),
    ('Uplifting Trance', 2, '138-142', 'Emotional trance subgenre');

INSERT INTO track (title, artist_id, genre_id, bpm, key, duration, file_format, file_path, rating, play_count, date_added, comments) VALUES
    ('Animals', 1922, 24, 128.0, 'Fm', 302, 'MP3', '/music/animals.mp3', 4.5, 150, '2026-01-15', 'Festival anthem'),
    ('Adagio for Strings', 1923, 25, 140.0, 'Dm', 480, 'FLAC', '/music/adagio.flac', 4.9, 200, '2026-02-20', 'Trance classic'),
    ('This Is What It Feels Like', 1924, 25, 132.0, 'Gm', 215, 'MP3', '/music/this_is.mp3', 4.7, 180, '2026-03-10', 'Vocal trance hit');

INSERT INTO collection (name, type, description, style, planned_duration, created_at, notes, total_duration) VALUES
    ('Festival Bangers', 'Playlist', 'Best festival tracks', 'Big Room', 900, CURRENT_DATE, 'For main stage', NULL);


INSERT INTO collectiontrack (collection_id, track_id, position, transition_notes) VALUES
    (11, 2791, 1, 'Opening track'),
    (11, 2792, 2, 'Build energy'),
    (11, 2793, 3, 'Peak time');

INSERT INTO event (venue, city, date, audience_size, event_type, collection_id, feedback, earnings) VALUES
    ('Tomorrowland', 'Boom', '2026-07-20', 50000, 'Festival', 11, 'Incredible show!', 100000.00);
```

![[Pasted image 20260318215226.png]]
### 2.3. Разработка SELECT-запросов

#### Запрос 1: Соединение таблиц с выборкой и проекцией
```sql
-- Запрос: Получить информацию о треках с именами артистов и жанров
SELECT
    t.title AS "Название трека",
    a.name AS "Артист",
    g.name AS "Жанр",
    t.bpm AS "BPM",
    t.key AS "Тональность",
    t.rating AS "Рейтинг",
    t.play_count AS "Воспроизведений"
FROM track t
    JOIN artist a ON t.artist_id = a.artist_id
    JOIN genre g ON t.genre_id = g.genre_id
ORDER BY t.rating DESC, t.play_count DESC
LIMIT 20;
```
![[Pasted image 20260318215611.png]]
#### Запрос 2: Запрос с групповыми операциями
```sql
-- Запрос: Статистика по жанрам (количество треков, средний BPM, средний рейтинг)
SELECT
    g.name AS "Жанр",
    COUNT(t.track_id) AS "Количество треков",
    ROUND(AVG(t.bpm), 2) AS "Средний BPM",
    ROUND(AVG(t.rating), 2) AS "Средний рейтинг",
    SUM(t.play_count) AS "Общее число воспроизведений"
FROM genre g
    LEFT JOIN track t ON g.genre_id = t.genre_id
GROUP BY g.genre_id, g.name
HAVING COUNT(t.track_id) > 0
ORDER BY COUNT(t.track_id) DESC;
```
![[Pasted image 20260318215702.png]]

#### Запрос 3: Сложный запрос с несколькими соединениями и агрегацией
```sql
-- Запрос: Информация о мероприятиях с коллекциями и треками
SELECT
    e.venue AS "Площадка",
    e.city AS "Город",
    e.date AS "Дата",
    e.event_type AS "Тип события",
    e.audience_size AS "Аудитория",
    c.name AS "Коллекция",
    COUNT(ct.track_id) AS "Треков в сете",
    ROUND(e.earnings, 2) AS "Доход ($)"
FROM event e
    LEFT JOIN collection c ON e.collection_id = c.collection_id
    LEFT JOIN collectiontrack ct ON c.collection_id = ct.collection_id
GROUP BY e.event_id, c.collection_id
ORDER BY e.date DESC;
```
![[Pasted image 20260318215727.png]]

#### Запрос 4: Запрос с переименованием атрибутов и фильтрацией
```sql
-- Запрос: Артисты по странам с количеством треков
SELECT
    a.country AS "Страна",
    a.name AS "Артист",
    a.style AS "Стиль",
    COUNT(t.track_id) AS "Треков",
    MAX(t.rating) AS "Лучший рейтинг"
FROM artist a
    LEFT JOIN track t ON a.artist_id = t.artist_id
WHERE a.country IS NOT NULL AND a.country != ''
GROUP BY a.artist_id, a.country, a.name, a.style
ORDER BY a.country, COUNT(t.track_id) DESC
LIMIT 20;
```
![[Pasted image 20260318215819.png]]

### 2.4. Разработка представлений (Views)

Согласно модели «Черный ящик» (lab 1), система должна формировать следующие выходные данные:
- Списки треков
- Подготовленные сеты
- История выступлений
- Отчеты

#### Представление 1: Списки треков

**Листинг SQL-команд:**
```sql
CREATE OR REPLACE VIEW v_track_lists AS
SELECT
    t.track_id,
    t.title AS track_title,
    a.name AS artist_name,
    g.name AS genre_name,
    t.bpm,
    t.key,
    t.duration,
    t.rating,
    t.play_count
FROM track t
    JOIN artist a ON t.artist_id = a.artist_id
    JOIN genre g ON t.genre_id = g.genre_id
ORDER BY t.title;

SELECT * FROM v_track_lists LIMIT 20;
```
![[Pasted image 20260318215952.png]]

#### Представление 2: Подготовленные сеты
```sql
CREATE OR REPLACE VIEW v_prepared_sets AS
SELECT
    c.collection_id,
    c.name AS set_name,
    c.type,
    c.style,
    c.planned_duration,
    ARRAY_AGG(t.title ORDER BY ct.position) AS tracks,
    COUNT(ct.track_id) AS total_tracks,
    SUM(t.duration) AS actual_duration_seconds
FROM collection c
    LEFT JOIN collectiontrack ct ON c.collection_id = ct.collection_id
    LEFT JOIN track t ON ct.track_id = t.track_id
GROUP BY c.collection_id, c.name, c.type, c.style, c.planned_duration
ORDER BY c.created_at DESC;

SELECT * FROM v_prepared_sets;
```
![[Pasted image 20260318220051.png]]
#### Представление 3: История выступлений
```sql
CREATE OR REPLACE VIEW v_performance_history AS
SELECT
    e.event_id,
    e.venue,
    e.city,
    e.date,
    e.event_type,
    e.audience_size,
    c.name AS set_name,
    e.feedback,
    e.earnings
FROM event e
    LEFT JOIN collection c ON e.collection_id = c.collection_id
ORDER BY e.date DESC;

-- Использование представления
SELECT * FROM v_performance_history;
```
![[Pasted image 20260318220159.png]]
#### Представление 4: Отчеты (Event Manager)
```sql
CREATE OR REPLACE VIEW v_event_manager_report AS
SELECT
    e.event_id,
    e.venue AS "Площадка",
    e.city AS "Город",
    e.date AS "Дата",
    e.event_type AS "Тип события",
    e.audience_size AS "Аудитория",
    c.name AS "Название сета",
    COUNT(ct.track_id) AS "Треков в сете",
    ROUND((SUM(t.duration) / 60.0)::numeric, 2) AS "Длительность сета (мин)",
    ROUND(AVG(t.rating), 2) AS "Средний рейтинг треков",
    e.earnings AS "Доход ($)",
    CASE
        WHEN e.audience_size >= 10000 THEN 'Крупное'
        WHEN e.audience_size >= 1000 THEN 'Среднее'
        ELSE 'Небольшое'
    END AS "Масштаб"
FROM event e
    LEFT JOIN collection c ON e.collection_id = c.collection_id
    LEFT JOIN collectiontrack ct ON c.collection_id = ct.collection_id
    LEFT JOIN track t ON ct.track_id = t.track_id
GROUP BY e.event_id, c.collection_id
ORDER BY e.date DESC;

SELECT * FROM v_event_manager_report;
```
![[Pasted image 20260318220229.png]]

## 3. ТАБЛИЦЫ БАЗЫ ДАННЫХ

**Таблица 6.2 – Таблицы базы данных dj_db:**

| №   | Наименование    | Тип         | Поле                                    | Ссылка                    | Действия         | Комментарий                                            |
| --- | --------------- | ----------- | --------------------------------------- | ------------------------- | ---------------- | ------------------------------------------------------ |
| 1   | artist          | PRIMARY KEY | artist_id                               | —                         | —                | Идентификатор артиста (GENERATED ALWAYS AS IDENTITY)   |
| 2   | artist          | —           | name, country, style, active_years, bio | —                         | —                | Информация об артисте                                  |
| 3   | genre           | PRIMARY KEY | genre_id                                | —                         | —                | Идентификатор жанра (GENERATED ALWAYS AS IDENTITY)     |
| 4   | genre           | FOREIGN KEY | parent_genre_id                         | genre(genre_id)           | RESTRICT/CASCADE | Рекурсивная связь для иерархии жанров                  |
| 5   | track           | PRIMARY KEY | track_id                                | —                         | —                | Идентификатор трека (GENERATED ALWAYS AS IDENTITY)     |
| 6   | track           | FOREIGN KEY | artist_id                               | artist(artist_id)         | RESTRICT/CASCADE | Ссылка на артиста                                      |
| 7   | track           | FOREIGN KEY | genre_id                                | genre(genre_id)           | RESTRICT/CASCADE | Ссылка на жанр                                         |
| 8   | track           | CHECK       | rating                                  | —                         | —                | Рейтинг от 0 до 5                                      |
| 9   | collection      | PRIMARY KEY | collection_id                           | —                         | —                | Идентификатор коллекции (GENERATED ALWAYS AS IDENTITY) |
| 10  | collectiontrack | PRIMARY KEY | (collection_id, track_id)               | —                         | —                | Составной первичный ключ                               |
| 11  | collectiontrack | FOREIGN KEY | collection_id                           | collection(collection_id) | CASCADE          | Ссылка на коллекцию                                    |
| 12  | collectiontrack | FOREIGN KEY | track_id                                | track(track_id)           | CASCADE          | Ссылка на трек                                         |
| 13  | event           | PRIMARY KEY | event_id                                | —                         | —                | Идентификатор события (GENERATED ALWAYS AS IDENTITY)   |
| 14  | event           | FOREIGN KEY | collection_id                           | collection(collection_id) | RESTRICT/CASCADE | Ссылка на коллекцию                                    |

**Индексы в базе данных:**
- `artist_pkey` — PRIMARY KEY на artist(artist_id)
- `idx_artist_name` — индекс на artist(name)
- `idx_artist_country_style` — индекс на artist(country, style)
- `genre_pkey` — PRIMARY KEY на genre(genre_id)
- `uq_genre_name` — UNIQUE на genre(name)
- `idx_genre_name` — индекс на genre(name)
- `idx_genre_bpm_range` — индекс на genre(bpm_range)
- `idx_genre_parent` — индекс на genre(parent_genre_id)
- `track_pkey` — PRIMARY KEY на track(track_id)
- `collectiontrack_pkey` — PRIMARY KEY на collectiontrack(collection_id, track_id)
- `event_pkey` — PRIMARY KEY на event(event_id)

## 4. ОТВЕТЫ НА КОНТРОЛЬНЫЕ ВОПРОСЫ

1. **Что такое DML-команды в SQL?**

   DML (Data Manipulation Language) — это язык манипулирования данными в SQL. Основные команды DML: INSERT (добавление данных), UPDATE (обновление данных), DELETE (удаление данных), SELECT (выборка данных). Эти команды позволяют работать с данными в таблицах базы данных.

2. **В чем разница между командами DELETE и TRUNCATE?**

   DELETE удаляет записи построчно с возможностью указания условия WHERE, может быть отменена (ROLLBACK), триггеры выполняются. TRUNCATE удаляет все данные из таблицы мгновенно, сбрасывая таблицу до пустого состояния, не может быть отменена в некоторых СУБД, триггеры не выполняются, быстрее чем DELETE.

3. **Что такое представление (VIEW) и для чего оно используется?**

   Представление — это виртуальная таблица, результат выполнения SQL-запроса. Используется для: упрощения сложных запросов, обеспечения безопасности (скрытие определенных колонок), абстракции данных (модель «Черный ящик»), переиспользования часто используемых запросов.

4. **Какие типы соединений (JOIN) вы знаете?**

   INNER JOIN — возвращает только совпадающие записи из обеих таблиц; LEFT JOIN (LEFT OUTER JOIN) — все записи из левой таблицы и совпадающие из правой; RIGHT JOIN (RIGHT OUTER JOIN) — все записи из правой таблицы и совпадающие из левой; FULL JOIN (FULL OUTER JOIN) — все записи из обеих таблиц; CROSS JOIN — декартово произведение таблиц.

5. **Для чего используются групповые операции в SQL?**

   Групповые операции (COUNT, SUM, AVG, MIN, MAX) используются для агрегации данных по группам строк. Применяются с предложением GROUP BY для группировки результатов и HAVING для фильтрации групп. Позволяют получать сводную статистику и аналитические данные.

## 5. ВЫВОДЫ

В ходе выполнения лабораторной работы:
1. Были разработаны и протестированы DML-команды (INSERT, UPDATE, DELETE) для всех 6 таблиц базы данных `dj_db`
2. Введены тестовые наборы данных для демонстрации работы запросов
3. Разработано 4 SELECT-запроса различной сложности, включая соединения таблиц, групповые операции и переименование атрибутов
4. Создано 4 представления (VIEW) согласно модели «Черный ящик» из лабораторной работы №1:
   - `v_track_lists` — списки треков
   - `v_prepared_sets` — подготовленные сеты
   - `v_performance_history` — история выступлений
   - `v_event_manager_report` — отчеты для Event Manager
5. Освоены инструменты psql и pgAdmin для выполнения SQL-запросов и визуализации результатов

В результате была закреплена работа с DML-командами SQL и средствами СУБД PostgreSQL для управления данными в базе данных. Разработанные запросы позволяют эффективно извлекать информацию из БД, а представления обеспечивают удобный интерфейс для получения выходных данных согласно требованиям модели «Черный ящик».

**Статистика выполненной работы:**
- Всего в БД: 1921 артист, 23 жанра, 2790 треков, 10 коллекций, 5 событий
- Разработано DML-команд: 18 (INSERT, UPDATE, DELETE для каждой таблицы)
- Разработано SELECT-запросов: 4
- Создано представлений (VIEW): 4

## ПРИЛОЖЕНИЕ А. Полный SQL-скрипт

```sql
-- ============================================================================
-- Лабораторная работа №6
-- Полный скрипт DML-команд и запросов
-- ============================================================================

-- ----------------------------------------------------------------------------
-- ЧАСТЬ 1: DML-команды для управления данными
-- ----------------------------------------------------------------------------

-- Artist: INSERT, UPDATE, DELETE
INSERT INTO artist (name, country, style, active_years, bio) VALUES
    ('Deadmau5', 'Canada', 'Progressive House', '1998-present', 'Canadian electronic music producer and DJ');

UPDATE artist
SET name = 'Deadmau5 Official',
    country = 'Canada',
    style = 'Progressive House',
    active_years = '1998-present',
    bio = 'Canadian electronic music producer and DJ (updated)'
WHERE artist_id = 1922;

-- Примечание: Сначала удалить зависимые записи из track
DELETE FROM track WHERE artist_id = 1922;
DELETE FROM artist WHERE artist_id = 1922;

-- Genre: INSERT, UPDATE, DELETE
INSERT INTO genre (name, parent_genre_id, bpm_range, description) VALUES
    ('Deep Techno', 5, '120-128', 'A deeper, more atmospheric subgenre of techno');

UPDATE genre
SET name = 'Deep Techno',
    parent_genre_id = 5,
    bpm_range = '120-130',
    description = 'A deeper, more atmospheric subgenre of techno (updated)'
WHERE genre_id = 24;

-- Примечание: Сначала удалить зависимые записи из track и дочерних жанров
DELETE FROM track WHERE genre_id = 24;
DELETE FROM genre WHERE parent_genre_id = 24;
DELETE FROM genre WHERE genre_id = 24;

-- Track: INSERT, UPDATE, DELETE
INSERT INTO track (title, artist_id, genre_id, bpm, key, duration, file_format, file_path, rating, play_count, date_added, comments) VALUES
    ('Strobe', 2, 1, 128.0, 'Am', 634, 'FLAC', '/music/strobe.flac', 4.8, 0, CURRENT_DATE, 'Classic progressive house track');

UPDATE track
SET title = 'Strobe (Remastered)',
    artist_id = 2,
    genre_id = 1,
    bpm = 128.5,
    key = 'Am',
    duration = 634,
    file_format = 'FLAC',
    file_path = '/music/strobe.flac',
    rating = 5.0,
    play_count = 1,
    date_added = CURRENT_DATE,
    comments = 'Classic progressive house track (updated)'
WHERE track_id = 2791;

DELETE FROM track WHERE track_id = 2791;

-- Collection: INSERT, UPDATE, DELETE
INSERT INTO collection (name, type, description, style, planned_duration, created_at, notes, total_duration) VALUES
    ('Summer Vibes 2026', 'DJ Set', 'Summer party collection', 'House', 3600, CURRENT_DATE, 'Ready for summer events', NULL);

UPDATE collection
SET name = 'Summer Vibes 2026',
    type = 'DJ Set',
    description = 'Summer party collection (updated)',
    style = 'House',
    planned_duration = 3600,
    created_at = CURRENT_DATE,
    notes = 'Ready for summer events',
    total_duration = 3540
WHERE collection_id = 11;

-- Примечание: Сначала удалить зависимые записи
DELETE FROM collectiontrack WHERE collection_id = 11;
DELETE FROM event WHERE collection_id = 11;
DELETE FROM collection WHERE collection_id = 11;

-- CollectionTrack: INSERT, UPDATE, DELETE
INSERT INTO collectiontrack (collection_id, track_id, position, transition_notes) VALUES
    (1, 100, 10, 'Smooth transition from track 100');

UPDATE collectiontrack
SET collection_id = 1,
    track_id = 100,
    position = 10,
    transition_notes = 'Smooth transition from track 100 (updated)'
WHERE collection_id = 1 AND track_id = 100;

DELETE FROM collectiontrack WHERE collection_id = 1 AND track_id = 100;

-- Event: INSERT, UPDATE, DELETE
INSERT INTO event (venue, city, date, audience_size, event_type, collection_id, feedback, earnings) VALUES
    ('Club Paradise', 'Ibiza', '2026-07-15', 500, 'Club Night', 1, 'Amazing crowd energy!', 5000.00);

UPDATE event
SET venue = 'Club Paradise',
    city = 'Ibiza',
    date = '2026-07-15',
    audience_size = 550,
    event_type = 'Club Night',
    collection_id = 1,
    feedback = 'Amazing crowd energy! (updated)',
    earnings = 5500.00
WHERE event_id = 11;

DELETE FROM event WHERE event_id = 11;

-- ----------------------------------------------------------------------------
-- ЧАСТЬ 2: Ввод тестовых данных
-- ----------------------------------------------------------------------------

INSERT INTO artist (name, country, style, active_years, bio) VALUES
    ('Martin Garrix', 'Netherlands', 'Big Room', '2012-present', 'Dutch DJ and record producer'),
    ('Tiësto', 'Netherlands', 'Trance', '1994-present', 'Legendary Dutch DJ'),
    ('Armin van Buuren', 'Netherlands', 'Trance', '1995-present', 'Dutch DJ and producer');

INSERT INTO genre (name, parent_genre_id, bpm_range, description) VALUES
    ('Big Room', 1, '126-132', 'Festival house music'),
    ('Uplifting Trance', 2, '138-142', 'Emotional trance subgenre');

INSERT INTO track (title, artist_id, genre_id, bpm, key, duration, file_format, file_path, rating, play_count, date_added, comments) VALUES
    ('Animals', 1922, 24, 128.0, 'Fm', 302, 'MP3', '/music/animals.mp3', 4.5, 150, '2026-01-15', 'Festival anthem'),
    ('Adagio for Strings', 1923, 25, 140.0, 'Dm', 480, 'FLAC', '/music/adagio.flac', 4.9, 200, '2026-02-20', 'Trance classic'),
    ('This Is What It Feels Like', 1924, 25, 132.0, 'Gm', 215, 'MP3', '/music/this_is.mp3', 4.7, 180, '2026-03-10', 'Vocal trance hit');

INSERT INTO collection (name, type, description, style, planned_duration, created_at, notes, total_duration) VALUES
    ('Festival Bangers', 'Playlist', 'Best festival tracks', 'Big Room', 900, CURRENT_DATE, 'For main stage', NULL);

INSERT INTO collectiontrack (collection_id, track_id, position, transition_notes) VALUES
    (11, 2791, 1, 'Opening track'),
    (11, 2792, 2, 'Build energy'),
    (11, 2793, 3, 'Peak time');

INSERT INTO event (venue, city, date, audience_size, event_type, collection_id, feedback, earnings) VALUES
    ('Tomorrowland', 'Boom', '2026-07-20', 50000, 'Festival', 11, 'Incredible show!', 100000.00);

-- ----------------------------------------------------------------------------
-- ЧАСТЬ 3: SELECT-запросы
-- ----------------------------------------------------------------------------

-- Запрос 1: Треки с артистами и жанрами
SELECT
    t.title AS "Название трека",
    a.name AS "Артист",
    g.name AS "Жанр",
    t.bpm AS "BPM",
    t.key AS "Тональность",
    t.rating AS "Рейтинг",
    t.play_count AS "Воспроизведений"
FROM track t
    JOIN artist a ON t.artist_id = a.artist_id
    JOIN genre g ON t.genre_id = g.genre_id
WHERE t.rating IS NOT NULL AND t.rating >= 4.5
ORDER BY t.rating DESC, t.play_count DESC
LIMIT 20;

-- Запрос 2: Статистика по жанрам
SELECT
    g.name AS "Жанр",
    COUNT(t.track_id) AS "Количество треков",
    ROUND(AVG(t.bpm), 2) AS "Средний BPM",
    ROUND(AVG(t.rating), 2) AS "Средний рейтинг",
    SUM(t.play_count) AS "Общее число воспроизведений"
FROM genre g
    LEFT JOIN track t ON g.genre_id = t.genre_id
GROUP BY g.genre_id, g.name
HAVING COUNT(t.track_id) > 0
ORDER BY COUNT(t.track_id) DESC;

-- Запрос 3: Мероприятия с коллекциями
SELECT
    e.venue AS "Площадка",
    e.city AS "Город",
    e.date AS "Дата",
    e.event_type AS "Тип события",
    e.audience_size AS "Аудитория",
    c.name AS "Коллекция",
    COUNT(ct.track_id) AS "Треков в сете",
    ROUND(e.earnings, 2) AS "Доход ($)"
FROM event e
    LEFT JOIN collection c ON e.collection_id = c.collection_id
    LEFT JOIN collectiontrack ct ON c.collection_id = ct.collection_id
GROUP BY e.event_id, c.collection_id
ORDER BY e.date DESC;

-- Запрос 4: Артисты по странам
SELECT
    a.country AS "Страна",
    a.name AS "Артист",
    a.style AS "Стиль",
    COUNT(t.track_id) AS "Треков",
    MAX(t.rating) AS "Лучший рейтинг"
FROM artist a
    LEFT JOIN track t ON a.artist_id = t.artist_id
WHERE a.country IS NOT NULL AND a.country != ''
GROUP BY a.artist_id, a.country, a.name, a.style
ORDER BY a.country, COUNT(t.track_id) DESC
LIMIT 20;

-- ----------------------------------------------------------------------------
-- ЧАСТЬ 4: Создание представлений (модель «Черный ящик»)
-- ----------------------------------------------------------------------------

-- Представление 1: Списки треков
CREATE OR REPLACE VIEW v_track_lists AS
SELECT
    t.track_id,
    t.title AS track_title,
    a.name AS artist_name,
    g.name AS genre_name,
    t.bpm,
    t.key,
    t.duration,
    t.rating,
    t.play_count
FROM track t
    JOIN artist a ON t.artist_id = a.artist_id
    JOIN genre g ON t.genre_id = g.genre_id
ORDER BY t.title;

-- Представление 2: Подготовленные сеты
CREATE OR REPLACE VIEW v_prepared_sets AS
SELECT
    c.collection_id,
    c.name AS set_name,
    c.type,
    c.style,
    c.planned_duration,
    ARRAY_AGG(t.title ORDER BY ct.position) AS tracks,
    COUNT(ct.track_id) AS total_tracks,
    SUM(t.duration) AS actual_duration_seconds
FROM collection c
    LEFT JOIN collectiontrack ct ON c.collection_id = ct.collection_id
    LEFT JOIN track t ON ct.track_id = t.track_id
GROUP BY c.collection_id, c.name, c.type, c.style, c.planned_duration
ORDER BY c.created_at DESC;

-- Представление 3: История выступлений
CREATE OR REPLACE VIEW v_performance_history AS
SELECT
    e.event_id,
    e.venue,
    e.city,
    e.date,
    e.event_type,
    e.audience_size,
    c.name AS set_name,
    e.feedback,
    e.earnings
FROM event e
    LEFT JOIN collection c ON e.collection_id = c.collection_id
ORDER BY e.date DESC;

-- Представление 4: Отчеты (Event Manager)
CREATE OR REPLACE VIEW v_event_manager_report AS
SELECT
    e.event_id,
    e.venue AS "Площадка",
    e.city AS "Город",
    e.date AS "Дата",
    e.event_type AS "Тип события",
    e.audience_size AS "Аудитория",
    c.name AS "Название сета",
    COUNT(ct.track_id) AS "Треков в сете",
    ROUND((SUM(t.duration) / 60.0)::numeric, 2) AS "Длительность сета (мин)",
    ROUND(AVG(t.rating), 2) AS "Средний рейтинг треков",
    e.earnings AS "Доход ($)",
    CASE
        WHEN e.audience_size >= 10000 THEN 'Крупное'
        WHEN e.audience_size >= 1000 THEN 'Среднее'
        ELSE 'Небольшое'
    END AS "Масштаб"
FROM event e
    LEFT JOIN collection c ON e.collection_id = c.collection_id
    LEFT JOIN collectiontrack ct ON c.collection_id = ct.collection_id
    LEFT JOIN track t ON ct.track_id = t.track_id
GROUP BY e.event_id, c.collection_id
ORDER BY e.date DESC;
```

---

**Конец отчёта**
