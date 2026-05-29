using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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

                _columnNames.Clear();
                _allRows.Clear();
                _colWidths.Clear();
                _headerTextBlocks.Clear();
                _sortColumn = -1;
                _sortDirection = SortDirection.None;

                foreach (DataColumn col in table.Columns)
                {
                    _columnNames.Add(col.ColumnName);
                    _colWidths.Add(140);
                }

                foreach (DataRow row in table.Rows)
                    _allRows.Add(row);

                _searchText = "";
                searchBox.Text = "";
                searchInfo.Text = "";
                ApplyFilterAndSort();
                _currentPage = 0;
                BuildList();
                UpdatePagination();
                tblCurrentTable.Text = displayName;
                statusBar.Text = $"{displayName}: {_allRows.Count} строк";
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void ApplyFilterAndSort()
        {
            if (string.IsNullOrWhiteSpace(_searchText))
            {
                _filteredRows = new List<DataRow>(_allRows);
            }
            else
            {
                var search = _searchText.ToLower();
                _filteredRows = new List<DataRow>();
                foreach (var row in _allRows)
                {
                    foreach (var col in _columnNames)
                    {
                        var val = row[col]?.ToString()?.ToLower();
                        if (val != null && val.Contains(search))
                        {
                            _filteredRows.Add(row);
                            break;
                        }
                    }
                }
            }

            if (_sortColumn >= 0 && _sortColumn < _columnNames.Count && _sortDirection != SortDirection.None)
            {
                var colName = _columnNames[_sortColumn];
                bool isNumeric = false;
                if (_filteredRows.Count > 0)
                {
                    var val = _filteredRows[0][colName];
                    if (val != DBNull.Value)
                        isNumeric = val is int or long or float or double or decimal;
                }

                _sortedRows = new List<DataRow>(_filteredRows);
                if (_sortDirection == SortDirection.Ascending)
                    _sortedRows.Sort((a, b) => CompareRows(a, b, colName, isNumeric));
                else
                    _sortedRows.Sort((a, b) => CompareRows(b, a, colName, isNumeric));
            }
            else
            {
                _sortedRows = new List<DataRow>(_filteredRows);
            }

            searchInfo.Text = !string.IsNullOrWhiteSpace(_searchText)
                ? $"Найдено: {_filteredRows.Count} из {_allRows.Count}"
                : "";
        }

        private int CompareRows(DataRow a, DataRow b, string colName, bool isNumeric)
        {
            var valA = a[colName];
            var valB = b[colName];

            bool aNull = valA == DBNull.Value || valA == null;
            bool bNull = valB == DBNull.Value || valB == null;

            if (aNull && bNull) return 0;
            if (aNull) return 1;
            if (bNull) return -1;

            if (isNumeric)
            {
                var numA = Convert.ToDouble(valA);
                var numB = Convert.ToDouble(valB);
                return numA.CompareTo(numB);
            }

            var strA = valA.ToString()!.ToLower();
            var strB = valB.ToString()!.ToLower();
            return string.Compare(strA, strB, StringComparison.Ordinal);
        }

        private void BuildList()
        {
            dataList.Items.Clear();
            _headerTextBlocks.Clear();

            var headerPanel = new StackPanel { Orientation = Orientation.Horizontal, Background = new SolidColorBrush(Color.Parse("#3C3C3C")) };
            for (int i = 0; i < _columnNames.Count; i++)
            {
                var displayName = FieldMapper.GetDisplayName(_currentTable, _columnNames[i]);
                var sortIndicator = _sortColumn == i ? (_sortDirection == SortDirection.Ascending ? " ▲" : " ▼") : "";

                var tb = new TextBlock
                {
                    Text = displayName + sortIndicator,
                    FontWeight = FontWeight.Bold,
                    Foreground = new SolidColorBrush(Color.Parse("#E0E0E0")),
                    Width = _colWidths[i],
                    Margin = new Thickness(2),
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    Cursor = new Cursor(StandardCursorType.Hand),
                    Tag = i
                };
                tb.PointerPressed += Header_Click;
                _headerTextBlocks[i] = tb;
                headerPanel.Children.Add(tb);
            }
            dataList.Items.Add(headerPanel);

            var start = _currentPage * PageSize;
            var end = Math.Min(start + PageSize, _sortedRows.Count);

            for (int r = start; r < end; r++)
            {
                var row = _sortedRows[r];
                var rowPanel = new StackPanel { Orientation = Orientation.Horizontal };
                for (int c = 0; c < _columnNames.Count; c++)
                {
                    var val = row[_columnNames[c]] == DBNull.Value ? "" : row[_columnNames[c]]!.ToString();
                    rowPanel.Children.Add(new TextBlock
                    {
                        Text = val,
                        Width = _colWidths[c],
                        Margin = new Thickness(2),
                        TextTrimming = TextTrimming.CharacterEllipsis
                    });
                }
                dataList.Items.Add(rowPanel);
            }
        }

        private void Header_Click(object? sender, PointerPressedEventArgs e)
        {
            if (sender is TextBlock tb && tb.Tag is int colIndex)
            {
                if (_sortColumn == colIndex)
                {
                    _sortDirection = _sortDirection switch
                    {
                        SortDirection.None => SortDirection.Ascending,
                        SortDirection.Ascending => SortDirection.Descending,
                        SortDirection.Descending => SortDirection.None,
                        _ => SortDirection.None
                    };
                }
                else
                {
                    _sortColumn = colIndex;
                    _sortDirection = SortDirection.Ascending;
                }

                ApplyFilterAndSort();
                _currentPage = 0;
                BuildList();
                UpdatePagination();
            }
        }

        private void UpdatePagination()
        {
            if (TotalPages <= 1)
            {
                btnPrev.IsEnabled = false;
                btnNext.IsEnabled = false;
                pageInfo.Text = _sortedRows.Count > 0 ? $"Всего: {_sortedRows.Count} строк" : "Нет данных";
            }
            else
            {
                btnPrev.IsEnabled = _currentPage > 0;
                btnNext.IsEnabled = _currentPage < TotalPages - 1;
                var start = _currentPage * PageSize + 1;
                var end = Math.Min((_currentPage + 1) * PageSize, _sortedRows.Count);
                pageInfo.Text = $"{start}-{end} из {_sortedRows.Count} (стр. {_currentPage + 1}/{TotalPages})";
            }
        }

        private void Btn_Prev_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                BuildList();
                UpdatePagination();
            }
        }

        private void Btn_Next_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentPage < TotalPages - 1)
            {
                _currentPage++;
                BuildList();
                UpdatePagination();
            }
        }

        private void Btn_Search_Click(object? sender, RoutedEventArgs e)
        {
            _searchText = searchBox.Text?.Trim() ?? "";
            ApplyFilterAndSort();
            _currentPage = 0;
            BuildList();
            UpdatePagination();
        }

        private void Btn_ResetSearch_Click(object? sender, RoutedEventArgs e)
        {
            searchBox.Text = "";
            _searchText = "";
            ApplyFilterAndSort();
            _currentPage = 0;
            BuildList();
            UpdatePagination();
        }

        private void MenuItem_Artist_Click(object? sender, RoutedEventArgs e) => LoadData("artist", "Таблица: Artist");
        private void MenuItem_Genre_Click(object? sender, RoutedEventArgs e) => LoadData("genre", "Таблица: Genre");
        private void MenuItem_Track_Click(object? sender, RoutedEventArgs e) => LoadData("track", "Таблица: Track");
        private void MenuItem_Collection_Click(object? sender, RoutedEventArgs e) => LoadData("collection", "Таблица: Collection");
        private void MenuItem_Event_Click(object? sender, RoutedEventArgs e) => LoadData("event", "Таблица: Event");
        private void MenuItem_TrackLists_Click(object? sender, RoutedEventArgs e) => LoadData("v_track_lists", "Представление: v_track_lists", true);
        private void MenuItem_PreparedSets_Click(object? sender, RoutedEventArgs e) => LoadData("v_prepared_sets", "Представление: v_prepared_sets", true);
        private void MenuItem_PerformanceHistory_Click(object? sender, RoutedEventArgs e) => LoadData("v_performance_history", "Представление: v_performance_history", true);
        private void MenuItem_EventManager_Click(object? sender, RoutedEventArgs e) => LoadData("v_event_manager_report", "Представление: v_event_manager_report", true);

        private DataRow? GetSelectedRow()
        {
            var idx = dataList.SelectedIndex;
            if (idx <= 0) return null;
            var start = _currentPage * PageSize;
            var rowIdx = idx - 1;
            if (rowIdx >= _sortedRows.Count) return null;
            return _sortedRows[start + rowIdx];
        }

        private async void Btn_Add_Click(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentTable)) { ShowMessage("Сначала выберите таблицу"); return; }
            if (_isView) { ShowMessage("Нельзя добавлять записи в представление"); return; }

            var editWindow = new EditWindow(_currentTable, null);
            await editWindow.ShowDialog(this);
            if (editWindow.Saved)
            {
                LoadData(_currentTable, tblCurrentTable.Text);
                statusBar.Text = "Запись добавлена";
            }
        }

        private async void Btn_Edit_Click(object? sender, RoutedEventArgs e)
        {
            if (_isView) { ShowMessage("Нельзя редактировать представление"); return; }
            var row = GetSelectedRow();
            if (row == null) { ShowMessage("Выберите запись для редактирования"); return; }

            var editWindow = new EditWindow(_currentTable, row);
            await editWindow.ShowDialog(this);
            if (editWindow.Saved)
            {
                LoadData(_currentTable, tblCurrentTable.Text);
                statusBar.Text = "Запись обновлена";
            }
        }

        private async void Btn_Delete_Click(object? sender, RoutedEventArgs e)
        {
            if (_isView) { ShowMessage("Нельзя удалять из представления"); return; }
            var row = GetSelectedRow();
            if (row == null) { ShowMessage("Выберите запись для удаления"); return; }

            var id = row[_primaryKey];
            if (await ShowConfirm("Удалить выбранную запись?"))
            {
                try
                {
                    DbHelper.ExecuteNonQuery($"DELETE FROM public.\"{_currentTable}\" WHERE {_primaryKey} = {id}");
                    LoadData(_currentTable, tblCurrentTable.Text);
                    statusBar.Text = "Запись удалена";
                }
                catch (Npgsql.NpgsqlException ex) when (ex.SqlState == "23503")
                {
                    ShowError("Нельзя удалить: запись используется в связанных таблицах.\nСначала удалите зависимые записи.");
                }
                catch (Exception ex)
                {
                    ShowError($"Ошибка удаления: {ex.Message}");
                }
            }
        }

        private void Btn_Refresh_Click(object? sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentTable)) LoadData(_currentTable, tblCurrentTable.Text, _isView);
        }

        private void ShowMessage(string msg) => ShowDialog("Внимание", msg);
        private void ShowError(string msg) => ShowDialog("Ошибка", msg);

        private async void ShowDialog(string title, string msg)
        {
            var dialog = new Window
            {
                Title = title, Width = 400, Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Background = new SolidColorBrush(Color.Parse("#1E1E1E"))
            };
            var sp = new StackPanel { Margin = new Thickness(20) };
            sp.Children.Add(new TextBlock { Text = msg, TextWrapping = TextWrapping.Wrap, Foreground = Brushes.White });
            var btn = new Button { Content = "OK", Width = 80, Margin = new Thickness(0, 15, 0, 0), HorizontalAlignment = HorizontalAlignment.Center };
            btn.Click += (_, _) => dialog.Close();
            sp.Children.Add(btn);
            dialog.Content = sp;
            await dialog.ShowDialog(this);
        }

        private async Task<bool> ShowConfirm(string msg)
        {
            var tcs = new TaskCompletionSource<bool>();
            var dialog = new Window
            {
                Title = "Подтверждение", Width = 350, Height = 160,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Background = new SolidColorBrush(Color.Parse("#1E1E1E"))
            };
            var sp = new StackPanel { Margin = new Thickness(20) };
            sp.Children.Add(new TextBlock { Text = msg, Margin = new Thickness(0, 0, 0, 15), Foreground = Brushes.White });
            var btns = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center, Spacing = 10 };
            var btnYes = new Button { Content = "Да", Width = 80 };
            var btnNo = new Button { Content = "Нет", Width = 80 };
            btnYes.Click += (_, _) => { tcs.TrySetResult(true); dialog.Close(); };
            btnNo.Click += (_, _) => { tcs.TrySetResult(false); dialog.Close(); };
            btns.Children.Add(btnYes);
            btns.Children.Add(btnNo);
            sp.Children.Add(btns);
            dialog.Content = sp;
            await dialog.ShowDialog(this);
            return await tcs.Task;
        }
    }
}
