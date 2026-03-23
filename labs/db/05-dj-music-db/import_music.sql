-- Скрипт импорта музыки из luxiut_3.txt в БД dj_db
-- Запуск: psql -d dj_db -f import_music.sql

-- Очистка таблиц (если нужно)
-- TRUNCATE Track, Artist, Genre RESTART IDENTITY CASCADE;

-- Временная таблица для сырых данных
CREATE TEMP TABLE raw_tracks (
    line TEXT
);

-- Копируем данные из файла
\copy raw_tracks FROM '/tmp/luxiut_3.txt' WITH (encoding 'UTF8');

-- Таблица для распарсенных данных
CREATE TEMP TABLE parsed_tracks (
    artist_str TEXT,
    title_str TEXT
);

-- Парсим строки вида "Artist - Title"
INSERT INTO parsed_tracks (artist_str, title_str)
SELECT 
    TRIM(SUBSTRING(line FROM '^(.+?)\s*-\s*(.+)$')) as artist_part,
    TRIM(SUBSTRING(line FROM '^.+?\s*-\s*(.+)$')) as title_part
FROM raw_tracks
WHERE line ~ '^.+?\s*-\s*.+$';  -- только строки с разделителем " - "

-- ============================================
-- 1. Заполняем таблицу Artist
-- ============================================

-- Вставляем уникальных артистов
INSERT INTO Artist (name, country, style, active_years, bio)
SELECT DISTINCT 
    TRIM(artist_str) as name,
    NULL as country,
    NULL as style,
    NULL as active_years,
    NULL as bio
FROM parsed_tracks
WHERE TRIM(artist_str) IS NOT NULL
  AND TRIM(artist_str) != ''
ON CONFLICT DO NOTHING;

-- ============================================
-- 2. Заполняем таблицу Genre (базовые жанры)
-- ============================================

INSERT INTO Genre (name, parent_genre_id, bpm_range, description)
VALUES 
    ('House', NULL, '115-130', 'Электронная танцевальная музыка'),
    ('Techno', NULL, '125-140', 'Техно'),
    ('Trance', NULL, '125-140', 'Транс'),
    ('Drum and Bass', NULL, '160-180', 'Драм-н-бейс'),
    ('Dubstep', NULL, '138-142', 'Дабстеп'),
    ('Hardstyle', NULL, '150-160', 'Хардстайл'),
    ('Hip-Hop', NULL, '80-115', 'Хип-хоп'),
    ('Rap', NULL, '80-115', 'Рэп'),
    ('Pop', NULL, '90-140', 'Поп-музыка'),
    ('Rock', NULL, '100-180', 'Рок'),
    ('Metal', NULL, '120-200', 'Метал'),
    ('Alternative', NULL, '80-160', 'Альтернатива'),
    ('Electronic', NULL, '60-180', 'Электронная музыка'),
    ('Phonk', NULL, '100-120', 'Фонк'),
    ('Trap', NULL, '130-170', 'Трап'),
    ('Hardcore', NULL, '160-200', 'Хардкор'),
    ('Frenchcore', NULL, '190-220', 'Френчкор'),
    ('Jumpstyle', NULL, '140-150', 'Джампстайл'),
    ('Ambient', NULL, '60-100', 'Эмбиент'),
    ('Classical', NULL, '60-140', 'Классическая музыка'),
    ('Soundtrack', NULL, '60-180', 'Саундтреки'),
    ('Other', NULL, NULL, 'Прочее')
ON CONFLICT (name) DO NOTHING;

-- ============================================
-- 3. Заполняем таблицу Track
-- ============================================

INSERT INTO Track (title, artist_id, genre_id, bpm, key, duration, file_format, file_path, rating, play_count, date_added, comments)
SELECT 
    pt.title_str as title,
    a.artist_id,
    g.genre_id,
    NULL as bpm,
    NULL as key,
    NULL as duration,
    NULL as file_format,
    NULL as file_path,
    NULL as rating,
    0 as play_count,
    CURRENT_DATE as date_added,
    NULL as comments
FROM parsed_tracks pt
JOIN Artist a ON TRIM(pt.artist_str) = TRIM(a.name)
LEFT JOIN Genre g ON g.name = 'Other'  -- можно определить жанр по ключевым словам
WHERE TRIM(pt.title_str) IS NOT NULL
  AND TRIM(pt.title_str) != ''
ON CONFLICT DO NOTHING;

-- ============================================
-- 4. Обновляем жанры на основе ключевых слов в названии трека/артиста
-- ============================================

-- Phonk
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Phonk')
WHERE (LOWER(title) LIKE '%phonk%' OR LOWER(title) LIKE '%drift%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- Hardstyle / Hardcore
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Hardstyle')
WHERE (LOWER(title) LIKE '%hardstyle%' OR LOWER(title) LIKE '%hardcore%' OR LOWER(title) LIKE '%frenchcore%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- Hip-Hop / Rap
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Hip-Hop')
WHERE (LOWER(title) LIKE '%hip hop%' OR LOWER(title) LIKE '%rap%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- Techno
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Techno')
WHERE (LOWER(title) LIKE '%techno%' OR LOWER(title) LIKE '%tek%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- House
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'House')
WHERE (LOWER(title) LIKE '%house%' OR LOWER(title) LIKE '%deep house%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- Metal
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Metal')
WHERE (LOWER(title) LIKE '%metal%' OR LOWER(title) LIKE '%death%' OR LOWER(title) LIKE '%black%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- Rock
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Rock')
WHERE (LOWER(title) LIKE '%rock%' OR LOWER(title) LIKE '%punk%' OR LOWER(title) LIKE '%grunge%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- Electronic
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Electronic')
WHERE (LOWER(title) LIKE '%electronic%' OR LOWER(title) LIKE '%edm%' OR LOWER(title) LIKE '%dance%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- Ambient
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Ambient')
WHERE (LOWER(title) LIKE '%ambient%' OR LOWER(title) LIKE '%chill%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- Soundtrack
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Soundtrack')
WHERE (LOWER(title) LIKE '%ost%' OR LOWER(title) LIKE '%soundtrack%' OR LOWER(title) LIKE '%from "%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- Classical
UPDATE Track 
SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Classical')
WHERE (LOWER(title) LIKE '%classical%' OR LOWER(title) LIKE '%symphony%' OR LOWER(title) LIKE '%concerto%')
  AND genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other');

-- ============================================
-- 5. Вывод статистики
-- ============================================

SELECT 'Artist' as table_name, COUNT(*) as count FROM Artist
UNION ALL
SELECT 'Genre', COUNT(*) FROM Genre
UNION ALL
SELECT 'Track', COUNT(*) FROM Track;

-- Очистка временных таблиц
DROP TABLE raw_tracks;
DROP TABLE parsed_tracks;
