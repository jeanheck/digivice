use serde::Serialize;

#[derive(Debug, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct MemoryError {
    pub code: &'static str,
    pub detail: Option<String>,
}

impl MemoryError {
    pub fn process_not_found() -> Self {
        Self {
            code: "process_not_found",
            detail: None,
        }
    }

    pub fn mapping_not_found(detail: impl ToString) -> Self {
        Self {
            code: "mapping_not_found",
            detail: Some(detail.to_string()),
        }
    }

    pub fn connection_failed(detail: impl ToString) -> Self {
        Self {
            code: "connection_failed",
            detail: Some(detail.to_string()),
        }
    }

    pub fn memory_read_failed(detail: impl ToString) -> Self {
        Self {
            code: "memory_read_failed",
            detail: Some(detail.to_string()),
        }
    }
}
