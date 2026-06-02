import { Attribute } from "./stat/attribute/attribute";
import { Element } from "./stat/element";
import { Condition } from "./stat/condition";
import type { Constants as ConstantsType } from "./constants";
import { Constants } from "./constants";

export const IconConstants: Record<ConstantsType, string> = {
    [Attribute.strength]: "👊",
    [Attribute.defense]: "🛡️",
    [Attribute.spirit]: "🧙‍♂️",
    [Attribute.wisdom]: "📖",
    [Attribute.speed]: "🏃",
    [Attribute.charisma]: "✨",
    [Element.fire]: "🔥",
    [Element.water]: "💧",
    [Element.ice]: "🧊",
    [Element.wind]: "🍃",
    [Element.thunder]: "⚡",
    [Element.machine]: "⚙️",
    [Element.dark]: "🌑",
    [Condition.poison]: "☠️",
    [Condition.paralyze]: "⚡",
    [Condition.confuse]: "😵",
    [Condition.sleep]: "💤",
    [Condition.ko]: "💀",
    [Condition.drain]: "🧛",
    [Condition.steal]: "🦝",
    [Condition.escape]: "🏃",
    [Constants.physical]: "👊",
    [Constants.magical]: "🧙‍♂️",
    [Constants.heal]: "💚",
    [Constants.support]: "🟡",
};
