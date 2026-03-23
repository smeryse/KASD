#!/usr/bin/env python3
"""
Скрипт для простановки метаданных в MP3 файлы из luxiut_3.txt
Использует правильные названия из экспорта Яндекс.Музыки
"""

import subprocess
from pathlib import Path
from mutagen.mp3 import MP3
from mutagen.id3 import ID3, TIT2, TPE1, APIC, USLT
from mutagen.easyid3 import EasyID3
import requests
import re

MUSIC_DIR = Path("music")
INPUT_FILE = Path("luxiut_3.txt")

def parse_track_line(line):
    """Парсим строку 'Артист - Название' или просто 'Название'"""
    if ' - ' in line:
        parts = line.split(' - ', 1)
        return parts[0].strip(), parts[1].strip()
    return "", line.strip()

def download_thumbnail(track_name):
    """Скачиваем обложку через YouTube"""
    try:
        result = subprocess.run([
            'yt-dlp',
            '--extract-audio',
            '--audio-format', 'mp3',
            '--write-thumbnail',
            '--convert-thumbnail', 'jpg',
            '-o', '/tmp/cover.%(ext)s',
            '--skip-download',
            f'ytsearch1:{track_name}'
        ], capture_output=True, timeout=30)
        
        cover_path = Path('/tmp/cover.jpg')
        if cover_path.exists():
            return cover_path.read_bytes()
    except Exception as e:
        print(f"  ✗ Обложка: {e}")
    return None

def add_metadata(mp3_path, artist, title):
    """Добавляем метаданные в MP3"""
    try:
        audio = EasyID3(mp3_path)
    except:
        audio = MP3(mp3_path)
        audio.add_tags()
        audio = EasyID3(mp3_path)
    
    # Основные метаданные
    audio['title'] = title
    if artist:
        audio['artist'] = artist
        audio['albumartist'] = artist
    
    audio.save()
    
    # Добавляем обложку
    try:
        # Ищем обложку в том же каталоге
        cover_path = mp3_path.with_suffix('.jpg')
        if not cover_path.exists():
            cover_path = mp3_path.with_suffix('.png')
        
        if cover_path.exists():
            with open(cover_path, 'rb') as f:
                cover_data = f.read()
            
            audio = ID3(mp3_path)
            audio['APIC'] = APIC(
                encoding=3,
                mime='image/jpeg',
                type=3,  # обложка
                desc='Cover',
                data=cover_data
            )
            audio.save()
            print(f"  ✓ Обложка добавлена")
    except Exception as e:
        print(f"  ✗ Обложка: {e}")

def main():
    # Читаем треки из файла
    with open(INPUT_FILE, 'r', encoding='utf-8') as f:
        tracks = [line.strip() for line in f if line.strip()]
    
    print(f"Всего треков в файле: {len(tracks)}")
    
    # Получаем список скачанных файлов
    mp3_files = list(MUSIC_DIR.glob('*.mp3'))
    print(f"Скачано MP3 файлов: {len(mp3_files)}\n")
    
    # Сопоставляем файлы с треками
    for mp3_file in mp3_files:
        filename = mp3_file.stem  # имя без расширения
        
        # Ищем匹配的 трек в списке
        matched_track = None
        for track in tracks:
            # Проверяем совпадение по имени файла
            if track in filename or filename in track:
                matched_track = track
                break
        
        if matched_track:
            artist, title = parse_track_line(matched_track)
            print(f"Файл: {mp3_file.name}")
            print(f"  Артист: {artist or 'N/A'}")
            print(f"  Название: {title}")
            
            add_metadata(mp3_file, artist, title)
            
            # Скачиваем обложку если нет
            if not mp3_file.with_suffix('.jpg').exists():
                print(f"  → Скачиваем обложку...")
                cover_data = download_thumbnail(matched_track)
                if cover_data:
                    cover_path = mp3_file.with_suffix('.jpg')
                    cover_path.write_bytes(cover_data)
                    print(f"  ✓ Обложка сохранена")
            
            print()
        else:
            print(f"✗ Не найдено совпадение для: {filename}\n")

if __name__ == "__main__":
    main()
