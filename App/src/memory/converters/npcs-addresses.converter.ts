import type {
  NpcAddresses,
  NpcAddressesRaw,
  NpcBattleAddresses,
  NpcBattleAddressesRaw,
  NpcsAddresses,
  NpcsAddressesRaw,
} from "@/memory/addresses/npcs.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

export class NpcsAddressesConverter {
  private static convertBattle(raw: NpcBattleAddressesRaw): NpcBattleAddresses {
    return {
      address: AddressValueConverter.convert(raw.address),
      bitMask: AddressValueConverter.convert(raw.bitMask),
    };
  }

  private static convertNpc(raw: NpcAddressesRaw): NpcAddresses {
    const battles = Object.entries(raw.battles ?? {}).map(([battleId, battle]) => [
      battleId,
      NpcsAddressesConverter.convertBattle(battle),
    ]);

    return { battles: Object.fromEntries(battles) };
  }

  public static convert(raw: NpcsAddressesRaw): NpcsAddresses {
    const npcs = Object.entries(raw).map(([name, npc]) => [
      name,
      NpcsAddressesConverter.convertNpc(npc),
    ]);

    return Object.fromEntries(npcs) as NpcsAddresses;
  }
}
