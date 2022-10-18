namespace VirtualGameStore.Services.Captcha
{
    public class CaptchaValidator
    {
        public static bool Validate(string userInputCode, HttpContext context)
        {
            // Create an extension method on ISession to get and remove a key
            var isValid = userInputCode.ToLower() == context.Session.GetString("CaptchaCode")?.ToLower();
            context.Session.Remove("CaptchaCode");
            return isValid;
        }
    }
}
