import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

export interface AttributesAddressesRaw {
  strength: AddressValueRaw;
  defense: AddressValueRaw;
  spirit: AddressValueRaw;
  wisdom: AddressValueRaw;
  speed: AddressValueRaw;
  charisma: AddressValueRaw;
}

export interface AttributesAddresses {
  strength: number;
  defense: number;
  spirit: number;
  wisdom: number;
  speed: number;
  charisma: number;
}
