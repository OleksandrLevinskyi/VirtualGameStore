using System.ComponentModel.DataAnnotations;
using VirtualGameStore.Models.ValidationAttributes;

namespace VirtualGameStore.Test;

public class PaymentOptionsTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void IsDateValid_MonthYearAfterNow_ReturnsTrue()
    {
        var validator = new ValidFutureMonthYear();
        Assert.That(validator.IsValid("11/24"), Is.True);
    }

    [Test]
    public void IsBirthDateValid_MonthYearBeforeNow_ReturnsFalse()
    {
        var validator = new ValidFutureMonthYear();
        Assert.That(validator.IsValid("11/19"), Is.False);
    }
}