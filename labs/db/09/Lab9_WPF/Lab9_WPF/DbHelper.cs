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
