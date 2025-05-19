using FluentAssertions;
using OpenQA.Selenium.Appium;

namespace DatesAndStuff.Mobile.Tests
{
    internal class IncrementCounterTest:BaseTest
    {
        [Test]
        public void ClickCounterTest()
        {
            // Arrange

            // navigate to the counter page
            var drawer = App.FindElement(MobileBy.XPath("//android.widget.ImageButton[@content-desc=\"Open navigation drawer\"]"));
            drawer.Click();
            var counterMenu = App.FindElement(MobileBy.XPath("//android.widget.TextView[@text=\"Counter\"]"));
            counterMenu.Click();

            // check the current count
            var currentCountTextView = FindUIElement("CounterNumberLabel");
            int originalCount = 0;
            string currentCountValue = currentCountTextView.Text.Substring(currentCountTextView.Text.IndexOf(':') + 1);
            int.TryParse(currentCountValue, out originalCount);

            var buttonToClick = FindUIElement("CounterIncreaseBtn");

            // Act
            buttonToClick.Click();
            //Task.Delay(500).Wait(); // Wait for the click to register and show up on the screenshot

            // Assert
            currentCountValue = currentCountTextView.Text.Substring(currentCountTextView.Text.IndexOf(':') + 1);
            int updatedCount = 0;
            int.TryParse(currentCountValue, out updatedCount);

            // Assert
            updatedCount.Should().Be(originalCount + 1);
        }
    }
}
