using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation
{
    class Methods
    {
        public static void SearchWeb()
        {

            IWebDriver driver = new FirefoxDriver();

            //enter URL for log in page
            driver.Navigate().GoToUrl("Bing Log in page");

            //getting email textbox & passing your@email
            driver.FindElement(By.Id("i0116")).SendKeys("your@email");
            driver.FindElement(By.Id("idSIButton9")).Click();
            Thread.Sleep(1500);

            //getting password textbox & passing your@email
            driver.FindElement(By.Id("i0118")).SendKeys("yourPassword");
            driver.FindElement(By.Id("idSIButton9")).Click();
            Thread.Sleep(1500);

            int x = 0;

            //list of words going to be searched
            var words = new List<string>
                { "C#", "JavaScript", "SQL", "Visual Studio", "Coffee" };

            while (x < words.Count)
            {
                Random random = new Random();
                int index = random.Next(words.Count);
                var name = words[index];
                //remove searched word from list (Only want to search each word once)
                words.RemoveAt(index);

                //passing word from list into search bar
                driver.FindElement(By.Id("sb_form_q")).SendKeys(name);
                Thread.Sleep(1000);

                driver.FindElement(By.Id("sb_form_go")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.Id("sb_form_q")).Clear();

            }

            //Close the browser
            driver.Quit();


        }

    }
}
