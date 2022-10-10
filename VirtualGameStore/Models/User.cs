using Microsoft.AspNetCore.Identity;
// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8618

namespace VirtualGameStore.Models;

public class User : IdentityUser
{
    // Personal
    [PersonalData] public string? FirstName { get; set; }
    [PersonalData] public string? LastName { get; set; }
    [PersonalData] public DateTime? BirthDate { get; set; }
    [PersonalData] public bool IsEmailMarketingEnabled { get; set; }
    [PersonalData] public Gender? Gender { get; set; }
    
    // Relations
    public virtual IEnumerable<FriendsFamilyListEntry> FriendsEntries { get; }
    public virtual IEnumerable<FriendsFamilyListEntry> FriendsOfEntries { get; }
    public virtual IEnumerable<PaymentOption> PaymentOptions { get; }
    public virtual IEnumerable<Event> EventsCreated { get; }
    public virtual IEnumerable<Registration> EventRegistrations { get; }
    
}