using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Android.Enums;
using OpenQA.Selenium.Support.UI;

namespace DatesAndStuff.Mobile.Tests
{
    public class EmagAutomationTest : BaseTest
    {
        [Test]
        public void SearchProductAndAddToCart()
        {
            var androidDriver = App as AndroidDriver;
            var wait = new WebDriverWait(App, TimeSpan.FromSeconds(15));

            androidDriver?.StartRecordingScreen();

            try
            {
                // Your automation steps
                var searchBar = wait.Until(drv => App.FindElement(MobileBy.XPath("//android.widget.TextView[@resource-id=\"ro.emag.android:id/tvCategoriesSearch\"]")));
                searchBar.Click();

                var searchInput = wait.Until(drv => App.FindElement(MobileBy.XPath("//android.widget.EditText[@resource-id=\"ro.emag.android:id/etSearchQuery\"]")));
                searchInput.SendKeys("laptop");

                var searchedElement = wait.Until(drv => App.FindElement(MobileBy.XPath("//android.widget.TextView[@resource-id=\"ro.emag.android:id/tvSearchSuggestion\" and @text=\"laptop\"]")));
                searchedElement.Click();

                wait.Until(drv => App.FindElement(MobileBy.XPath("//androidx.recyclerview.widget.RecyclerView[@resource-id=\"ro.emag.android:id/rvContent\"]")));

                var addToCartBtn = wait.Until(drv => App.FindElement(MobileBy.XPath("//android.widget.ImageView[@content-desc=\"Adaugă în coș\"]")));
                addToCartBtn.Click();

                var cartBtn = wait.Until(drv => App.FindElement(MobileBy.XPath("//android.widget.FrameLayout[@content-desc=\"Coș, 1 notificare nouă\"]")));
                cartBtn.Click();

                var checkoutBtn = wait.Until(drv => App.FindElement(MobileBy.XPath("//android.widget.Button[@resource-id=\"ro.emag.android:id/btnCartContinue\"]")));
                checkoutBtn.Displayed.Should().BeTrue();

                checkoutBtn.Click();
            }
            finally
            {
                var videoBase64 = androidDriver?.StopRecordingScreen();
                if (videoBase64 != null)
                {
                    var videoBytes = Convert.FromBase64String(videoBase64);
                    var filePath = Path.Combine("/Users/eszti/Documents/egyetem/VI/VerVal/Laborok/VerVal/DatesAndStuff.Mobile/Recording", "EmagTestRecording.mp4");
                    File.WriteAllBytes(filePath, videoBytes);
                    Console.WriteLine($"Video saved to: {filePath}");
                }
            }
        }
    }
}