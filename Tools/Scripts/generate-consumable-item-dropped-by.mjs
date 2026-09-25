import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const scriptDirectory = path.dirname(fileURLToPath(import.meta.url));
const repositoryRoot = path.resolve(scriptDirectory, "../..");

const consumableItemPath = path.join(
  repositoryRoot,
  "Frontend/src/database/consumable-item/consumable-item.json",
);
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

const consumableItemTable = readJson(consumableItemPath);
const enemyTable = readJson(enemyPath);

const droppedByByConsumableId = new Map();

for (const [enemyId, enemyRaw] of Object.entries(enemyTable)) {
  for (const drop of enemyRaw.drops ?? []) {
    if (drop.type !== "consumableItem") {
      continue;
    }

    const consumableId = String(drop.dropId);
    const entries = droppedByByConsumableId.get(consumableId) ?? [];
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
    droppedByByConsumableId.set(consumableId, entries);
  }
}

for (const [consumableId, consumableRaw] of Object.entries(consumableItemTable)) {
  const entries = droppedByByConsumableId.get(consumableId) ?? [];

  entries.sort((first, second) => {
    const byId = first.id.localeCompare(second.id);
    if (byId !== 0) {
      return byId;
    }

    return (first.locationOnly ?? "").localeCompare(second.locationOnly ?? "");
  });

  consumableRaw.droppedBy = entries;
}

writeJson(consumableItemPath, consumableItemTable);

const withSources = Object.values(consumableItemTable).filter((consumableRaw) => {
  return consumableRaw.droppedBy.length > 0;
}).length;

console.log(
  `Updated consumable-item.droppedBy from enemy: ${withSources}/${Object.keys(consumableItemTable).length} items have sources.`,
);
