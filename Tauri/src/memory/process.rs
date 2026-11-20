use std::mem::size_of;

use windows::Win32::Foundation::WAIT_TIMEOUT;
use windows::Win32::System::Diagnostics::ToolHelp::{
    CreateToolhelp32Snapshot, Process32FirstW, Process32NextW, PROCESSENTRY32W, TH32CS_SNAPPROCESS,
};
use windows::Win32::System::Threading::{OpenProcess, WaitForSingleObject, PROCESS_SYNCHRONIZE};

use super::handle::OwnedHandle;

pub fn find_process_id(process_name: &str) -> windows::core::Result<Option<u32>> {
    let executable_name = format!("{process_name}.exe");

    // SAFETY: plain Win32 call; the returned handle is owned and closed by OwnedHandle.
    let snapshot = OwnedHandle::new(unsafe { CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0)? });

    let mut entry = PROCESSENTRY32W {
        dwSize: size_of::<PROCESSENTRY32W>() as u32,
        ..Default::default()
    };

    // SAFETY: `entry` is a valid PROCESSENTRY32W with dwSize set, as required by the API.
    let mut has_entry = unsafe { Process32FirstW(snapshot.raw(), &mut entry) }.is_ok();
    while has_entry {
        let name_length = entry
            .szExeFile
            .iter()
            .position(|&character| character == 0)
            .unwrap_or(entry.szExeFile.len());
        let name = String::from_utf16_lossy(&entry.szExeFile[..name_length]);

        if name.eq_ignore_ascii_case(&executable_name) {
            return Ok(Some(entry.th32ProcessID));
        }

        // SAFETY: same snapshot and entry as above.
        has_entry = unsafe { Process32NextW(snapshot.raw(), &mut entry) }.is_ok();
    }

    Ok(None)
}

pub struct EmulatorProcess {
    handle: OwnedHandle,
}

impl EmulatorProcess {
    pub fn open(process_id: u32) -> windows::core::Result<Self> {
        // SAFETY: plain Win32 call; the returned handle is owned and closed by OwnedHandle.
        let handle = unsafe { OpenProcess(PROCESS_SYNCHRONIZE, false, process_id)? };
        Ok(Self {
            handle: OwnedHandle::new(handle),
        })
    }

    pub fn is_alive(&self) -> bool {
        // SAFETY: the handle stays open for the lifetime of `self`; a zero timeout never blocks.
        unsafe { WaitForSingleObject(self.handle.raw(), 0) == WAIT_TIMEOUT }
    }
}
