using Microsoft.Extensions.Configuration;
using PostmarkDotNet;

namespace TaskManager.Email
{
    public class EmailClient : IEmailClient
    {
        private readonly IConfiguration _configuration;
        private readonly string apiKey;
        private readonly string ccEmail;
        private readonly string fromEmail;
        private readonly string emailProvider;
        private readonly string replyTo;
        public EmailClient(IConfiguration configuration)
        {
            _configuration = configuration;
            apiKey = _configuration["EmailSettings:PostmarkKey"]!.ToString();
            ccEmail = _configuration["EmailSettings:CcEmail"]!.ToString();
            fromEmail = _configuration["EmailSettings:FromEmail"]!.ToString();
            emailProvider = _configuration["EmailSettings:EmailProvider"]!.ToString();
            replyTo = _configuration["EmailSettings:ReplyTo"]!.ToString();

        }

        public async Task<bool> SendEmail(string toEmail, string emailSubject, string htmlEmailBody, string fallbackemailBody, bool TrackOpens = false, string emailCategory = "")
        {
            //IDictionary<string, string> myHeaders = new Dictionary<string, string>
            //{
            //    {"X-CUSTOM-HEADER", "Header content"}
            //};
            // Send an email asynchronously:
            if (emailProvider.Equals("Postmark",StringComparison.OrdinalIgnoreCase))
            {
                var message = new PostmarkMessage()
                {
                    To = toEmail,
                    From = fromEmail,
                    TrackOpens = TrackOpens,
                    Subject = emailSubject,
                    HtmlBody = htmlEmailBody,
                    //MessageStream = "outbound",
                };
                if (!string.IsNullOrWhiteSpace(ccEmail))
                {
                    message.Cc = ccEmail;
                }
                if (!string.IsNullOrWhiteSpace(emailCategory))
                {
                    message.Tag = emailCategory;
                }
                if (!string.IsNullOrWhiteSpace(fallbackemailBody))
                {
                    message.TextBody = fallbackemailBody;
                }
                if (!string.IsNullOrWhiteSpace(replyTo))
                {
                    message.ReplyTo = replyTo;
                }
                //Headers = new HeaderCollection(myHeaders),
                //message.AddAttachment(imageContent, "test.jpg", "image/jpg", "cid:embed_name.jpg");

                var client = new PostmarkClient(apiKey);
                var sendResult = await client.SendMessageAsync(message);

                return sendResult.Status == PostmarkStatus.Success; 
            }
            return false;
        }
    }
}
