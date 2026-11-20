import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export interface SnapshotProvider {
  read(): Promise<MemorySnapshot>;
}
