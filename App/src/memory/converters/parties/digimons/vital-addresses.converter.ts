import type {
  VitalAddresses,
  VitalAddressesRaw,
} from "@/memory/addresses/parties/digimons/vital.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

export class VitalAddressesConverter {
  public static convert(raw: VitalAddressesRaw): VitalAddresses {
    return {
      current: AddressValueConverter.convert(raw.current),
      max: AddressValueConverter.convert(raw.max),
    };
  }
}
