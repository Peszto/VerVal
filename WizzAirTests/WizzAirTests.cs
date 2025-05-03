using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Globalization;

namespace WizzAirTests;

[TestFixture]
public class WizzAirTests
{
    private IWebDriver driver;
    private WebDriverWait wait;
    private const string BaseUrl = "https://wizzair.com/";
    private const string DepartureCity = "Tirgu Mures";
    private const string DepartureAirport = "TGM";
    private const string ArrivalCity = "Budapest";
    private const string ArrivalAirport = "BUD";

    [SetUp]
    public void Setup()
    {
        // Set up Chrome driver with necessary options
        var options = new ChromeOptions();
        options.AddArgument("--disable-notifications");
        options.AddArgument("--window-size=1920,1080");
    

        driver = new ChromeDriver(options);
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        driver.Navigate().GoToUrl(BaseUrl);

        // Accept cookies if the dialog appears
        try
        {
            var cookieButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("onetrust-accept-btn-handler")));
            cookieButton.Click();
        }
        catch
        {
            // Continue if no cookie dialog appears
        }
    }

    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
        driver?.Dispose();
    }

    [Test]
    public void CheckTwoFlightsAvailableNextWeek_TgmToBudapest()
    {
        // Calculate date range for next week
        DateTime startDate = DateTime.Today.AddDays(1);
        DateTime endDate = startDate.AddDays(7);

        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@data-test='oneway']")));

        var oneWayButton = driver.FindElement(By.XPath("//input[@data-test='oneway']"));
        oneWayButton.Click();
       
        SearchForFlights(DepartureCity, ArrivalCity);

        // Get all flights in the date range
        int flightCount = CountAvailableFlights(startDate, endDate);

        // // Assert that at least two flights are available
        Assert.GreaterOrEqual(flightCount, 2,
            $"Expected at least 2 flights between {DepartureCity} and {ArrivalCity} in the next week, but found {flightCount}");

        Console.WriteLine($"Found {flightCount} flights between {DepartureCity} and {ArrivalCity} from {startDate.ToShortDateString()} to {endDate.ToShortDateString()}");
    }

    private void SearchForFlights(string from, string to)
    {
        // Click on the flight search form
        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//form[@data-test='flights']")));

        // Set departure location
        var departureInput = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//input[contains(@data-test, 'search-departure-station')]")));
        departureInput.Clear();
        departureInput.SendKeys(from);
        var departureAirportElement = wait.Until(ExpectedConditions.ElementIsVisible(
        By.XPath($"//mark[normalize-space(text()) = '{DepartureCity}']")));
        departureAirportElement.Click();


        // Set arrival location
        var arrivalInput = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//input[contains(@data-test, 'search-arrival-station')]")));
        arrivalInput.Clear();
        arrivalInput.SendKeys(to);
        wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath($"//mark[normalize-space(text()) = '{ArrivalCity}']"))).Click();

        // // Click on the departure date picker
        // wait.Until(ExpectedConditions.ElementToBeClickable(
        //     By.XPath("//div[contains(@data-test, 'search-departure-date')]"))).Click();

        // Select the departure date
        // SelectDate(startDate);

        // // // Click the search button
        // var searchButton = wait.Until(ExpectedConditions.ElementToBeClickable(
        // By.XPath("//button[@data-test='flight-search-submit']")));
        // searchButton.Click();

        // wait.Until(driver => driver.WindowHandles.Count > 1);

        // // Switch to the new tab
        // var tabs = driver.WindowHandles;
        // driver.SwitchTo().Window(tabs[1]);

        // wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@data-test='booking-flow']")));

    }

    private bool IsDateSelectable(DateTime date)
    {
        // Month names might be different based on the website language
        string monthName = date.ToString("MMMM", CultureInfo.InvariantCulture);
        string day = date.Day.ToString();

        // Navigate to the correct month if needed
        bool foundMonth = false;
        int attempts = 0;

        while (!foundMonth && attempts < 12)
        {
            try
            {
                driver.FindElement(By.XPath($"//div[contains(@class, 'vc-title') and contains(text(), '{monthName}')]"));
                foundMonth = true;
            }
            catch
            {
                var nextButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[@data-test='calendar-page-forward']")));
                nextButton.Click();
                attempts++;
            }
        }

        try
        {
            // Check if the day element is clickable
            var dayElement = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath($"//span[contains(@class, 'vc-day-content') and @role='button' and normalize-space(text())='{day}' and @aria-disabled='false']")));
            return true;
        }
        catch
        {
            return false;
        }
    }

    private int CountAvailableFlights(DateTime startDate, DateTime endDate)
    {
        int flightCount = 0;

        // Loop through the date range and count available flights
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (IsDateSelectable(date))
            {
                flightCount++;
            }
        }

        return flightCount;
    }

    // private int CountAvailableFlights()
    // {
    //     // Wait for flight list to load
    //     Thread.Sleep(2000); // Additional wait to ensure full loading

    //     // Get all flight elements
    //     try
    //     {
    //         wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@data-test='flight-info']")));

    //         // Find all flight-info elements using XPath
    //         var flightInfoElements = driver.FindElements(By.XPath("//div[@data-test='flight-info']"));

    //         // Count the number of flight-info elements
    //         return flightInfoElements.Count;

    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error counting flights: {ex.Message}");
    //         return 0;
    //     }
    // }


}