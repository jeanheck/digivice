import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";
import type { SlotAddresses, SlotAddressesRaw } from "@/memory/addresses/parties/slot.addresses";

export interface PartyAddressesRaw {
  slots: SlotAddressesRaw[];
  emptySlotId: AddressValueRaw;
}

export interface PartyAddresses {
  slots: SlotAddresses[];
  emptySlotId: number;
}
