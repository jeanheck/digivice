use tauri::Manager;
use tauri_plugin_shell::ShellExt;
use std::sync::Mutex;
use std::net::TcpListener;
use tauri::State;
use tauri::Emitter;

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

fn emit_backend_crashed(app_handle: &tauri::AppHandle, exit_code: Option<i32>) {
    let _ = app_handle.emit("backend-crashed", exit_code);
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
  tauri::Builder::default()
    .plugin(tauri_plugin_shell::init())
    .invoke_handler(tauri::generate_handler![get_backend_port])
    .setup(|app| {
      let port = get_free_port();
      app.manage(BackendPort(port));

      let sidecar_command = app.shell().sidecar("backend").unwrap()
          .args(["--urls", &format!("http://127.0.0.1:{}", port)]);
      match sidecar_command.spawn() {
          Ok((mut rx, child)) => {
              println!("Backend sidecar spawned successfully.");
              app.manage(BackendProcess(Mutex::new(Some(child))));

              let app_handle = app.handle().clone();
              tauri::async_runtime::spawn(async move {
                  while let Some(event) = rx.recv().await {
                      if let tauri_plugin_shell::process::CommandEvent::Terminated(payload) = event {
                          if payload.code != Some(0) {
                              println!("Backend crashed with code {:?}", payload.code);
                              emit_backend_crashed(&app_handle, payload.code);
                          }
                          break;
                      }
                  }
              });
          }
          Err(e) => {
              eprintln!("Failed to spawn backend sidecar: {}", e);
              emit_backend_crashed(app.handle(), None);
          }
      }

      Ok(())
    })
    .on_window_event(|window, event| {
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
