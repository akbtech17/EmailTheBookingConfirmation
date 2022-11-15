using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace EmailTheBookingConfirmation
{
    public class EmailTheBookingStatus
    {
        [FunctionName("EmailTheBookingStatus")]
        public void Run([BlobTrigger("licenses/{name}", Connection = "BlobConnectionString")]Stream myBlob, string name, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            { 
                myBlob.Close();
            }
        }
    }
}
