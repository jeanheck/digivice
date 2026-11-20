import type {
  DigimonsAddresses,
  DigimonsAddressesRaw,
} from "@/memory/addresses/parties/digimon.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

export class DigimonsAddressesConverter {
  public static convert(raw: DigimonsAddressesRaw): DigimonsAddresses {
    const digimonsAddresses: DigimonsAddresses = new Map();

    for (const [id, digimon] of Object.entries(raw)) {
      digimonsAddresses.set(AddressValueConverter.convert(id), {
        name: digimon.name,
        memoryBlockAddress: AddressValueConverter.convert(digimon.memoryBlockAddress),
        blastAddress: AddressValueConverter.convert(digimon.blastAddress),
      });
    }

    return digimonsAddresses;
  }
}
