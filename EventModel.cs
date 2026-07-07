namespace EventEaseApp.Models;

public class EventModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int RegisteredCount { get; set; }

    public bool IsFull => RegisteredCount >= Capacity;
}
