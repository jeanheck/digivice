namespace Backend.Infrastructure.Duckstation;

public readonly record struct ConnectionAttemptResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorCode { get; init; }
    public string? ErrorDetail { get; init; }

    public static ConnectionAttemptResult Success() => new() { IsSuccess = true };

    public static ConnectionAttemptResult Failure(string errorCode, string? errorDetail = null) =>
        new()
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            ErrorDetail = errorDetail
        };
}
