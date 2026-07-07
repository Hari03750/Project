using System.ComponentModel.DataAnnotations;

namespace EventEaseApp.Models;

public class AttendeeModel
{
    public int Id { get; set; }

    public int EventId { get; set; }

    [Required(ErrorMessage = "Please enter your full name.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a phone number.")]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Range(1, 10, ErrorMessage = "You can register between 1 and 10 guests.")]
    public int NumberOfGuests { get; set; } = 1;

    public bool HasCheckedIn { get; set; } = false;
}
