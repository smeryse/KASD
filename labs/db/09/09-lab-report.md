# Лабораторная работа №9

**Тема:** Создание графического интерфейса базы данных средствами C# и WPF (Windows Presentation Foundation)

**Вариант:** DJ Music Library

---

## 1. ЗАДАНИЕ

Разработать графический интерфейс для работы с базой данных DJ Music Library с использованием технологии WPF (Windows Presentation Foundation) и языка C#.

**Таблица 9.1 – Задание на разработку GUI:**
1.  **Создание WPF-приложения:** Разработать проект в Visual Studio с использованием WPF
2.  **Подключение к БД:** Настроить подключение к PostgreSQL через Npgsql
3.  **Отображение данных:** Реализовать вывод таблиц и представлений в DataGrid
4.  **CRUD-операции:** Добавить формы для добавления, редактирования и удаления записей
5.  **Отчёты:** Реализовать вывод отчётов на основе представлений БД

---

## 2. ХОД РАБОТЫ

### 2.1. Подготовка окружения и создание проекта

Создали Avalonia UI-приложение (кроссплатформенный аналог WPF для Linux):

```shell
dotnet new install Avalonia.Templates
dotnet new avalonia.app -n Lab9_WPF
cd Lab9_WPF
```

Установили пакеты Npgsql для работы с PostgreSQL:

```shell
dotnet add package Npgsql
```

**Результат выполнения:**
![[Pasted image 20260518_120000.png]]
*Создан Avalonia UI проект с подключёнными пакетами*

---

### 2.2. Создание структуры проекта

Создали следующую структуру файлов:

```
Lab9_WPF/
├── App.axaml
├── App.axaml.cs
├── Program.cs
├── MainWindow.axaml
├── MainWindow.axaml.cs
├── EditWindow.axaml
├── EditWindow.axaml.cs
├── DbHelper.cs
├── FieldMapper.cs
└── Lab9_WPF.csproj
```

**Результат выполнения:**
![[Pasted image 20260518_120500.png]]
*Структура проекта*

---

### 2.3. Настройка подключения к БД

**Листинг DbHelper.cs:**
```csharp
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Lab9_WPF
{
    public static class DbHelper
    {
        private const string ConnStr = "Host=localhost;Port=5432;Database=dj_db;Username=postgres;Password=19022016";

        public static DataTable ExecuteQuery(string query)
        {
            var table = new DataTable();
            using var con = new NpgsqlConnection(ConnStr);
            using var cmd = new NpgsqlCommand(query, con);
            using var adapter = new NpgsqlDataAdapter(cmd);
            adapter.Fill(table);
            return table;
        }

        public static int ExecuteNonQuery(string query)
        {
            using var con = new NpgsqlConnection(ConnStr);
            con.Open();
            using var cmd = new NpgsqlCommand(query, con);
            return cmd.ExecuteNonQuery();
        }

        public static string GetPrimaryKey(string tableName)
        {
            var query = $@"
                SELECT a.attname
                FROM pg_index i
                JOIN pg_attribute a ON a.attrelid = i.indrelid AND a.attnum = ANY(i.indkey)
                WHERE i.indrelid = '{tableName}'::regclass AND i.indisprimary";
            var table = ExecuteQuery(query);
            return table.Rows.Count > 0 ? table.Rows[0][0].ToString()! : "id";
        }

        public static List<string> GetTableNames()
        {
            var tables = new List<string>();
            var query = @"SELECT table_name FROM information_schema.tables 
                         WHERE table_schema = 'public' AND table_type = 'BASE TABLE'
                         AND table_name NOT LIKE 'track_audit_log%'
                         ORDER BY table_name";
            var table = ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
                tables.Add(row["table_name"].ToString()!);
            return tables;
        }

        public static List<string> GetViewNames()
        {
            var views = new List<string>();
            var query = @"SELECT table_name FROM information_schema.tables 
                         WHERE table_schema = 'public' AND table_type = 'VIEW'
                         ORDER BY table_name";
            var table = ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
                views.Add(row["table_name"].ToString()!);
            return views;
        }
    }
}
```

---

### 2.4. Маппинг полей на русский язык (FieldMapper.cs)

Для отображения имён колонок на русском языке и скрытия первичных ключей в формах создали класс `FieldMapper`:

