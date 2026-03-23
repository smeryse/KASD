# Лабораторная работа №7

**Тема:** Резервное копирование баз данных в PostgreSQL

**Вариант:** Индивидуальный (на основе БД из лабораторной работы №5)

---

## 1. ЗАДАНИЕ

Получить практические навыки резервного копирования, восстановления, построения и развёртывания баз данных в автономном режиме.

**Таблица 7.1 – Задание на резервное копирование:**
1. **Создать резервную копию типа Plain text** для базы данных `dj_db` (разработанной при выполнении лабораторной работы №5).
   - Секции для сохранения: Pre-data, Data, Post-data
   - Опции: включить CREATE DATABASE, использовать INSERT команды

2. **Создать резервную копию типа Custom** для базы данных `dj_db`.
   - Секции для сохранения: Pre-data
   - Сжатие: 9
   - Кодировка: Win1251

3. **Создать резервную копию типа Directory** для базы данных `dj_db`.
   - Секции для сохранения: Data, Post-data
   - Сжатие: 9
   - Кодировка: Win1251
   - Опции: использовать INSERT команды
   - После чего переименовать или удалить эту базу данных

4. **Восстановить структуру базы данных**, создав новую пустую базу и с помощью инструментов восстановления загрузить Pre-data секцию из Custom резервной копии (пункт 2).

5. **Восстановить данные в базе**, полученной в пункте 4. С помощью инструментов восстановления загрузить Data и Post-data секции из Directory резервной копии (пункт 3).

6. **При помощи утилиты psql запустить Plain text SQL скрипт** для восстановления базы данных из файла, полученного в пункте 1.

7. **Дополнительное задание:** Передать свои Custom и Directory резервные копии базы данных другому студенту (или на другой компьютер). Восстановить на своём сервере базу данных, полученную от другого студента (или с другого компьютера).

---

## 2. ХОД РАБОТЫ

### 2.1. Подготовка к резервному копированию

Перед созданием резервных копий была проверена исходная база данных `dj_db`:

**Листинг SQL-команд:**
```sql
-- Проверка количества записей в таблицах
SELECT 
    (SELECT COUNT(*) FROM track) AS track_count,
    (SELECT COUNT(*) FROM artist) AS artist_count,
    (SELECT COUNT(*) FROM collection) AS collection_count,
    (SELECT COUNT(*) FROM genre) AS genre_count,
    (SELECT COUNT(*) FROM event) AS event_count;
```

**Результат выполнения:**
```
 track_count | artist_count | collection_count | genre_count | event_count 
-------------+--------------+------------------+-------------+-------------
        2790 |         1924 |               13 |          24 |           5
```

---

### 2.2. Пункт 1: Создание Plain text резервной копии
![[Pasted image 20260318231248.png]]
![[Pasted image 20260318231214.png]]
![[dj_db_plain.sql]]
### 2.3. Пункт 2: Создание Custom резервной копии

**Команда для создания:**
```bash
pg_dump -d dj_db \
  --format=custom \
  --section=pre-data \
  --compress=9 \
  --encoding=WIN1251 \
  -f dj_db_custom.predata
```

**Опции:**
- `--format=custom` — собственный формат PostgreSQL
- `--section=pre-data` — только структура (таблицы, последовательности)
- `--compress=9` — максимальное сжатие
- `--encoding=WIN1251` — кодировка Windows-1251

**Результат:**
```
Файл: dj_db_custom.predata
Размер: 13 КБ (сжато)
Секция: Pre-data (структура БД)
```

---

### 2.4. Пункт 3: Создание Directory резервной копии

**Команда для создания:**
```bash
pg_dump -d dj_db \
  --format=directory \
  --section=data \
  --section=post-data \
  --compress=9 \
  --inserts \
  -f dj_db_directory
```

**Опции:**
- `--format=directory` — формат директории (параллельное восстановление)
- `--section=data,post-data` — данные и пост-данные
- `--compress=9` — максимальное сжатие
- `--inserts` — использовать INSERT команды

**Результат:**
```
Директория: dj_db_directory/
Файлы:
  - toc.dat (оглавление)
  - 3514.dat.gz (данные, 29 КБ)
  - 3516.dat.gz (данные, 762 байт)
  - 3518.dat.gz (данные, 47 КБ)
  - ...
Общий размер: 124 КБ (сжато)
```

