import type {
  ImportantItemsAddresses,
  ImportantItemsAddressesRaw,
} from "@/memory/addresses/important-items.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

export class ImportantItemsAddressesConverter {
  public static convert(raw: ImportantItemsAddressesRaw): ImportantItemsAddresses {
    return {
      treeBoots: AddressValueConverter.convert(raw.treeBoots),
      fishingPole: AddressValueConverter.convert(raw.fishingPole),
      asukaTrophy: AddressValueConverter.convert(raw.asukaTrophy),
      sunTrophy: AddressValueConverter.convert(raw.sunTrophy),
    };
  }
}
