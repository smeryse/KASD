#!/usr/bin/env python3
"""
Сравнивает список треков из luxiut_3.txt с скачанными файлами
Выводит список нескачанных треков
"""

from pathlib import Path
import re

INPUT_FILE = Path("luxiut_3.txt")
MUSIC_DIR = Path("music")

def sanitize(name):
    """Очищает имя для сравнения"""
    return re.sub(r'[^\w\s\u0400-\u04FF\u00C0-\u00FF]', '', name.lower().strip())

def main():
    # Читаем треки из файла
    with open(INPUT_FILE, 'r', encoding='utf-8') as f:
        tracks = [line.strip() for line in f if line.strip()]
    
    print(f"Всего треков в файле: {len(tracks)}")
    
    # Получаем список скачанных файлов
    downloaded = set()
    for mp3 in MUSIC_DIR.glob("*.mp3"):
        downloaded.add(sanitize(mp3.stem))
    
    print(f"Скачано файлов: {len(downloaded)}")
    
    # Находим нескачанные
    missing = []
    for track in tracks:
        if sanitize(track) not in downloaded:
            missing.append(track)
    
    print(f"Не скачалось: {len(missing)}")
    
    # Сохраняем список нескачанных
    if missing:
        with open("missing_tracks.txt", 'w', encoding='utf-8') as f:
            f.write('\n'.join(missing))
        print(f"\nСписок нескачанных сохранён в missing_tracks.txt")
        print("\nПервые 50 нескачанных:")
        for t in missing[:50]:
            print(f"  {t}")
        if len(missing) > 50:
            print(f"  ... и ещё {len(missing) - 50}")
    else:
        print("\n✓ Все треки скачаны!")

if __name__ == "__main__":
    main()
