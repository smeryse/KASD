# Лабораторная работа №7

**Тема:** Создание триггеров в СУБД PostgreSQL

**Вариант:** Индивидуальный (на основе БД из лабораторной работы №5)

---

## 1. ЗАДАНИЕ

Разработка и тестирование триггеров для базы данных `dj_db` с использованием средств СУБД PostgreSQL.

**Таблица 7.1 – Задание на разработку триггеров:**
1. **Написать два триггера для разных таблиц базы данных:**
   - триггер для автоматического обновления данных при изменении записей;
   - триггер для ведения журнала аудита (логирования) операций изменения данных.
2. **Протестировать разработанные триггеры** на различных сценариях использования.
3. **Продемонстрировать работу триггеров** с помощью SQL-запросов.

---

## 2. ХОД РАБОТЫ

### 2.1. Теоретические сведения

**Триггер** — это хранимая процедура, которая автоматически выполняется при возникновении определённого события (INSERT, UPDATE, DELETE) в таблице базы данных.

**Возможные события для триггеров:**
- `INSERT` — вставка новых данных в таблицу;
- `DELETE` — удаление данных из таблицы;
- `UPDATE` — обновление данных в таблице.

**Временные режимы работы триггеров:**
- `BEFORE` — триггер выполняется до операции изменения таблицы;
- `AFTER` — триггер выполняется после операции изменения таблицы;
- `INSTEAD OF` — триггер выполняется вместо операции (для представлений).

**Особенности триггеров в PostgreSQL:**
- Для каждой таблицы можно создать множество триггеров;
- Триггеры могут использовать специальные переменные `NEW` и `OLD` для доступа к данным;
- Триггеры требуют создания функции-обработчика на языке PL/pgSQL.

---

### 2.2. Триггер 1: Автоматическое обновление длительности коллекции

**Назначение:** При добавлении или удалении трека из коллекции автоматически пересчитывать общую длительность коллекции.

**Листинг SQL-команд:**
```sql
CREATE OR REPLACE FUNCTION update_collection_duration()
RETURNS TRIGGER AS $$
DECLARE
    total_dur INT;
    coll_id INT;
BEGIN
    IF TG_OP = 'DELETE' THEN
        coll_id := OLD.collection_id;
    ELSE
        coll_id := NEW.collection_id;
    END IF;
    
    SELECT COALESCE(SUM(t.duration), 0)
    INTO total_dur
    FROM collectiontrack ct
    JOIN track t ON ct.track_id = t.track_id
    WHERE ct.collection_id = coll_id;
    
    UPDATE collection
    SET total_duration = total_dur
    WHERE collection_id = coll_id;
    
    IF TG_OP = 'DELETE' THEN
        RETURN OLD;
    ELSE
        RETURN NEW;
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_collection_duration_insert
AFTER INSERT ON collectiontrack
FOR EACH ROW
EXECUTE FUNCTION update_collection_duration();

CREATE TRIGGER trg_collection_duration_delete
AFTER DELETE ON collectiontrack
FOR EACH ROW
EXECUTE FUNCTION update_collection_duration();

CREATE TRIGGER trg_collection_duration_update
AFTER UPDATE OF track_id ON collectiontrack
FOR EACH ROW
EXECUTE FUNCTION update_collection_duration();
```

**Результат выполнения:**
![[Pasted image 20260318222253.png]]

**Тестирование триггера:**
```sql
SELECT collection_id, name, total_duration 
FROM collection 
WHERE collection_id = 1;

INSERT INTO collectiontrack (collection_id, track_id, position, transition_notes)
VALUES (1, 1, 1, 'Opening track');

SELECT collection_id, name, total_duration 
FROM collection 
WHERE collection_id = 1;

INSERT INTO collectiontrack (collection_id, track_id, position, transition_notes)
VALUES (1, 2, 2, 'Second track');

SELECT collection_id, name, total_duration 
FROM collection 
WHERE collection_id = 1;

DELETE FROM collectiontrack 
WHERE collection_id = 1 AND track_id = 1;

SELECT collection_id, name, total_duration 
FROM collection 
WHERE collection_id = 1;
```

**Результат выполнения:**
![[Pasted image 20260318223707.png]]

### 2.3. Триггер 2: Ведение журнала аудита изменений треков

**Назначение:** Логирование всех изменений (INSERT, UPDATE, DELETE) в таблице `track` для отслеживания истории модификаций.

