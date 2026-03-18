# Лабораторная работа №3: PostgreSQL (четный вариант — база Teachers)

## Подготовка

### 1. Установка PostgreSQL
```bash
sudo apt update
sudo apt install postgresql postgresql-contrib pgadmin4
```
![[Pasted image 20260301143741.png]]
### 2. Запуск и вход в psql
```bash
sudo -i -u postgres
psql
```
![[Pasted image 20260301143754.png]]
## Шаг 1: Создание базы данных Teachers

### Вариант A: Через psql
```sql
CREATE DATABASE "Teachers"
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'ru_RU.UTF-8'
    LC_CTYPE = 'ru_RU.UTF-8'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

ALTER DATABASE "Teachers" SET effective_cache_size = 131072;
```
![[Pasted image 20260301144255.png]]
## 🔗 Шаг 2: Подключение к базе Teachers
```bash
\c Teachers
```
![[Pasted image 20260301144316.png]]
## 🏗️ Шаг 3: Создание таблиц (DDL)
### Таблица 1: Faculties (Табл. 3.3)
```sql
CREATE TABLE Faculties (
    FacultyID int GENERATED ALWAYS AS IDENTITY UNIQUE NOT NULL,
    FacultyName text NOT NULL,
    Descript text NOT NULL
);
ALTER TABLE Faculties ADD CONSTRAINT PK_Faculties PRIMARY KEY (FacultyID);
CREATE UNIQUE INDEX IX_Faculties_ID ON Faculties(FacultyID);
```
![[Pasted image 20260301144615.png]]
![[Pasted image 20260301144628.png]]
![[Pasted image 20260301144642.png]]
### Таблица 2: SubFaculties (Табл. 3.4)
```sql
CREATE TABLE SubFaculties (
    SubFacultiesID int GENERATED ALWAYS AS IDENTITY UNIQUE NOT NULL,
    FacultyID int NOT NULL,
    ShortName char(10) NOT NULL,
    LongName char(75) NOT NULL,
    LetOut bit NOT NULL
);
ALTER TABLE SubFaculties ADD CONSTRAINT PK_SubFaculties PRIMARY KEY (SubFacultiesID);
```
![[Pasted image 20260301144701.png]]
![[Pasted image 20260301144712.png]]
### Таблица 3: Postes (Табл. 3.11)
```sql
CREATE TABLE Postes (
    PostesID int GENERATED ALWAYS AS IDENTITY UNIQUE NOT NULL,
    Post char(25) NOT NULL,
    Rate float8
);
ALTER TABLE Postes ADD CONSTRAINT PK_Postes PRIMARY KEY (PostesID);
```
![[Pasted image 20260301144739.png]]
![[Pasted image 20260301144751.png]]
### Таблица 4: Subjects (Табл. 3.12)
```sql
CREATE TABLE Subjects (
    SubjectID int GENERATED ALWAYS AS IDENTITY UNIQUE NOT NULL,
    SubjectName text NOT NULL,
    Descript text NOT NULL
);
ALTER TABLE Subjects ADD CONSTRAINT PK_Subjects PRIMARY KEY (SubjectID);
```
![[Pasted image 20260301144808.png]]![[Pasted image 20260301144824.png]]
### Таблица 5: Teach_Subj (Табл. 3.13)
```sql
CREATE TABLE Teach_Subj (
    Teach_SubjID int GENERATED ALWAYS AS IDENTITY UNIQUE NOT NULL,
    TeachersID int NOT NULL,
    SubjectID int NOT NULL
);
ALTER TABLE Teach_Subj ADD CONSTRAINT PK_Teach_Subj PRIMARY KEY (Teach_SubjID);
```
![[Pasted image 20260301144848.png]]![[Pasted image 20260301144901.png]]
### Таблица 6: Teachers (Табл. 3.10)

Эту таблицу создай **через pgAdmin** (по заданию):
1. В pgAdmin: открой свою БД Teachers → Databases (2) → Schemas (1) → Tables → Create → Table
2. Заполни поля согласно **Таблице 3.10**:

| Поле           | Тип  | Размер | NULL | Описание          |
| -------------- | ---- | ------ | ---- | ----------------- |
| TeachersID     | int  | 4      | нет  | PK, IDENTITY      |
| SubFacultiesID | int  | 4      | нет  | FK → SubFaculties |
| Surname        | text | -      | нет  | Фамилия           |
| Firstname      | text | -      | нет  | Имя               |
| Lastname       | text | -      | нет  | Отчество          |
| PostesID       | int  | 4      | нет  | FK → Postes       |
| Degree         | char | 20     | нет  | Учёная степень    |
| Birthday       | date | 4      | нет  | Дата рождения     |
| Birthplace     | text | -      | да   | Место рождения    |
| Phone          | text | -      | да   | Телефон           |
| Address        | text | -      | да   | Адрес             |
![[Pasted image 20260301151611.png]]
## 🔐 Шаг 5: Добавление ключей и связей (ALTER TABLE)