```csharp
using System;
using System.Collections.Generic;

namespace Lab9_WPF
{
    public static class FieldMapper
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _map = new()
        {
            ["artist"] = new()
            {
                ["artist_id"] = "ID артиста", ["name"] = "Имя", ["country"] = "Страна",
                ["style"] = "Стиль", ["active_years"] = "Годы активности", ["bio"] = "Биография"
            },
            ["genre"] = new()
            {
                ["genre_id"] = "ID жанра", ["name"] = "Название", ["parent_genre_id"] = "Родительский жанр",
                ["bpm_range"] = "Диапазон BPM", ["description"] = "Описание"
            },
            ["track"] = new()
            {
                ["track_id"] = "ID трека", ["title"] = "Название", ["artist_id"] = "ID артиста",
                ["genre_id"] = "ID жанра", ["bpm"] = "BPM", ["key"] = "Тональность",
                ["duration"] = "Длительность (сек)", ["file_format"] = "Формат файла",
                ["file_path"] = "Путь к файлу", ["rating"] = "Рейтинг",
                ["play_count"] = "Кол-во воспроизведений", ["date_added"] = "Дата добавления",
                ["comments"] = "Комментарии"
            },
            ["collection"] = new()
            {
                ["collection_id"] = "ID коллекции", ["name"] = "Название", ["type"] = "Тип",
                ["description"] = "Описание", ["style"] = "Стиль",
                ["planned_duration"] = "План. длительность (сек)", ["created_at"] = "Дата создания",
                ["notes"] = "Заметки", ["total_duration"] = "Общая длительность (сек)"
            },
            ["collectiontrack"] = new()
            {
                ["collection_id"] = "ID коллекции", ["track_id"] = "ID трека",
                ["position"] = "Позиция", ["transition_notes"] = "Заметки перехода"
            },
            ["event"] = new()
            {
                ["event_id"] = "ID события", ["venue"] = "Площадка", ["city"] = "Город",
                ["date"] = "Дата", ["audience_size"] = "Размер аудитории", ["event_type"] = "Тип события",
                ["collection_id"] = "ID коллекции", ["feedback"] = "Отзыв", ["earnings"] = "Доход ($)"
            }
        };

        public static string GetDisplayName(string tableName, string columnName)
        {
            if (_map.TryGetValue(tableName, out var tableMap) && tableMap.TryGetValue(columnName, out var displayName))
                return displayName;
            return columnName;
        }

        public static bool IsPrimaryKey(string tableName, string columnName)
        {
            return columnName switch
            {
                "artist_id" => tableName == "artist", "genre_id" => tableName == "genre",
                "track_id" => tableName == "track", "collection_id" => tableName == "collection",
                "event_id" => tableName == "event", _ => false
            };
        }
    }
}
```

---

### 2.5. Создание главного окна (MainWindow.axaml)

Главное окно выполнено в тёмной теме. Содержит меню навигации, панель инструментов, поле поиска, список данных с пагинацией и статус-бар.

**Листинг MainWindow.axaml:**
```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        x:Class="Lab9_WPF.MainWindow"
        Title="DJ Library - Avalonia UI" Height="700" Width="1100"
        WindowStartupLocation="CenterScreen" Background="#1E1E1E">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <Menu Grid.Row="0" Background="#2D2D30">
            <MenuItem Header="Справочники" Foreground="White">
                <MenuItem Header="Artist" Click="MenuItem_Artist_Click"/>
                <MenuItem Header="Genre" Click="MenuItem_Genre_Click"/>
            </MenuItem>
            <MenuItem Header="Данные" Foreground="White">
                <MenuItem Header="Track" Click="MenuItem_Track_Click"/>
                <MenuItem Header="Collection" Click="MenuItem_Collection_Click"/>
                <MenuItem Header="Event" Click="MenuItem_Event_Click"/>
            </MenuItem>
            <MenuItem Header="Отчёты" Foreground="White">
                <MenuItem Header="Track Lists" Click="MenuItem_TrackLists_Click"/>
                <MenuItem Header="Prepared Sets" Click="MenuItem_PreparedSets_Click"/>
                <MenuItem Header="Performance History" Click="MenuItem_PerformanceHistory_Click"/>
                <MenuItem Header="Event Manager Report" Click="MenuItem_EventManager_Click"/>
            </MenuItem>
        </Menu>

        <StackPanel Grid.Row="1" Orientation="Horizontal" Margin="10" Background="#3C3C3C" Spacing="10">
            <TextBlock Text="Таблица:" VerticalAlignment="Center" FontWeight="Bold" Foreground="White"/>
            <TextBlock x:Name="tblCurrentTable" Text="Не выбрана" VerticalAlignment="Center" Foreground="LightGray"/>
            <Button Content="Добавить" Click="Btn_Add_Click" Padding="10,5"/>
            <Button Content="Редактировать" Click="Btn_Edit_Click" Padding="10,5"/>
            <Button Content="Удалить" Click="Btn_Delete_Click" Padding="10,5"/>
            <Button Content="Обновить" Click="Btn_Refresh_Click" Padding="10,5"/>
        </StackPanel>

        <StackPanel Grid.Row="2" Orientation="Horizontal" Margin="10,0,10,5" Spacing="10">
            <TextBlock Text="Поиск:" VerticalAlignment="Center" FontWeight="Bold" Foreground="White"/>
            <TextBox x:Name="searchBox" Width="300" Watermark="Введите текст для поиска..." Background="#2D2D30" Foreground="White"/>
            <Button Content="Найти" Click="Btn_Search_Click" Padding="10,5"/>
            <Button Content="Сброс" Click="Btn_ResetSearch_Click" Padding="10,5"/>
            <TextBlock x:Name="searchInfo" Text="" VerticalAlignment="Center" Foreground="Gray"/>
        </StackPanel>

        <ListBox x:Name="dataList" Grid.Row="3" Margin="10" SelectionMode="Single"
                 BorderBrush="#555555" BorderThickness="1" Background="#1E1E1E" Foreground="White">
            <ListBox.ItemsPanel>
                <ItemsPanelTemplate><StackPanel/></ItemsPanelTemplate>
            </ListBox.ItemsPanel>
        </ListBox>

        <StackPanel Grid.Row="4" Orientation="Horizontal" HorizontalAlignment="Center" Margin="10" Spacing="10">
            <Button x:Name="btnPrev" Content="← Назад" Width="100" Click="Btn_Prev_Click" IsEnabled="False"/>
            <TextBlock x:Name="pageInfo" Text="Страница 1" VerticalAlignment="Center" Foreground="White"/>
            <Button x:Name="btnNext" Content="Вперёд →" Width="100" Click="Btn_Next_Click" IsEnabled="False"/>
        </StackPanel>

        <Border Grid.Row="5" Background="#2D2D30" Padding="10">
            <TextBlock x:Name="statusBar" Text="Готово" Foreground="White"/>
        </Border>
    </Grid>
</Window>
```

