import { Condition } from "@/constants/stat/condition";

interface ResistanceConditionDefinition {
    kind: "resistance";
    canField: "canPoison" | "canParalyze" | "canConfuse" | "canSleep" | "canKO";
    valueField: "poison" | "paralyze" | "confuse" | "sleep" | "ko";
    resistanceTooltipKey: string;
    canSufferTooltipKey: string;
}

interface BooleanConditionDefinition {
    kind: "boolean";
    canField: "canDrain" | "canSteal" | "canEscape";
    tooltipKey: string;
}

export type ConditionDefinition = ResistanceConditionDefinition | BooleanConditionDefinition;

export const ConditionDefinition: Record<Condition, ConditionDefinition> = {
    [Condition.poison]: {
        kind: "resistance",
        canField: "canPoison",
        valueField: "poison",
        resistanceTooltipKey: "conditions.resistancePoison",
        canSufferTooltipKey: "conditions.canPoison",
    },
    [Condition.paralyze]: {
        kind: "resistance",
        canField: "canParalyze",
        valueField: "paralyze",
        resistanceTooltipKey: "conditions.resistanceParalyze",
        canSufferTooltipKey: "conditions.canParalyze",
    },
    [Condition.confuse]: {
        kind: "resistance",
        canField: "canConfuse",
        valueField: "confuse",
        resistanceTooltipKey: "conditions.resistanceConfuse",
        canSufferTooltipKey: "conditions.canConfuse",
    },
    [Condition.sleep]: {
        kind: "resistance",
        canField: "canSleep",
        valueField: "sleep",
        resistanceTooltipKey: "conditions.resistanceSleep",
        canSufferTooltipKey: "conditions.canSleep",
    },
    [Condition.ko]: {
        kind: "resistance",
        canField: "canKO",
        valueField: "ko",
        resistanceTooltipKey: "conditions.resistanceKo",
        canSufferTooltipKey: "conditions.canKo",
    },
    [Condition.drain]: {
        kind: "boolean",
        canField: "canDrain",
        tooltipKey: "conditions.drain",
    },
    [Condition.steal]: {
        kind: "boolean",
        canField: "canSteal",
        tooltipKey: "conditions.steal",
    },
    [Condition.escape]: {
        kind: "boolean",
        canField: "canEscape",
        tooltipKey: "conditions.escape",
    },
};
