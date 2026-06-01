import { StatKey } from "@/constants/stat/stat-key";
import { Attribute } from "./attribute/attribute";

export const StatIcon: Record<StatKey, string> = {
    [Attribute.strength]: "👊",
    [StatKey.defense]: "🛡️",
    [StatKey.spirit]: "🧙‍♂️",
    [StatKey.wisdom]: "📖",
    [StatKey.speed]: "🏃",
    [StatKey.charisma]: "✨",
    [StatKey.fire]: "🔥",
    [StatKey.water]: "💧",
    [StatKey.ice]: "🧊",
    [StatKey.wind]: "🍃",
    [StatKey.thunder]: "⚡",
    [StatKey.machine]: "⚙️",
    [StatKey.dark]: "🌑",
};
