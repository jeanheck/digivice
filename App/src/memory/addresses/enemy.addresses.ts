import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";
import type {
  VitalAddresses,
  VitalAddressesRaw,
} from "@/memory/addresses/parties/digimons/vital.addresses";

export interface EnemyAddressesRaw {
  enemySlotBase: AddressValueRaw;
  slotStride: AddressValueRaw;
  slotCount: AddressValueRaw;
  activeEnemySlotIndex: AddressValueRaw;
  groupId: AddressValueRaw;
  id: AddressValueRaw;
  condition: AddressValueRaw;
  strength: AddressValueRaw;
  defense: AddressValueRaw;
  speed: AddressValueRaw;
  hp: VitalAddressesRaw;
}

export interface EnemyAddresses {
  enemySlotBase: number;
  slotStride: number;
  slotCount: number;
  activeEnemySlotIndex: number;
  groupId: number;
  id: number;
  condition: number;
  strength: number;
  defense: number;
  speed: number;
  hp: VitalAddresses;
}
