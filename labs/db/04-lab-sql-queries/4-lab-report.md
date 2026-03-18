---

---
## Тема: Формирование области запросов к БД в PostgreSQL
## Индивидуальное задание (Вариант 12)

**Таблица 4.1 – Задание на редактирование данных:**
1.  **Добавление:** Добавить в таблицу `SubFaculties` данные о новой кафедре Английского языка.
2.  **Изменение:** В таблице `SubFaculties` назначить новую кафедру другому факультету.
3.  **Удаление:** Из таблицы `Postes` удалить определенную должность.

**Таблица 4.2 – Задание на выборку данных:**
1.  **Выборка из одной таблицы:** Список преподавателей, упорядоченный по месту рождения.
2.  **Выборка из нескольких таблиц:** Список профессоров и докторов наук по выпускающим кафедрам.
3.  **Выборка с вычислениями:** Количество дисциплин, читаемых каждым преподавателем данной кафедры.

### Ход работы

#### Модификация данных (DML-команды)
*Выполнено в утилите SQL Shell (psql).*
**0. Подготовка**
Добавлен новый факультет в таблицу `faculties`:
```sql
INSERT INTO faculties (facultyname, descript)
VALUES ('КИЯ', 'Кафедра иностранных языков');
```
![[Pasted image 20260317213543.png]]

**1. Добавление данных (INSERT):**
Добавлена новая кафедра в таблицу `SubFaculties`.
```sql
INSERT INTO subfaculties (facultyid, shortname, longname, letout)
VALUES (3, 'КАЯ', 'Кафедра Английского языка', B'0');
```
![[Pasted image 20260317214234.png]]

**2. Изменение данных (UPDATE):**
Изменен факультет для newly added кафедры.
```sql
UPDATE subfaculties
SET FacultyID = 2
WHERE SubFacultyName = 'Кафедра Английского языка';
```
![[Pasted image 20260317214559.png]]

**3. Удаление данных (DELETE):**
Удалена должность из таблицы `Postes` (например, 'Лаборант').
```sql
DELETE FROM Postes
WHERE PostName = 'Лаборант';
```
![[Pasted image 20260317214813.png]]

---

#### Выборка данных (SELECT)

**Запрос 1. Список преподавателей, упорядоченный по месту рождения:**
```sql
SELECT surname, firstname, lastname, birthplace
FROM teachers
ORDER BY birthplace;
```
![[Pasted image 20260317231557.png]]

**Запрос 2. Список профессоров и докторов наук по выпускающим кафедрам:**
```sql
SELECT t.surname, t.firstname, t.lastname, t.degree, p.post, sf.longname
FROM teachers t
JOIN postes p ON t.postesid = p.postesid
JOIN subfaculties sf ON t.subfacultiesid = sf.subfacultiesid
WHERE sf.letout = B'1' AND (p.post LIKE '%Профессор%' OR t.degree LIKE '%доктор%')
ORDER BY sf.longname, t.surname;
```
![[Pasted image 20260317234349.png]]

**Запрос 3. Количество дисциплин, читаемых каждым преподавателем данной кафедры:**
```sql
SELECT t.surname, t.firstname, t.lastname, COUNT(ts.subjectid) as discipline_count
FROM teachers t
JOIN teach_subj ts ON t.teachersid = ts.teachersid
JOIN subfaculties sf ON t.subfacultiesid = sf.subfacultiesid
WHERE sf.longname = 'Кафедра информационных технологий'
GROUP BY t.surname, t.firstname, t.lastname
ORDER BY discipline_count DESC;
```
![[Pasted image 20260318000048.png]]

---

#### 3.3. Представления (Views)

**Задание 3. Создание представлений для запросов 1 и 2 через Конструктор (Designer):**
Представления созданы визуально через интерфейс pgAdmin.
![[Pasted image 20260318022101.png]]

**Задание 4. Создание представления для запроса 3 через SQL:**
```sql
CREATE VIEW v_teachers_disciplines_by_kafedra AS
SELECT t.surname, t.firstname, t.lastname,
COUNT(ts.subjectid) AS discipline_count
FROM teachers t
JOIN teach_subj ts ON t.teachersid = ts.teachersid
JOIN subfaculties sf ON t.subfacultiesid = sf.subfacultiesid
WHERE sf.longname = 'Кафедра информационных технологий'  
GROUP BY t.surname, t.firstname, t.lastname
ORDER BY discipline_count DESC; 
```
![[Pasted image 20260318022906.png]]

---

#### 3.4. Курсоры

**Задание 5. Создание курсора для запроса 1:**
Курсор объявлен для выборки преподавателей по месту рождения.
```sql
DO $$
DECLARE
teacher_cursor CURSOR FOR 
SELECT surname, firstname, lastname, birthplace
FROM teachers
ORDER BY birthplace;
    v_surname teachers.surname%TYPE;
    v_firstname teachers.firstname%TYPE;
    v_lastname teachers.lastname%TYPE;
    v_birthplace teachers.birthplace%TYPE;
BEGINYY
    OPEN teacher_cursor;
    LOOP
        FETCH NEXT FROM teacher_cursor 
        INTO v_surname, v_firstname, v_lastname, v_birthplace;
        EXIT WHEN NOT FOUND;
        RAISE NOTICE 'Преподаватель: % % %, место рождения: %', 
                     v_surname, v_firstname, v_lastname, v_birthplace;
    END LOOP;
    CLOSE teacher_cursor;
END $$;
```
![[Pasted image 20260318023900.png]]

---

#### 3.5. Хранимые процедуры

