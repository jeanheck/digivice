import { describe, expect, it } from "vitest";
import { NpcsAssembler } from "@/memory/assemblers/npcs.assembler";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { NpcResource, NpcsResource } from "@/memory/resources/npcs.resource";

function createResource(
  createNpc: (name: string) => NpcResource = () => ({ battles: [] }),
): NpcsResource {
  const npcs = Object.keys(AddressesRepository.getNpcsAddresses()).map((name) => [
    name,
    createNpc(name),
  ]);

  return Object.fromEntries(npcs) as NpcsResource;
}

describe("NpcsAssembler", () => {
  describe("assemble", () => {
    it("should map battle values to won booleans in the same order", () => {
      const resource: NpcsResource = {
        ...createResource(),
        genji: {
          battles: [
            { id: "first", value: 0x20 },
            { id: "second", value: 0x01 },
          ],
        },
        natsumi: { battles: [{ id: "first", value: 0x02 }] },
        catherine: { battles: [{ id: "first", value: 0 }] },
        lucia: { battles: [{ id: "first", value: 0x80 }] },
      };

      const npcs = NpcsAssembler.assemble(resource);

      expect(npcs.genji.battles).toEqual([
        { id: "first", won: true },
        { id: "second", won: true },
      ]);
      expect(npcs.natsumi.battles).toEqual([{ id: "first", won: true }]);
      expect(npcs.catherine.battles).toEqual([{ id: "first", won: false }]);
      expect(npcs.lucia.battles).toEqual([{ id: "first", won: true }]);
      expect(npcs.alice.battles).toEqual([]);
    });

    it("should map every NPC to its own property", () => {
      const resource = createResource((name) => ({ battles: [{ id: name, value: 1 }] }));

      const npcs = NpcsAssembler.assemble(resource);

      expect(Object.keys(npcs)).toHaveLength(35);
      for (const [name, npc] of Object.entries(npcs)) {
        expect(npc.battles).toEqual([{ id: name, won: true }]);
      }
    });
  });
});
