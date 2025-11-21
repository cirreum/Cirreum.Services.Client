namespace Cirreum.State;

/// <summary>
/// Central orchestrator for application state management, providing state
/// retrieval, subscription, and notification capabilities.
/// </summary>
/// <remarks>
/// <para>
/// The state manager serves as the primary interface for managing application state throughout the lifecycle
/// of your application. It provides a unified API for:
/// </para>
/// <list type="bullet">
/// <item><description>Retrieving state instances from dependency injection</description></item>
/// <item><description>Subscribing to state change notifications</description></item>
/// <item><description>Broadcasting state changes to subscribers</description></item>
/// <item><description>Managing subscription lifecycles with automatic cleanup</description></item>
/// </list>
/// <para>
/// The state manager is designed to work seamlessly with dependency injection containers and supports
/// both interface-based and concrete type state management patterns. All state types must implement
/// <see cref="IApplicationState"/> to ensure type safety and consistent behavior.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Register state and state manager in DI
/// services.AddScoped&lt;IMyState, MyState&gt;();
/// 
/// // Use in a Blazor component
/// public class UserProfileComponent : ComponentBase, IDisposable
/// {
///     [Inject] private IStateManager StateManager { get; set; }
///     private IDisposable? _subscription;
///     [Inject] private MyState MyState = default!;
///     
///     protected override void OnInitialized()
///     {
///         // Subscribe to state changes and trigger re-render
///         _subscription = StateManager.Subscribe&lt;IMyState&gt;(() => StateHasChanged());
///     }
///     
///     private async Task UpdateName(string newName)
///     {
///         var myState = StateManager.Get&lt;IMyState&gt;();
///         myState.Name = newName;
///         StateManager.NotifySubscribers(myState);
///         
///			// OR if using concrete type
///         this.MyState.Name = newName;
///         StateManager.NotifySubscribers&lt;IMyState&gt;(this.MyState);
///     }
///     
///     public void Dispose() => _subscription?.Dispose();
/// }
/// </code>
/// </example>
public interface IStateManager {

	/// <summary>
	/// Retrieves the registered state instance of the specified type from the dependency injection container.
	/// </summary>
	/// <typeparam name="TState">The state type to retrieve, must implement <see cref="IApplicationState"/></typeparam>
	/// <returns>The singleton state instance registered in the DI container</returns>
	/// <exception cref="InvalidOperationException">Thrown when the state type is not registered in the DI container</exception>
	/// <remarks>
	/// <para>
	/// This method provides convenient access to state instances without directly depending on 
	/// <see cref="IServiceProvider"/>. The state manager handles the complexity of resolving 
	/// both interface and concrete types through its internal caching and resolution logic.
	/// </para>
	/// <para>
	/// State instances are typically registered as singletons to ensure consistent state 
	/// across the application lifecycle.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// // Retrieve state instance
	/// var userState = stateManager.Get&lt;IUserState&gt;();
	/// var currentUser = userState.CurrentUser;
	/// 
	/// // Works with both interfaces and concrete types
	/// var cartState = stateManager.Get&lt;ShoppingCartState&gt;();
	/// </code>
	/// </example>
	TState Get<TState>() where TState : IApplicationState;

	/// <summary>
	/// Subscribes to state change notifications for the specified state type with a parameter-less handler.
	/// </summary>
	/// <typeparam name="TState">The state type to monitor for changes</typeparam>
	/// <param name="handler">Action to invoke when the state changes (receives no parameters)</param>
	/// <returns>A disposable subscription token that must be disposed to unsubscribe and prevent memory leaks</returns>
	/// <remarks>
	/// <para>
	/// Use this overload when you only need to know that a state change occurred, but don't need 
	/// access to the updated state instance. This is common in UI scenarios where you just need 
	/// to trigger a re-render or refresh.
	/// </para>
	/// <para>
	/// The subscription is active until the returned <see cref="IDisposable"/> is disposed.
	/// Failing to dispose subscriptions will result in memory leaks and continued notifications
	/// to dead objects.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// // Simple notification subscription
	/// var subscription = stateManager.Subscribe&lt;IUserState&gt;(() => 
	/// {
	///     Console.WriteLine("User state changed!");
	///     RefreshUI();
	/// });
	/// 
	/// // Always dispose when done
	/// subscription.Dispose();
	/// 
	/// // Or use using statement for automatic disposal
	/// using var sub = stateManager.Subscribe&lt;IUserState&gt;(() => UpdateUI());
	/// </code>
	/// </example>
	IDisposable Subscribe<TState>(Action handler) where TState : IApplicationState;