#### **Задание 6. Хранимая процедура, возвращающая значение из запроса 3:**
```sql
CREATE OR REPLACE PROCEDURE 
get_discipline_count_proc(p_kafedra_name TEXT)
LANGUAGE plpgsql
AS $$
BEGIN
    CREATE TEMP TABLE IF NOT EXISTS temp_discipline_result (
        surname TEXT,
        firstname TEXT,
        lastname TEXT,
        discipline_count BIGINT
    );
     
    DELETE FROM temp_discipline_result;
     
    INSERT INTO temp_discipline_result
    SELECT 
         t.surname, 
         t.firstname, 
         t.lastname, 
         COUNT(ts.subjectid) AS discipline_count
    FROM teachers t
    JOIN teach_subj ts ON t.teachersid = ts.teachersid
    JOIN subfaculties sf ON t.subfacultiesid = 
sf.subfacultiesid
    WHERE sf.longname = p_kafedra_name
    GROUP BY t.surname, t.firstname, t.lastname
    ORDER BY discipline_count DESC;

    RAISE NOTICE 'Результат сохранён во временную таблицу 
temp_discipline_result';
END;
$$;
```

Вызов процедуры
```sql
CALL get_discipline_count_proc('Кафедра информационных технологий');
SELECT * FROM temp_discipline_result;
$$;
```
![[Pasted image 20260318030756.png]]
#### **Задание 7. Хранимая процедура с параметром-курсором (через Конструктор):**
Процедура создана с использованием визуального конструктора pgAdmin.
```sql
CREATE OR REPLACE PROCEDURE show_teachers_by_birthplace()
LANGUAGE plpgsql
AS $$
DECLARE
   teacher_cursor CURSOR FOR 
      SELECT surname, firstname, lastname, birthplace
      FROM teachers
      ORDER BY birthplace;
   
   v_surname teachers.surname%TYPE;
   v_firstname teachers.firstname%TYPE;
   v_lastname teachers.lastname%TYPE;
   v_birthplace teachers.birthplace%TYPE;
BEGIN
   CREATE TABLE IF NOT EXISTS teachers_birthplace_result (
      surname TEXT,
      firstname TEXT,
      lastname TEXT,
      birthplace TEXT
   );
   
   DELETE FROM teachers_birthplace_result;
   
   OPEN teacher_cursor;
   
   LOOP
      FETCH NEXT FROM teacher_cursor 
      INTO v_surname, v_firstname, v_lastname, v_birthplace;
      EXIT WHEN NOT FOUND;
      
      INSERT INTO teachers_birthplace_result 
      VALUES (v_surname, v_firstname, v_lastname, v_birthplace);
   END LOOP;
   
   CLOSE teacher_cursor;
END $$;
```
```sql
CALL show_teachers_by_birthplace();
SELECT * FROM teachers_birthplace_result;
```
![[Pasted image 20260318032212.png]]

#### **Задание 8. Тестирование:**

Все объекты базы данных (представления, курсоры, процедуры) работают корректно, ошибки при выполнении отсутствуют.
![[Pasted image 20260318033214.png]]

---

### 4. Ответы на контрольные вопросы

1.  **Подмножество языка SQL-DML. Команды модификации данных:**
    DML (Data Manipulation Language) включает команды `INSERT` (добавление), `UPDATE` (обновление), `DELETE` (удаление). Они позволяют изменять содержимое таблиц без изменения структуры.
2.  **Команда SELECT и ее синтаксис. Сортировка данных:**
    `SELECT` используется для выборки данных. Сортировка осуществляется с помощью предложения `ORDER BY` (по возрастанию `ASC` или убыванию `DESC`).
3.  **Операции загрузки и обновление данных в PostgreSQL:**
    Загрузка через `INSERT INTO ... VALUES`, обновление через `UPDATE ... SET ... WHERE`. Важно соблюдать целостность данных (сначала родительские таблицы, потом дочерние).
4.  **Выборка из нескольких таблиц:**
    Осуществляется через операторы соединения `JOIN` (INNER, LEFT, RIGHT и т.д.) в предложении `FROM`.
5.  **Виды соединений:**
    Внутреннее (`INNER JOIN`), внешние (`LEFT/RIGHT JOIN`), полные (`FULL JOIN`). В данной работе использовалось `INNER JOIN`.
6.  **Предложение WHERE:**
    Используется для фильтрации строк. Поддерживает операторы сравнения, `LIKE`, `IN`, `BETWEEN`, логические `AND`, `OR`.
7.  **Группирование данных:**
    Выполняется через `GROUP BY`. Для вычислений используются агрегатные функции (`COUNT`, `SUM`, `AVG`, `MAX`, `MIN`).
8.  **Представления (Views):**
    Виртуальные таблицы, хранящие результат запроса. Упрощают доступ к данным и повышают безопасность. Создаются через `CREATE VIEW`.
9.  **Курсоры:**
    Объекты для построчной обработки результата запроса. Позволяют проходить по выборке циклически.
10. **Отличия представлений и курсоров:**
    View хранит запрос как объект БД для многократного использования как таблица. Курсор используется для временной обработки результата внутри транзакции/функции.
11. **Курсоры и хранимые процедуры:**
    Курсоры часто используются внутри процедур для обработки наборов данных.
12. **Хранимые процедуры:**
    Набор SQL-команд, хранящийся на сервере. Выполняются через `CALL`. Поддерживают логику (циклы, условия).
13. **Ограничения процедур:**
    Нельзя использовать в некоторых контекстах (например, внутри SELECT), требуют прав на выполнение. Модифицируются через `CREATE OR REPLACE`.

### 5. Вывод
В ходе выполнения лабораторной работы были освоены команды манипулирования данными (DML), изучены методы формирования сложных выборок с использованием соединений и группировки. Также были созданы и протестированы объекты базы данных PostgreSQL: представления, курсоры и хранимые процедуры. Навыки работы в pgAdmin и SQL Shell закреплены на практике.
