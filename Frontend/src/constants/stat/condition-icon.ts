import { Condition } from "@/constants/stat/condition";

export const ConditionIcon: Record<Condition, string> = {
    [Condition.poison]: "☠️",
    [Condition.paralyze]: "⚡",
    [Condition.confuse]: "😵",
    [Condition.sleep]: "💤",
    [Condition.ko]: "💀",
    [Condition.drain]: "🧛",
    [Condition.steal]: "🦝",
    [Condition.escape]: "🏃",
};
