import type {
  DigimonStatusAddresses,
  DigimonStatusAddressesRaw,
} from "@/memory/addresses/parties/digimon-status.addresses";
import type {
  AttributesAddresses,
  AttributesAddressesRaw,
} from "@/memory/addresses/parties/digimons/attributes.addresses";
import type {
  DigievolutionsAddresses,
  DigievolutionsAddressesRaw,
} from "@/memory/addresses/parties/digimons/digievolutions.addresses";
import type {
  ElementsAddresses,
  ElementsAddressesRaw,
} from "@/memory/addresses/parties/digimons/elements.addresses";
import type {
  EquipmentsAddresses,
  EquipmentsAddressesRaw,
} from "@/memory/addresses/parties/digimons/equipments.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";
import { VitalAddressesConverter } from "@/memory/converters/parties/digimons/vital-addresses.converter";
import { SlotAddressesConverter } from "@/memory/converters/parties/slot-addresses.converter";

export class DigimonStatusAddressesConverter {
  private static convertAttributes(raw: AttributesAddressesRaw): AttributesAddresses {
    return {
      strength: AddressValueConverter.convert(raw.strength),
      defense: AddressValueConverter.convert(raw.defense),
      spirit: AddressValueConverter.convert(raw.spirit),
      wisdom: AddressValueConverter.convert(raw.wisdom),
      speed: AddressValueConverter.convert(raw.speed),
      charisma: AddressValueConverter.convert(raw.charisma),
    };
  }

  private static convertElements(raw: ElementsAddressesRaw): ElementsAddresses {
    return {
      fire: AddressValueConverter.convert(raw.fire),
      water: AddressValueConverter.convert(raw.water),
      ice: AddressValueConverter.convert(raw.ice),
      wind: AddressValueConverter.convert(raw.wind),
      thunder: AddressValueConverter.convert(raw.thunder),
      machine: AddressValueConverter.convert(raw.machine),
      dark: AddressValueConverter.convert(raw.dark),
    };
  }

  private static convertEquipments(raw: EquipmentsAddressesRaw): EquipmentsAddresses {
    return {
      head: AddressValueConverter.convert(raw.head),
      body: AddressValueConverter.convert(raw.body),
      right: AddressValueConverter.convert(raw.right),
      left: AddressValueConverter.convert(raw.left),
      accessory1: AddressValueConverter.convert(raw.accessory1),
      accessory2: AddressValueConverter.convert(raw.accessory2),
    };
  }

  private static convertDigievolutions(raw: DigievolutionsAddressesRaw): DigievolutionsAddresses {
    return {
      activeDigievolution: AddressValueConverter.convert(raw.activeDigievolution),
      slots: (raw.slots ?? []).map((slot) => SlotAddressesConverter.convert(slot)),
      unlockedDigievolutionsStart: AddressValueConverter.convert(raw.unlockedDigievolutionsStart),
      unlockedDigievolutionEntryStride: AddressValueConverter.convert(
        raw.unlockedDigievolutionEntryStride,
      ),
      maxUnlockedDigievolutions: AddressValueConverter.convert(raw.maxUnlockedDigievolutions),
      id: AddressValueConverter.convert(raw.id),
      level: AddressValueConverter.convert(raw.level),
      dvexp: AddressValueConverter.convert(raw.dvexp),
    };
  }

  public static convert(raw: DigimonStatusAddressesRaw): DigimonStatusAddresses {
    return {
      experience: AddressValueConverter.convert(raw.experience),
      level: AddressValueConverter.convert(raw.level),
      tp: AddressValueConverter.convert(raw.tp),
      hp: VitalAddressesConverter.convert(raw.hp),
      mp: VitalAddressesConverter.convert(raw.mp),
      attributes: DigimonStatusAddressesConverter.convertAttributes(raw.attributes),
      elements: DigimonStatusAddressesConverter.convertElements(raw.elements),
      equipments: DigimonStatusAddressesConverter.convertEquipments(raw.equipments),
      digievolutions: DigimonStatusAddressesConverter.convertDigievolutions(raw.digievolutions),
    };
  }
}
