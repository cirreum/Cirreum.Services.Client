namespace Cirreum.State.DataStores;

/// <summary>
/// Default implementation of <see cref="IDataStoreInitializationState"/> that tracks
/// data store initialization progress during application startup.
/// </summary>
/// <remarks>
/// <para>
/// This class maintains a count of active initialization tasks and a collection of
/// any errors that occur. State changes are propagated through the <see cref="IStateManager"/>
/// to enable reactive UI updates.
/// </para>
/// </remarks>
public class DataStoreInitializationState(
	IStateManager stateManager
) : ScopedNotificationState, IDataStoreInitializationState {

	private int _taskCount;
	private readonly List<DataStoreInitializationError> _errors = [];

	/// <inheritdoc />
	public bool IsInitializing => this._taskCount > 0;

	/// <inheritdoc />
	public string DisplayStatus { get; private set; } = string.Empty;

	/// <inheritdoc />
	public void StartTask(string status) {
		this._taskCount++;
		this.DisplayStatus = status;
		this.NotifyStateChanged();
	}

	/// <inheritdoc />
	public void SetDisplayStatus(string status) {
		this.DisplayStatus = status;
		this.NotifyStateChanged();
	}

	/// <inheritdoc />
	public void CompleteTask() {
		if (this._taskCount > 0) {
			this._taskCount--;
			if (this._taskCount == 0) {
				this.DisplayStatus = string.Empty;
			}
			this.NotifyStateChanged();
		}
	}

	/// <inheritdoc />
	public int GetTaskCount() => this._taskCount;

	/// <inheritdoc />
	public IReadOnlyList<DataStoreInitializationError> Errors => this._errors.AsReadOnly();

	/// <inheritdoc />
	public bool HasErrors => this._errors.Count > 0;

	/// <inheritdoc />
	public int ErrorCount => this._errors.Count;

	/// <inheritdoc />
	public void LogError(string storeName, Exception exception) {
		this._errors.Add(DataStoreInitializationError.FromException(storeName, exception));
		this.NotifyStateChanged();
	}

	/// <inheritdoc />
	public void ClearErrors() {
		this._errors.Clear();
		this.NotifyStateChanged();
	}

	/// <inheritdoc />
	protected override void OnStateHasChanged() {
		stateManager.NotifySubscribers<IDataStoreInitializationState>(this);
	}

}