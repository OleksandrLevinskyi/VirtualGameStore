namespace VirtualGameStore.Models;

public class Registration
{
    public int Id { get; set; }
    public Event Event { get; set; }
    public User User { get; set; }
    public DateTime DateRegistered { get; set; }
}