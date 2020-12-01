using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CEBAlert
{
    public static class AlertPerHour
    {
        [FunctionName("AlertPerHour")]
        public static void Run([TimerTrigger(" 0 */6 * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                string CEBURL = "https://ceb.mu/customer-corner/power-outage-information";
                string[] AlertingTown = new string[]
                {
                "magnien",
                "hill"
                };
                StringBuilder SMSMessage = new StringBuilder();
                List<string> Districts = new List<string>();
                HttpClient httpClient = new HttpClient();
                string website = httpClient.GetAsync(CEBURL).Result.Content.ReadAsStringAsync().Result;
                int IndexStart = website.IndexOf("var arDistrictLocations =");
                website = website.Substring(IndexStart);
                int IndexEnd = website.IndexOf("$(document).ready(function(){");
                website = website.Substring(0, IndexEnd - 2);
                website = website.Replace("var arDistrictLocations = ", "");

                dynamic value = JsonConvert.DeserializeObject<dynamic>(website);
                Districts.Add(value.grandport.ToString());
                Districts.Add(value.plainewilhems.ToString());

                foreach (string district in Districts)
                {
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(district);
                    foreach (HtmlNode record in document.DocumentNode.Descendants("tr"))
                    {
                        string OutageTime = record.Descendants("td").ElementAt(0).InnerText;
                        string Locality = record.Descendants("td").ElementAt(1).InnerText;
                        string LocalityDetails = record.Descendants("td").ElementAt(2).InnerText;
                        if (AlertingTown.Any(x => Locality.ToLower().Contains(x)))
                        {
                            SMSMessage.AppendLine($"{Locality} on {OutageTime} ");
                        }

                    }
                }

                if (SMSMessage.Length != 0)
                {
                    SMSMessage.AppendLine($"URL: https://tinyurl.com/yy37htvx");
                    string SMS = SMSMessage.ToString();
                    string accountSid = Environment.GetEnvironmentVariable("TwilioaccountSid");
                    string authToken = Environment.GetEnvironmentVariable("TwilioauthToken");
                    string SenderNumber = Environment.GetEnvironmentVariable("TwilioSender");
                    string ReceiverNumber = Environment.GetEnvironmentVariable("TwilioReceiver");
                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: SMS,
                        from: new Twilio.Types.PhoneNumber(SenderNumber),
                        to: new Twilio.Types.PhoneNumber(ReceiverNumber)
                    );

                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
