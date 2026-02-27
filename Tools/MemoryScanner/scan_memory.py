import sys
import mmap
import time
import subprocess

# Get duckstation PID
def get_pid():
    try:
        output = subprocess.check_output('tasklist /fi "IMAGENAME eq duckstation-qt-x64-ReleaseLTCG.exe" /FO CSV /NH', shell=True).decode()
        if 'duckstation' in output.lower():
            # CSV format: "ImageName","PID","SessionName","Session#","MemUsage"
            pid_str = output.split(',')[1].strip('"')
            return int(pid_str)
    except Exception:
        pass
    return None

if len(sys.argv) < 2:
    print("Commands: snapshot <file.bin> | compare <file1.bin> <file2.bin> [targetValue]")
    sys.exit(0)

cmd = sys.argv[1].lower()

if cmd == 'snapshot':
    pid = get_pid()
    if not pid:
        print("DuckStation not found!")
        sys.exit(1)
        
    map_name = f"duckstation_{pid}"
    print(f"Opening memory map: {map_name}")
    try:
        shm = mmap.mmap(-1, 2 * 1024 * 1024, map_name)
    except Exception as e:
        print(f"Failed to open map: {e}")
        sys.exit(1)
        
    data = shm.read(2 * 1024 * 1024)
    with open(sys.argv[2], "wb") as f:
        f.write(data)
    print(f"Saved {sys.argv[2]}")
    shm.close()

elif cmd == 'compare':
    f1 = open(sys.argv[2], "rb").read()
    f2 = open(sys.argv[3], "rb").read()
    target = int(sys.argv[4]) if len(sys.argv) > 4 else None
    
    count = 0
    for i in range(len(f1)):
        if f1[i] != f2[i]:
            if target is not None and f2[i] != target:
                continue
                
                # Filter standard PS1 memory bounds
                if count < 50:
                    print(f"0x{i:08X}: {f1[i]} -> {f2[i]}")
                count += 1
            
    print(f"\nTotal differences: {count}")
