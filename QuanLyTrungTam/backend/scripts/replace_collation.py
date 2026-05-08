import pathlib
p = pathlib.Path(__file__).resolve().parent.parent / 'Migrations'
old = 'utf8mb4_0900_ai_ci'
new = 'utf8mb4_general_ci'
count = 0
for fp in p.glob('**/*.cs'):
    text = fp.read_text(encoding='utf-8')
    if old in text:
        text2 = text.replace(old, new)
        fp.write_text(text2, encoding='utf-8')
        print(f'Updated {fp}')
        count += 1
print(f'Done. Files updated: {count}')
