namespace VirtualGameStore.Extensions
{
    public static class IConfigurationExtension
    {
        public static bool GetUseEmail(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("UseEmail");
        }
    }
}
