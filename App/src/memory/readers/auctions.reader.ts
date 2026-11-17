import type { AuctionAddresses, AuctionsAddresses } from "@/memory/addresses/auctions.addresses";
import type { AuctionsResource } from "@/memory/resources/auctions.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class AuctionsReader {
  private static readAuction(snapshot: MemorySnapshot, addresses: AuctionAddresses): number {
    return snapshot.readByte(addresses.address, addresses.bitMask);
  }

  public static read(snapshot: MemorySnapshot, addresses: AuctionsAddresses): AuctionsResource {
    return {
      divineBarrier: AuctionsReader.readAuction(snapshot, addresses.divineBarrier),
      hazardShield: AuctionsReader.readAuction(snapshot, addresses.hazardShield),
      sniperShield: AuctionsReader.readAuction(snapshot, addresses.sniperShield),
      dramonShield: AuctionsReader.readAuction(snapshot, addresses.dramonShield),
      yinYangWand: AuctionsReader.readAuction(snapshot, addresses.yinYangWand),
      hazardFang: AuctionsReader.readAuction(snapshot, addresses.hazardFang),
      sniperCannon: AuctionsReader.readAuction(snapshot, addresses.sniperCannon),
      roninBlade: AuctionsReader.readAuction(snapshot, addresses.roninBlade),
      sniperGoggle: AuctionsReader.readAuction(snapshot, addresses.sniperGoggle),
      divineCrown: AuctionsReader.readAuction(snapshot, addresses.divineCrown),
      braveHelmet: AuctionsReader.readAuction(snapshot, addresses.braveHelmet),
    };
  }
}
