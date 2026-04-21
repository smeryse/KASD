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
