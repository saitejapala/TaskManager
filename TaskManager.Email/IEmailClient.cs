using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Email
{
    public interface IEmailClient
    {
        public Task<bool> SendEmail(string toEmail, string emailSubject, string htmlEmailBody, string fallbackemailBody, bool TrackOpens = false, string emailCategory = "");
    }
}
