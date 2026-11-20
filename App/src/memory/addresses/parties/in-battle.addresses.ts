import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";
import type {
  VitalAddresses,
  VitalAddressesRaw,
} from "@/memory/addresses/parties/digimons/vital.addresses";

export interface InBattleAddressesRaw {
  allySlotBase: AddressValueRaw;
  slotStride: AddressValueRaw;
  condition: AddressValueRaw;
  strength: AddressValueRaw;
  defense: AddressValueRaw;
  speed: AddressValueRaw;
  hp: VitalAddressesRaw;
  mp: VitalAddressesRaw;
}

export interface InBattleAddresses {
  allySlotBase: number;
  slotStride: number;
  condition: number;
  strength: number;
  defense: number;
  speed: number;
  hp: VitalAddresses;
  mp: VitalAddresses;
}
