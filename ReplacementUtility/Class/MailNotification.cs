//  -------------------------------------------------------------------------
//  <copyright file="MailNotification.cs"  author="Rajesh Thomas | iamrajthomas" >
//      Copyright (c) 2021 All Rights Reserved.
//  </copyright>
// 
//  <summary>
//       Mail Notification
//  </summary>
//  -------------------------------------------------------------------------

namespace ReplacementUtility.Class
{
    using System;
    using System.Net.Mail;
    using System.Net;

    //  <summary>
    //       Entity For Sending Mail Notification
    //  </summary>
    public class MailNotification
    {
        private readonly ReadAppConfigData _readAppConfigData = null;
        private bool IsOperationSucceed = false;

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public MailNotification()
        {
            _readAppConfigData = new ReadAppConfigData();
        }

        /// <summary>
        /// Initiate Mail Notification
        /// </summary>
        public bool Initiate()
        {
            IsOperationSucceed = SendMail();
            return IsOperationSucceed;
        }

        /// <summary>
        /// Send Email
        /// </summary>
        public bool SendMail()
        {
            #region Read SMTP Cofiguration Values

            string EmailServer = _readAppConfigData.ReadValueByKey("EmailServer");
            string EmailServerPort = _readAppConfigData.ReadValueByKey("EmailServerPort");
            string EmailUserName = _readAppConfigData.ReadValueByKey("EmailUserName");
            string EmailPassword = _readAppConfigData.ReadValueByKey("EmailPassword");
            string EmailTo = _readAppConfigData.ReadValueByKey("EmailTo");
            string EmailCC = _readAppConfigData.ReadValueByKey("EmailCC");
            string EmailSubject = _readAppConfigData.ReadValueByKey("EmailSubject");
            string EmailBody = _readAppConfigData.ReadValueByKey("EmailBody");
            string EmailSignature = _readAppConfigData.ReadValueByKey("EmailSignature");

            #endregion

            #region Instatiate SMTP Client

            SmtpClient smtp = new SmtpClient
            {
                Host = EmailServer,
                Port = Convert.ToInt32(EmailServerPort),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(EmailUserName, EmailPassword)
            };

            //using (MailMessage mail = new MailMessage(new MailAddress(EmailUserName, EmailUserName), new MailAddress(EmailTo, EmailTo))
            //{
            //    Subject = EmailSubject + " On " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"),
            //    Body = "<b>" + EmailSubject + " </b> " + "<br> You can do chill now.<b></b>.",
            //    BodyEncoding = System.Text.Encoding.UTF8,
            //    IsBodyHtml = true
            //})

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(EmailUserName, EmailUserName);
            foreach (var to in EmailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.To.Add(to);
            }
            mail.Subject = EmailSubject + " On " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            mail.Body = "<b>" + EmailSubject + " </b> " + "<br>" + EmailBody + "<br><br><br><br>With Regards,<br>" + EmailSignature + "<b></b>";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;

            #endregion

            try
            {
                Console.WriteLine("=================================================================");
                Console.WriteLine("Sending Mail Started.");
                Console.WriteLine("================================================================= \n");

                smtp.Send(mail);

                Console.WriteLine("=================================================================");
                Console.WriteLine("Mail is Sent.");
                Console.WriteLine("================================================================= \n");
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("***************************** ERROR *****************************");
                throw new ApplicationException("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("***************************** ERROR *****************************");
                Console.WriteLine("Exception caught in SendMail {0}", ex.ToString());
                Console.WriteLine("***************************** ERROR *****************************");
                return true;
            }
        }
    }
}
