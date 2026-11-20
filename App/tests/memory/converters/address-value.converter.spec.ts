import { describe, expect, it } from "vitest";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

describe("AddressValueConverter", () => {
  describe("convert", () => {
    it.each([
      ["20", 20],
      [20, 20],
      ["0x14", 20],
      ["0X14", 20],
      ["0xff", 0xff],
      ["0xFF", 0xff],
      ["0x0004B3F8", 0x4b3f8],
      ["0x00", 0],
      ["0", 0],
      ["-4", -4],
      ["-5", -5],
    ])("should convert %j to %i", (value, expected) => {
      expect(AddressValueConverter.convert(value)).toBe(expected);
    });

    it.each([[""], ["invalid"], ["0x"], ["0xZZ"], ["1.5"], ["-0x4"], [" 20"], [1.5], [Number.NaN]])(
      "should throw when the value is %j",
      (value) => {
        expect(() => AddressValueConverter.convert(value)).toThrow("Invalid address value");
      },
    );

    it("should throw when the value is missing", () => {
      expect(() => AddressValueConverter.convert(undefined)).toThrow("Invalid address value");
    });
  });

  describe("convertList", () => {
    it("should convert every value in order", () => {
      expect(AddressValueConverter.convertList(["0x01", "0x80", "4"])).toEqual([0x01, 0x80, 4]);
    });

    it("should return an empty list when the list is empty or missing", () => {
      expect(AddressValueConverter.convertList([])).toEqual([]);
      expect(AddressValueConverter.convertList(undefined)).toEqual([]);
    });

    it("should throw when any value is invalid", () => {
      expect(() => AddressValueConverter.convertList(["0x01", ""])).toThrow(
        "Invalid address value",
      );
    });
  });
});
