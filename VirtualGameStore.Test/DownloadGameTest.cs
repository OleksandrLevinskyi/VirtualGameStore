using VirtualGameStore.Models;

namespace VirtualGameStore.Test
{
    public class DownloadGameTest
    {
        [Test]
        public void GenerateFileName_GameNameIsEmpty_ReturnsDefaultFileName()
        {
            Game game = new Game() { Id = 1 };

            string fileName = game.GenerateFileName();

            Assert.That(fileName, Is.EqualTo("default_file_name.txt"));
        }

        [Test]
        public void GenerateFileName_GameNameIsNotEmpty_ReturnsGeneratedFileNameWithReplacedSpaces()
        {
            Game game = new Game() { Id = 1, Name = "Quick Race" };

            string fileName = game.GenerateFileName();

            Assert.That(fileName, Is.EqualTo("Quick_Race.txt"));
        }
    }
}