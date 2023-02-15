using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumApp.Api.Dtos;

namespace SeleniumApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> Load()
        {
            List<ReservationListDto> reservationLists = new();
            ChromeOptions chromeOptions = new();
            chromeOptions.AddArgument("--window-size=1300,1000");
            //chromeOptions.AddArgument("headless");


            IWebDriver driver = new ChromeDriver(chromeOptions);
            driver.Navigate().GoToUrl("https://www.sednainfonetwork.com/Account/SignIn");
            Thread.Sleep(2000);

            IWebElement userName = driver.FindElement(By.Name("UserName"));
            IWebElement password = driver.FindElement(By.Name("Password"));
            IWebElement loginButton = driver.FindElement(By.Id("btnLogin"));
            userName.SendKeys("******");
            password.SendKeys("******");
            loginButton.Click();

            Thread.Sleep(2000);

            IWebElement filterButton = driver.FindElement(By.Id("btnFilter"));
            filterButton.Click();

            //<table> class ="table" -> tek.
            //tbody içinde dön
            //tbody içinde dönerken -> data-row-type'ı reservation olan objeyi al (dönme) ->
            //bu obje içerisinde ki data-columun-typeları al valuelarına göre bkz : data-columun-type="config"
            //daha sonra data-row-type ı rezervation dışında bulunan 2 tane note alanı mevcut eğer burda bir text değeri var ise al. yok ise boş geç



            //IReadOnlyCollection<IWebElement> tables = driver.FindElements(By.XPath("//table[@class='table']"));
            IReadOnlyCollection<IWebElement> tbodys = driver.FindElements(By.XPath("//table[@class='table']/tbody"));
            int tbodyIndex = 0;
            foreach (IWebElement tbodyElement in tbodys)
            {
                string agency = "", hotelName = "", _operator = "", voucher = "", roomType = "", note1 = "", note2 = "";
                agency = tbodyElement.FindElements(By.XPath("//*[@data-column-type='config']"))[tbodyIndex].Text;
                hotelName = tbodyElement.FindElements(By.XPath("//*[@data-column-type='hotelName']"))[tbodyIndex].Text;
                _operator = tbodyElement.FindElements(By.XPath("//*[@data-column-type='operator']"))[tbodyIndex].Text;
                voucher = tbodyElement.FindElements(By.XPath("//*[@data-column-type='voucher']"))[tbodyIndex].Text;
                roomType = tbodyElement.FindElements(By.XPath("//*[@data-column-type='roomType']"))[tbodyIndex].Text;

                reservationLists.Add(new ReservationListDto
                {
                    Agency = agency,
                    Hotel = hotelName,
                    Operator = _operator,
                    Voucher = voucher,
                    NoteMessage = note1,
                    RoomType = roomType,
                    WarningMessage = note2
                });

                tbodyIndex++;

                //}





                // IReadOnlyCollection<IWebElement> tables = driver.FindElements(By.XPath("//table[@class='table']/tbody"));

                //foreach (IWebElement tableElement in tables)
                //{
                //    IReadOnlyCollection<IWebElement> reservations = tableElement.FindElements(By.XPath("//*[@data-row-type='reservation']"));
                //    foreach (IWebElement element in reservations)
                //    {
                //        var td = element.FindElement(By.TagName("td"));
                //        string agency = td.FindElement(By.XPath("//*[@data-column-type='config']")).Text;
                //        string hotelName = td.FindElement(By.XPath("//*[@data-column-type='hotelName']")).Text;
                //        string _operator = td.FindElement(By.XPath("//*[@data-column-type='operator']")).Text;
                //        string voucher = td.FindElement(By.XPath("//*[@data-column-type='voucher']")).Text;
                //        string roomType = td.FindElement(By.XPath("//*[@data-column-type='roomType']")).Text;
                //        string note1 = element.FindElements(By.XPath("//*[@data-row-type='note']")).First().Text;
                //        string note2 = element.FindElements(By.XPath("//*[@data-row-type='note']")).Last().Text;
                //        reservationLists.Add(new ReservationListDto
                //        {
                //            Agency = agency,
                //            Hotel = hotelName,
                //            Operator = _operator,
                //            Voucher = voucher,
                //            NoteMessage = note1,
                //            RoomType = roomType,
                //            WarningMessage = note2
                //        });
                //    }

                //}

            }
            //driver.Quit();
            return JsonConvert.SerializeObject(reservationLists);
        }
    }
}