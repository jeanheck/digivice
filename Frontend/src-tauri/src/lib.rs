use tauri::Manager;
use tauri_plugin_shell::ShellExt;
use std::sync::Mutex;

struct BackendProcess(Mutex<Option<tauri_plugin_shell::process::CommandChild>>);

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
  tauri::Builder::default()
    .plugin(tauri_plugin_shell::init())
    .setup(|app| {
      if cfg!(debug_assertions) {
        app.handle().plugin(
          tauri_plugin_log::Builder::default()
            .level(log::LevelFilter::Info)
            .build(),
        )?;
      }

      // Despacha o nosso Sidecar do backend
      let sidecar_command = app.shell().sidecar("backend").unwrap();
      match sidecar_command.spawn() {
          Ok((_rx, child)) => {
              println!("Backend sidecar spawned successfully.");
              app.manage(BackendProcess(Mutex::new(Some(child))));
          }
          Err(e) => {
              eprintln!("Failed to spawn backend sidecar: {}", e);
          }
      }

      Ok(())
    })
    .on_window_event(|window, event| {
         // Mata agressivamente o sidecar se o aplicativo for fechado
         if let tauri::WindowEvent::Destroyed = event {
            if let Some(state) = window.try_state::<BackendProcess>() {
                if let Ok(mut child_lock) = state.0.lock() {
                    if let Some(child) = child_lock.take() {
                        println!("Killing backend process...");
                        let _ = child.kill();
                    }
                }
            }
         }
    })
    .run(tauri::generate_context!())
    .expect("error while running tauri application");
}
