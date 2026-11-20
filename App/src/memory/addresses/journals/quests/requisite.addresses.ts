import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

export interface RequisiteAddressesRaw {
  id: string;
  address: AddressValueRaw;
  bitMasks?: AddressValueRaw[];
}

export interface RequisiteAddresses {
  id: string;
  address: number;
  bitMasks: number[];
}
