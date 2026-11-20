import type { PartyAddresses, PartyAddressesRaw } from "@/memory/addresses/party.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";
import { SlotAddressesConverter } from "@/memory/converters/parties/slot-addresses.converter";

export class PartyAddressesConverter {
  public static convert(raw: PartyAddressesRaw): PartyAddresses {
    return {
      slots: (raw.slots ?? []).map((slot) => SlotAddressesConverter.convert(slot)),
      emptySlotId: AddressValueConverter.convert(raw.emptySlotId),
    };
  }
}
