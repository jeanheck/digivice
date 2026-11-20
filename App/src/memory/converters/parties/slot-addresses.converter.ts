import type { SlotAddresses, SlotAddressesRaw } from "@/memory/addresses/parties/slot.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

export class SlotAddressesConverter {
  public static convert(raw: SlotAddressesRaw): SlotAddresses {
    return {
      index: AddressValueConverter.convert(raw.index),
      address: AddressValueConverter.convert(raw.address),
    };
  }
}
