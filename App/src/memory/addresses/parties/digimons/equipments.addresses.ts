import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

export interface EquipmentsAddressesRaw {
  head: AddressValueRaw;
  body: AddressValueRaw;
  right: AddressValueRaw;
  left: AddressValueRaw;
  accessory1: AddressValueRaw;
  accessory2: AddressValueRaw;
}

export interface EquipmentsAddresses {
  head: number;
  body: number;
  right: number;
  left: number;
  accessory1: number;
  accessory2: number;
}
