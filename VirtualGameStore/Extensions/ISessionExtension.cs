namespace VirtualGameStore.Extensions
{
    public static class ISessionExtension
    {
        /// <summary>
        /// Retrieves and removes a string from the session.
        /// </summary>
        public static string? PullString(this ISession session, string key)
        {
            var value = session.GetString(key);
            session.Remove(key);
            return value;
        }
    }
}
