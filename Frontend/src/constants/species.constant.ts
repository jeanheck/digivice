export enum SpeciesConstant {
  insect = "insect",
  dino = "dino",
  machine = "speciesMachine",
  mammal = "mammal",
  fish = "fish",
  evil = "evil",
  plant = "plant",
  bird = "bird",
  dragon = "dragon",
  ghoul = "ghoul",
  rare = "rare",
}

const SPECIES_RAW_TO_CONSTANT: Record<string, SpeciesConstant> = {
  insect: SpeciesConstant.insect,
  dino: SpeciesConstant.dino,
  machine: SpeciesConstant.machine,
  mammal: SpeciesConstant.mammal,
  fish: SpeciesConstant.fish,
  evil: SpeciesConstant.evil,
  plant: SpeciesConstant.plant,
  bird: SpeciesConstant.bird,
  dragon: SpeciesConstant.dragon,
  ghoul: SpeciesConstant.ghoul,
  rare: SpeciesConstant.rare,
};

export function toSpeciesConstant(species: string | null): SpeciesConstant | null {
  if (species === null || species.trim() === "") {
    return null;
  }

  return SPECIES_RAW_TO_CONSTANT[species] ?? null;
}
