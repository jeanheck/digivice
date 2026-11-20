use std::sync::Mutex;

use super::error::MemoryError;
use super::mapping::SharedMemoryView;
use super::process::{find_process_id, EmulatorProcess};
use super::{EMULATOR_PROCESS_NAME, RAM_SIZE};

#[derive(Default)]
pub struct MemoryState(pub Mutex<Option<MemorySession>>);

pub struct MemorySession {
    process: EmulatorProcess,
    view: SharedMemoryView,
}

impl MemorySession {
    pub fn connect() -> Result<Self, MemoryError> {
        let process_id = find_process_id(EMULATOR_PROCESS_NAME)
            .map_err(MemoryError::connection_failed)?
            .ok_or_else(MemoryError::process_not_found)?;

        let process = EmulatorProcess::open(process_id).map_err(MemoryError::connection_failed)?;

        let mapping_name = format!("duckstation_{process_id}");
        let view = SharedMemoryView::open(&mapping_name)
            .map_err(|error| MemoryError::mapping_not_found(format!("{mapping_name}: {error}")))?;

        Ok(Self { process, view })
    }

    pub fn is_alive(&self) -> bool {
        self.process.is_alive()
    }

    pub fn copy_ram(&self) -> Result<Vec<u8>, MemoryError> {
        if self.view.size() < RAM_SIZE {
            return Err(MemoryError::memory_read_failed(format!(
                "Shared memory view has {} bytes; expected at least {RAM_SIZE}.",
                self.view.size()
            )));
        }

        Ok(self.view.copy_prefix(RAM_SIZE))
    }
}
