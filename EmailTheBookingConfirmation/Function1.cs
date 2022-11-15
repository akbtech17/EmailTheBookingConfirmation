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
                StreamReader reader = new StreamReader(myBlob); 
                string transactionDetails = reader.ReadToEnd();

                string message = GetBetween(transactionDetails, "Msssage : ", "\r\n");
                string email = GetBetween(transactionDetails, "Email : ", "\r\n");

                sendGridMessage = new SendGridMessage
                {
                    From = new EmailAddress("akb.tech17@gmail.com", "Book My Movie"),
                };
                sendGridMessage.AddTo(email);

                if (message.Equals("BCNF"))
                {
                    string customerName = GetBetween(transactionDetails, "Name : ", "\r\n");
                    string movieName = GetBetween(transactionDetails, "Movie name : ", "\r\n");
                    string noOfTicketsBooked = GetBetween(transactionDetails, "Number of tickets booked : ", "\r\n");
                    string seatNos = GetBetween(transactionDetails, "Seat numbers : ", "\r\n");
                    string totalCost = GetBetween(transactionDetails, "Total cost : ", "\r\n");
                    string transactionTime = GetBetween(transactionDetails, "Transaction time : ", "\r\n");
                    string showTime = GetBetween(transactionDetails, "Show time : ", "\r\n");

                    
                    sendGridMessage.SetSubject("Your booking is Confirmed!");

                    string htmlContent = "" +
                        $"<h1>Thankyou {customerName}</h1>" +
                        $"<h3>for booking ticket from book my movie</h3>" +
                        $"<hr/>" +
                        $"<br>" +
                        $"<h2>Movie Name : {movieName}</h2>" +
                        $"<h2>Show Time :  {showTime}</h2>" +
                        $"<h3>No of Seats Booked :  {noOfTicketsBooked}</h3>" +
                        $"<h3>Seat Numbers :  {seatNos}</h3>" +
                        $"<h4>Venue : Cinepolis: WESTEND Mall Aundh, Pune(AUDI 03) Pune, Pune</h4>" +
                        $"<hr/>" +
                        $"<h5>Transaction Time : {transactionTime}</h5>" +
                        $"<h5>Total Cost : {totalCost}</h5>";
                      
                    sendGridMessage.AddContent("text/html", htmlContent);
                }
                else 
                {
                    string customerName = GetBetween(transactionDetails, "Name : ", "\r\n");
                    string movieName = GetBetween(transactionDetails, "Movie name : ", "\r\n");
                    string noOfTicketsCancelled = GetBetween(transactionDetails, "Number of tickets cancelled : ", "\r\n");
                    string seatNos = GetBetween(transactionDetails, "Seat numbers : ", "\r\n");
                    string refundCost = GetBetween(transactionDetails, "Refund cost : ", "\r\n");
                    string transactionTime = GetBetween(transactionDetails, "Transaction time : ", "\r\n");
                    string showTime = GetBetween(transactionDetails, "Show time : ", "\r\n");

                    sendGridMessage.SetSubject("Your booking is Cancelled!");

                    string htmlContent = "" +
                        $"<h1>Thankyou {customerName}</h1>" +
                        $"<h3>for using book my movie</h3>" +
                        $"<hr/>" +
                        $"<br>" +
                        $"<h2>Movie Name : {movieName}</h2>" +
                        $"<h2>Show Time :  {showTime}</h2>" +
                        $"<h3>No of Seats Cancelled :  {noOfTicketsCancelled}</h3>" +
                        $"<h3>Seat Numbers :  {seatNos}</h3>" +
                        $"<h4>Venue : Cinepolis: WESTEND Mall Aundh, Pune(AUDI 03) Pune, Pune</h4>" +
                        $"<hr/>" +
                        $"<h5>Cancellation Time : {transactionTime}</h5>" +
                        $"<h5>Refund Cost : {refundCost}</h5>";

                    sendGridMessage.AddContent("text/html", htmlContent);
                }

                

 

                log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
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

        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (strEnd.Equals(""))
            {
                int start, end;
                start = strSource.IndexOf(strStart, 0) + strStart.Length;
                end = strSource.Length;
                return strSource.Substring(start, end - start);
            }
            else if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int start, end;
                start = strSource.IndexOf(strStart, 0) + strStart.Length;
                end = strSource.IndexOf(strEnd, start);
                return strSource.Substring(start, end - start);
            }

            return "";
        }
    }
}
