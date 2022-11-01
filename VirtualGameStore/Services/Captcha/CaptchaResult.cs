using SkiaSharp;

namespace VirtualGameStore.Services.Captcha
{
    public class CaptchaData
    {
        public string Code { get; }
        public byte[] Bytes { get; }
        public string Base64 { get; }
        public string Base64ImageSource { get; }
        public SKEncodedImageFormat Format { get; }
        public DateTime Timestamp { get; }

        public CaptchaData(string code, byte[] bytes, SKEncodedImageFormat format)
        {
            Code = code;
            Bytes = bytes;
            Format = format; // TODO: do something with this
            Base64 = Convert.ToBase64String(Bytes);
            Base64ImageSource = $"data:image/jpeg;charset=utf-8;base64,{Base64}";
            Timestamp = DateTime.Now;
        }
    }
}
