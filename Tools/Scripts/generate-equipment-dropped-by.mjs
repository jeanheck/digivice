import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const scriptDirectory = path.dirname(fileURLToPath(import.meta.url));
const repositoryRoot = path.resolve(scriptDirectory, "../..");

const equipmentPath = path.join(repositoryRoot, "Frontend/src/database/equipment/equipment.json");
const enemyPath = path.join(repositoryRoot, "Frontend/src/database/enemy/enemy.json");

function readJson(filePath) {
  return JSON.parse(fs.readFileSync(filePath, "utf8"));
}

function writeJson(filePath, value) {
  fs.writeFileSync(filePath, `${JSON.stringify(value, null, 2)}\n`, "utf8");
}

function entryKey(entry) {
  return `${entry.kind}|${entry.id}|${entry.locationOnly ?? ""}`;
}

const equipmentTable = readJson(equipmentPath);
const enemyTable = readJson(enemyPath);

const droppedByByEquipmentId = new Map();

for (const [enemyId, enemyRaw] of Object.entries(enemyTable)) {
  for (const drop of enemyRaw.drops ?? []) {
    if (drop.type !== "equipment") {
      continue;
    }

    const equipmentId = String(drop.dropId);
    const entries = droppedByByEquipmentId.get(equipmentId) ?? [];
    const nextEntry = {
      kind: "enemy",
      id: enemyId,
    };
    if (drop.locationOnly !== undefined) {
      nextEntry.locationOnly = drop.locationOnly;
    }

    const alreadyPresent = entries.some((entry) => {
      return entryKey(entry) === entryKey(nextEntry);
    });
    if (alreadyPresent) {
      continue;
    }

    entries.push(nextEntry);
    droppedByByEquipmentId.set(equipmentId, entries);
  }
}

for (const [equipmentId, equipmentRaw] of Object.entries(equipmentTable)) {
  const entries = droppedByByEquipmentId.get(equipmentId) ?? [];

  entries.sort((first, second) => {
    const byId = first.id.localeCompare(second.id);
    if (byId !== 0) {
      return byId;
    }

    return (first.locationOnly ?? "").localeCompare(second.locationOnly ?? "");
  });

  if (entries.length > 0) {
    equipmentRaw.droppedBy = entries;
  } else {
    delete equipmentRaw.droppedBy;
  }
}

writeJson(equipmentPath, equipmentTable);

const withSources = Object.values(equipmentTable).filter((equipmentRaw) => {
  return (equipmentRaw.droppedBy?.length ?? 0) > 0;
}).length;

console.log(
  `Updated equipment.droppedBy from enemy: ${withSources}/${Object.keys(equipmentTable).length} items have sources.`,
);
