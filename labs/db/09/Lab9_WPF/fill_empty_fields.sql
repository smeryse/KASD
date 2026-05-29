-- ============================================================================
-- Скрипт заполнения пустых полей в БД DJ Music Library
-- ============================================================================

-- ----------------------------------------------------------------------------
-- 1. Artist: заполнить country, style, active_years, bio
-- ----------------------------------------------------------------------------
UPDATE artist SET country = 'Россия' WHERE country IS NULL AND artist_id % 5 = 0;
UPDATE artist SET country = 'США' WHERE country IS NULL AND artist_id % 5 = 1;
UPDATE artist SET country = 'Германия' WHERE country IS NULL AND artist_id % 5 = 2;
UPDATE artist SET country = 'Великобритания' WHERE country IS NULL AND artist_id % 5 = 3;
UPDATE artist SET country = 'Нидерланды' WHERE country IS NULL AND country IS NULL;

UPDATE artist SET style = 'Techno' WHERE style IS NULL AND artist_id % 4 = 0;
UPDATE artist SET style = 'House' WHERE style IS NULL AND artist_id % 4 = 1;
UPDATE artist SET style = 'Trance' WHERE style IS NULL AND artist_id % 4 = 2;
UPDATE artist SET style = 'Drum & Bass' WHERE style IS NULL AND style IS NULL;

UPDATE artist SET active_years = '2010-н.в.' WHERE active_years IS NULL AND artist_id % 3 = 0;
UPDATE artist SET active_years = '2005-2020' WHERE active_years IS NULL AND artist_id % 3 = 1;
UPDATE artist SET active_years = '2015-н.в.' WHERE active_years IS NULL AND active_years IS NULL;

UPDATE artist SET bio = 'Известный DJ и продюсер электронной музыки.' WHERE bio IS NULL AND artist_id % 2 = 0;
UPDATE artist SET bio = 'Участник крупнейших фестивалей электронной музыки.' WHERE bio IS NULL AND bio IS NULL;

-- ----------------------------------------------------------------------------
-- 2. Genre: заполнить parent_genre_id, bpm_range
-- ----------------------------------------------------------------------------
UPDATE genre SET bpm_range = '120-130' WHERE bpm_range IS NULL AND name IN ('House', 'Deep House', 'Tech House');
UPDATE genre SET bpm_range = '125-140' WHERE bpm_range IS NULL AND name IN ('Techno', 'Minimal', 'Acid');
UPDATE genre SET bpm_range = '130-150' WHERE bpm_range IS NULL AND name IN ('Trance', 'Progressive', 'Uplifting');
UPDATE genre SET bpm_range = '170-180' WHERE bpm_range IS NULL AND name IN ('Drum & Bass', 'Jungle', 'Liquid');
UPDATE genre SET bpm_range = '140-150' WHERE bpm_range IS NULL AND name IN ('Dubstep', 'Garage');
UPDATE genre SET bpm_range = '110-125' WHERE bpm_range IS NULL;

-- ----------------------------------------------------------------------------
-- 3. Track: заполнить bpm, key, duration, file_format, file_path, rating, comments
-- ----------------------------------------------------------------------------
UPDATE track SET bpm = 128.00 WHERE bpm IS NULL AND track_id % 5 = 0;
UPDATE track SET bpm = 130.00 WHERE bpm IS NULL AND track_id % 5 = 1;
UPDATE track SET bpm = 125.00 WHERE bpm IS NULL AND track_id % 5 = 2;
UPDATE track SET bpm = 135.00 WHERE bpm IS NULL AND track_id % 5 = 3;
UPDATE track SET bpm = 140.00 WHERE bpm IS NULL AND bpm IS NULL;

UPDATE track SET key = 'Am' WHERE key IS NULL AND track_id % 8 = 0;
UPDATE track SET key = 'Cm' WHERE key IS NULL AND track_id % 8 = 1;
UPDATE track SET key = 'Dm' WHERE key IS NULL AND track_id % 8 = 2;
UPDATE track SET key = 'Em' WHERE key IS NULL AND track_id % 8 = 3;
UPDATE track SET key = 'Fm' WHERE key IS NULL AND track_id % 8 = 4;
UPDATE track SET key = 'Gm' WHERE key IS NULL AND track_id % 8 = 5;
UPDATE track SET key = 'Bbm' WHERE key IS NULL AND track_id % 8 = 6;
UPDATE track SET key = 'Abm' WHERE key IS NULL AND key IS NULL;

UPDATE track SET duration = 360 WHERE duration IS NULL AND track_id % 4 = 0;
UPDATE track SET duration = 420 WHERE duration IS NULL AND track_id % 4 = 1;
UPDATE track SET duration = 300 WHERE duration IS NULL AND track_id % 4 = 2;
UPDATE track SET duration = 480 WHERE duration IS NULL AND duration IS NULL;

UPDATE track SET file_format = 'MP3' WHERE file_format IS NULL AND track_id % 3 = 0;
UPDATE track SET file_format = 'WAV' WHERE file_format IS NULL AND track_id % 3 = 1;
UPDATE track SET file_format = 'FLAC' WHERE file_format IS NULL AND file_format IS NULL;

UPDATE track SET file_path = '/music/tracks/' || track_id || '.' || COALESCE(file_format, 'mp3') WHERE file_path IS NULL;

UPDATE track SET rating = 4.50 WHERE rating IS NULL AND track_id % 5 = 0;
UPDATE track SET rating = 4.00 WHERE rating IS NULL AND track_id % 5 = 1;
UPDATE track SET rating = 3.50 WHERE rating IS NULL AND track_id % 5 = 2;
UPDATE track SET rating = 5.00 WHERE rating IS NULL AND track_id % 5 = 3;
UPDATE track SET rating = 3.00 WHERE rating IS NULL AND rating IS NULL;

