import type { Npcs } from "@/state/models";
import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

export interface NpcBattleAddressesRaw {
  address: AddressValueRaw;
  bitMask: AddressValueRaw;
}

export interface NpcAddressesRaw {
  battles: Record<string, NpcBattleAddressesRaw>;
}

export type NpcsAddressesRaw = Record<keyof Npcs, NpcAddressesRaw>;

export interface NpcBattleAddresses {
  address: number;
  bitMask: number;
}

export interface NpcAddresses {
  battles: Record<string, NpcBattleAddresses>;
}

export type NpcsAddresses = Record<keyof Npcs, NpcAddresses>;