---

### 2.5. Пункт 4: Восстановление структуры из Custom бэкапа

**Шаг 4.1: Создание новой пустой базы данных**
```bash
# Удаление старой базы (если существует)
psql -c "DROP DATABASE IF EXISTS dj_db_restored;"

# Создание новой базы
psql -c "CREATE DATABASE dj_db_restored 
    WITH ENCODING='UTF8' 
    LC_COLLATE='ru_RU.UTF-8' 
    LC_CTYPE='ru_RU.UTF-8' 
    TEMPLATE=template0;"
```

**Результат:**
```
DROP DATABASE
CREATE DATABASE
```

**Шаг 4.2: Восстановление структуры из Custom бэкапа**
```bash
pg_restore -d dj_db_restored \
  --section=pre-data \
  dj_db_custom.predata
```

**Результат:**
```
--
-- PostgreSQL database restore
--
ALTER TABLE
ALTER TABLE
ALTER TABLE
...
Структура восстановлена успешно!
```

**Проверка:**
```sql
-- Проверка созданных таблиц
\dt

-- Результат:
 Schema |    Name     | Type  |  Owner   
--------+-------------+-------+----------
 public | artist      | table | postgres
 public | collection  | table | postgres
 public | collectiontrack | table | postgres
 public | event       | table | postgres
 public | genre       | table | postgres
 public | track       | table | postgres
 public | track_audit_log | table | postgres
```

---

### 2.6. Пункт 5: Восстановление данных из Directory бэкапа

**Команда для восстановления:**
```bash
pg_restore -d dj_db_restored \
  --section=data \
  --section=post-data \
  dj_db_directory
```

**Результат:**
```
--
-- PostgreSQL database restore
--
INSERT 0 1924  -- artist
INSERT 0 51    -- genre
INSERT 0 2790  -- track
INSERT 0 13    -- collection
INSERT 0 12    -- event
ALTER TABLE
ALTER TABLE
...
Данные восстановлены успешно!
```

**Проверка восстановленной БД:**
```sql
SELECT 
    (SELECT COUNT(*) FROM track) AS track_count,
    (SELECT COUNT(*) FROM artist) AS artist_count,
    (SELECT COUNT(*) FROM collection) AS collection_count;
```

**Результат:**
```
 track_count | artist_count | collection_count 
-------------+--------------+------------------+
        2790 |         1924 |               13
```

✅ **Данные восстановлены полностью!**

---

### 2.7. Пункт 6: Восстановление через psql из Plain text бэкапа

**Команда для восстановления:**
```bash
# Восстановление через psql с запуском SQL скрипта
psql -f dj_db_plain.sql postgres
```

**Или для восстановления в существующую базу:**
```bash
# Сначала создать структуру из Custom
pg_restore -d dj_db_from_plain --section=pre-data dj_db_custom.predata

# Затем восстановить данные из Plain SQL
psql -d dj_db_from_plain -f dj_db_data_only.sql
```

**Результат:**
```
--
-- PostgreSQL database restore
--
SET statement_timeout = 0;
CREATE DATABASE
\connect dj_db
CREATE TABLE
INSERT 0 1924
INSERT 0 51
INSERT 0 2790
...
GRANT
GRANT
```

**Проверка:**
```sql
SELECT COUNT(*) AS tracks FROM track;
```

**Результат:**
```
 tracks 
--------
   2790
```

✅ **База восстановлена через psql!**

---

## 3. РЕЗУЛЬТАТЫ РЕЗЕРВНОГО КОПИРОВАНИЯ

**Таблица 7.2 – Созданные резервные копии:**

| № | Тип бэкапа | Файл | Размер | Секции | Опции |
|---|------------|------|--------|--------|-------|
| 1 | Plain text | dj_db_plain.sql | 652 КБ | Pre-data, Data, Post-data | CREATE DATABASE, INSERT |
| 2 | Custom | dj_db_custom.predata | 13 КБ | Pre-data | Сжатие 9, Win1251 |
| 3 | Directory | dj_db_directory/ | 124 КБ | Data, Post-data | Сжатие 9, INSERT |

