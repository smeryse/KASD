#!/bin/bash
# Пост-обработка: добавление метаданных в MP3 файлы

MUSIC_DIR="music"
INPUT_FILE="luxiut_3.txt"

echo "Добавляем метаданные в MP3 файлы..."

# Читаем треки из файла
while IFS= read -r track || [[ -n "$track" ]]; do
    [[ -z "$track" ]] && continue
    
    # Извлекаем артиста и название
    if [[ "$track" == *" - "* ]]; then
        artist="${track%% - *}"
        title="${track#* - }"
    else
        artist=""
        title="$track"
    fi
    
    # Находим файл
    safe_filename=$(echo "$track" | sed 's/[\\/:*?"<>|]/_/g')
    mp3_file="$MUSIC_DIR/${safe_filename}.mp3"
    
    if [[ -f "$mp3_file" ]]; then
        echo "Обработка: $track"
        
        # Временный файл
        temp_file="$MUSIC_DIR/.temp_${safe_filename}.mp3"
        
        # Ищем обложку
        cover_file="$MUSIC_DIR/${safe_filename}.jpg"
        
        if [[ -f "$cover_file" ]]; then
            # С обложкой
            ffmpeg -y -i "$mp3_file" -i "$cover_file" \
                -c:a copy -c:v mjpeg -q:v 2 \
                -metadata "title=$title" \
                -metadata "artist=$artist" \
                -metadata "albumartist=$artist" \
                "$temp_file" 2>/dev/null
        else
            # Без обложки
            ffmpeg -y -i "$mp3_file" \
                -c:a copy \
                -metadata "title=$title" \
                -metadata "artist=$artist" \
                -metadata "albumartist=$artist" \
                "$temp_file" 2>/dev/null
        fi
        
        if [[ -f "$temp_file" ]]; then
            mv "$temp_file" "$mp3_file"
            echo "  ✓ Метаданные добавлены"
        fi
    fi
done < "$INPUT_FILE"

echo "Готово!"
