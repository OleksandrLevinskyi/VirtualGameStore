using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Models;

namespace VirtualGameStore.Test;

public class SelectingGamesTest
{
    [Test]
    public void GamesShortDesc_AlreadyShortDesc_Equal()
    {
        Game g = new Game();
        g.Description = "Description that is 39 characters long.";
        Assert.That(g.ShortDescription, Is.EqualTo(g.Description));
    }

    [Test]
    public void GamesShortDesc_JustBarelyShortEnough_Equal()
    {
        Game g = new Game();
        g.Description = "Description that is 94 characters long" +
                        " blah blah blah blah blah blah blah blah blah blah blah ";
        Assert.That(g.ShortDescription, Is.EqualTo(g.Description));
    }

    [Test]
    public void GamesShortDesc_JustBarelyTooLong_NotEqual()
    {
        Game g = new Game();
        g.Description = "Description that is over 100 characters long" +
                        " blah blah blah blah blah blah blah blah blah blah blah blah ";
        Assert.That(g.ShortDescription, Is.Not.EqualTo(g.Description));
    }

    [Test]
    public void GamesShortDesc_ProperlyEllipses_Equal()
    {
        Game g = new Game();
        g.Description = "Description that is over 100 characters long" +
                        " blah blah blah blah blah blah blah blah blah blah blah blah ";
        string expect = "Description that is over 100 characters long" +
                        " blah blah blah blah blah blah blah blah blah blah...";
        Assert.That(g.ShortDescription, Is.EqualTo(expect));
    }

    [Test]
    public void GamesShortDesc_ProperlyEllipsesWithFewSpaces_Equal()
    {
        Game g = new Game();
        g.Description = "Description that is over 100 characters long" +
                        " blah blah blah blah blah blah blah_blah_blah_blah_blah_blah ";
        string expect = "Description that is over 100 characters long" +
                        " blah blah blah blah blah blah...";
        Assert.That(g.ShortDescription, Is.EqualTo(expect));
    }

    [Test]
    public void GamesShortDesc_MakeSureShorterThan100_LessThan100()
    {
        Game g = new Game();
        g.Description = "Description that is over 100 characters long and ends with a lot of single characters" +
                        "x x x x x x x x x x x x x x x x x x x x x x x x x x ";
        Assert.That(g.ShortDescription.Length <= 100);
    }
}