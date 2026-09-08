import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const scriptDirectory = path.dirname(fileURLToPath(import.meta.url));
const repositoryRoot = path.resolve(scriptDirectory, "../..");

const boosterPath = path.join(repositoryRoot, "Frontend/src/database/tcg/booster.json");
const tamerPath = path.join(repositoryRoot, "Frontend/src/database/npc/tamer.json");
const duelIslandPath = path.join(repositoryRoot, "Frontend/src/database/npc/duel-island.json");

const NPC_KINDS = new Set(["tamer", "duelIsland"]);

function readJson(filePath) {
  return JSON.parse(fs.readFileSync(filePath, "utf8"));
}

function writeJson(filePath, value) {
  fs.writeFileSync(filePath, `${JSON.stringify(value, null, 2)}\n`, "utf8");
}

function collectBoosterIdsFromCardBattles(npcTable, kind, droppedByByBoosterId) {
  for (const [npcId, npcRaw] of Object.entries(npcTable)) {
    for (const cardBattle of Object.values(npcRaw.cardBattles ?? {})) {
      const boosterId = String(cardBattle.boosterId);
      const entries = droppedByByBoosterId.get(boosterId) ?? [];
      const alreadyPresent = entries.some((entry) => {
        return entry.kind === kind && entry.id === npcId;
      });
      if (alreadyPresent) {
        continue;
      }

      entries.push({ kind, id: npcId });
      droppedByByBoosterId.set(boosterId, entries);
    }
  }
}

const boosterTable = readJson(boosterPath);
const tamerTable = readJson(tamerPath);
const duelIslandTable = readJson(duelIslandPath);

const droppedByByBoosterId = new Map();

collectBoosterIdsFromCardBattles(tamerTable, "tamer", droppedByByBoosterId);
collectBoosterIdsFromCardBattles(duelIslandTable, "duelIsland", droppedByByBoosterId);

for (const [boosterId, boosterRaw] of Object.entries(boosterTable)) {
  const preservedEnemyEntries = (boosterRaw.droppedBy ?? []).filter((entry) => {
    return !NPC_KINDS.has(entry.kind);
  });
  const npcEntries = droppedByByBoosterId.get(boosterId) ?? [];

  npcEntries.sort((first, second) => {
    const byKind = first.kind.localeCompare(second.kind);
    if (byKind !== 0) {
      return byKind;
    }

    return first.id.localeCompare(second.id);
  });

  boosterRaw.droppedBy = [...preservedEnemyEntries, ...npcEntries];
}

writeJson(boosterPath, boosterTable);

const withNpcSources = Object.values(boosterTable).filter((boosterRaw) => {
  return boosterRaw.droppedBy.some((entry) => {
    return NPC_KINDS.has(entry.kind);
  });
}).length;

console.log(
  `Updated booster.droppedBy from tamer/duel-island: ${withNpcSources}/${Object.keys(boosterTable).length} boosters have NPC sources.`,
);
