import type { EnemyAddresses, EnemyAddressesRaw } from "@/memory/addresses/enemy.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";
import { VitalAddressesConverter } from "@/memory/converters/parties/digimons/vital-addresses.converter";

export class EnemyAddressesConverter {
  public static convert(raw: EnemyAddressesRaw): EnemyAddresses {
    return {
      enemySlotBase: AddressValueConverter.convert(raw.enemySlotBase),
      slotStride: AddressValueConverter.convert(raw.slotStride),
      slotCount: AddressValueConverter.convert(raw.slotCount),
      activeEnemySlotIndex: AddressValueConverter.convert(raw.activeEnemySlotIndex),
      groupId: AddressValueConverter.convert(raw.groupId),
      id: AddressValueConverter.convert(raw.id),
      condition: AddressValueConverter.convert(raw.condition),
      strength: AddressValueConverter.convert(raw.strength),
      defense: AddressValueConverter.convert(raw.defense),
      speed: AddressValueConverter.convert(raw.speed),
      hp: VitalAddressesConverter.convert(raw.hp),
    };
  }
}
