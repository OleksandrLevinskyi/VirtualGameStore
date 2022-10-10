namespace VirtualGameStore.Models;

public class FriendsFamilyListEntry
{
    public int Id { get; set; }
    public User Owner { get; set; }
    public User? Friend { get; set; }
}