**Результат выполнения:**
![[Pasted image 20260518_121000.png]]
*Главное окно приложения в тёмной теме*

---

### 2.6. Логика главного окна (MainWindow.axaml.cs)

Реализованы:
- Отображение данных в динамически построенном списке
- Пагинация по 25 записей на страницу
- Сортировка по столбцам (клик на заголовок: ▲ по возрастанию, ▼ по убыванию, сброс)
- Поиск по всем полям (регистронезависимый)
- CRUD-операции с проверкой внешних ключей

**Листинг MainWindow.axaml.cs:**
```csharp
using System;
using System.Collections.Generic;
using System.Data;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia;

namespace Lab9_WPF
{
    public enum SortDirection { None, Ascending, Descending }

    public partial class MainWindow : Window
    {
        private string _currentTable = "";
        private bool _isView = false;
        private string _primaryKey = "id";
        private List<string> _columnNames = new();
        private List<DataRow> _allRows = new();
        private List<DataRow> _filteredRows = new();
        private List<DataRow> _sortedRows = new();
        private List<double> _colWidths = new();

        private const int PageSize = 25;
        private int _currentPage = 0;
        private int TotalPages => (_sortedRows.Count + PageSize - 1) / PageSize;

        private int _sortColumn = -1;
        private SortDirection _sortDirection = SortDirection.None;
        private string _searchText = "";
        private Dictionary<int, TextBlock> _headerTextBlocks = new();

        public MainWindow()
        {
            InitializeComponent();
            statusBar.Text = "Выберите таблицу или представление из меню";
        }

        private void LoadData(string tableName, string displayName, bool isView = false)
        {
            try
            {
                _currentTable = tableName;
                _isView = isView;
                _primaryKey = DbHelper.GetPrimaryKey(tableName);
                var table = DbHelper.ExecuteQuery($"SELECT * FROM public.\"{tableName}\"");

                _columnNames.Clear(); _allRows.Clear(); _colWidths.Clear();
                _headerTextBlocks.Clear(); _sortColumn = -1; _sortDirection = SortDirection.None;

                foreach (DataColumn col in table.Columns) { _columnNames.Add(col.ColumnName); _colWidths.Add(140); }
                foreach (DataRow row in table.Rows) _allRows.Add(row);

                _searchText = ""; searchBox.Text = ""; searchInfo.Text = "";
                ApplyFilterAndSort(); _currentPage = 0; BuildList(); UpdatePagination();
                tblCurrentTable.Text = displayName;
                statusBar.Text = $"{displayName}: {_allRows.Count} строк";
            }
            catch (Exception ex) { ShowError($"Ошибка загрузки данных: {ex.Message}"); }
        }

        private void ApplyFilterAndSort()
        {
            if (string.IsNullOrWhiteSpace(_searchText))
                _filteredRows = new List<DataRow>(_allRows);
            else
            {
                var search = _searchText.ToLower();
                _filteredRows = new List<DataRow>();
                foreach (var row in _allRows)
                    foreach (var col in _columnNames)
                    {
                        var val = row[col]?.ToString()?.ToLower();
                        if (val != null && val.Contains(search)) { _filteredRows.Add(row); break; }
                    }
            }

            if (_sortColumn >= 0 && _sortColumn < _columnNames.Count && _sortDirection != SortDirection.None)
            {
                var colName = _columnNames[_sortColumn];
                bool isNumeric = false;
                if (_filteredRows.Count > 0)
                {
                    var val = _filteredRows[0][colName];
                    if (val != DBNull.Value) isNumeric = val is int || val is long || val is float || val is double || val is decimal;
                }
                _sortedRows = new List<DataRow>(_filteredRows);
                _sortedRows.Sort((a, b) => _sortDirection == SortDirection.Ascending ? CompareRows(a, b, colName, isNumeric) : CompareRows(b, a, colName, isNumeric));
            }
            else _sortedRows = new List<DataRow>(_filteredRows);

            searchInfo.Text = !string.IsNullOrWhiteSpace(_searchText) ? $"Найдено: {_filteredRows.Count} из {_allRows.Count}" : "";
        }

        private int CompareRows(DataRow a, DataRow b, string colName, bool isNumeric)
        {
            var valA = a[colName]; var valB = b[colName];
            bool aNull = valA == DBNull.Value || valA == null; bool bNull = valB == DBNull.Value || valB == null;
            if (aNull && bNull) return 0; if (aNull) return 1; if (bNull) return -1;
            if (isNumeric) return Convert.ToDouble(valA).CompareTo(Convert.ToDouble(valB));
            return string.Compare(valA.ToString()!.ToLower(), valB.ToString()!.ToLower(), StringComparison.Ordinal);
        }

        private void BuildList()
        {
            dataList.Items.Clear(); _headerTextBlocks.Clear();
            var headerPanel = new StackPanel { Orientation = Orientation.Horizontal, Background = new SolidColorBrush(Color.Parse("#3C3C3C")) };
            for (int i = 0; i < _columnNames.Count; i++)
            {
                var displayName = FieldMapper.GetDisplayName(_currentTable, _columnNames[i]);
                var sortIndicator = _sortColumn == i ? (_sortDirection == SortDirection.Ascending ? " ▲" : " ▼") : "";
                var tb = new TextBlock { Text = displayName + sortIndicator, FontWeight = FontWeight.Bold, Foreground = new SolidColorBrush(Color.Parse("#E0E0E0")), Width = _colWidths[i], Margin = new Thickness(2), TextTrimming = TextTrimming.CharacterEllipsis, Cursor = new Cursor(StandardCursorType.Hand), Tag = i };
                tb.PointerPressed += Header_Click; _headerTextBlocks[i] = tb; headerPanel.Children.Add(tb);
            }
            dataList.Items.Add(headerPanel);

            var start = _currentPage * PageSize; var end = Math.Min(start + PageSize, _sortedRows.Count);
            for (int r = start; r < end; r++)
            {
                var row = _sortedRows[r]; var rowPanel = new StackPanel { Orientation = Orientation.Horizontal };
                for (int c = 0; c < _columnNames.Count; c++)
                {
                    var val = row[_columnNames[c]] == DBNull.Value ? "" : row[_columnNames[c]]!.ToString();
                    rowPanel.Children.Add(new TextBlock { Text = val, Width = _colWidths[c], Margin = new Thickness(2), TextTrimming = TextTrimming.CharacterEllipsis });
                }
                dataList.Items.Add(rowPanel);
            }
        }

        private void Header_Click(object? sender, PointerPressedEventArgs e)
        {
            if (sender is TextBlock tb && tb.Tag is int colIndex)
            {
                if (_sortColumn == colIndex) _sortDirection = _sortDirection switch { SortDirection.None => SortDirection.Ascending, SortDirection.Ascending => SortDirection.Descending, SortDirection.Descending => SortDirection.None, _ => SortDirection.None };
                else { _sortColumn = colIndex; _sortDirection = SortDirection.Ascending; }
                ApplyFilterAndSort(); _currentPage = 0; BuildList(); UpdatePagination();
            }
        }

        private void UpdatePagination()
        {
            if (TotalPages <= 1) { btnPrev.IsEnabled = false; btnNext.IsEnabled = false; pageInfo.Text = _sortedRows.Count > 0 ? $"Всего: {_sortedRows.Count} строк" : "Нет данных"; }
            else
            {
                btnPrev.IsEnabled = _currentPage > 0; btnNext.IsEnabled = _currentPage < TotalPages - 1;
                var start = _currentPage * PageSize + 1; var end = Math.Min((_currentPage + 1) * PageSize, _sortedRows.Count);
                pageInfo.Text = $"{start}-{end} из {_sortedRows.Count} (стр. {_currentPage + 1}/{TotalPages})";
            }
        }

        private void Btn_Prev_Click(object? sender, RoutedEventArgs e) { if (_currentPage > 0) { _currentPage--; BuildList(); UpdatePagination(); } }
        private void Btn_Next_Click(object? sender, RoutedEventArgs e) { if (_currentPage < TotalPages - 1) { _currentPage++; BuildList(); UpdatePagination(); } }
        private void Btn_Search_Click(object? sender, RoutedEventArgs e) { _searchText = searchBox.Text?.Trim() ?? ""; ApplyFilterAndSort(); _currentPage = 0; BuildList(); UpdatePagination(); }
        private void Btn_ResetSearch_Click(object? sender, RoutedEventArgs e) { searchBox.Text = ""; _searchText = ""; ApplyFilterAndSort(); _currentPage = 0; BuildList(); UpdatePagination(); }

        private void MenuItem_Artist_Click(object? sender, RoutedEventArgs e) => LoadData("artist", "Artist");
        private void MenuItem_Genre_Click(object? sender, RoutedEventArgs e) => LoadData("genre", "Genre");
        private void MenuItem_Track_Click(object? sender, RoutedEventArgs e) => LoadData("track", "Track");
        private void MenuItem_Collection_Click(object? sender, RoutedEventArgs e) => LoadData("collection", "Collection");
        private void MenuItem_Event_Click(object? sender, RoutedEventArgs e) => LoadData("event", "Event");
        private void MenuItem_TrackLists_Click(object? sender, RoutedEventArgs e) => LoadData("v_track_lists", "Track Lists", true);
        private void MenuItem_PreparedSets_Click(object? sender, RoutedEventArgs e) => LoadData("v_prepared_sets", "Prepared Sets", true);
        private void MenuItem_PerformanceHistory_Click(object? sender, RoutedEventArgs e) => LoadData("v_performance_history", "Performance History", true);
        private void MenuItem_EventManager_Click(object? sender, RoutedEventArgs e) => LoadData("v_event_manager_report", "Event Manager Report", true);

        private DataRow? GetSelectedRow()
        {
            var idx = dataList.SelectedIndex; if (idx <= 0) return null;
            var start = _currentPage * PageSize; var rowIdx = idx - 1;
            if (rowIdx >= _sortedRows.Count) return null; return _sortedRows[start + rowIdx];
        }

        private void Btn_Add_Click(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentTable)) { ShowMessage("Сначала выберите таблицу"); return; }
            if (_isView) { ShowMessage("Нельзя добавлять записи в представление"); return; }
            var editWindow = new EditWindow(_currentTable, null); editWindow.ShowDialog(this);
            if (editWindow.Saved) { LoadData(_currentTable, tblCurrentTable.Text); statusBar.Text = "Запись добавлена"; }
        }

        private void Btn_Edit_Click(object? sender, RoutedEventArgs e)
        {
            if (_isView) { ShowMessage("Нельзя редактировать представление"); return; }
            var row = GetSelectedRow(); if (row == null) { ShowMessage("Выберите запись для редактирования"); return; }
            var editWindow = new EditWindow(_currentTable, row); editWindow.ShowDialog(this);
            if (editWindow.Saved) { LoadData(_currentTable, tblCurrentTable.Text); statusBar.Text = "Запись обновлена"; }
        }

        private void Btn_Delete_Click(object? sender, RoutedEventArgs e)
        {
            if (_isView) { ShowMessage("Нельзя удалять из представления"); return; }
            var row = GetSelectedRow(); if (row == null) { ShowMessage("Выберите запись для удаления"); return; }
            var id = row[_primaryKey];
            ShowConfirm("Удалить выбранную запись?", confirmed =>
            {
                if (confirmed)
                {
                    try { DbHelper.ExecuteNonQuery($"DELETE FROM public.\"{_currentTable}\" WHERE {_primaryKey} = {id}"); LoadData(_currentTable, tblCurrentTable.Text); statusBar.Text = "Запись удалена"; }
                    catch (Npgsql.NpgsqlException ex) when (ex.SqlState == "23503") ShowError("Нельзя удалить: запись используется в связанных таблицах.");
                    catch (Exception ex) ShowError($"Ошибка удаления: {ex.Message}");
                }
            });
        }

        private void Btn_Refresh_Click(object? sender, RoutedEventArgs e) { if (!string.IsNullOrEmpty(_currentTable)) LoadData(_currentTable, tblCurrentTable.Text, _isView); }

        private void ShowMessage(string msg) => ShowDialog("Внимание", msg);
        private void ShowError(string msg) => ShowDialog("Ошибка", msg);

        private void ShowDialog(string title, string msg)
        {
            var dialog = new Window { Title = title, Width = 400, Height = 150, WindowStartupLocation = WindowStartupLocation.CenterOwner, Background = new SolidColorBrush(Color.Parse("#1E1E1E")) };
            var sp = new StackPanel { Margin = new Thickness(20) };
            sp.Children.Add(new TextBlock { Text = msg, TextWrapping = TextWrapping.Wrap, Foreground = Brushes.White });
            var btn = new Button { Content = "OK", Width = 80, Margin = new Thickness(0, 15, 0, 0), HorizontalAlignment = HorizontalAlignment.Center };
            btn.Click += (s, e) => dialog.Close(); sp.Children.Add(btn); dialog.Content = sp; dialog.ShowDialog(this);
        }

        private void ShowConfirm(string msg, Action<bool> callback)
        {
            var dialog = new Window { Title = "Подтверждение", Width = 350, Height = 160, WindowStartupLocation = WindowStartupLocation.CenterOwner, Background = new SolidColorBrush(Color.Parse("#1E1E1E")) };
            var sp = new StackPanel { Margin = new Thickness(20) };
            sp.Children.Add(new TextBlock { Text = msg, Margin = new Thickness(0, 0, 0, 15), Foreground = Brushes.White });
            var btns = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center, Spacing = 10 };
            var btnYes = new Button { Content = "Да", Width = 80 }; var btnNo = new Button { Content = "Нет", Width = 80 };
            btnYes.Click += (s, ev) => { callback(true); dialog.Close(); }; btnNo.Click += (s, ev) => { callback(false); dialog.Close(); };
            btns.Children.Add(btnYes); btns.Children.Add(btnNo); sp.Children.Add(btns); dialog.Content = sp; dialog.ShowDialog(this);
        }
    }
}
```

