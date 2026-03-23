#!/bin/bash

# Скрипт для скачивания музыки из luxiut_3.txt через yt-dlp
# С обложками и последующей простановкой метаданных

INPUT_FILE="luxiut_3.txt"
OUTPUT_DIR="music"
LOG_FILE="download_log.txt"

# Создаём директорию для музыки
mkdir -p "$OUTPUT_DIR"

# Обновляем yt-dlp
echo "Обновляем yt-dlp..."
pip install --upgrade yt-dlp -q 2>/dev/null

# Очищаем лог
> "$LOG_FILE"

echo "Начинаем скачивание треков..."
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

    # Создаём безопасное имя файла из названия трека
    safe_filename=$(echo "$track" | sed 's/[\\/:*?"<>|]/_/g' | sed 's/  */ /g' | sed 's/^ *//;s/ *$//')
    
    # Скачиваем аудио
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

    # Небольшая задержка между скачиваниями
    sleep 2

done < "$INPUT_FILE"

echo "========================================"
echo "Скачивание завершено!"
echo "Успешно: $success"
echo "Ошибок: $failed"
echo "========================================"

echo ""
echo "Запускаем простановку метаданных..."
bash add_metadata.sh
