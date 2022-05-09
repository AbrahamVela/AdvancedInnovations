using DiscordStats.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;



namespace DiscordStats_Tests
{
    public class AbrahamSprint6_Tests
    {

        [SetUp]
        public void Setup()
        {
           

        }

        [Test]
        public void ActiveVoiceChannelTimeReturnsFileResultJsonContentType()
        {
            // Arrange
            StatsController statsController = new StatsController(null, null, null, null, null, null, null, null);
            statsController.ControllerContext = new ControllerContext();
            var stringData = @"[{

                }]";

            // Act
            FileResult result = (FileResult)statsController.ActiveVoiceChannelTime(stringData);

            // Assert
            Assert.AreEqual(result.ContentType, "application/json");

        }

        [Test]
        public void ActiveMessageTimeReturnsFileResultJsonContentType()
        {
            // Arrange
            StatsController statsController = new StatsController(null, null, null, null, null, null, null, null);
            statsController.ControllerContext = new ControllerContext();
            var stringData = @"[{
  
                }]";

            // Act
            FileResult result = (FileResult)statsController.ActiveMessageTime(stringData);

            // Assert
            Assert.AreEqual(result.ContentType, "application/json");
        }

        [Test]
        public void ActivePresenceTimeReturnsFileResultJsonContentType()
        {
            // Arrange
            StatsController statsController = new StatsController(null, null, null, null, null, null, null, null);
            statsController.ControllerContext = new ControllerContext();
            var stringData = @"[{
  
                }]";

            // Act
            FileResult result = (FileResult)statsController.ActivePresenceTime(stringData);

            // Assert
            Assert.AreEqual(result.ContentType, "application/json");
        }

        [Test]
        public void HoursPerGameFileResultJsonContentType()
        {
            // Arrange
            StatsController statsController = new StatsController(null, null, null, null, null, null, null, null);
            statsController.ControllerContext = new ControllerContext();
            var stringData = @"[{
  
                }]";

            // Act
            FileResult result = (FileResult)statsController.HoursPerGame(stringData);

            // Assert
            Assert.AreEqual(result.ContentType, "application/json");

        }

    }
}
