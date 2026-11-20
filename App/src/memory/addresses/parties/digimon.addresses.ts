import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

export interface DigimonAddressRaw {
  name: string;
  memoryBlockAddress: AddressValueRaw;
  blastAddress: AddressValueRaw;
}

export type DigimonsAddressesRaw = Record<string, DigimonAddressRaw>;

export interface DigimonAddress {
  name: string;
  memoryBlockAddress: number;
  blastAddress: number;
}

export type DigimonsAddresses = Map<number, DigimonAddress>;