UPDATE track SET comments = 'Отличный трек для сета.' WHERE comments IS NULL AND track_id % 3 = 0;
UPDATE track SET comments = 'Популярный трек на мероприятиях.' WHERE comments IS NULL AND track_id % 3 = 1;
UPDATE track SET comments = 'Классика электронной музыки.' WHERE comments IS NULL AND comments IS NULL;

-- ----------------------------------------------------------------------------
-- 4. Collection: заполнить type, style, planned_duration, notes, total_duration
-- ----------------------------------------------------------------------------
UPDATE collection SET type = 'DJ Set' WHERE type IS NULL AND collection_id % 3 = 0;
UPDATE collection SET type = 'Playlist' WHERE type IS NULL AND collection_id % 3 = 1;
UPDATE collection SET type = 'Live Mix' WHERE type IS NULL AND type IS NULL;

UPDATE collection SET style = 'Techno' WHERE style IS NULL AND collection_id % 4 = 0;
UPDATE collection SET style = 'House' WHERE style IS NULL AND collection_id % 4 = 1;
UPDATE collection SET style = 'Trance' WHERE style IS NULL AND collection_id % 4 = 2;
UPDATE collection SET style = 'Mixed' WHERE style IS NULL AND style IS NULL;

UPDATE collection SET planned_duration = 3600 WHERE planned_duration IS NULL AND collection_id % 3 = 0;
UPDATE collection SET planned_duration = 5400 WHERE planned_duration IS NULL AND collection_id % 3 = 1;
UPDATE collection SET planned_duration = 7200 WHERE planned_duration IS NULL AND planned_duration IS NULL;

UPDATE collection SET notes = 'Подборка для клубного сета.' WHERE notes IS NULL AND collection_id % 2 = 0;
UPDATE collection SET notes = 'Треки для фестиваля.' WHERE notes IS NULL AND notes IS NULL;

-- ----------------------------------------------------------------------------
-- 5. CollectionTrack: заполнить transition_notes
-- ----------------------------------------------------------------------------
UPDATE collectiontrack SET transition_notes = 'Плавный переход' WHERE transition_notes IS NULL AND position % 3 = 0;
UPDATE collectiontrack SET transition_notes = 'Резкий переход' WHERE transition_notes IS NULL AND position % 3 = 1;
UPDATE collectiontrack SET transition_notes = 'Микс' WHERE transition_notes IS NULL AND transition_notes IS NULL;

-- ----------------------------------------------------------------------------
-- 6. Event: заполнить city, audience_size, event_type, feedback, earnings
-- ----------------------------------------------------------------------------
UPDATE event SET city = 'Москва' WHERE city IS NULL AND event_id % 5 = 0;
UPDATE event SET city = 'Санкт-Петербург' WHERE city IS NULL AND event_id % 5 = 1;
UPDATE event SET city = 'Берлин' WHERE city IS NULL AND event_id % 5 = 2;
UPDATE event SET city = 'Амстердам' WHERE city IS NULL AND event_id % 5 = 3;
UPDATE event SET city = 'Лондон' WHERE city IS NULL AND city IS NULL;

UPDATE event SET audience_size = 5000 WHERE audience_size IS NULL AND event_id % 4 = 0;
UPDATE event SET audience_size = 15000 WHERE audience_size IS NULL AND event_id % 4 = 1;
UPDATE event SET audience_size = 2000 WHERE audience_size IS NULL AND event_id % 4 = 2;
UPDATE event SET audience_size = 25000 WHERE audience_size IS NULL AND audience_size IS NULL;

UPDATE event SET event_type = 'Festival' WHERE event_type IS NULL AND event_id % 4 = 0;
UPDATE event SET event_type = 'Club Night' WHERE event_type IS NULL AND event_id % 4 = 1;
UPDATE event SET event_type = 'Rave' WHERE event_type IS NULL AND event_id % 4 = 2;
UPDATE event SET event_type = 'Concert' WHERE event_type IS NULL AND event_type IS NULL;

UPDATE event SET feedback = 'Отличное мероприятие!' WHERE feedback IS NULL AND event_id % 2 = 0;
UPDATE event SET feedback = 'Публика была в восторге.' WHERE feedback IS NULL AND feedback IS NULL;

UPDATE event SET earnings = 15000.00 WHERE earnings IS NULL AND event_id % 4 = 0;
UPDATE event SET earnings = 50000.00 WHERE earnings IS NULL AND event_id % 4 = 1;
UPDATE event SET earnings = 8000.00 WHERE earnings IS NULL AND event_id % 4 = 2;
UPDATE event SET earnings = 75000.00 WHERE earnings IS NULL AND earnings IS NULL;

-- ----------------------------------------------------------------------------
-- Проверка результатов
-- ----------------------------------------------------------------------------
SELECT 'artist' as tbl,
  count(*) as total,
  count(country) as with_country,
  count(style) as with_style,
  count(active_years) as with_years,
  count(bio) as with_bio
FROM artist
UNION ALL
SELECT 'genre', count(*), count(parent_genre_id), count(bpm_range), count(description) FROM genre
UNION ALL
SELECT 'track', count(*), count(bpm), count(key), count(duration), count(file_format) FROM track
UNION ALL
SELECT 'collection', count(*), count(type), count(style), count(planned_duration), count(notes) FROM collection
UNION ALL
SELECT 'collectiontrack', count(*), count(transition_notes), count(*), count(*), count(*) FROM collectiontrack
UNION ALL
SELECT 'event', count(*), count(city), count(audience_size), count(event_type), count(feedback) FROM event;