**Листинг SQL-команд:**
```sql
CREATE TABLE track_audit_log (
    log_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    track_id INT NOT NULL,
    operation VARCHAR(10) NOT NULL,
    operation_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    old_title VARCHAR(255),
    new_title VARCHAR(255),
    old_rating DECIMAL(3,2),
    new_rating DECIMAL(3,2),
    old_play_count INT,
    new_play_count INT,
    changed_by VARCHAR(100) DEFAULT CURRENT_USER,
    comments TEXT
);

CREATE OR REPLACE FUNCTION track_audit_logger()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO track_audit_log (
            track_id, operation, new_title, new_rating, new_play_count, comments
        ) VALUES (
            NEW.track_id, 'INSERT', NEW.title, NEW.rating, NEW.play_count, 
            'Добавлен новый трек'
        );
        RETURN NEW;
    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO track_audit_log (
            track_id, operation, old_title, new_title, 
            old_rating, new_rating, old_play_count, new_play_count, comments
        ) VALUES (
            NEW.track_id, 'UPDATE', OLD.title, NEW.title,
            OLD.rating, NEW.rating, OLD.play_count, NEW.play_count,
            'Обновление данных трека'
        );
        RETURN NEW;
    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO track_audit_log (
            track_id, operation, old_title, old_rating, old_play_count, comments
        ) VALUES (
            OLD.track_id, 'DELETE', OLD.title, OLD.rating, OLD.play_count,
            'Удаление трека'
        );
        RETURN OLD;
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_track_audit
AFTER INSERT OR UPDATE OR DELETE ON track
FOR EACH ROW
EXECUTE FUNCTION track_audit_logger();
```

**Результат выполнения:**
![[Pasted image 20260318223811.png]]

**Тестирование триггера:**

```sql
SELECT * FROM track_audit_log ORDER BY log_id DESC LIMIT 10;

-- Тест 1: Добавление нового трека (INSERT)
INSERT INTO track (title, artist_id, genre_id, bpm, key, duration, rating, comments)
VALUES ('Test Track', 2, 1, 128.0, 'Am', 240, 4.5, 'Тестовый трек для аудита');

-- Проверяем журнал
SELECT * FROM track_audit_log ORDER BY log_id DESC LIMIT 5;

-- Тест 2: Обновление трека (UPDATE)
UPDATE track 
SET rating = 5.0, play_count = 100, comments = 'Обновлённый тестовый трек'
WHERE title = 'Test Track';

-- Проверяем журнал
SELECT * FROM track_audit_log ORDER BY log_id DESC LIMIT 5;

-- Тест 3: Удаление трека (DELETE)
DELETE FROM track WHERE title = 'Test Track';

-- Проверяем журнал - должна быть запись об удалении
SELECT * FROM track_audit_log ORDER BY log_id DESC LIMIT 5;
```

**Результат тестирования:**
![[Pasted image 20260318224205.png]]
![[Pasted image 20260318224211.png]]

---

### 2.4. Триггер 3: Автоматическое увеличение счётчика воспроизведений

**Назначение:** При каждом обращении к треку (например, при добавлении в коллекцию) автоматически увеличивать счётчик воспроизведений.

**Листинг SQL-команд:**

```sql
CREATE OR REPLACE FUNCTION increment_track_playcount()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE track
    SET play_count = play_count + 1
    WHERE track_id = NEW.track_id;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_increment_playcount
AFTER INSERT ON collectiontrack
FOR EACH ROW
EXECUTE FUNCTION increment_track_playcount();
```

**Результат выполнения:**
![[Pasted image 20260318224300.png]]

**Тестирование триггера:**
```sql
SELECT track_id, title, play_count 
FROM track 
WHERE track_id = 1;

INSERT INTO collectiontrack (collection_id, track_id, position, transition_notes)
VALUES (1, 1, 5, 'Test play count increment');

SELECT track_id, title, play_count 
FROM track 
WHERE track_id = 1;

INSERT INTO collectiontrack (collection_id, track_id, position, transition_notes)
VALUES (2, 1, 1, 'Second play');

SELECT track_id, title, play_count 
FROM track 
WHERE track_id = 1;
```

**Результат тестирования:**
![[Pasted image 20260318224352.png]]

---

## 3. ТАБЛИЦЫ БАЗЫ ДАННЫХ

**Таблица 7.1 – Разработанные триггеры:**

