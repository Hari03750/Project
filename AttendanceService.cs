using EventEaseApp.Models;

namespace EventEaseApp.Services;

public class AttendanceService
{
    // Static backing store so attendance persists across the app for this demo,
    // even though the service itself is Scoped per user.
    private static readonly List<AttendeeModel> _attendees = new();
    private static int _nextId = 1;

    public IReadOnlyList<AttendeeModel> GetAttendeesForEvent(int eventId) =>
        _attendees.Where(a => a.EventId == eventId).ToList();

    public AttendeeModel Register(AttendeeModel attendee)
    {
        attendee.Id = _nextId++;
        _attendees.Add(attendee);
        return attendee;
    }

    public void ToggleCheckIn(int attendeeId)
    {
        var attendee = _attendees.FirstOrDefault(a => a.Id == attendeeId);
        if (attendee is not null)
            attendee.HasCheckedIn = !attendee.HasCheckedIn;
    }

    public int GetCheckedInCount(int eventId) =>
        _attendees.Count(a => a.EventId == eventId && a.HasCheckedIn);
}
