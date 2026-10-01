import type { DigimonAttribute } from "@/constants/digimon-attribute.constant";
import type { DigimonCondition } from "@/constants/digimon-condition.constant";
import type { DigimonElement } from "@/constants/digimon-element.constant";
import type { DigimonSpecies } from "@/types/digimon-species.type";
import type { DigimonStat } from "@/types/digimon-stat.type";
import type { EnemySource } from "@/types/enemy-source.type";
import type { NpcBattleKind } from "@/types/npc-battle-kind.type";
import type { SeabedDirection } from "@/types/seabed-direction.type";
import type { TechniqueType } from "@/types/technique-type.type";

export const DigimonAttributeIcon = {
  strength: "👊",
  defense: "🛡️",
  spirit: "🧙‍♂️",
  wisdom: "📖",
  speed: "🏃",
  charisma: "✨",
} as const satisfies Record<DigimonAttribute, string>;

export const DigimonElementIcon = {
  fire: "🔥",
  water: "💧",
  ice: "🧊",
  wind: "🍃",
  thunder: "⚡",
  machine: "⚙️",
  dark: "🌑",
} as const satisfies Record<DigimonElement, string>;

export const DigimonStatIcon = {
  ...DigimonAttributeIcon,
  ...DigimonElementIcon,
} as const satisfies Record<DigimonStat, string>;

export const DigimonConditionIcon = {
  poison: "☠️",
  paralyze: "🗲",
  confuse: "😵",
  sleep: "💤",
  ko: "💀",
  drain: "🧛",
  steal: "🦝",
  escape: "🏃",
} as const satisfies Record<DigimonCondition, string>;

export const DigimonSpeciesIcon = {
  insect: "🪰",
  dino: "🦕",
  machine: "🤖",
  beast: "🦁",
  fish: "🐟",
  evil: "😈",
  plant: "🌿",
  bird: "🐦",
  dragon: "🐉",
  ghoul: "👻",
  rare: "✨",
} as const satisfies Record<DigimonSpecies, string>;

export const TechniqueTypeIcon = {
  physical: "👊",
  magical: "🧙‍♂️",
  heal: "💚",
  support: "🟡",
  field: "🔵",
} as const satisfies Record<TechniqueType, string>;

export const EnemySourceIcon = {
  walking: "🏃‍➡️",
  fishing: "🎣",
  kickingTree: "🌴",
  boss: "☠️",
} as const satisfies Record<EnemySource, string>;

export const SeabedDirectionIcon = {
  topLeft: "↖️",
  top: "⬆️",
  topRight: "↗️",
} as const satisfies Record<SeabedDirection, string>;

export const NpcBattleKindIcon = {
  card: "🎴",
  digimon: "⚔️",
} as const satisfies Record<NpcBattleKind, string>;
