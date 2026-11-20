import type {
  DigimonBattleAddresses,
  DigimonBattleAddressesRaw,
} from "@/memory/addresses/digimon-battle.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

export class DigimonBattleAddressesConverter {
  public static convert(raw: DigimonBattleAddressesRaw): DigimonBattleAddresses {
    return {
      field: AddressValueConverter.convert(raw.field),
    };
  }
}
