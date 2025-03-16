using System.Diagnostics.CodeAnalysis;

namespace TodoApp.Presentation;

public readonly record struct Result
{
    public Error? Error { get; }

    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }

    private Result(bool success, Error? error = null)
    {
        Success = success;
        Error = error;
    }

    public static Result Ok() => new(true);

    public static implicit operator Result(Error error) => new(false, error);

    public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> fail) => Success
        ? success()
        : fail(Error);
}

public readonly record struct Result<T>
{
    private Result(T value)
    {
        Value = value;
        Success = true;
        Error = null;
    }

    private Result(Error error)
    {
        Value = default;
        Success = false;
        Error = error;
    }

    public T? Value { get; }

    public Error? Error { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Error error) => new(error);

    public TResult Match<TResult>(Func<T, TResult> success, Func<Error, TResult> fail) => Success
        ? success(Value)
        : fail(Error);
}

public record Error(string Message);