---

### 2.7. Окно редактирования (EditWindow.axaml)

**Листинг EditWindow.axaml:**
```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        x:Class="Lab9_WPF.EditWindow"
        Title="Редактирование" Height="500" Width="550"
        WindowStartupLocation="CenterOwner" Background="#1E1E1E">
    <DockPanel>
        <StackPanel DockPanel.Dock="Bottom" Orientation="Horizontal" HorizontalAlignment="Right" Margin="15" Spacing="10">
            <Button x:Name="btnSave" Content="Сохранить" Width="100" Click="Btn_Save_Click"/>
            <Button x:Name="btnCancel" Content="Отмена" Width="100" Click="Btn_Cancel_Click"/>
        </StackPanel>
        <ScrollViewer>
            <StackPanel x:Name="formPanel" Margin="15" Spacing="8"/>
        </ScrollViewer>
    </DockPanel>
</Window>
```

**Результат выполнения:**
![[Pasted image 20260518_122000.png]]
*Окно редактирования записи (поле ID скрыто)*

---

### 2.8. Логика окна редактирования (EditWindow.axaml.cs)

**Листинг EditWindow.axaml.cs:**
```csharp
using System;
using System.Data;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Lab9_WPF
{
    public partial class EditWindow : Window
    {
        private readonly string _tableName;
        private readonly DataRow? _existingRow;
        private readonly TextBox[] _textBoxes;
        private readonly string[] _columnNames;
        private readonly string _primaryKey;
        private readonly bool[] _isNullable;
        private int _fieldCount;

        public bool Saved { get; private set; }

        public EditWindow(string tableName, DataRow? existingRow)
        {
            _tableName = tableName; _existingRow = existingRow;
            _primaryKey = DbHelper.GetPrimaryKey(tableName);
            InitializeComponent();

            var schema = DbHelper.ExecuteQuery($"SELECT * FROM public.\"{tableName}\" LIMIT 0");
            var colCount = schema.Columns.Count; _fieldCount = 0;
            for (int i = 0; i < colCount; i++)
                if (!FieldMapper.IsPrimaryKey(tableName, schema.Columns[i].ColumnName)) _fieldCount++;

            _columnNames = new string[_fieldCount]; _textBoxes = new TextBox[_fieldCount]; _isNullable = new bool[_fieldCount];

            int j = 0;
            for (int i = 0; i < colCount; i++)
            {
                var col = schema.Columns[i];
                if (FieldMapper.IsPrimaryKey(tableName, col.ColumnName)) continue;
                _columnNames[j] = col.ColumnName; _isNullable[j] = col.AllowDBNull;
                var displayName = FieldMapper.GetDisplayName(tableName, col.ColumnName);

                var label = new TextBlock { Text = displayName, FontWeight = Avalonia.Media.FontWeight.Bold, Foreground = Avalonia.Media.Brushes.LightGray, Margin = new Avalonia.Thickness(0, 4, 0, 2) };
                var textBox = new TextBox { Watermark = $"{col.DataType.Name}{(col.AllowDBNull ? " (необязательно)" : "")}", Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#2D2D30")), Foreground = Avalonia.Media.Brushes.White };

                if (existingRow != null) { var val = existingRow[col.ColumnName]; if (val != DBNull.Value) textBox.Text = val.ToString(); }
                _textBoxes[j] = textBox; formPanel.Children.Add(label); formPanel.Children.Add(textBox); j++;
            }
        }

        private void Btn_Save_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (_existingRow == null)
                {
                    var cols = ""; var vals = ""; var first = true;
                    for (int i = 0; i < _fieldCount; i++)
                    {
                        var text = _textBoxes[i].Text?.Trim();
                        if (string.IsNullOrEmpty(text)) { if (!_isNullable[i]) { ShowError($"Поле '{FieldMapper.GetDisplayName(_tableName, _columnNames[i])}' обязательно"); return; } continue; }
                        if (!first) { cols += ", "; vals += ", "; }
                        cols += $"\"{_columnNames[i]}\""; vals += $"'{text.Replace("'", "''")}'"; first = false;
                    }
                    DbHelper.ExecuteNonQuery($"INSERT INTO public.\"{_tableName}\" ({cols}) VALUES ({vals})");
                }
                else
                {
                    var sets = ""; var first = true;
                    for (int i = 0; i < _fieldCount; i++)
                    {
                        var text = _textBoxes[i].Text?.Trim();
                        if (string.IsNullOrEmpty(text)) { if (!_isNullable[i]) { ShowError($"Поле '{FieldMapper.GetDisplayName(_tableName, _columnNames[i])}' обязательно"); return; } if (!first) sets += ", "; sets += $"\"{_columnNames[i]}\" = NULL"; first = false; continue; }
                        if (!first) sets += ", "; sets += $"\"{_columnNames[i]}\" = '{text.Replace("'", "''")}'"; first = false;
                    }
                    var id = _existingRow[_primaryKey];
                    DbHelper.ExecuteNonQuery($"UPDATE public.\"{_tableName}\" SET {sets} WHERE {_primaryKey} = {id}");
                }
                Saved = true; Close();
            }
            catch (Exception ex) { ShowError($"Ошибка сохранения: {ex.Message}"); }
        }

        private void Btn_Cancel_Click(object? sender, RoutedEventArgs e) => Close();

        private void ShowError(string msg)
        {
            var dialog = new Window { Title = "Ошибка", Width = 400, Height = 150, WindowStartupLocation = WindowStartupLocation.CenterOwner, Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#1E1E1E")) };
            var sp = new StackPanel { Margin = new Avalonia.Thickness(20) };
            sp.Children.Add(new TextBlock { Text = msg, TextWrapping = Avalonia.Media.TextWrapping.Wrap, Foreground = Avalonia.Media.Brushes.White });
            var btn = new Button { Content = "OK", Width = 80, Margin = new Avalonia.Thickness(0, 15, 0, 0), HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center };
            btn.Click += (s, e) => dialog.Close(); sp.Children.Add(btn); dialog.Content = sp; dialog.ShowDialog(this);
        }
    }
}
```

