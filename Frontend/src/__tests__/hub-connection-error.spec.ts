import { describe, it, expect } from "vitest";
import { formatHubConnectionError } from "@/events/hub-connection-error";

describe("formatHubConnectionError", () => {
    it("returns message from Error instances", () => {
        expect(formatHubConnectionError(new Error("Network failed"))).toBe("Network failed");
    });

    it("returns string errors as-is", () => {
        expect(formatHubConnectionError("timeout")).toBe("timeout");
    });

    it("stringifies unknown values", () => {
        expect(formatHubConnectionError({ code: 500 })).toBe("[object Object]");
    });
});
