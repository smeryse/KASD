## 1. ЗАДАНИЕ

1.  При помощи инструментального средства pgAdmin создать базу данных преподавателей с именем **Teachers**.
    *   На вкладке Definition указать наборы символов и сопоставление на `Russian_Russia.1251` и кодировку `UTF8`.
    *   На вкладке Parameters указать `effective_cache_size` размером `131072`.
    *   SQL код создания БД скопировать в текстовый документ.
2.  Открыть SQL Shell (psql). Подключиться к базе данных postgres на локальном сервере от имени главного пользователя postgres. Составить команду `CREATE DATABASE` для создания новой БД преподавателей с именем **Teachers** на основе скопированного кода. Выполнить запрос.
3.  В pgAdmin открыть Query Tool для разработки структуры БД (Вариант: **Четный** — Teachers).
4.  В созданный бланк запросов включить SQL-команды `CREATE TABLE` для создания таблиц №1–4 и 6 соответствующей БД. Сохранить и выполнить запрос.
5.  При помощи конструктора создать в соответствующей БД таблицу №5 (**Teachers**).
6.  Перейти в SQL Shell (psql), подключиться к своей БД и выполнить SQL-команды `ALTER TABLE` для создания первичных и внешних ключей. Предусмотреть обеспечение целостности данных.
7.  Перейти в PGAdmin и сформировать ER-диаграммы БД.
8.  Создать разными способами несколько некластерных индексов и ограничений по условию на значение (check constraints).
9.  Разными способами посредством Query Tool и SQL Shell (psql) составить и выполнить SQL-команды `INSERT` для добавления данных в таблицы (по 2-3 записи в каждую).
10. Разными способами посредством Query Tool и SQL Shell (psql) составить и выполнить SQL-команды `SELECT` ко всем таблицам БД.

---

## 2. DDL-КОМАНДЫ (СОЗДАНИЕ СТРУКТУРЫ)

### 2.1. Создание базы данных Teachers

**Команда CREATE DATABASE (из pgAdmin или psql):**
```sql
-- Вставьте сюда команду создания БД
CREATE DATABASE "Teachers" ...;
```

**Результат выполнения (скриншот терминала):**
![[Pasted image 20260301144255.png]]
### 2.2. Создание таблиц 1-4 и 6 (Faculties, SubFaculties, Postes, Subjects, Teach_Subj)

**Листинг SQL-команд:**
```sql
-- Таблица 1: Faculties
CREATE TABLE Faculties (...);
ALTER TABLE ...;

-- Таблица 2: SubFaculties
CREATE TABLE SubFaculties (...);
ALTER TABLE ...;

-- Таблица 3: Postes
CREATE TABLE Postes (...);
ALTER TABLE ...;

-- Таблица 4: Subjects
CREATE TABLE Subjects (...);
ALTER TABLE ...;

-- Таблица 6: Teach_Subj
CREATE TABLE Teach_Subj (...);
ALTER TABLE ...;
```

**Результат выполнения:**
### Таблица 1: Faculties (Табл. 3.3)
![[Pasted image 20260301144615.png]]
![[Pasted image 20260301144628.png]]
![[Pasted image 20260301144642.png]]
### Таблица 2: SubFaculties (Табл. 3.4)
![[Pasted image 20260301144701.png]]
![[Pasted image 20260301144712.png]]
### Таблица 3: Postes (Табл. 3.11)
![[Pasted image 20260301144739.png]]
![[Pasted image 20260301144751.png]]
### Таблица 4: Subjects (Табл. 3.12)
![[Pasted image 20260301144808.png]]![[Pasted image 20260301144824.png]]
### Таблица 5: Teach_Subj (Табл. 3.13)
![[Pasted image 20260301144848.png]]![[Pasted image 20260301144901.png]]
### Таблица 6: Teachers (Табл. 3.10)

**Способ создания:** *(Конструктор pgAdmin / SQL-команда)*

**Результат выполнения:**
![[Pasted image 20260301151611.png]]
![[Pasted image 20260301161631.png]]

### 2.4. Создание связей (ALTER TABLE) и Индексов

**Листинг команд установки внешних ключей и индексов:**
```sql
-- Внешние ключи
ALTER TABLE SubFaculties ADD CONSTRAINT FK_SubFaculties_Faculties ...;
ALTER TABLE Teachers ADD CONSTRAINT FK_Teachers_SubFaculties ...;
-- ... остальные ключи

-- Индексы
CREATE INDEX I_SubFaculties_FacultyID ON SubFaculties(FacultyID);
-- ... остальные индексы
```

**Результат выполнения:**

![[Pasted image 20260301160130.png]]
## 3. ПЕРЕЧЕНЬ СОЗДАННЫХ ОГРАНИЧЕНИЙ

