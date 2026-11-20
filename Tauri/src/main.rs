// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

mod commands;
mod memory;

fn main() {
  tauri::Builder::default()
    .plugin(tauri_plugin_shell::init())
    .plugin(tauri_plugin_window_state::Builder::default().build())
    .manage(memory::MemoryState::default())
    .invoke_handler(tauri::generate_handler![commands::read_ram])
    .run(tauri::generate_context!())
    .expect("error while running tauri application");
}
