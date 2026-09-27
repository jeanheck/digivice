import type { DigimonDigievolutionRaw } from "./digimon-digievolution.raw";

export interface DigimonRaw {
  name: string;
  experience: Record<string, number>;
  digievolutions: DigimonDigievolutionRaw;
}