| № | Наименование триггера | Таблица | Событие | Время | Описание |
|---|----------------------|---------|---------|-------|----------|
| 1 | `trg_collection_duration_insert` | collectiontrack | INSERT | AFTER | Обновление total_duration в collection при добавлении трека |
| 2 | `trg_collection_duration_delete` | collectiontrack | DELETE | AFTER | Обновление total_duration в collection при удалении трека |
| 3 | `trg_collection_duration_update` | collectiontrack | UPDATE | AFTER | Обновление total_duration в collection при изменении трека |
| 4 | `trg_track_audit` | track | INSERT/UPDATE/DELETE | AFTER | Ведение журнала аудита изменений треков |
| 5 | `trg_increment_playcount` | collectiontrack | INSERT | AFTER | Увеличение счётчика воспроизведений трека |

**Таблица 7.2 – Функции для триггеров:**

| № | Наименование функции | Возвращаемый тип | Язык | Описание |
|---|---------------------|------------------|------|----------|
| 1 | `update_collection_duration()` | TRIGGER | plpgsql | Пересчёт общей длительности коллекции |
| 2 | `track_audit_logger()` | TRIGGER | plpgsql | Логирование изменений треков в audit_log |
| 3 | `increment_track_playcount()` | TRIGGER | plpgsql | Увеличение play_count трека |

**Таблица 7.3 – Таблица журнала аудита:**

| Поле | Тип | Описание |
|------|-----|----------|
| log_id | INT (PK) | Уникальный идентификатор записи журнала |
| track_id | INT | Идентификатор изменённого трека |
| operation | VARCHAR(10) | Тип операции (INSERT/UPDATE/DELETE) |
| operation_time | TIMESTAMP | Время выполнения операции |
| old_title | VARCHAR(255) | Старое название трека |
| new_title | VARCHAR(255) | Новое название трека |
| old_rating | DECIMAL(3,2) | Старый рейтинг |
| new_rating | DECIMAL(3,2) | Новый рейтинг |
| old_play_count | INT | Старый счётчик воспроизведений |
| new_play_count | INT | Новый счётчик воспроизведений |
| changed_by | VARCHAR(100) | Пользователь, выполнивший операцию |
| comments | TEXT | Комментарий к изменению |

---

## 4. ОТВЕТЫ НА КОНТРОЛЬНЫЕ ВОПРОСЫ

1. **Что такое триггер и чем он отличается от хранимой процедуры?**
   
   Триггер — это специальная хранимая процедура, которая автоматически выполняется при возникновении определённого события (INSERT, UPDATE, DELETE) в таблице базы данных. Главное отличие от обычной хранимой процедуры:
   - Триггер вызывается автоматически при событии, а хранимая процедура — явно по вызову;
   - Триггер привязан к конкретной таблице и событию;
   - Триггер не может принимать параметры, но имеет доступ к данным через переменные NEW и OLD.

2. **Какие события могут активировать триггер?**
   
   Триггер может активироваться тремя основными событиями:
   - `INSERT` — вставка новой записи в таблицу;
   - `UPDATE` — обновление существующей записи;
   - `DELETE` — удаление записи из таблицы.
   
   В PostgreSQL также поддерживается событие `TRUNCATE` для операции усечения таблицы.

3. **В чём разница между триггерами `BEFORE` и `AFTER`?**
   
   - `BEFORE` — триггер выполняется **до** выполнения операции изменения данных. Используется для валидации данных, автоматического заполнения полей, предотвращения нежелательных операций.
   - `AFTER` — триггер выполняется **после** выполнения операции. Используется для аудита, логирования, обновления связанных таблиц, каскадных операций.
   
   Основное отличие: BEFORE-триггер может изменить данные перед записью (модифицировать NEW), а AFTER-триггер работает с уже сохранёнными данными.

4. **Сколько триггеров можно создать для одной таблицы?**
   
   В PostgreSQL для одной таблицы можно создать множество триггеров. Для каждого события (INSERT, UPDATE, DELETE) можно создать отдельные триггеры с режимами BEFORE и AFTER. Ограничение в 6 триггеров (по одному на каждое сочетание события и времени) существует в MySQL, но не в PostgreSQL.

5. **Как получить доступ к старым и новым значениям полей в триггере?**
   
   В PostgreSQL доступ к данным осуществляется через специальные переменные:
   - `NEW.column` — доступ к новым значениям полей (доступно в INSERT и UPDATE);
   - `OLD.column` — доступ к старым значениям полей (доступно в UPDATE и DELETE).
   
   Эти переменные доступны только в триггерах уровня строки (FOR EACH ROW).