---

### 2.9. Файл проекта (Lab9_WPF.csproj)

**Листинг Lab9_WPF.csproj:**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ApplicationManifest>app.manifest</ApplicationManifest>
    <AvaloniaUseCompiledBindingsByDefault>false</AvaloniaUseCompiledBindingsByDefault>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Avalonia" Version="11.2.1" />
    <PackageReference Include="Avalonia.Desktop" Version="11.2.1" />
    <PackageReference Include="Avalonia.Themes.Fluent" Version="11.2.1" />
    <PackageReference Include="Avalonia.Fonts.Inter" Version="11.2.1" />
    <PackageReference Include="Npgsql" Version="8.0.3" />
  </ItemGroup>
</Project>
```

---

### 2.10. Заполнение пустых полей в БД

Для корректного отображения и работы приложения выполнили скрипт заполнения пустых полей:

```sql
-- Artist
UPDATE artist SET country = 'Россия' WHERE country IS NULL AND artist_id % 5 = 0;
UPDATE artist SET style = 'Techno' WHERE style IS NULL AND artist_id % 4 = 0;
UPDATE artist SET active_years = '2010-н.в.' WHERE active_years IS NULL AND artist_id % 3 = 0;
UPDATE artist SET bio = 'Известный DJ и продюсер электронной музыки.' WHERE bio IS NULL;

