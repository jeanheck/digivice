use tauri::Manager;
use tauri_plugin_shell::ShellExt;
use std::sync::Mutex;
use std::net::TcpListener;
use tauri::State;

struct BackendProcess(Mutex<Option<tauri_plugin_shell::process::CommandChild>>);
struct BackendPort(u16);

#[tauri::command]
fn get_backend_port(port: State<'_, BackendPort>) -> u16 {
    port.0
}

fn get_free_port() -> u16 {
    TcpListener::bind("127.0.0.1:0")
        .and_then(|listener| listener.local_addr())
        .map(|addr| addr.port())
        .unwrap_or(5000)
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
  tauri::Builder::default()
    .plugin(tauri_plugin_shell::init())
    .invoke_handler(tauri::generate_handler![get_backend_port])
    .setup(|app| {
      let port = get_free_port();
      app.manage(BackendPort(port));

      if cfg!(debug_assertions) {
        app.handle().plugin(
          tauri_plugin_log::Builder::default()
            .level(log::LevelFilter::Info)
            .build(),
        )?;
      }

      // Despacha o nosso Sidecar do backend
      let sidecar_command = app.shell().sidecar("backend").unwrap()
          .args(["--urls", &format!("http://127.0.0.1:{}", port)]);
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
