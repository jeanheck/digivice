import { describe, it, expect } from "vitest";
import { parseConnectionStatus } from "@/events/connection-status";

describe("parseConnectionStatus", () => {
    it("reads camelCase isConnected", () => {
        expect(parseConnectionStatus({ isConnected: false })).toEqual({ isConnected: false });
    });

    it("reads PascalCase IsConnected", () => {
        expect(parseConnectionStatus({ IsConnected: true })).toEqual({ isConnected: true });
    });

    it("defaults to disconnected for invalid payload", () => {
        expect(parseConnectionStatus(null)).toEqual({ isConnected: false });
    });
});
