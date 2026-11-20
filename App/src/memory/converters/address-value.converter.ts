import type { AddressValueRaw } from "@/memory/addresses/address-value.raw";

const HexadecimalPattern = /^0x[0-9a-f]+$/i;
const DecimalPattern = /^-?\d+$/;

export class AddressValueConverter {
  public static convert(value: AddressValueRaw | undefined): number {
    if (typeof value === "number" && Number.isSafeInteger(value)) {
      return value;
    }

    if (typeof value === "string") {
      if (HexadecimalPattern.test(value)) {
        return Number.parseInt(value.slice(2), 16);
      }

      if (DecimalPattern.test(value)) {
        return Number.parseInt(value, 10);
      }
    }

    throw new Error(`Invalid address value: ${JSON.stringify(value)}.`);
  }

  public static convertList(values: AddressValueRaw[] | undefined): number[] {
    return (values ?? []).map((value) => AddressValueConverter.convert(value));
  }
}
