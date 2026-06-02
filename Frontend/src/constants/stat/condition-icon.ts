import { Condition } from "@/constants/stat/condition";

export const ConditionIcon: Record<Condition, string> = {
    [Condition.canPoison]: "☠️",
    [Condition.canParalyze]: "⚡",
    [Condition.canConfuse]: "😵",
    [Condition.canSleep]: "💤",
    [Condition.canKo]: "💀",
    [Condition.canDrain]: "🧛",
    [Condition.canSteal]: "🦝",
    [Condition.canEscape]: "🏃",
};
