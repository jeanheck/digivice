import {
  DigimonElements,
  WeakElementValue,
  type DigimonElement,
} from "@/constants/digimon-element.constant";

export interface EnemyAffinities {
  effective: DigimonElement[];
  notEffective: DigimonElement[];
}

export class EnemyAffinityService {
  public static getAffinities(elements: Record<DigimonElement, number>): EnemyAffinities {
    const values = DigimonElements.map((element) => elements[element]);
    const firstValue = values[0];

    if (values.every((value) => value === firstValue)) {
      return { effective: [], notEffective: [] };
    }

    const effective = DigimonElements.filter((element) => {
      return elements[element] === WeakElementValue;
    });

    return {
      effective,
      notEffective: EnemyAffinityService.getNotEffective(elements),
    };
  }

  private static getNotEffective(elements: Record<DigimonElement, number>): DigimonElement[] {
    const candidates = DigimonElements.filter((element) => {
      return elements[element] !== WeakElementValue;
    });

    if (candidates.length === 0) {
      return [];
    }

    const candidateValues = candidates.map((element) => elements[element]);
    const highestValue = Math.max(...candidateValues);
    const lowestValue = Math.min(...candidateValues);

    if (highestValue === lowestValue) {
      return [];
    }

    return candidates.filter((element) => {
      return elements[element] === highestValue;
    });
  }
}
