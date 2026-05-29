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
                ["artist_id"] = "ID артиста",
                ["name"] = "Имя",
                ["country"] = "Страна",
                ["style"] = "Стиль",
                ["active_years"] = "Годы активности",
                ["bio"] = "Биография"
            },
            ["genre"] = new()
            {
                ["genre_id"] = "ID жанра",
                ["name"] = "Название",
                ["parent_genre_id"] = "Родительский жанр",
                ["bpm_range"] = "Диапазон BPM",
                ["description"] = "Описание"
            },
            ["track"] = new()
            {
                ["track_id"] = "ID трека",
                ["title"] = "Название",
                ["artist_id"] = "ID артиста",
                ["genre_id"] = "ID жанра",
                ["bpm"] = "BPM",
                ["key"] = "Тональность",
                ["duration"] = "Длительность (сек)",
                ["file_format"] = "Формат файла",
                ["file_path"] = "Путь к файлу",
                ["rating"] = "Рейтинг",
                ["play_count"] = "Кол-во воспроизведений",
                ["date_added"] = "Дата добавления",
                ["comments"] = "Комментарии"
            },
            ["collection"] = new()
            {
                ["collection_id"] = "ID коллекции",
                ["name"] = "Название",
                ["type"] = "Тип",
                ["description"] = "Описание",
                ["style"] = "Стиль",
                ["planned_duration"] = "План. длительность (сек)",
                ["created_at"] = "Дата создания",
                ["notes"] = "Заметки",
                ["total_duration"] = "Общая длительность (сек)"
            },
            ["collectiontrack"] = new()
            {
                ["collection_id"] = "ID коллекции",
                ["track_id"] = "ID трека",
                ["position"] = "Позиция",
                ["transition_notes"] = "Заметки перехода"
            },
            ["event"] = new()
            {
                ["event_id"] = "ID события",
                ["venue"] = "Площадка",
                ["city"] = "Город",
                ["date"] = "Дата",
                ["audience_size"] = "Размер аудитории",
                ["event_type"] = "Тип события",
                ["collection_id"] = "ID коллекции",
                ["feedback"] = "Отзыв",
                ["earnings"] = "Доход ($)"
            },
            ["v_track_lists"] = new()
            {
                ["track_id"] = "ID трека",
                ["track_title"] = "Название трека",
                ["artist_name"] = "Имя артиста",
                ["genre_name"] = "Жанр",
                ["bpm"] = "BPM",
                ["key"] = "Тональность",
                ["duration"] = "Длительность (сек)",
                ["rating"] = "Рейтинг",
                ["play_count"] = "Кол-во воспроизведений"
            },
            ["v_prepared_sets"] = new()
            {
                ["collection_id"] = "ID коллекции",
                ["set_name"] = "Название сета",
                ["type"] = "Тип",
                ["style"] = "Стиль",
                ["planned_duration"] = "План. длительность (сек)",
                ["tracks"] = "Треки",
                ["total_tracks"] = "Кол-во треков",
                ["actual_duration_seconds"] = "Факт. длительность (сек)"
            },
            ["v_performance_history"] = new()
            {
                ["event_id"] = "ID события",
                ["venue"] = "Площадка",
                ["city"] = "Город",
                ["date"] = "Дата",
                ["event_type"] = "Тип события",
                ["audience_size"] = "Аудитория",
                ["set_name"] = "Название сета",
                ["feedback"] = "Отзыв",
                ["earnings"] = "Доход ($)"
            },
            ["v_event_manager_report"] = new()
            {
                ["event_id"] = "ID события",
                ["Площадка"] = "Площадка",
                ["Город"] = "Город",
                ["Дата"] = "Дата",
                ["Тип события"] = "Тип события",
                ["Аудитория"] = "Аудитория",
                ["Название сета"] = "Название сета",
                ["Треков в сете"] = "Треков в сете",
                ["Длительность сета (мин)"] = "Длительность (мин)",
                ["Средний рейтинг треков"] = "Средний рейтинг",
                ["Доход ($)"] = "Доход ($)",
                ["Масштаб"] = "Масштаб"
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
                "artist_id" => tableName == "artist",
                "genre_id" => tableName == "genre",
                "track_id" => tableName == "track",
                "collection_id" => tableName == "collection",
                "event_id" => tableName == "event",
                _ => false
            };
        }
    }
}
