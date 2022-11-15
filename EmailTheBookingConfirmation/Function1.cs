using System;
using System.IO;
using Microsoft.Azure.KeyVault.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Nest;
using SendGrid.Helpers.Mail;

namespace EmailTheBookingConfirmation
{
    public class EmailTheBookingStatus
    {
        [FunctionName("EmailTheBookingStatus")]
        public static void Run(
            [BlobTrigger("licenses/{name}", Connection = "BlobConnectionString")]Stream myBlob,
            [SendGrid(ApiKey = "SendgridAPIKey")] out SendGridMessage sendGridMessage,
            string name, 
            ILogger log)
        {
            try
            {
                log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
                sendGridMessage = new SendGridMessage
                {
                    From = new EmailAddress("akb.tech17@gmail.com", "AzureFuncApps"),
                };
                sendGridMessage.AddTo("anshulkumar.bansal@lntinfotech.com");
                sendGridMessage.SetSubject("Awesome Azure Function app");
                sendGridMessage.AddContent("text/html", "Content");
            }
            catch (Exception ex)
            {
                sendGridMessage = new SendGridMessage();
                Console.WriteLine(ex.Message);
            }
            finally
            { 
                myBlob.Close();
            }
        }
    }
}
