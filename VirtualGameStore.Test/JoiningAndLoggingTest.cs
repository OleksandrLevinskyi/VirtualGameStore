using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGameStore.Services.Captcha;

namespace VirtualGameStore.Test
{
    internal class JoiningAndLoggingTest
    {
        [Test]
        public void GenerateCaptchaCode_Called_Returns4DigitAlphanumnericCode()
        {
            var code = CaptchaGenerator.GenerateCaptchaCode();

            Assert.Multiple(() =>
            {
                Assert.That(code, Has.Length.EqualTo(4));
                Assert.That(IsStringAlphanumneric(code));
            });
        }

        [Test]
        public void IsCaptchaValid_ProvidingCorrectCode_ReturnsTure()
        {
            var expectedCode = "D4Y6";
            var providedCode = "d4y6";

            var isCapthcaValid = CaptchaGenerator.IsCaptchaValid(providedCode, expectedCode);

            Assert.That(isCapthcaValid, Is.True);
        }

        private static bool IsStringAlphanumneric(string value)
        {
            return value.All(char.IsLetterOrDigit);
        }
    }
}
