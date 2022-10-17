namespace VirtualGameStore.Captcha
{
    public class CaptchaValidator
    {
        public static bool Validate(string userInputCode, HttpContext context)
        {
            var isValid = userInputCode == context.Session.GetString("CaptchaCode");
            context.Session.Remove("CaptchaCode");
            return isValid;
        }
    }
}
