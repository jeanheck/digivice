import { Condition } from "@/constants/stat/condition";

interface ResistanceConditionDefinition {
    kind: "resistance";
    resistanceTooltipKey: string;
    canSufferTooltipKey: string;
}

interface BooleanConditionDefinition {
    kind: "boolean";
    tooltipKey: string;
}

export type ConditionDefinition = ResistanceConditionDefinition | BooleanConditionDefinition;

export const ConditionDefinition: Record<Condition, ConditionDefinition> = {
    [Condition.poison]: {
        kind: "resistance",
        resistanceTooltipKey: "conditions.resistancePoison",
        canSufferTooltipKey: "conditions.canPoison",
    },
    [Condition.paralyze]: {
        kind: "resistance",
        resistanceTooltipKey: "conditions.resistanceParalyze",
        canSufferTooltipKey: "conditions.canParalyze",
    },
    [Condition.confuse]: {
        kind: "resistance",
        resistanceTooltipKey: "conditions.resistanceConfuse",
        canSufferTooltipKey: "conditions.canConfuse",
    },
    [Condition.sleep]: {
        kind: "resistance",
        resistanceTooltipKey: "conditions.resistanceSleep",
        canSufferTooltipKey: "conditions.canSleep",
    },
    [Condition.ko]: {
        kind: "resistance",
        resistanceTooltipKey: "conditions.resistanceKo",
        canSufferTooltipKey: "conditions.canKo",
    },
    [Condition.drain]: {
        kind: "boolean",
        tooltipKey: "conditions.drain",
    },
    [Condition.steal]: {
        kind: "boolean",
        tooltipKey: "conditions.steal",
    },
    [Condition.escape]: {
        kind: "boolean",
        tooltipKey: "conditions.escape",
    },
};
