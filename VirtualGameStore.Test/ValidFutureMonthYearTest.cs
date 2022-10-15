using System.ComponentModel.DataAnnotations;
using VirtualGameStore.Models.ValidationAttributes;

namespace VirtualGameStore.Test;

public class ValidFutureMonthYearTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void IsDateValid_ReturnsTrueIfMonthYearAfterNow()
    {
        var validator = new ValidFutureMonthYear();
        Assert.That(validator.IsValid("11/24"), Is.True);
    }

    [Test]
    public void IsBirthDateValid_ReturnsFalseIfMonthYearBeforeNow()
    {
        var validator = new ValidFutureMonthYear();
        Assert.That(validator.IsValid("11/19"), Is.False);
    }
}