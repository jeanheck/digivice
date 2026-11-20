import type {
  InBattleAddresses,
  InBattleAddressesRaw,
} from "@/memory/addresses/parties/in-battle.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";
import { VitalAddressesConverter } from "@/memory/converters/parties/digimons/vital-addresses.converter";

export class InBattleAddressesConverter {
  public static convert(raw: InBattleAddressesRaw): InBattleAddresses {
    return {
      allySlotBase: AddressValueConverter.convert(raw.allySlotBase),
      slotStride: AddressValueConverter.convert(raw.slotStride),
      condition: AddressValueConverter.convert(raw.condition),
      strength: AddressValueConverter.convert(raw.strength),
      defense: AddressValueConverter.convert(raw.defense),
      speed: AddressValueConverter.convert(raw.speed),
      hp: VitalAddressesConverter.convert(raw.hp),
      mp: VitalAddressesConverter.convert(raw.mp),
    };
  }
}
