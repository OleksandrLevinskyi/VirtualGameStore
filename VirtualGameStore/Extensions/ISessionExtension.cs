namespace VirtualGameStore.Extensions
{
    public static class ISessionExtension
    {
        public static string? PullString(this ISession session, string key)
        {
            var value = session.GetString(key);
            session.Remove(key);
            return value;
        }
    }
}