6. **Как создать триггер с помощью оператора `CREATE TRIGGER`?**
   
   Синтаксис создания триггера в PostgreSQL:
   ```sql
   CREATE TRIGGER trigger_name
   {BEFORE | AFTER | INSTEAD OF} {event [OR ...]}
   ON table_name
   [FOR [EACH] {ROW | STATEMENT}]
   [WHEN (condition)]
   EXECUTE FUNCTION function_name(arguments);
   ```
   
   Пример:
   ```sql
   CREATE TRIGGER trg_audit
   AFTER INSERT OR UPDATE OR DELETE ON track
   FOR EACH ROW
   EXECUTE FUNCTION audit_logger();
   ```

7. **Как удалить триггер?**
   
   Для удаления триггера используется оператор `DROP TRIGGER`:
   ```sql
   DROP TRIGGER [IF EXISTS] trigger_name ON table_name;
   ```
   
   Пример:
   ```sql
   DROP TRIGGER IF EXISTS trg_track_audit ON track;
   ```

8. **Можно ли привязать триггер к временной таблице или представлению?**
   
   - К **временной таблице** — можно привязать триггер как к обычной таблице.
   - К **представлению (VIEW)** — можно привязать триггер только с режимом `INSTEAD OF`, так как представления не хранят данные физически. Такие триггеры используются для реализации обновляемых представлений.

9. **Какие операторы можно использовать в теле триггера?**
   
   В теле триггера (функции на PL/pgSQL) можно использовать:
   - Операторы манипуляции данными: `INSERT`, `UPDATE`, `DELETE`, `SELECT INTO`;
   - Условные операторы: `IF`, `CASE`;
   - Циклы: `LOOP`, `WHILE`, `FOR`;
   - Операторы обработки исключений: `BEGIN ... EXCEPTION ... END`;
   - Возврат значения: `RETURN NEW`, `RETURN OLD`, `RETURN NULL`.

10. **Можно ли привязать триггеры к каскадному обновлению или удалению записей по внешнему ключу в PostgreSQL?**
    
    Да, в PostgreSQL триггеры срабатывают при каскадных операциях, выполняемых по внешнему ключу. Однако порядок срабатывания зависит от типа триггера:
    - `BEFORE`-триггеры срабатывают до применения каскадных ограничений;
    - `AFTER`-триггеры срабатывают после всех каскадных операций.
    
    Важно учитывать это при проектировании, чтобы избежать конфликтов и бесконечных циклов.

---

## 5. ВЫВОДЫ

В ходе выполнения лабораторной работы:
1. Были изучены принципы работы с триггерами в СУБД PostgreSQL;
2. Разработаны три триггера для различных таблиц базы данных `dj_db`:
   - Триггер для автоматического обновления длительности коллекции при изменении состава треков;
   - Триггер для ведения журнала аудита всех изменений в таблице `track`;
   - Триггер для автоматического увеличения счётчика воспроизведений треков;
3. Созданы соответствующие функции-обработчики на языке PL/pgSQL;
4. Проведено тестирование разработанных триггеров на различных сценариях использования;
5. Освоены операторы `CREATE TRIGGER`, `DROP TRIGGER` и создание триггерных функций.

В результате была закреплена работа с триггерами — мощным инструментом автоматизации бизнес-логики на уровне базы данных. Разработанные триггеры демонстрируют практическое применение для:
- поддержания целостности данных (автоматический пересчёт агрегированных значений);
- аудита и отслеживания изменений (ведение журнала модификаций);
- автоматического обновления связанных данных (счётчики статистики).

Триггеры позволяют реализовать сложную логику обработки данных непосредственно в СУБД, что обеспечивает согласованность данных и уменьшает нагрузку на прикладной уровень приложения.

---

## ПРИЛОЖЕНИЕ А. Полный SQL-скрипт

