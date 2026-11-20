use tauri::ipc::Response;
use tauri::State;

use crate::memory::{MemoryError, MemorySession, MemoryState};

#[tauri::command]
pub async fn read_ram(state: State<'_, MemoryState>) -> Result<Response, MemoryError> {
    let mut session_slot = state
        .0
        .lock()
        .unwrap_or_else(|poisoned| poisoned.into_inner());

    // A mapping kept open survives the emulator exiting and would return frozen RAM.
    if session_slot
        .as_ref()
        .is_some_and(|session| !session.is_alive())
    {
        *session_slot = None;
    }

    let session = match session_slot.take() {
        Some(session) => session,
        None => MemorySession::connect()?,
    };

    // On failure the session is dropped, so the next call reconnects from scratch.
    let bytes = session.copy_ram()?;
    *session_slot = Some(session);
    Ok(Response::new(bytes))
}
