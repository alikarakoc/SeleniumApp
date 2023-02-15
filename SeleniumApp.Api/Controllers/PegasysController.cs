using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumApp.Api.Dtos;

namespace SeleniumApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PegasysController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> Load()
        {
            List<PegasReservationListDto> reservationLists = new();
            ChromeOptions chromeOptions = new();
            chromeOptions.AddArgument("--window-size=1300,1000");
            //chromeOptions.AddArgument("headless");

            IWebDriver driver = new ChromeDriver(chromeOptions);
            driver.Navigate().GoToUrl("**");
            Thread.Sleep(2000);

            IWebElement userName = driver.FindElement(By.Name("Username"));
            IWebElement password = driver.FindElement(By.Name("Password"));
            IWebElement loginButton = driver.FindElement(By.ClassName("login-button"));
            userName.SendKeys("*******");
            password.SendKeys("*******");
            loginButton.Click();

            Thread.Sleep(2000);

            IReadOnlyCollection<IWebElement> divs = driver.FindElements(By.XPath("//*[@component-type='hotel-service-list-view-search-result-component']/div"));
            foreach (IWebElement item in divs)
            {
                string[] data = item.Text.Split("\r\n");
                string hotel = data[3];
                string child = data[4];
                string date = data[2];
                string description = data[8];
                PegasReservationListDto resItem = new PegasReservationListDto
                {
                    HotelName = hotel,
                    Child = child,
                    Date = date,
                    Description = description
                };
                reservationLists.Add(resItem);
            }
            return JsonConvert.SerializeObject(reservationLists);
        }
    }
}
