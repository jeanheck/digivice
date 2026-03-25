const fs = require('fs');
const path = require('path');

const baseDir = 'c:\\Projetos\\digivice\\Frontend\\src\\data\\static';

// --- EnemiesTable.json ---
function transformEnemies() {
    const p = path.join(baseDir, 'EnemiesTable.json');
    const d = JSON.parse(fs.readFileSync(p, 'utf-8'));

    const locTr = {
        "Asuka East": "Asuka Leste",
        "Asuka West": "Asuka Oeste",
        "Asuka South": "Asuka Sul",
        "Asuka North": "Asuka Norte",
        "Central Park": "Parque Central",
        "Protocol Ruins": "Ruínas Protocol",
        "Protocol Forest": "Floresta Protocol",
        "Divermon's Lake": "Lago do Divermon",
        "Wind Prairie": "Pradaria do Vento",
        "Kicking Forest": "Floresta dos Chutes",
        "Tyranno Valley": "Vale dos Tyranno",
        "East Station": "Estação Leste",
        "Wire Forest Entrance": "Entrada da Wire Forest",
        "West Wire Forest": "Wire Forest Oeste",
        "East Wire Forest": "Wire Forest Leste",
        "Forest Inn": "Pousada da Floresta",
        "Shell Beach": "Shell Beach",
        "Plug Cape": "Plug Cape",
        "Protocol Ruins": "Ruínas Protocol",
        "In The Dark": "No Escuro",
        "Jungle Grave": "Jungle Grave",
        "Dum Dum Factory": "Fábrica Dum Dum",
        "Operation Room": "Sala de Operação"
    };

    const elemTr = {
        "No Element": "Sem Elemento",
        "Fire Element": "Elemento Fogo",
        "Water Element": "Elemento Água",
        "Ice Element": "Elemento Gelo",
        "Wind Element": "Elemento Vento",
        "Thunder Element": "Elemento Trovão",
        "Machine Element": "Elemento Máquina",
        "Dark Element": "Elemento Trevas",
        "Thunder Elemental": "Elemento Trovão",
        "No Element (Chance of Poison)": "Sem Elemento (Chance de Envenenamento)",
        "Dark Elemental": "Elemento Trevas",
        "Machine Elemental": "Elemento Máquina",
        "Fire Elemental": "Elemento Fogo",
        "Water Elemental": "Elemento Água",
        "Ice Elemental": "Elemento Gelo",
        "Wind Elemental": "Elemento Vento"
    };

    const techTr = {
        "None": "Nenhum",
        "(Physical)": "(Físico)",
        "(Magic)": "(Mágico)",
        "(Physical, Chance of Poison)": "(Físico, Chance de Envenenamento)",
        "(Magic-Machine, Chance of KO)": "(Mágico-Máquina, Chance de KO)",
        "(Magic-Fire)": "(Mágico-Fogo)",
        "(Magic-Ice)": "(Mágico-Gelo)",
        "(Magic-Wind)": "(Mágico-Vento)",
        "(Magic-Thunder)": "(Mágico-Trovão)",
        "(Magic-Dark)": "(Mágico-Trevas)",
        "(Magic-Water)": "(Mágico-Água)",
        "Necromist (Physical, Chance of Poison)": "Névoa Necrótica (Físico, Chance de Envenenamento)",
        "Atomic Ray (Magic-Machine, Chance of KO)": "Raio Atômico (Mágico-Máquina, Chance de KO)",
        "Energy Suck (Magic)": "Suga Energia (Mágico)"
    };

    d.forEach(e => {
        // Name
        const name = e.Name;
        e.Name = { "PT-BR": name, "EN-US": name };

        // Location
        if (e.Location) {
            if (Array.isArray(e.Location)) {
                e.Location = e.Location.map(l => {
                    let translated = l;
                    for (const [en, pt] of Object.entries(locTr)) {
                        translated = translated.replace(en, pt);
                    }
                    return { "PT-BR": translated, "EN-US": l };
                });
            } else if (typeof e.Location === 'string') {
                const enLoc = e.Location;
                let ptLoc = enLoc;
                for (const [en, pt] of Object.entries(locTr)) {
                    ptLoc = ptLoc.replace(en, pt);
                }
                e.Location = { "PT-BR": ptLoc, "EN-US": enLoc };
            }
        }

        // RegularAttack
        if (e.RegularAttack) {
            const enAtk = e.RegularAttack;
            e.RegularAttack = { "PT-BR": elemTr[enAtk] || enAtk, "EN-US": enAtk };
        }

        // Technique
        if (e.Technique) {
            const enTech = e.Technique;
            let ptTech = enTech;
            for (const [en, pt] of Object.entries(techTr)) {
                ptTech = ptTech.replace(en, pt);
            }
            e.Technique = { "PT-BR": ptTech, "EN-US": enTech };
        }
    });

    fs.writeFileSync(p, JSON.stringify(d, null, 4), 'utf-8');
    console.log(`Transformed EnemiesTable.json`);
}

// --- Digievolution.json ---
function transformDigievolution() {
    const p = path.join(baseDir, 'Digievolution.json');
    const d = JSON.parse(fs.readFileSync(p, 'utf-8'));
    d.digievolutions.forEach(e => {
        e.name = { "PT-BR": e.name, "EN-US": e.name };
    });
    fs.writeFileSync(p, JSON.stringify(d, null, 4), 'utf-8');
    console.log(`Transformed Digievolution.json`);
}

// --- DigievolutionTechniques.json ---
function transformDigievolutionTechniques() {
    const p = path.join(baseDir, 'DigievolutionTechniques.json');
    const d = JSON.parse(fs.readFileSync(p, 'utf-8'));
    d.digievolutions.forEach(e => {
        e.name = { "PT-BR": e.name, "EN-US": e.name };
    });
    fs.writeFileSync(p, JSON.stringify(d, null, 4), 'utf-8');
    console.log(`Transformed DigievolutionTechniques.json`);
}

// --- DigievolutionTree.json ---
function transformDigievolutionTree() {
    const p = path.join(baseDir, 'DigievolutionTree.json');
    const d = JSON.parse(fs.readFileSync(p, 'utf-8'));
    // This file is an array of objects
    d.forEach(e => {
        e.Name = { "PT-BR": e.Name, "EN-US": e.Name };
    });
    fs.writeFileSync(p, JSON.stringify(d, null, 4), 'utf-8');
    console.log(`Transformed DigievolutionTree.json`);
}

transformEnemies();
transformDigievolution();
transformDigievolutionTechniques();
transformDigievolutionTree();
console.log('Done!');
