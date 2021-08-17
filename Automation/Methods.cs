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
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    class Methods
    {
        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();
            
            //enter Email and password for each account you want to search with
            User user1 = new User()
            {
                Email = "Your@Email",
                Password = "YourPassword"
            };
           
            users.Add(user1);
            return users;
        }
        
        public static void SearchWeb()
        {
            foreach (User user in GetUsers())
            {
            
                IWebDriver driver = new FirefoxDriver();

                //enter URL for log in page
                driver.Navigate().GoToUrl("Bing Log in page");

                //getting email textbox & passing your@email
                driver.FindElement(By.Id("i0116")).SendKeys(user.Email);
                driver.FindElement(By.Id("idSIButton9")).Click();
                Thread.Sleep(1500);

                //getting password textbox & passing your@email
                driver.FindElement(By.Id("i0118")).SendKeys(user.Password);
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
                    var searchItem = words[index];
                    //remove searched word from list (Only want to search each word once)
                    words.RemoveAt(index);

                    //clicking into search bar
                    driver.FindElement(By.Id("sb_form_q")).Click();
                    Thread.Sleep(1300);

                    //passing word from list into search bar
                    driver.FindElement(By.Id("sb_form_q")).SendKeys(searchItem);
                    Thread.Sleep(1200);

                    //execute search function
                    driver.FindElement(By.Id("sb_form_q")).SendKeys(Keys.Enter);
                    Thread.Sleep(1400);

                    //clear search bar
                    driver.FindElement(By.Id("sb_form_q")).Clear();
                    x++;
                }

            //Close the browser
            driver.Quit();
            Thread.Sleep(1400);
           }
        }
    }
}