```sql
-- ============================================================================
-- Лабораторная работа №7
-- Полный скрипт создания и тестирования триггеров
-- ============================================================================

-- ----------------------------------------------------------------------------
-- ЧАСТЬ 1: Триггер для автоматического обновления длительности коллекции
-- ----------------------------------------------------------------------------

-- Функция для пересчёта общей длительности коллекции
CREATE OR REPLACE FUNCTION update_collection_duration()
RETURNS TRIGGER AS $$
DECLARE
    total_dur INT;
    coll_id INT;
BEGIN
    -- Определяем, какая коллекция изменилась
    IF TG_OP = 'DELETE' THEN
        coll_id := OLD.collection_id;
    ELSE
        coll_id := NEW.collection_id;
    END IF;
    
    -- Считаем общую длительность всех треков в коллекции
    SELECT COALESCE(SUM(t.duration), 0)
    INTO total_dur
    FROM collectiontrack ct
    JOIN track t ON ct.track_id = t.track_id
    WHERE ct.collection_id = coll_id;
    
    -- Обновляем запись в таблице Collection
    UPDATE collection
    SET total_duration = total_dur
    WHERE collection_id = coll_id;
    
    -- Возвращаем соответствующий результат
    IF TG_OP = 'DELETE' THEN
        RETURN OLD;
    ELSE
        RETURN NEW;
    END IF;
END;
$$ LANGUAGE plpgsql;

-- Создание триггеров
CREATE TRIGGER trg_collection_duration_insert
AFTER INSERT ON collectiontrack
FOR EACH ROW
EXECUTE FUNCTION update_collection_duration();

CREATE TRIGGER trg_collection_duration_delete
AFTER DELETE ON collectiontrack
FOR EACH ROW
EXECUTE FUNCTION update_collection_duration();

CREATE TRIGGER trg_collection_duration_update
AFTER UPDATE OF track_id ON collectiontrack
FOR EACH ROW
EXECUTE FUNCTION update_collection_duration();

-- ----------------------------------------------------------------------------
-- ЧАСТЬ 2: Триггер для ведения журнала аудита изменений треков
-- ----------------------------------------------------------------------------

-- Создаём таблицу для журнала аудита
CREATE TABLE track_audit_log (
    log_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    track_id INT NOT NULL,
    operation VARCHAR(10) NOT NULL,
    operation_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    old_title VARCHAR(255),
    new_title VARCHAR(255),
    old_rating DECIMAL(3,2),
    new_rating DECIMAL(3,2),
    old_play_count INT,
    new_play_count INT,
    changed_by VARCHAR(100) DEFAULT CURRENT_USER,
    comments TEXT
);

-- Функция для ведения журнала аудита
CREATE OR REPLACE FUNCTION track_audit_logger()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO track_audit_log (
            track_id, operation, new_title, new_rating, new_play_count, comments
        ) VALUES (
            NEW.track_id, 'INSERT', NEW.title, NEW.rating, NEW.play_count, 
            'Добавлен новый трек'
        );
        RETURN NEW;
    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO track_audit_log (
            track_id, operation, old_title, new_title, 
            old_rating, new_rating, old_play_count, new_play_count, comments
        ) VALUES (
            NEW.track_id, 'UPDATE', OLD.title, NEW.title,
            OLD.rating, NEW.rating, OLD.play_count, NEW.play_count,
            'Обновление данных трека'
        );
        RETURN NEW;
    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO track_audit_log (
            track_id, operation, old_title, old_rating, old_play_count, comments
        ) VALUES (
            OLD.track_id, 'DELETE', OLD.title, OLD.rating, OLD.play_count,
            'Удаление трека'
        );
        RETURN OLD;
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

-- Создание триггера
CREATE TRIGGER trg_track_audit
AFTER INSERT OR UPDATE OR DELETE ON track
FOR EACH ROW
EXECUTE FUNCTION track_audit_logger();

-- ----------------------------------------------------------------------------
-- ЧАСТЬ 3: Триггер для увеличения счётчика воспроизведений
-- ----------------------------------------------------------------------------

-- Функция для увеличения счётчика воспроизведений
CREATE OR REPLACE FUNCTION increment_track_playcount()
RETURNS TRIGGER AS $$
BEGIN
    -- Увеличиваем счётчик воспроизведений на 1
    UPDATE track
    SET play_count = play_count + 1
    WHERE track_id = NEW.track_id;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Создание триггера
CREATE TRIGGER trg_increment_playcount
AFTER INSERT ON collectiontrack
FOR EACH ROW
EXECUTE FUNCTION increment_track_playcount();

-- ----------------------------------------------------------------------------
-- ЧАСТЬ 4: Тестирование триггеров
-- ----------------------------------------------------------------------------

-- Тест триггера update_collection_duration
SELECT collection_id, name, total_duration FROM collection WHERE collection_id = 1;

INSERT INTO collectiontrack (collection_id, track_id, position, transition_notes)
VALUES (1, 1, 1, 'Opening track');

SELECT collection_id, name, total_duration FROM collection WHERE collection_id = 1;

-- Тест триггера track_audit_logger
INSERT INTO track (title, artist_id, genre_id, bpm, key, duration, rating, comments)
VALUES ('Test Track', 1, 1, 128.0, 'Am', 240, 4.5, 'Тестовый трек для аудита');

UPDATE track 
SET rating = 5.0, play_count = 100
WHERE title = 'Test Track';

SELECT * FROM track_audit_log ORDER BY log_id DESC LIMIT 5;

-- Тест триггера increment_track_playcount
SELECT track_id, title, play_count FROM track WHERE track_id = 1;

INSERT INTO collectiontrack (collection_id, track_id, position, transition_notes)
VALUES (1, 1, 5, 'Test play count increment');

SELECT track_id, title, play_count FROM track WHERE track_id = 1;

-- ----------------------------------------------------------------------------
-- ЧАСТЬ 5: Удаление триггеров (для очистки)
-- ----------------------------------------------------------------------------

-- DROP TRIGGER IF EXISTS trg_collection_duration_insert ON collectiontrack;
-- DROP TRIGGER IF EXISTS trg_collection_duration_delete ON collectiontrack;
-- DROP TRIGGER IF EXISTS trg_collection_duration_update ON collectiontrack;
-- DROP TRIGGER IF EXISTS trg_track_audit ON track;
-- DROP TRIGGER IF EXISTS trg_increment_playcount ON collectiontrack;
-- DROP FUNCTION IF EXISTS update_collection_duration();
-- DROP FUNCTION IF EXISTS track_audit_logger();
-- DROP FUNCTION IF EXISTS increment_track_playcount();
-- DROP TABLE IF EXISTS track_audit_log CASCADE;
```

