import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

export interface ElementsAddressesRaw {
  fire: AddressValueRaw;
  water: AddressValueRaw;
  ice: AddressValueRaw;
  wind: AddressValueRaw;
  thunder: AddressValueRaw;
  machine: AddressValueRaw;
  dark: AddressValueRaw;
}

export interface ElementsAddresses {
  fire: number;
  water: number;
  ice: number;
  wind: number;
  thunder: number;
  machine: number;
  dark: number;
}