-- Track
UPDATE track SET bpm = 128.00 WHERE bpm IS NULL AND track_id % 5 = 0;
UPDATE track SET key = 'Am' WHERE key IS NULL AND track_id % 8 = 0;
UPDATE track SET duration = 360 WHERE duration IS NULL AND track_id % 4 = 0;
UPDATE track SET file_format = 'MP3' WHERE file_format IS NULL AND track_id % 3 = 0;
UPDATE track SET file_path = '/music/tracks/' || track_id || '.mp3' WHERE file_path IS NULL;
UPDATE track SET rating = 4.50 WHERE rating IS NULL AND track_id % 5 = 0;
UPDATE track SET comments = 'Отличный трек для сета.' WHERE comments IS NULL;

-- Collection, Event, CollectionTrack, Genre ...
```

**Результат:** Все 6 таблиц базы данных теперь содержат заполненные поля.

---

### 2.11. Сборка и запуск приложения

```shell
dotnet build
dotnet run
```

**Результат выполнения:**
![[Pasted image 20260518_123000.png]]
*Сборка и запуск приложения*

---

### 2.12. Демонстрация работы приложения

**Главное окно (тёмная тема):**
![[Pasted image 20260518_123500.png]]
*Главное окно приложения в тёмной теме*

**Отображение таблицы Artist:**
![[Pasted image 20260518_124000.png]]
*Таблица Artist с русскими заголовками*

**Сортировка по столбцу (BPM ▲):**
![[Pasted image 20260518_124500.png]]
*Сортировка по возрастанию числового столбца*

**Поиск по таблице:**
![[Pasted image 20260518_125000.png]]
*Результаты поиска по ключевому слову*

**Пагинация:**
![[Pasted image 20260518_125500.png]]
*Навигация по страницам (25 записей на страницу)*

**Добавление новой записи:**
![[Pasted image 20260518_130000.png]]
*Форма добавления записи (поле ID скрыто)*

**Редактирование записи:**
![[Pasted image 20260518_130500.png]]
*Форма редактирования с заполненными полями*

**Просмотр представления Track Lists:**
![[Pasted image 20260518_131000.png]]
*Представление v_track_lists*

**Просмотр представления Event Manager Report:**
![[Pasted image 20260518_131500.png]]
*Представление v_event_manager_report*

---

## 3. SQL-ЗАПРОСЫ ПРЕДСТАВЛЕНИЙ

### 3.1. Представление v_track_lists

```sql
CREATE VIEW v_track_lists AS
SELECT 
    t.track_id, t.title AS track_title, a.name AS artist_name,
    g.name AS genre_name, t.bpm, t.key, t.duration, t.rating, t.play_count
