from pathlib import Path


def generate_readme(
    title="Contests 3",
    output="README.md",
    extension=".cs"
):
    # base_dir = Path.cwd()
    base_dir = Path("../Contests/CT3");
    output_path = base_dir / output

    cs_files = sorted(base_dir.rglob(f"*{extension}"))

    with open(output_path, "w", encoding="utf-8") as f:
        f.write(f"# {title}\n\n")
        f.write("| # | Name | Description | Status |\n")
        f.write("| -- | -- | -- | -- |\n")

        for file in cs_files:
            stem = file.stem  # например: G-Guide

            if "-" not in stem:
                continue  # защита от файлов без нужного формата

            letter, name = stem.split("-", 1)
            rel_path = file.relative_to(base_dir).as_posix()

            f.write(
                f"| {letter} | [{name}]({rel_path}) |  |  |\n"
            )


if __name__ == "__main__":
    generate_readme()
