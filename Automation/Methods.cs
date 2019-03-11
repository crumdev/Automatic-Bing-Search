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
            driver.Navigate().GoToUrl("https://login.live.com/login.srf?wa=wsignin1.0&rpsnv=13&ct=1552314175&rver=6.7.6631.0&wp=MBI_SSL&wreply=https%3a%2f%2fwww.bing.com%2fsecure%2fPassport.aspx%3frequrl%3dhttps%253a%252f%252fwww.bing.com%252f%253fwlexpsignin%253d1%26sig%3d1590AE9E3BD0685321E7A3823A786966&lc=1033&id=264960&CSRFToken=053ce532-98dc-4eae-8c5f-4a46b7385945&aadredir=1");

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
