export const TechniqueIcon: Record<string, string> = {
  Physical: '👊',
  Magical: '🧙‍♂️',
  Heal: '💚',
  Support: '🟡',
};

export function getTechniqueTypeIcon(type: string): string {
  const normalizedType = type.charAt(0).toUpperCase() + type.slice(1).toLowerCase();

  return TechniqueIcon[normalizedType] ?? "?";
}