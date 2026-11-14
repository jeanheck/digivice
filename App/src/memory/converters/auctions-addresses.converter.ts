import type {
  AuctionAddresses,
  AuctionAddressesRaw,
  AuctionsAddresses,
  AuctionsAddressesRaw,
} from "@/memory/addresses/auctions.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

export class AuctionsAddressesConverter {
  private static convertAuction(raw: AuctionAddressesRaw): AuctionAddresses {
    return {
      address: AddressValueConverter.convert(raw.address),
      bitMask: AddressValueConverter.convert(raw.bitMask),
    };
  }

  public static convert(raw: AuctionsAddressesRaw): AuctionsAddresses {
    return {
      divineBarrier: AuctionsAddressesConverter.convertAuction(raw.divineBarrier),
      hazardShield: AuctionsAddressesConverter.convertAuction(raw.hazardShield),
      sniperShield: AuctionsAddressesConverter.convertAuction(raw.sniperShield),
      dramonShield: AuctionsAddressesConverter.convertAuction(raw.dramonShield),
      yinYangWand: AuctionsAddressesConverter.convertAuction(raw.yinYangWand),
      hazardFang: AuctionsAddressesConverter.convertAuction(raw.hazardFang),
      sniperCannon: AuctionsAddressesConverter.convertAuction(raw.sniperCannon),
      roninBlade: AuctionsAddressesConverter.convertAuction(raw.roninBlade),
      sniperGoggle: AuctionsAddressesConverter.convertAuction(raw.sniperGoggle),
      divineCrown: AuctionsAddressesConverter.convertAuction(raw.divineCrown),
      braveHelmet: AuctionsAddressesConverter.convertAuction(raw.braveHelmet),
    };
  }
}