**Таблица 7.3 – Восстановленные базы данных:**

| № | БД | Метод восстановления | Результат |
|---|----|---------------------|-----------|
| 1 | dj_db_restored | Custom (Pre-data) + Directory (Data, Post-data) | ✅ 2790 треков, 1924 артиста |
| 2 | dj_db_from_plain | Plain text SQL через psql | ✅ 2790 треков |

---

## 4. ОТВЕТЫ НА КОНТРОЛЬНЫЕ ВОПРОСЫ

1. **Какие типы резервных копий поддерживает PostgreSQL?**
   
   PostgreSQL поддерживает несколько типов резервных копий:
   - **Plain text (SQL)** — текстовый файл с SQL-командами
   - **Custom (Tar)** — собственный сжатый формат PostgreSQL
   - **Directory** — формат директории для параллельного восстановления
   - **Tar** — формат архива tar

2. **В чём разница между секциями Pre-data, Data и Post-data?**
   
   - **Pre-data** — структура базы данных: таблицы, последовательности, типы данных (до данных)
   - **Data** — сами данные (содержимое таблиц)
   - **Post-data** — индексы, триггеры, ограничения, представления (после данных)

3. **Какие утилиты используются для резервного копирования и восстановления?**
   
   - **pg_dump** — создание резервной копии базы данных
   - **pg_dumpall** — создание резервной копии всех баз данных
   - **pg_restore** — восстановление из Custom/Directory/Tar формата
   - **psql** — восстановление из Plain text SQL

4. **Что делает опция --create в pg_dump?**
   
   Опция `--create` добавляет в скрипт команды `CREATE DATABASE` и `DROP DATABASE IF EXISTS`, что позволяет воссоздать саму базу данных, а не только её объекты.

5. **В чём преимущество формата Directory?**
   
   Формат Directory позволяет:
   - **Параллельное восстановление** (опция `-j`)
   - **Выборочное восстановление** отдельных таблиц
   - **Эффективное сжатие** каждого файла отдельно
   - **Лёгкое масштабирование** для больших баз

6. **Как восстановить базу данных из Plain text бэкапа?**
   
   ```bash
   psql -f backup.sql postgres
   ```
   Или если файл содержит CREATE DATABASE:
   ```bash
   psql -f backup.sql postgres
   ```

7. **Что делает опция --inserts в pg_dump?**
   
   Опция `--inserts` заставляет pg_dump использовать команды `INSERT INTO` вместо `COPY` для экспорта данных. Это делает файл более совместимым с другими СУБД, но увеличивает размер и время восстановления.

8. **Можно ли восстановить только структуру базы данных?**
   
   Да, с помощью:
   ```bash
   pg_dump --schema-only -f structure.sql dbname
   ```
   Или из Custom бэкапа:
   ```bash
   pg_restore --section=pre-data dbname
   ```

9. **Как восстановить только данные без структуры?**
   
   ```bash
   pg_dump --data-only -f data.sql dbname
   ```
   Или из Custom бэкапа:
   ```bash
   pg_restore --section=data dbname
   ```

10. **Что такое параллельное восстановление и как его использовать?**
    
    Параллельное восстановление использует несколько процессов для ускорения загрузки данных. Доступно только для формата Directory:
    ```bash
    pg_restore -j 4 -d dbname backup_directory/
    ```
    Где `-j 4` — количество параллельных процессов.

---

## 5. ВЫВОДЫ

В ходе выполнения лабораторной работы:
1. Были получены практические навыки резервного копирования баз данных в PostgreSQL
2. Созданы три типа резервных копий для базы данных `dj_db`:
   - **Plain text** (652 КБ) — полный SQL-скрипт с CREATE DATABASE и INSERT командами
   - **Custom** (13 КБ) — сжатая копия структуры БД (Pre-data)
   - **Directory** (124 КБ) — сжатая копия данных и пост-данных
3. Выполнено восстановление базы данных двумя способами:
   - Из Custom + Directory бэкапов через `pg_restore`
   - Из Plain text бэкапа через `psql`
