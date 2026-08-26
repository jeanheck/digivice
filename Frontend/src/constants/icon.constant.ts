import { AttributeConstant } from "./stat/attribute/attribute.constant";
import { ElementConstant } from "./stat/element.constant";
import { ConditionConstant } from "./stat/condition.constant";
import type { Constant as ConstantsType } from "./constant";
import { Constant } from "./constant";
import { SeabedConstant } from "./seabed.constant";
import { EnemySourceConstant } from "./enemy-source.constant";
import { SpeciesConstant } from "./species.constant";

export const IconConstant: Record<ConstantsType, string> = {
  [AttributeConstant.strength]: "👊",
  [AttributeConstant.defense]: "🛡️",
  [AttributeConstant.spirit]: "🧙‍♂️",
  [AttributeConstant.wisdom]: "📖",
  [AttributeConstant.speed]: "🏃",
  [AttributeConstant.charisma]: "✨",
  [ElementConstant.fire]: "🔥",
  [ElementConstant.water]: "💧",
  [ElementConstant.ice]: "🧊",
  [ElementConstant.wind]: "🍃",
  [ElementConstant.thunder]: "⚡",
  [ElementConstant.machine]: "⚙️",
  [ElementConstant.dark]: "🌑",
  [ConditionConstant.poison]: "☠️",
  [ConditionConstant.paralyze]: "🗲",
  [ConditionConstant.confuse]: "😵",
  [ConditionConstant.sleep]: "💤",
  [ConditionConstant.ko]: "💀",
  [ConditionConstant.drain]: "🧛",
  [ConditionConstant.steal]: "🦝",
  [ConditionConstant.escape]: "🏃",
  [Constant.physical]: "👊",
  [Constant.magical]: "🧙‍♂️",
  [Constant.heal]: "💚",
  [Constant.support]: "🟡",
  [Constant.field]: "🔵",
  [SeabedConstant.topLeft]: "↖️",
  [SeabedConstant.top]: "⬆️",
  [SeabedConstant.topRight]: "↗️",
  [EnemySourceConstant.walking]: "🏃‍➡️",
  [EnemySourceConstant.fishing]: "🎣",
  [EnemySourceConstant.kickingTree]: "🌴",
  [EnemySourceConstant.boss]: "☠️",
  [SpeciesConstant.insect]: "🪰",
  [SpeciesConstant.dino]: "🦕",
  [SpeciesConstant.machine]: "🤖",
  [SpeciesConstant.mammal]: "🐕",
  [SpeciesConstant.fish]: "🐟",
  [SpeciesConstant.evil]: "😈",
  [SpeciesConstant.plant]: "🌿",
  [SpeciesConstant.bird]: "🐦",
  [SpeciesConstant.dragon]: "🐉",
  [SpeciesConstant.ghoul]: "👻",
  [SpeciesConstant.rare]: "✨",
};
