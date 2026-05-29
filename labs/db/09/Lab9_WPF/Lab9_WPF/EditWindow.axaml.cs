using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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

        public bool Saved { get; private set; }

        
        public EditWindow()
        {
            _tableName = "";
            _existingRow = null;
            _textBoxes = Array.Empty<TextBox>();
            _columnNames = Array.Empty<string>();
            _primaryKey = "";
            _isNullable = Array.Empty<bool>();
            InitializeComponent();
        }

        public EditWindow(string tableName, DataRow? existingRow) : this()
        {
            _tableName = tableName;
            _existingRow = existingRow;
            _primaryKey = DbHelper.GetPrimaryKey(tableName);

            var schema = DbHelper.ExecuteQuery($"SELECT * FROM public.\"{tableName}\" LIMIT 0");
            var colCount = schema.Columns.Count;

            var fieldCount = 0;
            for (int i = 0; i < colCount; i++)
                if (!FieldMapper.IsPrimaryKey(tableName, schema.Columns[i].ColumnName))
                    fieldCount++;

            _columnNames = new string[fieldCount];
            _textBoxes = new TextBox[fieldCount];
            _isNullable = new bool[fieldCount];

            int j = 0;
            for (int i = 0; i < colCount; i++)
            {
                var col = schema.Columns[i];
                if (FieldMapper.IsPrimaryKey(tableName, col.ColumnName))
                    continue;

                _columnNames[j] = col.ColumnName;
                _isNullable[j] = col.AllowDBNull;

                formPanel.Children.Add(new TextBlock
                {
                    Text = FieldMapper.GetDisplayName(tableName, col.ColumnName),
                    FontWeight = Avalonia.Media.FontWeight.Bold,
                    Foreground = Avalonia.Media.Brushes.LightGray,
                    Margin = new Avalonia.Thickness(0, 4, 0, 2)
                });

                var tb = new TextBox
                {
                    Watermark = $"{col.DataType.Name}{(col.AllowDBNull ? " (необязательно)" : "")}",
                    Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#2D2D30")),
                    Foreground = Avalonia.Media.Brushes.White
                };

                if (existingRow != null)
                {
                    var val = existingRow[col.ColumnName];
                    if (val != DBNull.Value)
                        tb.Text = val.ToString();
                }

                _textBoxes[j] = tb;
                formPanel.Children.Add(tb);
                j++;
            }

            Title = existingRow == null ? "Добавление записи" : "Редактирование записи";
        }

        private async void Btn_Save_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var colList = new List<string>();
                var valList = new List<string>();

                for (int i = 0; i < _textBoxes.Length; i++)
                {
                    var text = _textBoxes[i].Text?.Trim() ?? "";

                    if (string.IsNullOrEmpty(text))
                    {
                        if (!_isNullable[i])
                        {
                            await ShowError($"Поле '{FieldMapper.GetDisplayName(_tableName, _columnNames[i])}' обязательно для заполнения");
                            return;
                        }
                        if (_existingRow != null)
                        {
                            colList.Add($"\"{_columnNames[i]}\"");
                            valList.Add("NULL");
                        }
                        continue;
                    }

                    colList.Add($"\"{_columnNames[i]}\"");
                    valList.Add($"'{text.Replace("'", "''")}'");
                }

                if (_existingRow == null)
                {
                    if (colList.Count == 0)
                    {
                        await ShowError("Заполните хотя бы одно поле");
                        return;
                    }
                    DbHelper.ExecuteNonQuery(
                        $"INSERT INTO public.\"{_tableName}\" ({string.Join(", ", colList)}) VALUES ({string.Join(", ", valList)})");
                }
                else
                {
                    var sets = new List<string>();
                    for (int i = 0; i < colList.Count; i++)
                        sets.Add($"{colList[i]} = {valList[i]}");

                    if (sets.Count == 0)
                    {
                        await ShowError("Нет полей для обновления");
                        return;
                    }

                    var id = _existingRow[_primaryKey];
                    DbHelper.ExecuteNonQuery(
                        $"UPDATE public.\"{_tableName}\" SET {string.Join(", ", sets)} WHERE {_primaryKey} = {id}");
                }

                Saved = true;
                Close();
            }
            catch (Exception ex)
            {
                await ShowError($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void Btn_Cancel_Click(object? sender, RoutedEventArgs e) => Close();

        private async Task ShowError(string msg)
        {
            var tcs = new TaskCompletionSource();
            var dialog = new Window
            {
                Title = "Ошибка",
                Width = 450,
                Height = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#1E1E1E"))
            };
            var sp = new StackPanel { Margin = new Avalonia.Thickness(20) };
            sp.Children.Add(new TextBlock
            {
                Text = msg,
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Foreground = Avalonia.Media.Brushes.White
            });
            var btn = new Button
            {
                Content = "OK",
                Width = 80,
                Margin = new Avalonia.Thickness(0, 15, 0, 0),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            btn.Click += (_, _) => { tcs.TrySetResult(); dialog.Close(); };
            sp.Children.Add(btn);
            dialog.Content = sp;
            await dialog.ShowDialog(this);
            await tcs.Task;
        }
    }
}
