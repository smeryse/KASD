#!/usr/bin/env python3
"""
Скрипт для скачивания музыки из luxiut_3.txt с правильными метаданными
Метаданные берутся из названия трека в файле (Яндекс.Музыка экспорт)
"""

import subprocess
import time
import re
from pathlib import Path

INPUT_FILE = "luxiut_3.txt"
OUTPUT_DIR = Path("music")
LOG_FILE = Path("download_log.txt")

def parse_track_line(line):
    """Парсим строку 'Артист - Название'"""
    line = line.strip()
    if ' - ' in line:
        parts = line.split(' - ', 1)
        return parts[0].strip(), parts[1].strip()
    return "", line

def sanitize_filename(name):
    """Безопасное имя файла"""
    return re.sub(r'[\\/:*?"<>|]', '_', name)

def download_thumbnail(track_name, output_path):
    """Скачиваем обложку отдельно"""
    try:
        cmd = [
            "yt-dlp",
            "--write-thumbnail",
            "--convert-thumbnail", "jpg",
            "-o", str(output_path.with_suffix('')),
            "--skip-download",
            f"ytsearch1:{track_name}"
        ]
        subprocess.run(cmd, capture_output=True, timeout=60)
        
        # Проверяем есть ли обложка
        for ext in ['.jpg', '.webp', '.png']:
            cover = output_path.with_suffix(ext)
            if cover.exists():
                return cover
        return None
    except Exception as e:
        print(f"  ✗ Обложка: {e}")
        return None

def add_metadata(mp3_path, artist, title, cover_path=None):
    """Добавляем метаданные через ffmpeg"""
    try:
        temp_path = mp3_path.with_suffix('.temp.mp3')
        
        cmd = ["ffmpeg", "-y", "-i", str(mp3_path), "-c:a", "copy"]
        
        # Метаданные
        cmd.extend(["-metadata", f"title={title}"])
        if artist:
            cmd.extend(["-metadata", f"artist={artist}"])
            cmd.extend(["-metadata", f"albumartist={artist}"])
        
        # Обложка
        if cover_path and cover_path.exists():
            cmd.extend(["-i", str(cover_path), "-map", "0:a", "-map", "1:v", 
                       "-c:v", "mjpeg", "-q:v", "2"])
        else:
            cmd.extend(["-map", "0:a"])
        
        cmd.append(str(temp_path))
        
        result = subprocess.run(cmd, capture_output=True, timeout=120)
        
        if result.returncode == 0 and temp_path.exists():
            mp3_path.unlink()
            temp_path.rename(mp3_path)
            return True
        return False
    except Exception as e:
        print(f"  ✗ Метаданные: {e}")
        return False

def download_track(track_name, output_path):
    """Скачиваем трек"""
    cmd = [
        "yt-dlp",
        "-x",
        "--audio-format", "mp3",
        "--audio-quality", "192K",
        "-o", str(output_path),
        "--no-mtime",
        "--extractor-args", "youtube:player_client=tv_embedded",
        f"ytsearch1:{track_name}"
    ]
    
    result = subprocess.run(cmd, capture_output=True, text=True, timeout=180)
    return result.returncode == 0

def main():
    OUTPUT_DIR.mkdir(exist_ok=True)
    
    # Обновляем yt-dlp
    print("Обновляем yt-dlp...")
    subprocess.run(["pip", "install", "--upgrade", "yt-dlp", "-q"])
    
    # Читаем треки
    with open(INPUT_FILE, "r", encoding="utf-8") as f:
        tracks = [line.strip() for line in f if line.strip()]
    
    print(f"Всего треков: {len(tracks)}")
    
    success = 0
    failed = 0
    
    with open(LOG_FILE, "w", encoding="utf-8") as log:
        for i, track in enumerate(tracks, 1):
            print(f"\n[{i}/{len(tracks)}] {track}")
            
            artist, title = parse_track_line(track)
            safe_name = sanitize_filename(track)
            
            mp3_path = OUTPUT_DIR / f"{safe_name}.%(ext)s"
            
            # Скачиваем
            if download_track(track, mp3_path):
                # Находим скачанный файл
                mp3_files = list(OUTPUT_DIR.glob(f"{safe_name}.*"))
                if mp3_files:
                    mp3_path = mp3_files[0]
                    if mp3_path.suffix != '.mp3':
                        # Конвертируем в mp3 если нужно
                        final_path = OUTPUT_DIR / f"{safe_name}.mp3"
                        subprocess.run(["ffmpeg", "-y", "-i", str(mp3_path), 
                                      "-c:a", "libmp3lame", "-b:a", "192K", 
                                      str(final_path)], capture_output=True)
                        mp3_path.unlink()
                        mp3_path = final_path
                    
                    # Скачиваем обложку
                    print(f"  → Обложка...")
                    cover_path = OUTPUT_DIR / f"{safe_name}.cover"
                    downloaded_cover = download_thumbnail(track, cover_path)
                    
                    # Добавляем метаданные
                    print(f"  → Метаданные...")
                    if add_metadata(mp3_path, artist, title, downloaded_cover):
                        print(f"  ✓ Успешно: {artist} - {title}")
                        log.write(f"✓ {track}\n")
                        success += 1
                    else:
                        print(f"  ✗ Ошибка метаданных")
                        log.write(f"✓ {track} (без метаданных)\n")
                        success += 1
                    
                    # Чистим обложку
                    if downloaded_cover:
                        downloaded_cover.unlink()
                else:
                    print(f"  ✗ Файл не найден")
                    log.write(f"✗ {track}\n")
                    failed += 1
            else:
                print(f"  ✗ Ошибка скачивания")
                log.write(f"✗ {track}\n")
                failed += 1
            
            time.sleep(2)
    
    print("\n" + "=" * 40)
    print(f"Завершено! Успешно: {success}, Ошибок: {failed}")

if __name__ == "__main__":
    main()
