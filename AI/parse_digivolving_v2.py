import json
import re

def parse_requirements():
    with open("c:/Projetos/digivice/Frontend/src/data/static/DigivolvingRequirementsTable.json", "r", encoding="utf-8") as f:
        data = json.load(f)

    rookies = ["Agumon", "Guilmon", "Kotemon", "Kumamon", "Monmon", "Patamon", "Renamon", "Veemon"]
    
    parsed_data = {}
    
    for rookie, evolutions in data.items():
        parsed_data[rookie] = {}
        for evo_name, req_string in evolutions.items():
            reqs = []
            
            # Split by '+'
            parts = [p.strip() for p in req_string.split("+")]
            
            for part in parts:
                part = part.strip()
                if " lv " in part:
                    entity_name, level_str = part.split(" lv ")
                    entity_name = entity_name.strip()
                    level = int(level_str.strip())
                    
                    if entity_name in rookies:
                        # If it's the current rookie, just use DigimonLevel
                        if entity_name == rookie:
                            reqs.append({
                                "Type": "DigimonLevel",
                                "Value": level
                            })
                        else:
                            # It's a different rookie, we classify it as DigimonLevel but keep the name.
                            # Wait, the user didn't specify what to do with OTHER rookies.
                            # Let's say if it's ANY rookie, it's a DigimonLevel, but we include Digimon: <name>
                            # if it's different. But to be safe, we'll just use DigimonLevel with Digimon name if it's not the parent.
                            if entity_name == rookie:
                                reqs.append({"Type": "DigimonLevel", "Value": level})
                            else:
                                reqs.append({"Type": "DigimonLevel", "Digimon": entity_name, "Value": level})
                    else:
                        reqs.append({
                            "Type": "DigievolutionLevel",
                            "Digievolution": entity_name,
                            "Value": level
                        })
                else:
                    # Attribute
                    # Extract the word and the number
                    # e.g., "Speed 280", "Dark 140"
                    match = re.match(r"([A-Za-z]+)\s+(\d+)", part)
                    if match:
                        attr_name = match.group(1).strip()
                        value = int(match.group(2))
                        reqs.append({
                            "Type": "Attribute",
                            "Attribute": attr_name,
                            "Value": value
                        })
                    else:
                        print(f"Failed to parse attribute part: {part}")
            
            parsed_data[rookie][evo_name] = reqs
            
    with open("c:/Projetos/digivice/Frontend/src/data/static/DigivolvingRequirementsTable_v2.json", "w", encoding="utf-8") as f:
        json.dump(parsed_data, f, indent=4)
        print("Generated DigivolvingRequirementsTable_v2.json successfully.")

if __name__ == "__main__":
    parse_requirements()
