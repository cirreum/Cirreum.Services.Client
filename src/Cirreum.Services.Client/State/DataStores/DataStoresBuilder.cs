namespace Cirreum.State.DataStores;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// A fluent builder for configuring data stores within the State infrastructure.
/// </summary>
/// <remarks>
/// <para>
/// This builder provides a fluent API for registering data stores and configuring
/// their initialization behavior. It is accessed via the <c>AddDataStores()</c>
/// extension method on <see cref="IStateBuilder"/>.
/// </para>
/// <para>
/// Data stores registered through this builder are automatically integrated with
/// the State notification system. Stores implementing <see cref="IInitializableStore"/>
/// are discovered for startup initialization when <see cref="WithAutoInitialization()"/>
/// is enabled.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// services.AddClientState(state => {
///     state.AddDataStores()
///         .WithAutoInitialization()
///         .AddStore&lt;IEventsStore, EventsStore&gt;()
///         .AddStore&lt;IProductsStore, ProductsStore&gt;();
/// });
/// 
/// // With a custom initialization policy
/// services.AddClientState(state => {
///     state.AddDataStores()
///         .WithAutoInitialization&lt;AuthenticatedInitializationPolicy&gt;()
///         .AddStore&lt;IEventsStore, EventsStore&gt;();
/// });
/// 
/// // Without auto-initialization (manual loading)
/// services.AddClientState(state => {
///     state.AddDataStores()
///         .AddStore&lt;IEventsStore, EventsStore&gt;();
/// });
/// </code>
/// </example>
/// <seealso cref="IDataStore"/>
/// <seealso cref="IInitializableStore"/>
/// <seealso cref="IDataStoreInitializationPolicy"/>
public class DataStoresBuilder {

	private readonly IServiceCollection _services;
	private readonly IStateBuilderWithDataStores _stateBuilder;

	internal DataStoresBuilder(IStateBuilderWithDataStores stateBuilder) {
		this._stateBuilder = stateBuilder;
		this._services = stateBuilder.Services;
	}

	/// <summary>
	/// Enables automatic initialization of data stores during application startup
	/// using the <see cref="DefaultDataStoreInitializationPolicy"/>.
	/// </summary>
	/// <returns>The builder for method chaining.</returns>
	/// <remarks>
	/// <para>
	/// When enabled, all registered stores implementing <see cref="IInitializableStore"/>
	/// are automatically loaded during application startup. The initialization integrates
	/// with <see cref="IDataStoreInitializationState"/> to provide progress updates
	/// for splash screens or loading indicators.
	/// </para>
	/// <para>
	/// The default policy initializes stores immediately when the application starts.
	/// For applications requiring preconditions (such as user authentication), use
	/// <see cref="WithAutoInitialization{TPolicy}"/> with a custom policy.
	/// </para>
	/// </remarks>
	/// <seealso cref="WithAutoInitialization{TPolicy}"/>
	/// <seealso cref="DefaultDataStoreInitializationPolicy"/>
	public DataStoresBuilder WithAutoInitialization() {
		return this.WithAutoInitialization<DefaultDataStoreInitializationPolicy>();
	}

	/// <summary>
	/// Enables automatic initialization of data stores during application startup
	/// using a custom initialization policy.
	/// </summary>
	/// <typeparam name="TPolicy">
	/// The type of <see cref="IDataStoreInitializationPolicy"/> that controls
	/// when initialization can proceed.
	/// </typeparam>
	/// <returns>The builder for method chaining.</returns>
	/// <remarks>
	/// <para>
	/// Use this overload to provide a custom policy that controls when data store
	/// initialization should occur. Common scenarios include waiting for user
	/// authentication before loading protected data.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
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
	/// <seealso cref="IDataStoreInitializationPolicy"/>
	public DataStoresBuilder WithAutoInitialization<TPolicy>()
		where TPolicy : class, IDataStoreInitializationPolicy {
		this._services.TryAddScoped<IDataStoreInitializationState, DataStoreInitializationState>();
		this._services.TryAddTransient<IDataStoreInitializationPolicy, TPolicy>();
		return this;
	}

	/// <summary>
	/// Registers a data store with the dependency injection container.
	/// </summary>
	/// <typeparam name="TInterface">The interface type for the data store.</typeparam>
	/// <typeparam name="TImplementation">The implementation type for the data store.</typeparam>
	/// <returns>The builder for method chaining.</returns>
	/// <remarks>
	/// <para>
	/// The data store is registered as a scoped service and integrated with the
	/// State notification system via <see cref="IStateBuilder.RegisterState{TInterface, TImplementation}"/>.
	/// </para>
	/// <para>
	/// If the implementation type also implements <see cref="IInitializableStore"/>,
	/// it is automatically registered for discovery by the initialization system.
	/// When <see cref="WithAutoInitialization()"/> is enabled, these stores are
	/// loaded during application startup in the order specified by
	/// <see cref="IInitializableStore.Order"/>.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// state.AddDataStores()
	///     .AddStore&lt;IEventsStore, EventsStore&gt;()
	///     .AddStore&lt;IProductsStore, ProductsStore&gt;();
	/// </code>
	/// </example>
	/// <seealso cref="IDataStore"/>
	/// <seealso cref="IInitializableStore"/>
	public DataStoresBuilder AddStore<TInterface, TImplementation>()
		where TInterface : class, IDataStore
		where TImplementation : class, TInterface {
		this._stateBuilder.RegisterState<TInterface, TImplementation>();
		if (typeof(IInitializableStore).IsAssignableFrom(typeof(TImplementation))) {
			this._services.AddScoped(sp =>
				(IInitializableStore)sp.GetRequiredService<TInterface>());
		}
		return this;
	}

}