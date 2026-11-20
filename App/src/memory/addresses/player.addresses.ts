import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

export interface PlayerAddressesRaw {
  bits: AddressValueRaw;
  mapId: AddressValueRaw;
  previousMapId: AddressValueRaw;
  seabedRoute: AddressValueRaw;
  mapVariant: AddressValueRaw;
}

export interface PlayerAddresses {
  bits: number;
  mapId: number;
  previousMapId: number;
  seabedRoute: number;
  mapVariant: number;
}
