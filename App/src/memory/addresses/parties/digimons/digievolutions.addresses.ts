import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";
import type { SlotAddresses, SlotAddressesRaw } from "@/memory/addresses/parties/slot.addresses";

export interface DigievolutionsAddressesRaw {
  activeDigievolution: AddressValueRaw;
  slots: SlotAddressesRaw[];
  unlockedDigievolutionsStart: AddressValueRaw;
  unlockedDigievolutionEntryStride: AddressValueRaw;
  maxUnlockedDigievolutions: AddressValueRaw;
  id: AddressValueRaw;
  level: AddressValueRaw;
  dvexp: AddressValueRaw;
}

export interface DigievolutionsAddresses {
  activeDigievolution: number;
  slots: SlotAddresses[];
  unlockedDigievolutionsStart: number;
  unlockedDigievolutionEntryStride: number;
  maxUnlockedDigievolutions: number;
  id: number;
  level: number;
  dvexp: number;
}
