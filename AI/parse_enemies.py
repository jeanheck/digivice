import json
import os
import re

txt_path = r'c:\Projetos\digivice\AI\enemies.txt'
json_path = r'c:\Projetos\digivice\Frontend\src\data\static\EnemiesTable.json'

with open(txt_path, 'r', encoding='utf-8') as f:
    lines = f.readlines()

parsed_data = {}
current_name = None
current_str = None

for line in lines:
    line = line.strip()
    if line.startswith('----'):
        current_name = None
        current_str = None
        continue
    
    if not line:
        current_str = None
        continue
        
    if current_name is None and not line.startswith('Note:') and not line.startswith('***') and not line.startswith('('):
        current_name = line.strip()
        parsed_data[current_name] = {'Location': '', 'RegularAttack': '', 'Technique': ''}
        current_str = None
        continue
        
    if current_name:
        if line.startswith('Location:'):
            parsed_data[current_name]['Location'] = line.replace('Location:', '').strip()
            current_str = 'Location'
        elif line.startswith('Regular Attack:'):
            parsed_data[current_name]['RegularAttack'] = line.replace('Regular Attack:', '').strip()
            current_str = 'RegularAttack'
        elif line.startswith('Technique:'):
            parsed_data[current_name]['Technique'] = line.replace('Technique:', '').strip()
            current_str = 'Technique'
        elif line.startswith('Drops:') or line.startswith('Fishing:') or line.startswith('Kicking Trees:') or line.startswith('EXP:') or line.startswith('Stats:') or ('STR' in line and 'DEF' in line) or ('FIRE' in line and 'WATER' in line):
            current_str = None
        elif current_str:
            # Append multiline strings
            parsed_data[current_name][current_str] += ' ' + line.strip()

with open(json_path, 'r', encoding='utf-8') as f:
    data = json.load(f)

for item in data:
    name = item.get('Name')
    
    match_name = None
    if name in parsed_data:
        match_name = name
    elif f"{name} (Boss)" in parsed_data:
        match_name = f"{name} (Boss)"
    else:
        # Fallback for 2* names like "Airdramon 2*" mapped to "Airdramon 2* (Gold Colored)"
        for k in parsed_data.keys():
            if k.startswith(name + " 2*") or k.startswith(name + " 3*") or k.startswith(name + " 4*") or k.startswith(name + " 5*") or k.startswith(name + " 6*"):
                match_name = k
                break
            # Also try to match ignoring colors for exact matching start
            if k.startswith(name + " (") or k.startswith(name + "  ("): 
                match_name = k
                break
                
    if match_name:
        # Clean up any excessive spaces
        loc = re.sub(' +', ' ', parsed_data[match_name]['Location'])
        reg = re.sub(' +', ' ', parsed_data[match_name]['RegularAttack'])
        tech = re.sub(' +', ' ', parsed_data[match_name]['Technique'])
        
        item['Location'] = loc
        item['RegularAttack'] = reg
        item['Technique'] = tech
        
        # Clean up the old property if it exists
        if 'Regular Attack' in item:
            del item['Regular Attack']

with open(json_path, 'w', encoding='utf-8') as f:
    json.dump(data, f, indent=4)

print("Parsed stats for", len(parsed_data), "enemies.")
print("Updated EnemiesTable.json successfully.")
