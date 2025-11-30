namespace Cirreum.State.DataStores;

/// <summary>
/// Represents an error that occurred during data store initialization.
/// </summary>
/// <param name="StoreName">The display name of the store that failed to initialize.</param>
/// <param name="Exception">The exception that was thrown during initialization.</param>
/// <param name="ErrorMessage">The error message extracted from the exception.</param>
/// <param name="StackTrace">The stack trace from the exception, if available.</param>
/// <param name="Timestamp">The UTC time when the error occurred.</param>
public sealed record DataStoreInitializationError(
	string StoreName,
	Exception Exception,
	string ErrorMessage,
	string? StackTrace,
	DateTime Timestamp
) {

	/// <summary>
	/// Creates a new <see cref="DataStoreInitializationError"/> from an exception.
	/// </summary>
	/// <param name="storeName">The display name of the store that failed.</param>
	/// <param name="exception">The exception that occurred.</param>
	/// <returns>A new error record with details extracted from the exception.</returns>
	public static DataStoreInitializationError FromException(string storeName, Exception exception) =>
		new(
			StoreName: storeName,
			Exception: exception,
			ErrorMessage: exception.Message,
			StackTrace: exception.StackTrace,
			Timestamp: DateTime.UtcNow
		);

}