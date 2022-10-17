namespace VirtualGameStore.Captcha
{
    public class CaptchaValidator
    {
        public static bool Validate(string userInputCode, HttpContext context)
        {
            var isValid = userInputCode.ToLower() == context.Session.GetString("CaptchaCode")?.ToLower();
            context.Session.Remove("CaptchaCode");
            return isValid;
        }
    }
}
