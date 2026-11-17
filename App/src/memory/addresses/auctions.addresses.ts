import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

export interface AuctionAddressesRaw {
  address: AddressValueRaw;
  bitMask: AddressValueRaw;
}

export interface AuctionsAddressesRaw {
  divineBarrier: AuctionAddressesRaw;
  hazardShield: AuctionAddressesRaw;
  sniperShield: AuctionAddressesRaw;
  dramonShield: AuctionAddressesRaw;
  yinYangWand: AuctionAddressesRaw;
  hazardFang: AuctionAddressesRaw;
  sniperCannon: AuctionAddressesRaw;
  roninBlade: AuctionAddressesRaw;
  sniperGoggle: AuctionAddressesRaw;
  divineCrown: AuctionAddressesRaw;
  braveHelmet: AuctionAddressesRaw;
}

export interface AuctionAddresses {
  address: number;
  bitMask: number;
}

export interface AuctionsAddresses {
  divineBarrier: AuctionAddresses;
  hazardShield: AuctionAddresses;
  sniperShield: AuctionAddresses;
  dramonShield: AuctionAddresses;
  yinYangWand: AuctionAddresses;
  hazardFang: AuctionAddresses;
  sniperCannon: AuctionAddresses;
  roninBlade: AuctionAddresses;
  sniperGoggle: AuctionAddresses;
  divineCrown: AuctionAddresses;
  braveHelmet: AuctionAddresses;
}
