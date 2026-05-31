export const TechniqueElementColorClass: Record<string, string> = {
    fire: "text-orange-400",
    water: "text-blue-400",
    ice: "text-cyan-300",
    wind: "text-gray-300",
    thunder: "text-yellow-300",
    dark: "text-purple-400",
    machine: "text-gray-400",
    none: "text-white/60",
};

export function getTechniqueElementColorClass(element: string): string {
    const normalizedElement = element.toLowerCase();

    return TechniqueElementColorClass[normalizedElement] ?? "text-white/60";
}
