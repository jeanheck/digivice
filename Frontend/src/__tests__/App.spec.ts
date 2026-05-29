import { describe, it, expect, vi } from "vitest";
import { mount } from "@vue/test-utils";
import App from "../App.vue";

vi.mock("../stores/use-game-store", () => ({
  useGameStore: () => ({
    currentState: {
      player: { name: "Agumon", bits: 123, location: null },
      party: { slots: [null, null, null] },
      journal: { mainQuest: null, sideQuests: [] },
    },
    isConnected: true,
  }),
}));

describe("App", () => {
  it("mounts renders properly", () => {
    const wrapper = mount(App, {
      global: {
        mocks: {
          $t: (key: string) => key,
        },
        stubs: {
          Journal: true,
          Map: true,
          DigimonSlot: true,
          Footer: { template: "<div />" },
        },
      },
    });

    expect(wrapper.exists()).toBe(true);
  });
});