```sql
-- Связь subfaculties → faculties
ALTER TABLE subfaculties 
ADD CONSTRAINT fk_subfaculties_faculties 
FOREIGN KEY (facultyid) 
REFERENCES faculties(facultyid) 
ON UPDATE CASCADE;
-- Связь teachers → postes
ALTER TABLE teachers 
ADD CONSTRAINT fk_teachers_postes 
FOREIGN KEY (postesid) 
REFERENCES postes(postesid) 
ON UPDATE CASCADE;
-- Связь teachers → subfaculties
ALTER TABLE teachers 
ADD CONSTRAINT fk_teachers_subfaculties 
FOREIGN KEY (subfacultiesid) 
REFERENCES subfaculties(subfacultiesid) 
ON UPDATE CASCADE;
-- Связь teach_subj → teachers
ALTER TABLE teach_subj 
ADD CONSTRAINT fk_teach_subj_teachers 
FOREIGN KEY (teachersid) 
REFERENCES teachers(teachersid) 
ON UPDATE CASCADE ON DELETE CASCADE;
-- Связь teach_subj → subjects
ALTER TABLE teach_subj 
ADD CONSTRAINT fk_teach_subj_subjects 
FOREIGN KEY (subjectid) 
REFERENCES subjects(subjectid) 
ON UPDATE CASCADE ON DELETE CASCADE;
```
![[Pasted image 20260301160130.png]]
## 📊 Шаг 6: ER-диаграмма в pgAdmin

1. В pgAdmin: ПКМ на любой таблице → **ERD for Table**
2. Появится визуальная схема — сделай скриншот для отчёта
3. Должно получиться как на **Рис. 3.2** методички
![[Pasted image 20260301160107.png]]
---

## 🔍 Шаг 7: Индексы и ограничения (CHECK)

```sql
-- CHECK на дату рождения
ALTER TABLE teachers 
ADD CONSTRAINT chk_birthday 
CHECK (birthday <= CURRENT_DATE);

-- CHECK на ставку
ALTER TABLE postes 
ADD CONSTRAINT chk_rate_positive 
CHECK (rate >= 0);

-- Дополнительные CHECK с именами как в задании
ALTER TABLE teachers 
ADD CONSTRAINT "CHK_Birthday_Not_Future" 
CHECK (birthday <= CURRENT_DATE);

ALTER TABLE postes 
ADD CONSTRAINT "CHK_Rate_Positive" 
CHECK (rate >= 0);

-- Индекс на фамилию преподавателя
CREATE INDEX IX_Teachers_Surname ON Teachers(Surname);

-- Индекс на название предмета
CREATE INDEX IX_Subjects_Name ON Subjects(SubjectName);
```
![[Pasted image 20260301160626.png]]
---

## ➕ Шаг 8: Добавление данных (INSERT)

Начинай со справочников (без внешних зависимостей):

```sql
-- Faculties
INSERT INTO Faculties (FacultyName, Descript) VALUES
('ИТ', 'Факультет информационных технологий'),
('Экономики', 'Экономический факультет');

-- Postes
INSERT INTO Postes (Post, Rate) VALUES
('Доцент', 1.0),
('Профессор', 1.5),
('Ассистент', 0.75);

-- Subjects
INSERT INTO Subjects (SubjectName, Descript) VALUES
('БД', 'Базы данных'),
('Программирование', 'Основы программирования'),
('Алгоритмы', 'Структуры данных и алгоритмы');

-- SubFaculties
INSERT INTO SubFaculties (FacultyID, ShortName, LongName, LetOut) VALUES
(1, 'КИТ', 'Кафедра информационных технологий', B'1'),
(1, 'КПЗ', 'Кафедра программного обеспечения', B'0');

-- Teachers
INSERT INTO Teachers (SubFacultiesID, Surname, Firstname, Lastname, PostesID, Degree, Birthday) VALUES
(1, 'Иванов', 'Иван', 'Иванович', 2, 'д.т.н.', '1975-05-15'),
(2, 'Петрова', 'Анна', 'Сергеевна', 1, 'к.т.н.', '1982-11-30');

-- Teach_Subj
INSERT INTO Teach_Subj (TeachersID, SubjectID) VALUES
(1, 1),
(1, 3),
(2, 2);
```
![[Pasted image 20260301160743.png]]
## 🔎 Шаг 9: Проверка данных (SELECT)

```sql
-- Все таблицы
SELECT * FROM Faculties;
SELECT * FROM SubFaculties;
SELECT * FROM Postes;
SELECT * FROM Subjects;
SELECT * FROM Teachers;
SELECT * FROM Teach_Subj;

-- JOIN: преподаватели с кафедрами и должностями
SELECT 
    t.Surname,
    t.Firstname,
    sf.LongName AS Department,
    p.Post,
    t.Degree
FROM Teachers t
JOIN SubFaculties sf ON t.SubFacultiesID = sf.SubFacultiesID
JOIN Postes p ON t.PostesID = p.PostesID;
```
![[Pasted image 20260301160838.png]]
## 💡 Полезные команды psql

```bash
\l              -- список баз данных
\c Teachers     -- подключиться к БД
\dt             -- список таблиц
\d TableName    -- структура таблицы
\di             -- список индексов
\df             -- список функций
\q              -- выход из psql
```

