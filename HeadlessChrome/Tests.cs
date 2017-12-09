using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;

namespace HeadlessChrome
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void UsingChrome_ShouldWork()
        {
            // The line below gives us the following exception...

            //OpenQA.Selenium.DriverServiceNotFoundException
            //HResult = 0x80131500
            //Message = The chromedriver.exe file does not exist in the current directory or in a directory on the PATH environment variable.The driver can be downloaded at http://chromedriver.storage.googleapis.com/index.html.
            //          Source = WebDriver
            //StackTrace:
            //          at OpenQA.Selenium.DriverService.FindDriverServiceExecutable(String executableName, Uri downloadUrl)
            // at OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService()
            // at OpenQA.Selenium.Chrome.ChromeDriver..ctor(ChromeOptions options)
            // at HeadlessChrome.Tests.UsingChrome_ShouldWork() in C:\source\chrome - headless\HeadlessChrome\Tests.cs:line 12

            // We fix the above issue by getting the path to the directory we are executing from
            var pathToChromeDriverDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (var chrome = new ChromeDriver(pathToChromeDriverDirectory))
            {
                chrome.Navigate().GoToUrl("http://www.google.com");
            }
        }

        [TestMethod]
        public void UsingChromeHeadless_ShouldWork()
        {
            // path to the directory
            var pathToChromeDriverDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // headless option
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            //chromeOptions.AddArgument("--window-size=1920,1200"); // optionally set the size of the window
            
            // now we can use it...
            using (var chrome = new ChromeDriver(pathToChromeDriverDirectory, chromeOptions))
            {
                chrome.Navigate().GoToUrl("http://www.google.com");

                var filename = "google";

                // We'll search for a bogus text - the current datetime ~ useful for the screenshot?
                var queryElement = chrome.FindElementByName("q");
                queryElement.SendKeys(DateTime.Now.ToString("o"));
                queryElement.SendKeys(OpenQA.Selenium.Keys.Enter);

                // html for reference
                File.WriteAllText($"{pathToChromeDriverDirectory}\\{filename}_{GetFilenameDatestamp()}.html", chrome.PageSource);

                // screenshot for reference
                chrome.GetScreenshot().SaveAsFile($"{pathToChromeDriverDirectory}\\{filename}_{GetFilenameDatestamp()}.png", OpenQA.Selenium.ScreenshotImageFormat.Png);
            }
        }

        // Get back a string that we can read easy and could be deserialized back to a datetime on any machine (regardless of timezone)
        private string GetFilenameDatestamp()
        {
            return DateTime.Now.ToString("o").Replace(":", "_");
        }
    }
}