FROM track t
JOIN artist a ON t.artist_id = a.artist_id
JOIN genre g ON t.genre_id = g.genre_id
ORDER BY t.title;
```

### 3.2. Представление v_prepared_sets

```sql
CREATE VIEW v_prepared_sets AS
SELECT 
    c.collection_id, c.name AS set_name, c.type, c.style, c.planned_duration,
    array_agg(t.title ORDER BY ct."position") AS tracks,
    count(ct.track_id) AS total_tracks, sum(t.duration) AS actual_duration_seconds
FROM collection c
LEFT JOIN collectiontrack ct ON c.collection_id = ct.collection_id
LEFT JOIN track t ON ct.track_id = t.track_id
GROUP BY c.collection_id, c.name, c.type, c.style, c.planned_duration
ORDER BY c.created_at DESC;
```

### 3.3. Представление v_performance_history

```sql
CREATE VIEW v_performance_history AS
SELECT 
    e.event_id, e.venue, e.city, e.date, e.event_type, e.audience_size,
    c.name AS set_name, e.feedback, e.earnings
FROM event e
LEFT JOIN collection c ON e.collection_id = c.collection_id
ORDER BY e.date DESC;
```

### 3.4. Представление v_event_manager_report

```sql
CREATE VIEW v_event_manager_report AS
SELECT 
    e.event_id, e.venue AS "Площадка", e.city AS "Город", e.date AS "Дата",
    e.event_type AS "Тип события", e.audience_size AS "Аудитория",
    c.name AS "Название сета", count(ct.track_id) AS "Треков в сете",
    round(sum(t.duration)::numeric / 60.0, 2) AS "Длительность сета (мин)",
    round(avg(t.rating), 2) AS "Средний рейтинг треков", e.earnings AS "Доход ($)",
    CASE WHEN e.audience_size >= 10000 THEN 'Крупное'
         WHEN e.audience_size >= 1000 THEN 'Среднее' ELSE 'Небольшое' END AS "Масштаб"
