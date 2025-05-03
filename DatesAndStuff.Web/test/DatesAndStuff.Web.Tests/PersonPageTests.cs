using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DatesAndStuff.Web.Tests
{
    [TestFixture]
    public class PersonPageTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private const string BaseURL = "http://localhost:5091";
        private bool acceptNextAlert = true;

        private Process? _blazorProcess;

        [OneTimeSetUp]
        public void StartBlazorServer()
        {
            var webProjectPath = Path.GetFullPath(Path.Combine(
                Assembly.GetExecutingAssembly().Location,
                "../../../../../../src/DatesAndStuff.Web/DatesAndStuff.Web.csproj"
                ));

            var webProjFolderPath = Path.GetDirectoryName(webProjectPath);

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                //Arguments = $"run --project \"{webProjectPath}\"",
                Arguments = "dotnet run --no-build",
                WorkingDirectory = webProjFolderPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            _blazorProcess = Process.Start(startInfo);

            // Wait for the app to become available
            var client = new HttpClient();
            var timeout = TimeSpan.FromSeconds(30);
            var start = DateTime.Now;

            while (DateTime.Now - start < timeout)
            {
                try
                {
                    var result = client.GetAsync(BaseURL).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        [OneTimeTearDown]
        public void StopBlazorServer()
        {
            if (_blazorProcess != null && !_blazorProcess.HasExited)
            {
                _blazorProcess.Kill(true);
                _blazorProcess.Dispose();
            }
        }

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
                driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(-9)]
        public void Person_SalaryIncrease_ShouldIncrease(double percentage)
        {
            // Arrange
            double expectedSalary = 5000 * (1 + (percentage / 100));
            driver.Navigate().GoToUrl(BaseURL);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Navigate to person page
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@data-test='PersonPageNavigation']"))).Click();

            // Wait for input to appear
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']")));

            // Re-fetch input before using it to avoid staleness
            var input = driver.FindElement(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']"));
            input.Clear();
            input.SendKeys(percentage.ToString());

            // Submit form
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']"))).Click();

            // Assert salary value is displayed and correct
            var salaryText = wait.Until(driver =>
            {
                var salaryLabel = driver.FindElement(By.XPath("//*[@data-test='DisplayedSalary']"));
                return salaryLabel.Text;
            });

            double salaryAfterSubmission = double.Parse(salaryText);
            salaryAfterSubmission.Should().BeApproximately(expectedSalary, 0.001);
        }



        [Test]
        public void Person_SalaryIncrease_ShouldShowValidationErrors_WhenBelowMinusTen()
        {
            // Arrange

            driver.Navigate().GoToUrl(BaseURL);
            driver.FindElement(By.XPath("//*[@data-test='PersonPageNavigation']")).Click();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            var input = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']")));
            input.Clear();
            input.SendKeys("-15");

            // Act
            var submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']")));
            submitButton.Click();

            // Assert
            // wait.Until(driver =>
            // {
            //     var summary = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@data-test='ValidationSummary']")));
            //     summary.Text.Should().NotBeNullOrEmpty();
            //     return summary.Text.Should().Contain("The specified percentage should be between -10 and infinity."); 
            // });

            if (!IsElementPresent(By.XPath("//*[@data-test='SalaryIncreasePercentagInputError']")))
            {
                Thread.Sleep(1000); // Wait for the element to be present
            }
            var errorMessageElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@data-test='SalaryIncreasePercentageInputError']")));
            Assert.That(errorMessageElement.Text, Is.EqualTo("The specified percentage should be between -10 and infinity."));
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}