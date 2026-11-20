use windows::Win32::Foundation::{CloseHandle, HANDLE};

pub struct OwnedHandle(HANDLE);

// SAFETY: a Win32 HANDLE is a process-wide kernel object reference, valid from any thread.
unsafe impl Send for OwnedHandle {}

impl OwnedHandle {
    pub fn new(handle: HANDLE) -> Self {
        Self(handle)
    }

    pub fn raw(&self) -> HANDLE {
        self.0
    }
}

impl Drop for OwnedHandle {
    fn drop(&mut self) {
        // SAFETY: the handle is owned exclusively by this wrapper and closed exactly once.
        unsafe {
            let _ = CloseHandle(self.0);
        }
    }
}
