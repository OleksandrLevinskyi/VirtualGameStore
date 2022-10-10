namespace VirtualGameStore.Models;

public class Event
{
    // ID
    public int Id { get; set; }
    // Includes
    public User? Creator { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    // Relations
    public virtual List<Registration> RegisteredUsers { get; }
}