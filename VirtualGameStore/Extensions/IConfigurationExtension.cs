namespace VirtualGameStore.Extensions
{
    public static class IConfigurationExtension
    {
        public static bool IsEmailEnabled(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("IsEmailEnabled");
        }
    }
}
