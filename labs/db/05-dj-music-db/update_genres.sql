-- Обновление жанров по ключевым словам (артист + трек)
-- Запуск: sudo -u postgres psql -d dj_db -f update_genres.sql

-- Phonk / Drift
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Phonk')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%phonk%', '%dvrst%', '%interworld%', '%kordhell%', '%moonboy%'])
    OR LOWER(title) LIKE ANY(ARRAY['%phonk%', '%drift%', '%metamorphosis%', '%raid%'])
  );

-- Metal
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Metal')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%pantera%', '%megadeth%', '%iron maiden%', '%metallica%', '%avenged sevenfold%', '%helloween%', '%guns n%rose%', '%scorpions%', '%accept%', '%annihilator%', '%gojira%', '%machine head%', '%deftones%', '%my chemical romance%', '%him%', '%twisted sister%'])
    OR LOWER(title) LIKE ANY(ARRAY['%metal%', '%cemetery gates%', '%this love%', '%afterlife%', '%power%', '%paradise city%'])
  );

-- Rock / Alternative
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Rock')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%offspring%', '%green day%', '%the offspring%', '%weezer%', '%the cranberries%', 'radiohead', '%beck%', '%nine inch nails%'])
    OR LOWER(title) LIKE ANY(ARRAY['%holiday%', '%boulevard%', '%creep%', '%zombie%', '%american idiot%'])
  );

-- Hip-Hop / Rap
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Hip-Hop')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%snoop%dogg%', '%snoop dogg%', '%kanye west%', '%jay-z%', '%2pac%', '%dr. dre%', '%wiz khalifa%', '%big baby tape%', '%aarne%', '%og buda%', '%lida%', '%booker%'])
    OR LOWER(title) LIKE ANY(ARRAY['%rap%', '%hip hop%', '%trap%', '%bandana%', '%crystal%', '%мой моёт%'])
  );

-- Electronic / House / Techno
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Electronic')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%daft punk%', '%justice%', '%deadmau5%', '%skrillex%', '%diplo%', '%kavinsky%'])
    OR LOWER(title) LIKE ANY(ARRAY['%electronic%', '%house%', '%techno%', '%digital%', '%genesis%', '%nightcall%'])
  );

-- Hardstyle / Hardcore / Frenchcore
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Hardstyle')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%hardstyle%', '%frenchcore%', '%hardtek%', '%hard tekk%'])
    OR LOWER(title) LIKE ANY(ARRAY['%hardstyle%', '%hardcore%', '%frenchcore%', '%hardtekk%', '%jumpstyle%'])
  );

-- Ambient / Chillout
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Ambient')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%brian eno%', '%tycho%', '%bonobo%'])
    OR LOWER(title) LIKE ANY(ARRAY['%ambient%', '%chill%', '%meditation%', '%relax%'])
  );

-- Classical
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Classical')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%bach%', '%mozart%', '%beethoven%', '%vivaldi%', '%сати%', '%шопен%', '%чайковский%'])
    OR LOWER(title) LIKE ANY(ARRAY['%symphony%', '%concerto%', '%sonata%', '%nocturne%', '%gymnopédie%'])
  );

-- Soundtrack / OST
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Soundtrack')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER(title) LIKE ANY(ARRAY['%ost%', '%soundtrack%', '%from "%', '%theme%', '%main title%'])
  );

-- Trap
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Trap')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%trap%', '%migos%', '%future%'])
    OR LOWER(title) LIKE ANY(ARRAY['%trap%', '%migos%'])
  );

-- Drum and Bass
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Drum and Bass')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%pendulum%', '%noisia%', '%netsky%'])
    OR LOWER(title) LIKE ANY(ARRAY['%drum and bass%', '%dnb%', '%liquid%'])
  );

-- Dubstep
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Dubstep')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%skrillex%', '%excision%', '%rezz%'])
    OR LOWER(title) LIKE ANY(ARRAY['%dubstep%', '%bass%'])
  );

-- Pop
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Pop')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%taylor swift%', '%billie eilish%', '%ariana grande%', '%dua lipa%', 'inna', '%reflex%', '%tatu%'])
    OR LOWER(title) LIKE ANY(ARRAY['%pop%', '%love%', '%baby%'])
  );

-- Jumpstyle
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Jumpstyle')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER(title) LIKE ANY(ARRAY['%jumpstyle%', '%jump%'])
  );

-- Trance
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Trance')
WHERE genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other')
  AND (
    LOWER((SELECT name FROM Artist WHERE artist_id = Track.artist_id)) LIKE ANY(ARRAY['%armin%', '%tiësto%', '%above & beyond%'])
    OR LOWER(title) LIKE ANY(ARRAY['%trance%', '%progressive%'])
  );

-- Funk
UPDATE Track SET genre_id = (SELECT genre_id FROM Genre WHERE name = 'Other') -- нет жанра Funk, оставляем Other
WHERE FALSE; -- заглушка

-- Итоговая статистика
SELECT 
    g.name as genre,
    COUNT(t.track_id) as tracks_count
FROM Genre g
LEFT JOIN Track t ON g.genre_id = t.genre_id
GROUP BY g.genre_id, g.name
ORDER BY tracks_count DESC;
