use std::mem::size_of;

use windows::core::HSTRING;
use windows::Win32::System::Memory::{
    MapViewOfFile, OpenFileMappingW, UnmapViewOfFile, VirtualQuery, FILE_MAP_READ,
    MEMORY_BASIC_INFORMATION, MEMORY_MAPPED_VIEW_ADDRESS,
};

use super::handle::OwnedHandle;

pub struct SharedMemoryView {
    // Dropped after `Drop::drop` unmaps the view.
    _mapping: OwnedHandle,
    address: MEMORY_MAPPED_VIEW_ADDRESS,
    size: usize,
}

// SAFETY: the view is mapped into this process and only read; access is serialized by MemoryState's Mutex.
unsafe impl Send for SharedMemoryView {}

impl SharedMemoryView {
    pub fn open(mapping_name: &str) -> windows::core::Result<Self> {
        // SAFETY: plain Win32 call with a valid null-terminated name; the handle is owned by OwnedHandle.
        let mapping = OwnedHandle::new(unsafe {
            OpenFileMappingW(FILE_MAP_READ.0, false, &HSTRING::from(mapping_name))?
        });

        // SAFETY: `mapping` is a valid file mapping handle opened with read access.
        let address = unsafe { MapViewOfFile(mapping.raw(), FILE_MAP_READ, 0, 0, 0) };
        if address.Value.is_null() {
            return Err(windows::core::Error::from_win32());
        }

        let mut information = MEMORY_BASIC_INFORMATION::default();
        // SAFETY: `address` points to the view just mapped; `information` is a valid output buffer.
        let written = unsafe {
            VirtualQuery(
                Some(address.Value),
                &mut information,
                size_of::<MEMORY_BASIC_INFORMATION>(),
            )
        };
        let size = if written == 0 {
            0
        } else {
            information.RegionSize
        };

        Ok(Self {
            _mapping: mapping,
            address,
            size,
        })
    }

    pub fn size(&self) -> usize {
        self.size
    }

    /// Copies the first `length` bytes of the view. Caller must ensure `length <= size()`.
    pub fn copy_prefix(&self, length: usize) -> Vec<u8> {
        let mut bytes = vec![0u8; length];
        // SAFETY: the view is mapped for the lifetime of `self` and is at least `length` bytes long.
        // The emulator writes concurrently, so values may be torn within a tick, as with any live read.
        unsafe {
            std::ptr::copy_nonoverlapping(
                self.address.Value as *const u8,
                bytes.as_mut_ptr(),
                length,
            );
        }
        bytes
    }
}

impl Drop for SharedMemoryView {
    fn drop(&mut self) {
        // SAFETY: the view was mapped by `open` and is unmapped exactly once.
        unsafe {
            let _ = UnmapViewOfFile(self.address);
        }
    }
}