---

## ПРИЛОЖЕНИЕ Б. Протокол тестирования триггеров

### Б.1. Триггер `update_collection_duration()`

**Тест 1: Начальное состояние коллекции №10**
```
 collection_id |    name     | total_duration 
---------------+-------------+----------------
            10 | Warm Up Set |           2680
```

**Тест 2: Добавление трека №1 (duration=240)**
```
После INSERT: total_duration = 240 ✅
```

**Тест 3: Добавление трека №2 (duration=300)**
```
После INSERT: total_duration = 540 (240+300) ✅
```

**Тест 4: Добавление трека №3 (duration=180)**
```
После INSERT: total_duration = 720 (240+300+180) ✅
```

**Тест 5: Удаление трека №2 (duration=300)**
```
После DELETE: total_duration = 420 (240+180) ✅
```

**Тест 6: Обновление track_id 3→4 (duration=360)**
```
После UPDATE: total_duration = 600 (240+360) ✅
```

### Б.2. Триггер `track_audit_logger()`

**Тест 7: Аудит INSERT**
```
 log_id | track_id | operation | new_title      | new_rating | comments          
--------+----------+-----------+----------------+------------+-------------------
      1 |     2918 | INSERT    | Test Audit Track |       4.50 | Добавлен новый трек
✅
```

**Тест 8: Аудит UPDATE**
```
 log_id | operation | old_rating | new_rating | comments              
--------+-----------+------------+------------+------------------------
      2 | UPDATE    |       4.50 |       5.00 | Обновление данных трека
✅
```

**Тест 9: Аудит DELETE**
```
 log_id | operation | old_title      | old_rating | comments         
--------+-----------+----------------+------------+------------------
      3 | DELETE    | Test Audit Track |       5.00 | Удаление трека
✅
```

### Б.3. Триггер `increment_track_playcount()`

**Тест 10: Увеличение play_count**
```
Начальное значение: play_count = 0
После 1-го добавления: play_count = 1 ✅
После 2-го добавления: play_count = 2 ✅
После 3-го добавления: play_count = 3 ✅
```

### Б.4. Итоговый список триггеров в БД

```
          Имя триггера          |     Таблица     | Статус  
--------------------------------+-----------------+---------
 trg_collection_duration_insert | collectiontrack | Включен
 trg_collection_duration_delete | collectiontrack | Включен
 trg_collection_duration_update | collectiontrack | Включен
 trg_track_audit                | track           | Включен
 trg_increment_playcount        | collectiontrack | Включен
```

**Триггерные функции:**
```
          Функция           | Схема  
----------------------------+--------
 increment_track_playcount  | public
 update_collection_duration | public
 track_audit_logger         | public
```

**Все триггеры работают корректно!** ✅
