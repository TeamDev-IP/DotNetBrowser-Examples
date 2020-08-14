#region Copyright

// Copyright © 2020, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumChromeDriver
{
    public class SeleniumInstance
    {
        private string RemoteDebuggingAddress { get; }
        private string ApplicationFullPath { get; }

        public event Action Connected;

        public SeleniumInstance(int debuggingPort)
        {
            ApplicationFullPath = Process.GetCurrentProcess()?.MainModule?.FileName;
            RemoteDebuggingAddress = $"localhost:{debuggingPort}";
        }

        public async Task ConnectAndRun()
        {
            await Task.Run(async () =>
            {
                IWebDriver webDriver = await Connect();

                //Time for displaying the loaded page
                await Task.Delay(3000);

                await RunScenario(webDriver);

                webDriver.Quit();
            });
        }

        private async Task<IWebDriver> Connect()
        {
            return await Task.Run(() =>
            {
                ChromeOptions options = new ChromeOptions()
                {
                    BinaryLocation = ApplicationFullPath,
                    DebuggerAddress = RemoteDebuggingAddress
                };

                IWebDriver webDriver = new ChromeDriver(options)
                {
                    Url = "https://www.teamdev.com/dotnetbrowser"
                };

                OnConnected();

                return webDriver;
            });
        }

        private async Task RunScenario(IWebDriver webDriver)
        {
            await Task.Run(() =>
            {
                IWebElement evaluateButton = webDriver.FindElement(By.XPath("//*[@id='header']/div[1]/div/ul/li[5]/a"));
                evaluateButton.Click();

                IWebElement nameTextbox = webDriver.FindElement(By.Name("name"));
                nameTextbox.SendKeys("John Doe");

                IWebElement emailTextbox = webDriver.FindElement(By.Name("email"));
                emailTextbox.SendKeys("sales@teamdev.com");
            });
        }

        protected virtual void OnConnected()
            => Connected?.Invoke();
    }
}
