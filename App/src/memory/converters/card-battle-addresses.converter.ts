import type {
  CardBattleAddresses,
  CardBattleAddressesRaw,
} from "@/memory/addresses/card-battle.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

export class CardBattleAddressesConverter {
  public static convert(raw: CardBattleAddressesRaw): CardBattleAddresses {
    return {
      id: AddressValueConverter.convert(raw.id),
    };
  }
}
