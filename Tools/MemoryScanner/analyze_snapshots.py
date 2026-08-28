import struct
from pathlib import Path

SNAPSHOT_DIR = Path(__file__).parent / "Snapshots"

def read_i16(data: bytes, offset: int) -> int:
    if offset + 2 > len(data):
        return 0
    return struct.unpack_from("<h", data, offset)[0]

def read_u16(data: bytes, offset: int) -> int:
    if offset + 2 > len(data):
        return 0
    return struct.unpack_from("<H", data, offset)[0]

def read_combatant(data: bytes, base: int) -> dict:
    return {
        "level": read_i16(data, base + 0x00),
        "str": read_i16(data, base + 0x02),
        "def": read_i16(data, base + 0x04),
        "spi": read_i16(data, base + 0x06),
        "wis": read_i16(data, base + 0x08),
        "spd": read_i16(data, base + 0x0A),
        "fire": read_i16(data, base + 0x0C),
        "water": read_i16(data, base + 0x0E),
        "ice": read_i16(data, base + 0x10),
        "wind": read_i16(data, base + 0x12),
        "thunder": read_i16(data, base + 0x14),
        "machine": read_i16(data, base + 0x16),
        "dark": read_i16(data, base + 0x18),
        "poison": read_i16(data, base + 0x1A),
        "paralyze": read_i16(data, base + 0x1C),
        "confuse": read_i16(data, base + 0x1E),
        "sleep": read_i16(data, base + 0x20),
        "ko": read_i16(data, base + 0x22),
        "species": read_u16(data, base + 0x24),
    }

def read_slot(data: bytes, base: int) -> dict:
    return {
        "unit_id": read_i16(data, base + 0x00),
        "hp_max": read_i16(data, base + 0x06),
        "hp_cur": read_i16(data, base + 0x08),
        "d_str": read_i16(data, base + 0x10),
        "d_def": read_i16(data, base + 0x12),
        "d_spd": read_i16(data, base + 0x14),
    }

def match_block_to_json(block: dict) -> str:
    return (
        f"Lv{block['level']} STR{block['str']} DEF{block['def']} SPI{block['spi']} "
        f"WIS{block['wis']} SPD{block['spd']} "
        f"F{block['fire']}/W{block['water']}/I{block['ice']}/Wi{block['wind']}/"
        f"T{block['thunder']}/M{block['machine']}/D{block['dark']} "
        f"Po{block['poison']}/Pa{block['paralyze']}/Co{block['confuse']}/Sl{block['sleep']}/KO{block['ko']} "
        f"Sp=0x{block['species']:04X}"
    )

def main() -> None:
    for path in sorted(SNAPSHOT_DIR.glob("*.bin")):
        data = path.read_bytes()
        print(f"\n=== {path.name} ===")
        print(f"ActiveUnitId 0xA4558: {read_i16(data, 0xA4558)}")
        print(
            f"Strip 0x42B34 token={read_i16(data, 0x42B34)} "
            f"lv@0x42B38={read_i16(data, 0x42B38)} "
            f"hpMax@0x42B3A={read_i16(data, 0x42B3A)}"
        )
        for slot in range(3):
            slot_base = 0xA44D0 + slot * 0x20
            s = read_slot(data, slot_base)
            print(
                f"  EnemySlot{slot} @0x{slot_base:X}: unitId={s['unit_id']} "
                f"HP={s['hp_cur']}/{s['hp_max']} dSTR={s['d_str']} dDEF={s['d_def']} dSPD={s['d_spd']}"
            )
        for label, base in [("A4580", 0xA4580), ("A45C0", 0xA45C0)]:
            block = read_combatant(data, base)
            print(f"  Block {label}: {match_block_to_json(block)}")

if __name__ == "__main__":
    main()