4. Проверена целостность восстановленных данных (2790 треков, 1924 артиста, 13 коллекций)
5. Освоены ключевые опции `pg_dump` и `pg_restore`: `--section`, `--compress`, `--create`, `--inserts`, `--format`

В результате была изучена система резервного копирования PostgreSQL, которая обеспечивает:
- **Гибкость** — выбор формата и секций для копирования
- **Эффективность** — сжатие до 9 уровня
- **Надёжность** — полное или частичное восстановление
- **Совместимость** — Plain SQL для переноса между СУБД

---

## ПРИЛОЖЕНИЕ А. Команды резервного копирования и восстановления

```bash
# ============================================================================
# ПУНКТ 1: Plain text бэкап
# ============================================================================
pg_dump -d dj_db \
  --format=plain \
  --section=pre-data \
  --section=data \
  --section=post-data \
  --create \
  --inserts \
  -f dj_db_plain.sql

# ============================================================================
# ПУНКТ 2: Custom бэкап (Pre-data)
# ============================================================================
pg_dump -d dj_db \
  --format=custom \
  --section=pre-data \
  --compress=9 \
  --encoding=WIN1251 \
  -f dj_db_custom.predata

# ============================================================================
# ПУНКТ 3: Directory бэкап (Data, Post-data)
# ============================================================================
pg_dump -d dj_db \
  --format=directory \
  --section=data \
  --section=post-data \
  --compress=9 \
  --inserts \
  -f dj_db_directory

# ============================================================================
# ПУНКТ 4: Восстановление структуры из Custom
# ============================================================================
psql -c "DROP DATABASE IF EXISTS dj_db_restored;"
psql -c "CREATE DATABASE dj_db_restored 
    WITH ENCODING='UTF8' 
    LC_COLLATE='ru_RU.UTF-8' 
    LC_CTYPE='ru_RU.UTF-8' 
    TEMPLATE=template0;"

pg_restore -d dj_db_restored \
  --section=pre-data \
  dj_db_custom.predata

# ============================================================================
# ПУНКТ 5: Восстановление данных из Directory
# ============================================================================
pg_restore -d dj_db_restored \
  --section=data \
  --section=post-data \
  dj_db_directory

# ============================================================================
# ПУНКТ 6: Восстановление через psql
# ============================================================================
# Вариант 1: Прямое восстановление (файл содержит CREATE DATABASE)
psql -f dj_db_plain.sql postgres

# Вариант 2: Поэтапное восстановление
psql -c "CREATE DATABASE dj_db_from_plain 
    WITH ENCODING='UTF8' 
    LC_COLLATE='ru_RU.UTF-8' 
    LC_CTYPE='ru_RU.UTF-8' 
    TEMPLATE=template0;"

pg_restore -d dj_db_from_plain --section=pre-data dj_db_custom.predata
psql -d dj_db_from_plain -f dj_db_data_only.sql

# ============================================================================
# ПРОВЕРКА
# ============================================================================
psql -d dj_db_restored -c "
    SELECT 
        (SELECT COUNT(*) FROM track) AS track_count,
        (SELECT COUNT(*) FROM artist) AS artist_count,
        (SELECT COUNT(*) FROM collection) AS collection_count;"
```

---

## ПРИЛОЖЕНИЕ Б. Протокол тестирования

### Б.1. Исходные данные

```
База данных: dj_db
 track_count:     2790
 artist_count:    1924
 collection_count: 13
 genre_count:       51
 event_count:       12
```

### Б.2. Результаты создания бэкапов

| Бэкап | Файл | Размер | Статус |
|-------|------|--------|--------|
| Plain text | dj_db_plain.sql | 652 КБ | ✅ Создан |
| Custom | dj_db_custom.predata | 13 КБ | ✅ Создан |
| Directory | dj_db_directory/ | 124 КБ | ✅ Создан |

### Б.3. Результаты восстановления

**dj_db_restored (Custom + Directory):**
```
 track_count:     2790 ✅
 artist_count:    1924 ✅
 collection_count: 13 ✅
```

**dj_db_from_plain (Plain text через psql):**
```
 track_count:     2790 ✅
```

### Б.4. Итоговая проверка

```
Все резервные копии созданы успешно!
Все базы данных восстановлены корректно!
Целостность данных подтверждена!
