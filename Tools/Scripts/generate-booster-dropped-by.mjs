import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const scriptDirectory = path.dirname(fileURLToPath(import.meta.url));
const repositoryRoot = path.resolve(scriptDirectory, "../..");

const boosterPath = path.join(repositoryRoot, "Frontend/src/database/tcg/booster.json");
const enemyPath = path.join(repositoryRoot, "Frontend/src/database/enemy/enemy.json");
const tamerPath = path.join(repositoryRoot, "Frontend/src/database/npc/tamer.json");
const duelIslandPath = path.join(repositoryRoot, "Frontend/src/database/npc/duel-island.json");

function readJson(filePath) {
  return JSON.parse(fs.readFileSync(filePath, "utf8"));
}

function writeJson(filePath, value) {
  fs.writeFileSync(filePath, `${JSON.stringify(value, null, 2)}\n`, "utf8");
}

function entryKey(entry) {
  return `${entry.kind}|${entry.id}|${entry.locationOnly ?? ""}`;
}

function pushUnique(droppedByByBoosterId, boosterId, nextEntry) {
  const entries = droppedByByBoosterId.get(boosterId) ?? [];
  const alreadyPresent = entries.some((entry) => {
    return entryKey(entry) === entryKey(nextEntry);
  });
  if (alreadyPresent) {
    return;
  }

  entries.push(nextEntry);
  droppedByByBoosterId.set(boosterId, entries);
}

function collectFromCardBattles(npcTable, kind, droppedByByBoosterId) {
  for (const [npcId, npcRaw] of Object.entries(npcTable)) {
    for (const cardBattle of Object.values(npcRaw.cardBattles ?? {})) {
      pushUnique(droppedByByBoosterId, String(cardBattle.boosterId), {
        kind,
        id: npcId,
      });
    }
  }
}

const boosterTable = readJson(boosterPath);
const enemyTable = readJson(enemyPath);
const tamerTable = readJson(tamerPath);
const duelIslandTable = readJson(duelIslandPath);

const droppedByByBoosterId = new Map();

for (const [enemyId, enemyRaw] of Object.entries(enemyTable)) {
  for (const drop of enemyRaw.drops ?? []) {
    if (drop.type !== "booster") {
      continue;
    }

    const nextEntry = {
      kind: "enemy",
      id: enemyId,
    };
    if (drop.locationOnly !== undefined) {
      nextEntry.locationOnly = drop.locationOnly;
    }

    pushUnique(droppedByByBoosterId, String(drop.dropId), nextEntry);
  }
}

collectFromCardBattles(tamerTable, "tamer", droppedByByBoosterId);
collectFromCardBattles(duelIslandTable, "duelIsland", droppedByByBoosterId);

const kindOrder = {
  duelIsland: 0,
  enemy: 1,
  tamer: 2,
};

for (const [boosterId, boosterRaw] of Object.entries(boosterTable)) {
  const entries = droppedByByBoosterId.get(boosterId) ?? [];

  entries.sort((first, second) => {
    const byKind = (kindOrder[first.kind] ?? 99) - (kindOrder[second.kind] ?? 99);
    if (byKind !== 0) {
      return byKind;
    }

    const byId = first.id.localeCompare(second.id);
    if (byId !== 0) {
      return byId;
    }

    return (first.locationOnly ?? "").localeCompare(second.locationOnly ?? "");
  });

  if (entries.length > 0) {
    boosterRaw.droppedBy = entries;
  } else {
    delete boosterRaw.droppedBy;
  }
}

writeJson(boosterPath, boosterTable);

const withSources = Object.values(boosterTable).filter((boosterRaw) => {
  return (boosterRaw.droppedBy?.length ?? 0) > 0;
}).length;

const withEnemy = Object.values(boosterTable).filter((boosterRaw) => {
  return (boosterRaw.droppedBy ?? []).some((entry) => {
    return entry.kind === "enemy";
  });
}).length;

console.log(
  `Updated booster.droppedBy: ${withSources}/${Object.keys(boosterTable).length} have sources (${withEnemy} include enemy).`,
);
