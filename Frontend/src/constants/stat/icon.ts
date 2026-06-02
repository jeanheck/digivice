import { StatKey } from "@/constants/stat/stat-key";
import { Attribute } from "./attribute/attribute";
import { Element } from "./element";
import { Condition } from "./condition";

export const Icon: Record<StatKey, string> = {
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
};
