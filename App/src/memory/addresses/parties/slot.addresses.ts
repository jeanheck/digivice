import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

export interface SlotAddressesRaw {
  index: AddressValueRaw;
  address: AddressValueRaw;
}

export interface SlotAddresses {
  index: number;
  address: number;
}
