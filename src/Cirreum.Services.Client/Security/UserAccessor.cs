namespace Cirreum.Security;
sealed class UserAccessor(IUserState user) : IUserStateAccessor {
	public Task<IUserState> GetUser() => Task.FromResult(user);
}