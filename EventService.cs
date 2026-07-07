using EventEaseApp.Models;

namespace EventEaseApp.Services;

/// <summary>
/// Registered as Singleton so every user sees the same shared list of events
/// (e.g. capacity counts update for everyone). In a production app this would
/// call a database or API instead of an in-memory list.
/// </summary>
public class EventService
{
    private readonly List<EventModel> _events = new()
    {
        new EventModel { Id = 1, Name = "Tech Innovators Summit", Description = "A conference for emerging tech leaders.", Date = DateTime.Today.AddDays(14), Location = "Downtown Convention Center", Capacity = 100, RegisteredCount = 42 },
        new EventModel { Id = 2, Name = "Community Music Festival", Description = "Live music, food trucks, and local artists.", Date = DateTime.Today.AddDays(30), Location = "Riverside Park", Capacity = 500, RegisteredCount = 498 },
        new EventModel { Id = 3, Name = "Startup Networking Night", Description = "Meet founders, investors, and mentors.", Date = DateTime.Today.AddDays(7), Location = "The Innovation Hub", Capacity = 60, RegisteredCount = 60 },
    };

    public IReadOnlyList<EventModel> GetEvents() => _events;

    public EventModel? GetEventById(int id) => _events.FirstOrDefault(e => e.Id == id);

    public bool TryRegister(int eventId)
    {
        var ev = GetEventById(eventId);
        if (ev is null || ev.IsFull) return false;

        ev.RegisteredCount++;
        return true;
    }

    public void CancelRegistration(int eventId)
    {
        var ev = GetEventById(eventId);
        if (ev is not null && ev.RegisteredCount > 0)
            ev.RegisteredCount--;
    }
}
