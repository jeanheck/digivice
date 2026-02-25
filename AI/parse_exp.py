import json
import re
import os

input_file = r'c:\Projetos\digivice\AI\gamefaqs_exp_data.txt'
output_dir = r'c:\Projetos\digivice\Frontend\src\data\static'
output_file = os.path.join(output_dir, 'expTable.json')

if not os.path.exists(output_dir):
    os.makedirs(output_dir)

exp_table = {}
current_digimon = None

with open(input_file, 'r', encoding='utf-8') as f:
    for line in f:
        line = line.strip()
        if not line:
            continue
        
        # Check if line is a digimon header like "1- Agumon:" or "2- Guilmon"
        match_header = re.match(r'^\d+-\s*([A-Za-z]+):?$', line)
        if match_header:
            current_digimon = match_header.group(1)
            exp_table[current_digimon] = []
            continue
        
        if line == 'Lv Exp':
            continue
            
        # Match "Level Exp" pattern e.g "01 0", "99 876710"
        match = re.match(r'^(\d+)\s+(\d+)$', line)
        if match and current_digimon:
            level = int(match.group(1))
            exp = int(match.group(2))
            if 1 <= level <= 99:
                exp_table[current_digimon].append({str(level): exp})

with open(output_file, 'w', encoding='utf-8') as f:
    json.dump(exp_table, f, indent=2)

print(f"Successfully generated {output_file} with {len(exp_table.keys())} Digimons.")
