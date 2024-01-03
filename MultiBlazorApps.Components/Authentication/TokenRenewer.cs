using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace MultiBlazorApps.Components.Authentication
{
    public class TokenRenewer : IDisposable
    {
        public TokenRenewer(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        System.Timers.Timer timer;
        private readonly ILoginService loginService;

        public void Initiate()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000 * 60 * 4; // 4 minutes
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("timer elapsed");
            loginService.TryRenewToken();
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }

}