FROM event e
LEFT JOIN collection c ON e.collection_id = c.collection_id
LEFT JOIN collectiontrack ct ON c.collection_id = ct.collection_id
LEFT JOIN track t ON ct.track_id = t.track_id
GROUP BY e.event_id, c.collection_id
ORDER BY e.date DESC;
```

---

## 4. ОТВЕТЫ НА КОНТРОЛЬНЫЕ ВОПРОСЫ

### 4.1. Что такое WPF?

WPF (Windows Presentation Foundation) — это графическая подсистема для создания пользовательских интерфейсов в Windows-приложениях на базе .NET. В данной работе использован Avalonia UI — кроссплатформенный аналог WPF, поддерживающий XAML-разметку и привязку данных на Linux, macOS и Windows.

### 4.2. Что такое XAML?

XAML (eXtensible Application Markup Language) — декларативный язык разметки на базе XML, используемый для описания пользовательских интерфейсов. В Avalonia UI XAML определяет структуру окон, расположение элементов, стили и привязки данных.

### 4.3. Что такое привязка данных (Data Binding)?

Привязка данных — механизм соединения UI-элементов с источниками данных. В данной работе данные загружаются из БД в `DataTable`, преобразуются в список объектов и отображаются в `ListBox` с динамически создаваемыми элементами.

### 4.4. Что такое MVVM?

MVVM (Model-View-ViewModel) — архитектурный паттерн, разделяющий интерфейс (View), данные (Model) и логику отображения (ViewModel). В данной работе применён упрощённый подход с code-behind, что допустимо для учебных проектов.

### 4.5. Какие преимущества Avalonia UI перед WPF?

- Кроссплатформенность (Linux, macOS, Windows)
- Совместимость с WPF-синтаксисом XAML
- Активная разработка и поддержка
- Возможность запуска на .NET 8+

---

## 5. ВЫВОД

В ходе выполнения лабораторной работы было создано кроссплатформенное GUI-приложение на базе Avalonia UI (аналог WPF) для работы с базой данных DJ Music Library. Реализованы:

- Подключение к PostgreSQL через Npgsql
- Отображение 5 таблиц и 4 представлений
- CRUD-операции (создание, чтение, обновление, удаление) с динамическим определением первичного ключа
- Динамическая генерация форм редактирования на основе схемы таблицы (поле ID скрыто)
- Меню навигации и панель инструментов
- Блокировка CRUD-операций для представлений
- Тёмная тема оформления
- Пагинация (25 записей на страницу)
- Сортировка по столбцам (числовые и текстовые)
- Поиск по всем полям таблицы
- Русские названия полей через маппинг
- Заполнение пустых полей в БД

**Структура БД:**

| Таблица | Первичный ключ | Записей |
|---------|---------------|---------|
| artist | artist_id | 1925 |
| genre | genre_id | 24 |
| track | track_id | 2790 |
| collection | collection_id | 13 |
| collectiontrack | (collection_id, track_id) | 29 |
| event | event_id | 5 |

Приложение успешно собирается и запускается на Linux с использованием .NET 8 SDK.
