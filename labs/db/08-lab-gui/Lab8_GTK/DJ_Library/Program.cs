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
