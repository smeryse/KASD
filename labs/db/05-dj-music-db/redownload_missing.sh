#!/bin/bash

# Скрипт для докачки нескачанных треков из missing_tracks.txt

INPUT_FILE="missing_tracks.txt"
OUTPUT_DIR="music"
LOG_FILE="redownload_log.txt"

if [[ ! -f "$INPUT_FILE" ]]; then
    echo "Файл $INPUT_FILE не найден!"
    exit 1
fi

# Очищаем лог
> "$LOG_FILE"

echo "Начинаем докачку треков..."
echo "Всего треков: $(wc -l < "$INPUT_FILE")"

# Счётчики
success=0
failed=0

# Читаем файл построчно
while IFS= read -r track || [[ -n "$track" ]]; do
    # Пропускаем пустые строки
    [[ -z "$track" ]] && continue

    echo "----------------------------------------"
    echo "Треке: $track"
    echo "----------------------------------------"

    # Создаём безопасное имя файла
    safe_filename=$(echo "$track" | sed 's/[\\/:*?"<>|]/_/g' | sed 's/  */ /g' | sed 's/^ *//;s/ *$//')
    
    # Проверяем есть ли уже файл
    if [[ -f "$OUTPUT_DIR/${safe_filename}.mp3" ]]; then
        echo "✓ Уже скачан"
        ((success++))
        continue
    fi
    
    # Скачиваем
    echo "  → Скачиваем..."
    yt-dlp \
        -x \
        --audio-format mp3 \
        --audio-quality 192K \
        -o "$OUTPUT_DIR/${safe_filename}.%(ext)s" \
        --no-mtime \
        --extractor-args "youtube:player_client=tv_embedded" \
        "ytsearch1:$track" \
        2>> "$LOG_FILE"

    if [ $? -eq 0 ]; then
        echo "✓ Успешно: $track"
        echo "✓ $track" >> "$LOG_FILE"
        ((success++))
    else
        echo "✗ Ошибка: $track"
        echo "✗ $track" >> "$LOG_FILE"
        ((failed++))
    fi

    # Небольшая задержка
    sleep 2

done < "$INPUT_FILE"

echo "========================================"
echo "Докачка завершена!"
echo "Успешно: $success"
echo "Ошибок: $failed"
echo "========================================"
