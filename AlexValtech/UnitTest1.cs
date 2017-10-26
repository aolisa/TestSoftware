using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AlexValtech
{
    [TestClass]
    public class UnitTest1
    {
        private static IWebDriver _driver;
        public string _url = "https://www.valtech.com/";
        private static int _timeout = 60; 

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext textContext)
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_timeout);
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _driver.Navigate().GoToUrl(_url); 
            _driver.Manage().Window.Maximize();
        }


        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [TestMethod]
        public void TestLatestNewSectionIsDisplaying()
        {
            var label_LatestNews = _driver.FindElement(By.XPath("//h2[contains(text(),'Latest news')]"));
            var section_LatestNews = _driver.FindElement(By.XPath("//div[@class='bloglisting news-post__listing']"));
            var list_NewPosts = _driver.FindElements(By.XPath("//div[@class='bloglisting-compact']"));

            Assert.IsTrue(label_LatestNews.Displayed); 
            Assert.IsTrue(section_LatestNews.Displayed);
            Assert.IsTrue(list_NewPosts.Count > 0);
        }

        [TestMethod]
        public void TestPagesDisplayTheCorrectH1Tag()
        {
            var menuItemsToCheck = new [] { "About", "Services", "Work" };

            foreach (var item in menuItemsToCheck)
            {
                WaitForElementToBeClickable(MenuItemsBylabel(item));
                MenuItemsBylabel(item).Click();
                Assert.IsTrue(H1ContainsText(item));
            }

        }

        [TestMethod]
        public void TestNumberOfOfficesOnContactPage()
        {
            var contactIcon = _driver.FindElement(By.XPath("//i[@data-icon='earth-contact']"));
            var cities_OnContactPage =
                _driver.FindElements(By.XPath("//div[@class='contactcountry']//ul[@class='contactcities']//li"));
            WaitForElementToBeClickable(contactIcon);

            contactIcon.Click();

            Assert.IsTrue(cities_OnContactPage.Count == 37);
        }


        #region Private Methods

        private IWebElement MenuItemsBylabel(string h1Label)
        {
            return _driver.FindElement(By.XPath(
                $"//div//ul[@class='header__navigation__menu navigation__menu']/li//span[contains(text(),'{h1Label}')]"));
        }

        private bool H1ContainsText(string text)
        {
            return _driver.FindElement(By.XPath("//h1")).Text.Equals(text);
        }

        private void WaitForElementToBeClickable(IWebElement ele, int timeoutInSeconds = 5)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds)); //move timeout to config?
                wait.Until(ExpectedConditions.ElementToBeClickable(ele));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Element not clickable within {timeoutInSeconds} seconds. Exception message {e.Message}");
            }
            
        }


        #endregion
    }
}
