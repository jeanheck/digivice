import type { DigimonStatus } from "@/shared/types/digimon-status.type";

export const DigimonStatusColor: Readonly<Record<DigimonStatus, string>> = {
  healthy: "#00B6BF",
  injured: "#A3D956",
  debuffed: "#CB9200",
  ko: "#760F08",
};