	/// <summary>
	/// Subscribes to state change notifications for the specified state type with access to the updated state.
	/// </summary>
	/// <typeparam name="TState">The state type to monitor for changes</typeparam>
	/// <param name="handler">Action to invoke when the state changes, receives the updated state instance</param>
	/// <returns>A disposable subscription token that must be disposed to unsubscribe and prevent memory leaks</returns>
	/// <remarks>
	/// <para>
	/// Use this overload when you need access to the updated state instance in your handler.
	/// This is useful for logging state changes, conditional logic based on state values,
	/// or updating derived state based on the changes.
	/// </para>
	/// <para>
	/// The subscription is active until the returned <see cref="IDisposable"/> is disposed.
	/// Failing to dispose subscriptions will result in memory leaks and continued notifications
	/// to dead objects.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// // Subscription with state access
	/// var subscription = stateManager.Subscribe&lt;IUserState&gt;(userState => 
	/// {
	///     Console.WriteLine($"User {userState.Name} updated at {DateTime.Now}");
	///     
	///     if (userState.IsActive)
	///     {
	///         StartUserSession();
	///     }
	/// });
	/// 
	/// // Cleanup
	/// subscription.Dispose();
	/// </code>
	/// </example>
	IDisposable Subscribe<TState>(Action<TState> handler) where TState : IApplicationState;

	/// <summary>
	/// Notifies all subscribers that the specified state type has changed by automatically retrieving the current state instance.
	/// </summary>
	/// <typeparam name="TState">The state type that has changed</typeparam>
	/// <remarks>
	/// <para>
	/// This is a convenience method that automatically retrieves the current state instance from 
	/// the DI container and broadcasts it to all subscribers. Use this when you've modified state
	/// in-place and want to notify all subscribers of the changes.
	/// </para>
	/// <para>
	/// This method is equivalent to calling <c>NotifySubscribers(stateManager.Get&lt;TState&gt;())</c>
	/// but provides better ergonomics for common notification scenarios.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// // Modify state and notify subscribers
	/// var userState = stateManager.Get&lt;IUserState&gt;();
	/// userState.Name = "Jane Doe";
	/// userState.LastModified = DateTime.Now;
	/// 
	/// // Automatically gets current instance and notifies all subscribers
	/// stateManager.NotifySubscribers&lt;IUserState&gt;();
	/// 
	/// // Works great with notification scoping for batched updates
	/// using (userState.CreateNotificationScope())
	/// {
	///     userState.Name = "John Smith";
	///     userState.Email = "john@example.com";
	///     userState.Age = 30;
	///     // Notification sent when scope disposes
	/// }
	/// </code>
	/// </example>
	void NotifySubscribers<TState>() where TState : class, IApplicationState;

	/// <summary>
	/// Notifies all subscribers that the specified state instance has changed.
	/// </summary>
	/// <typeparam name="TState">The state type that has changed</typeparam>
	/// <param name="state">The updated state instance to broadcast to subscribers</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="state"/> is null</exception>
	/// <exception cref="ArgumentException">Thrown when the provided state instance doesn't match the DI-registered singleton instance</exception>
	/// <remarks>
	/// <para>
	/// This method validates that the provided state instance is the same reference as the one
	/// registered in the dependency injection container. This ensures that all state modifications
	/// go through the official singleton instance, maintaining consistency across the application.
	/// </para>
	/// <para>
	/// Use this overload when you already have a reference to the state instance and want to 
	/// avoid an additional DI container lookup for performance reasons.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// // Get state instance and modify it
	/// var userState = stateManager.Get&lt;IUserState&gt;();
	/// userState.Name = "Alice Johnson";
	/// 
	/// // Notify with the specific instance (avoids additional DI lookup)
	/// stateManager.NotifySubscribers(userState);
	/// 
	/// // This will throw ArgumentException - wrong instance
	/// var wrongInstance = new UserState();
	/// stateManager.NotifySubscribers(wrongInstance); // ❌ Exception!
	/// </code>
	/// </example>
	void NotifySubscribers<TState>(TState state) where TState : class, IApplicationState;

}