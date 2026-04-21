![[Pasted image 20260416130934.png]]
Подготовка окружения
```
sudo apt update
sudo apt install dotnet-sdk-8.0 libgdiplus x11-utils
```
Создали консольное приложение ![[Pasted image 20260419130833.png]]
Установили драйвер бд
```shell
dotnet add package Npgsql
```
![[Pasted image 20260419130804.png]]
```
dotnet add package Npgsql
```
![[Pasted image 20260419130957.png]]
Создали структуру файлов
![[Pasted image 20260419131217.png]]

Добавили npgsql
![[Pasted image 20260419132215.png]]

Привет. Инструкция по Avalonia UI для Linux. Без лишних слов.

### 1. Установка и создание проекта
```bash
# Установка шаблонов Gtk
dotnet new install Gtk.Templates

# Создание проекта (подтверди создание в текущей папке)
dotnet new Gtk.app -n Lab8

# Переход в папку и добавление Npgsql
cd Lab8
dotnet add package Npgsql
```

### 2. Заполнение файлов
**DbHelper.cs**
```c sharp
using System;
using System.Collections.Generic;
using Npgsql;

public static class DbHelper
{
    private const string ConnStr = "Host=localhost;Port=5432;Database=dj_db;Username=postgres;Password=19022016";

    public static List<List<string>> SelectAll(string tableName)
    {
        var result = new List<List<string>>();
        using var con = new NpgsqlConnection(ConnStr);
        con.Open();
        using var cmd = new NpgsqlCommand($"SELECT * FROM public.\"{tableName}\"", con);
        using var reader = cmd.ExecuteReader();

        var columns = new List<string>();
        for (int i = 0; i < reader.FieldCount; i++)
            columns.Add(reader.GetName(i));
        result.Add(columns);

        while (reader.Read())
        {
            var row = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
                row.Add(reader.GetValue(i)?.ToString() ?? "");
            result.Add(row);
        }
        return result;
    }
}

```

**DJ_Library.csproj**
```c sharp
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="GtkSharp" Version="3.24.24.95" />
    <PackageReference Include="Npgsql" Version="8.0.3" />
  </ItemGroup>
</Project>
```

