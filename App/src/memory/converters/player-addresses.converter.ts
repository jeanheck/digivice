import type { PlayerAddresses, PlayerAddressesRaw } from "@/memory/addresses/player.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

export class PlayerAddressesConverter {
  public static convert(raw: PlayerAddressesRaw): PlayerAddresses {
    return {
      bits: AddressValueConverter.convert(raw.bits),
      mapId: AddressValueConverter.convert(raw.mapId),
      previousMapId: AddressValueConverter.convert(raw.previousMapId),
      seabedRoute: AddressValueConverter.convert(raw.seabedRoute),
      mapVariant: AddressValueConverter.convert(raw.mapVariant),
    };
  }
}
