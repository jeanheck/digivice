import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

export interface VitalAddressesRaw {
  current: AddressValueRaw;
  max: AddressValueRaw;
}

export interface VitalAddresses {
  current: number;
  max: number;
}