**Program.cs**
```c
using System;
using System.Collections.Generic;
using Gtk;

class Program
{
    static Window mainWindow;
    static TreeView tableView;
    static ListStore listStore;
    static Label statusLabel;

    static void Main()
    {
        Application.Init();

        mainWindow = new Window("DJ Library");
        mainWindow.SetDefaultSize(1200, 800);
        mainWindow.DeleteEvent += (s, e) => Application.Quit();

        var vbox = new VBox(false, 5);
        vbox.BorderWidth = 10;

        // Меню
        var menuBar = CreateMenus();
        vbox.PackStart(menuBar, false, false, 0);

        // Статус
        statusLabel = new Label("Готово");
        statusLabel.Name = "statusLabel";
        vbox.PackStart(statusLabel, false, false, 0);

        // Таблица
        var scrollWin = new ScrolledWindow();
        scrollWin.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        
        tableView = new TreeView();
        tableView.EnableGridLines = TreeViewGridLines.Both;
        scrollWin.Add(tableView);
        vbox.PackStart(scrollWin, true, true, 0);

        mainWindow.Add(vbox);
        mainWindow.ShowAll();

        Application.Run();
    }

    static MenuBar CreateMenus()
    {
        var menuBar = new MenuBar();

        // Справочники
        var refMenu = new MenuItem("Справочники");
        var refSubMenu = new Menu();
        refMenu.Submenu = refSubMenu;
        
        var artistItem = new MenuItem("Artist");
        artistItem.Activated += (s, e) => LoadData("artist", "Artist");
        refSubMenu.Append(artistItem);

        var genreItem = new MenuItem("Genre");
        genreItem.Activated += (s, e) => LoadData("genre", "Genre");
        refSubMenu.Append(genreItem);

        // Данные
        var dataMenu = new MenuItem("Данные");
        var dataSubMenu = new Menu();
        dataMenu.Submenu = dataSubMenu;

        var trackItem = new MenuItem("Track");
        trackItem.Activated += (s, e) => LoadData("track", "Track");
        dataSubMenu.Append(trackItem);

        var collectionItem = new MenuItem("Collection");
        collectionItem.Activated += (s, e) => LoadData("collection", "Collection");
        dataSubMenu.Append(collectionItem);

        var eventItem = new MenuItem("Event");
        eventItem.Activated += (s, e) => LoadData("event", "Event");
        dataSubMenu.Append(eventItem);

        // Отчеты
        var reportMenu = new MenuItem("Отчеты");
        var reportSubMenu = new Menu();
        reportMenu.Submenu = reportSubMenu;

        var trackListsItem = new MenuItem("Track Lists");
        trackListsItem.Activated += (s, e) => LoadData("v_track_lists", "Track Lists");
        reportSubMenu.Append(trackListsItem);

        var preparedSetsItem = new MenuItem("Prepared Sets");
        preparedSetsItem.Activated += (s, e) => LoadData("v_prepared_sets", "Prepared Sets");
        reportSubMenu.Append(preparedSetsItem);

        var historyItem = new MenuItem("Performance History");
        historyItem.Activated += (s, e) => LoadData("v_performance_history", "Performance History");
        reportSubMenu.Append(historyItem);

        var managerItem = new MenuItem("Event Manager Report");
        managerItem.Activated += (s, e) => LoadData("v_event_manager_report", "Event Manager Report");
        reportSubMenu.Append(managerItem);

        // О программе
        var aboutMenu = new MenuItem("О программе");
        aboutMenu.Activated += ShowAbout;

        // Выход
        var exitMenu = new MenuItem("Выход");
        exitMenu.Activated += (s, e) => Application.Quit();

        menuBar.Append(refMenu);
        menuBar.Append(dataMenu);
        menuBar.Append(reportMenu);
        menuBar.Append(aboutMenu);
        menuBar.Append(exitMenu);

        return menuBar;
    }

    static void LoadData(string table, string title)
    {
        try
        {
            var data = DbHelper.SelectAll(table);
            if (data.Count < 2) return;

            // Удаление старых колонок
            foreach (TreeViewColumn col in tableView.Columns)
            {
                tableView.RemoveColumn(col);
            }

            // Создание колонок
            var columns = new List<TreeViewColumn>();
            var types = new List<Type>();

            for (int i = 0; i < data[0].Count; i++)
            {
                types.Add(typeof(string));
                var col = new TreeViewColumn(data[0][i], new CellRendererText(), "text", i);
                col.Resizable = true;
                col.SortColumnId = i;
                columns.Add(col);
            }

            // Создание модели данных
            listStore = new ListStore(types.ToArray());
            
            // Добавление строк
            foreach (var row in data.GetRange(1, data.Count - 1))
            {
                listStore.AppendValues(row.ToArray());
            }

            tableView.Model = listStore;
            
            // Добавление колонок в таблицу
            foreach (var col in columns)
                tableView.AppendColumn(col);

            // Обновление статуса
            statusLabel.Text = $"{title}: {data.Count - 1} строк";
        }
        catch (Exception ex)
        {
            var dialog = new MessageDialog(mainWindow, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, ex.Message);
            dialog.Run();
            dialog.Destroy();
        }
    }

    static void ShowAbout(object sender, EventArgs e)
    {
        var dialog = new AboutDialog();
        dialog.ProgramName = "DJ Library";
        dialog.Version = "1.0";
        dialog.Comments = "Лабораторная работа №8\nDJ Music Library\nPostgreSQL\nРазработал: Ярослав";
        dialog.TransientFor = mainWindow;
        dialog.Run();
        dialog.Destroy();
    }
}

```

### 4. Сборка и запуск
```bash
dotnet build
dotnet run
```

![[Pasted image 20260420134156.png]]