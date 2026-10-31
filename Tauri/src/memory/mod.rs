mod error;
mod handle;
mod mapping;
mod process;
mod session;

pub use error::MemoryError;
pub use session::{MemorySession, MemoryState};

pub const EMULATOR_PROCESS_NAME: &str = "duckstation-qt-x64-ReleaseLTCG";

/// PS1 main RAM. Every Digivice address lives below 0x200000, even when DuckStation runs with 8 MiB RAM.
pub const RAM_SIZE: usize = 2 * 1024 * 1024;
