import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";
import type {
  RequisiteAddresses,
  RequisiteAddressesRaw,
} from "@/memory/addresses/journals/quests/requisite.addresses";

export interface StepAddressesRaw {
  number: AddressValueRaw;
  address: AddressValueRaw;
  bitMasks?: AddressValueRaw[];
  requisites?: RequisiteAddressesRaw[];
}

export interface StepAddresses {
  number: number;
  address: number;
  bitMasks: number[];
  requisites: RequisiteAddresses[];
}
