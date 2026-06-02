import { Condition } from "@/constants/stat/condition";

export const BOOLEAN_CONDITION_KEYS = new Set<string>([
    Condition.canDrain,
    Condition.canSteal,
    Condition.canEscape,
]);

export const RESISTANCE_TOOLTIP_KEYS: Record<string, { resistance: string; canSuffer: string }> = {
    [Condition.canPoison]: {
        resistance: "conditions.resistancePoison",
        canSuffer: "conditions.canPoison",
    },
    [Condition.canParalyze]: {
        resistance: "conditions.resistanceParalyze",
        canSuffer: "conditions.canParalyze",
    },
    [Condition.canConfuse]: {
        resistance: "conditions.resistanceConfuse",
        canSuffer: "conditions.canConfuse",
    },
    [Condition.canSleep]: {
        resistance: "conditions.resistanceSleep",
        canSuffer: "conditions.canSleep",
    },
    [Condition.canKo]: {
        resistance: "conditions.resistanceKo",
        canSuffer: "conditions.canKo",
    },
};
