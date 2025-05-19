using FluentAssertions;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;

namespace DatesAndStuff.Mobile.Tests
{
    public class PersonPageMobileTests : BaseTest
    {
        [Test]
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(-9)]
        public void Person_SalaryIncrease_ShouldIncrease(double percentage)
        {
            // Navigate to person page
            var drawer = App.FindElement(MobileBy.XPath("//android.widget.ImageButton[@content-desc=\"Open navigation drawer\"]"));
            drawer.Click();
            var personMenu = App.FindElement(MobileBy.XPath("//android.widget.TextView[@text=\"Person\"]"));
            personMenu.Click();

            // Wait for input to appear and interact with it
            var salaryIncreaseInput = FindUIElement("SalaryIncreasePercentageInput");
            salaryIncreaseInput.Clear();
            salaryIncreaseInput.SendKeys(percentage.ToString());
            double salaryBeforeSubmission = double.Parse(FindUIElement("DisplayedSalary").Text);
            double expectedSalary = salaryBeforeSubmission * (1 + (percentage / 100));

            // Submit form
            var salaryIncreaseSubmitButton = FindUIElement("SalaryIncreaseSubmitButton");
            salaryIncreaseSubmitButton.Click();

            // Assert salary value
            var salaryLabel = FindUIElement("DisplayedSalary");
            string salaryText = salaryLabel.Text;
            double salaryAfterSubmission = double.Parse(salaryText);
            salaryAfterSubmission.Should().BeApproximately(expectedSalary, 0.001);
        }

        [Test]
        public void Person_SalaryIncrease_ShouldShowValidationErrors_WhenBelowMinusTen()
        {
            // Arrange
            var drawer = App.FindElement(MobileBy.XPath("//android.widget.ImageButton[@content-desc=\"Open navigation drawer\"]"));
            drawer.Click();
            var personMenu = App.FindElement(MobileBy.XPath("//android.widget.TextView[@text=\"Person\"]"));
            personMenu.Click();

            // Get initial salary
            var initialSalaryLabel = FindUIElement("DisplayedSalary");
            string initialSalaryText = initialSalaryLabel.Text;
            double initialSalary = double.Parse(initialSalaryText);

            // Enter an invalid percentage
            var salaryIncreaseInput = FindUIElement("SalaryIncreasePercentageInput");
            salaryIncreaseInput.Clear();
            salaryIncreaseInput.SendKeys("-11");

            // Submit the form
            var salaryIncreaseSubmitButton = FindUIElement("SalaryIncreaseSubmitButton");
            salaryIncreaseSubmitButton.Click();

            // Assert: Check for validation errors
            var validationSummaryLabel = FindUIElement("SalaryIncreasePercentageValidationMessage");
            validationSummaryLabel.Text.Should().Contain("The specified percentag should be between -10 and infinity.");

            // Assert that the salary has not changed
            var currentSalaryLabel = FindUIElement("DisplayedSalary");
            string currentSalaryText = currentSalaryLabel.Text;
            double currentSalary = double.Parse(currentSalaryText);
            currentSalary.Should().Be(initialSalary);
        }
    }

    
}