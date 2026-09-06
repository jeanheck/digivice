import { ConditionConstant } from "@/constants/stat/condition.constant";

const statusAilmentByCode: Readonly<Partial<Record<number, ConditionConstant>>> = {
  1: ConditionConstant.poison,
  2: ConditionConstant.paralyze,
  4: ConditionConstant.confuse,
  8: ConditionConstant.sleep,
};

export function resolveStatusAilment(condition: number): ConditionConstant | null {
  return statusAilmentByCode[condition] ?? null;
}