| №   | Наименование ограничения  | Тип ограничения | Таблица      | Поле           | Ссылка (для FK)              | Действия (ON UPDATE/DELETE) | Комментарий                            |
| --- | ------------------------- | --------------- | ------------ | -------------- | ---------------------------- | --------------------------- | -------------------------------------- |
| 1   | PK_Faculties              | PRIMARY KEY     | Faculties    | FacultyID      | —                            | —                           | Уникальный идентификатор факультета    |
| 2   | FK_SubFaculties_Faculties | FOREIGN KEY     | SubFaculties | FacultyID      | Faculties(FacultyID)         | **CASCADE / RESTRICT**      | Связь кафедры с факультетом            |
| 3   | PK_Teachers               | PRIMARY KEY     | Teachers     | TeachersID     | —                            | —                           | Уникальный идентификатор преподавателя |
| 4   | FK_Teachers_SubFaculties  | FOREIGN KEY     | Teachers     | SubFacultiesID | SubFaculties(SubFacultiesID) | **CASCADE / RESTRICT**      | Принадлежность к кафедре               |
| 5   | FK_Teachers_Postes        | FOREIGN KEY     | Teachers     | PostesID       | Postes(PostesID)             | **CASCADE / RESTRICT**      | Должность преподавателя                |
| 6   | CHK_Birthday_Not_Future   | CHECK           | Teachers     | Birthday       | —                            | —                           | Дата рождения не в будущем             |
| 7   | CHK_Rate_Positive         | CHECK           | Postes       | Rate           | —                            | —                           | Ставка не может быть отрицательной     |
| 8   | FK_Teach_Subj_Teachers    | FOREIGN KEY     | Teach_Subj   | TeachersID     | Teachers(TeachersID)         | **CASCADE / CASCADE**       | Связь с преподавателем                 |
| 9   | FK_Teach_Subj_Subjects    | FOREIGN KEY     | Teach_Subj   | SubjectID      | Subjects(SubjectID)          | **CASCADE / CASCADE**       | Связь с предметом                      |

---

## 4. DML-КОМАНДЫ (ДОБАВЛЕНИЕ ДАННЫХ)

### 4.1. Заполнение справочников (Faculties, Postes, Subjects)

**Листинг команд INSERT:**
```sql
INSERT INTO Faculties (FacultyName, Descript) VALUES ...;
INSERT INTO Postes (Post, Rate) VALUES ...;
INSERT INTO Subjects (SubjectName, Descript) VALUES ...;
```

**Результат выполнения (вывод данных):**
![[Pasted image 20260301164556.png]]

### 4.2. Заполнение зависимых таблиц (SubFaculties, Teachers, Teach_Subj)

**Листинг команд INSERT:**
```sql
INSERT INTO SubFaculties (FacultyID, ShortName, LongName, LetOut) VALUES ...;
INSERT INTO Teachers (SubFacultiesID, Surname, Firstname, Lastname, PostesID, Degree, Birthday, Birthplace, Phone, Address) VALUES ...;
INSERT INTO Teach_Subj (TeachersID, SubjectID) VALUES ...;
```

**Результат выполнения (вывод данных):**
![[Pasted image 20260301164622.png]]

---

## 5. ER-ДИАГРАММА БАЗЫ ДАННЫХ

**Инструмент формирования:** pgAdmin 4 (ERD Tool)

**Изображение диаграммы:**
![[Pasted image 20260301160107.png]]

---

## 6. SELECT-ЗАПРОСЫ (ВЫБОРКА ДАННЫХ)

### 6.1. Просмотр всех таблиц

**Листинг команд:**
```sql
-- Пример сложного запроса с объединением
SELECT t.surname, t.firstname, p.post, s.subjectname
FROM teachers t
JOIN postes p ON t.postesid = p.postesid
JOIN teach_subj ts ON t.teachersid = ts.teachersid
JOIN subjects s ON ts.subjectid = s.subjectid;
```

**Результаты выполнения:**
![[Pasted image 20260301164752.png]]

---

## 7. ВЫВОДЫ

В ходе выполнения лабораторной работы были освоены инструментальные средства PostgreSQL (pgAdmin, psql).  
Была создана база данных **Teachers** с соблюдением требований к кодировке и параметрам кеша.  
Разработана структура из 6 таблиц, установлены первичные и внешние ключи для обеспечения целостности данных.  
Созданы индексы для ускорения выборки и CHECK-ограничения для контроля вводимых данных.  
Произведено наполнение базы тестовыми данными и проверка корректности связей через SELECT-запросы.  
Сформирована ER-диаграмма, отражающая структуру базы данных.