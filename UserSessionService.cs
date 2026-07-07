namespace EventEaseApp.Services;

/// <summary>
/// Registered as Scoped, so one instance exists per user "circuit" (session)
/// in Blazor Server. Components inject this to read/update session-wide state,
/// such as who is logged in and which events they've registered for.
/// </summary>
public class UserSessionService
{
    public string? UserName { get; private set; }
    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserName);

    // Tracks which event IDs the current user has registered for,
    // used to drive two-way bound "Registered" checkboxes on EventCard.
    private readonly HashSet<int> _registeredEventIds = new();

    public event Action? OnChange;

    public void Login(string userName)
    {
        UserName = userName;
        NotifyStateChanged();
    }

    public void Logout()
    {
        UserName = null;
        _registeredEventIds.Clear();
        NotifyStateChanged();
    }

    public bool IsRegisteredFor(int eventId) => _registeredEventIds.Contains(eventId);

    public void SetRegistration(int eventId, bool isRegistered)
    {
        if (isRegistered)
            _registeredEventIds.Add(eventId);
        else
            _registeredEventIds.Remove(eventId);

        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
