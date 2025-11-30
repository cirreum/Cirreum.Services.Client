namespace Cirreum.State.DataStores;

/// <summary>
/// Defines the policy that controls when data store initialization can begin.
/// </summary>
/// <remarks>
/// <para>
/// Applications implement this interface to define their own initialization requirements.
/// For example, an admin portal might require authentication before loading data,
/// while a public site might initialize immediately.
/// </para>
/// <para>
/// The framework provides <see cref="DefaultDataStoreInitializationPolicy"/> as a default
/// implementation that initializes stores immediately on application startup.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Wait for user authentication before initializing stores
/// public class AuthenticatedInitializationPolicy(
///     IUserState userState
/// ) : IDataStoreInitializationPolicy {
///
///     public Task&lt;bool&gt; CanInitializeAsync() 
///         => Task.FromResult(userState.IsAuthenticated);
///
///     public IDisposable? OnReadyToInitialize(Func&lt;Task&gt; callback) {
///         if (userState.IsAuthenticated) {
///             _ = callback();
///             return null;
///         }
///         return userState.Subscribe(state => {
///             if (state.IsAuthenticated) {
///                 _ = callback();
///             }
///         });
///     }
/// }
/// </code>
/// </example>
public interface IDataStoreInitializationPolicy {

	/// <summary>
	/// Registers a callback to be invoked when initialization can proceed.
	/// </summary>
	/// <param name="callback">The callback to invoke when initialization should begin.</param>
	/// <returns>
	/// An <see cref="IDisposable"/> that can be used to cancel initialization and unsubscribe
	/// from the ready notification, or <c>null</c> if the callback was invoked immediately.
	/// </returns>
	/// <remarks>
	/// <para>
	/// If initialization can proceed immediately, implementations should invoke the callback
	/// synchronously and return <c>null</c>. Otherwise, implementations should subscribe to
	/// the appropriate state change and invoke the callback when the precondition is met.
	/// </para>
	/// </remarks>
	IDisposable? OnReadyToInitialize(Func<CancellationToken, Task> callback);

}