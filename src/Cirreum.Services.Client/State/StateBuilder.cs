namespace Cirreum.State;

using Microsoft.Extensions.DependencyInjection;

sealed class StateBuilder(
	IServiceCollection services
) : IStateBuilder {

	/// <inheritdoc/>
	public IStateBuilder RegisterState<TInterface, TImplementation>()
		where TInterface : class, IApplicationState
		where TImplementation : class, TInterface {
		services.AddScoped<TInterface, TImplementation>();
		return this;
	}

	/// <inheritdoc/>
	public IStateBuilder RegisterState<TImplementation>()
		where TImplementation : class, IApplicationState {
		services.AddScoped<TImplementation>();
		return this;
	}

	/// <inheritdoc/>
	public IStateBuilder RegisterEncryptor(IStateContainerEncryption encryption) {
		services.AddSingleton(encryption);
		services.AddKeyedSingleton(encryption.AlgorithmId, encryption);
		return this;
	}

	/// <inheritdoc/>
	public IStateBuilder RegisterDecryptor(IStateContainerEncryption previousEncryption) {
		if (previousEncryption.AlgorithmKindId != StateEncryptionKinds.NONE &&
			previousEncryption.AlgorithmKindId != StateEncryptionKinds.BASE64) {
			services.AddKeyedSingleton(previousEncryption.AlgorithmId, previousEncryption);
		}
		return this;
	}

}