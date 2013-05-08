using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace Global.Utilities
{
    public static class Email
    {
        /// <summary>
        /// SendEmail method that does not take in a from email address and will attempt to use
        /// the default from FromEmailAddress from the AppSettings 
        /// </summary>
        /// <param name="toEmail">The address to send the email to.</param>
        /// <param name="subject">The subject line of the email.</param>
        /// <param name="body">The contents of the email message itself.</param>
        /// <param name="attachments">Any attachments to go along with the email.</param>
        /// <param name="isBodyHTML">Whether the email message contains html tags.</param>
        public static void SendEmail(
            string toEmail
            , string subject
            , string body
            , List<Attachment> attachments
            , bool isBodyHTML)
        {
            if (ConfigurationManager.AppSettings["FromEmailAddress"] == "")
            {
                throw new ApplicationException("FromEmailAddress AppSetting is not set");
            }

            SendEmail(
                ConfigurationManager.AppSettings["FromEmailAddress"]
                , toEmail
                , subject
                , body
                , attachments
                , isBodyHTML);
        }

        /// <summary>
        /// SendEmail method that allows input of a from email address, if from email address is blank
        /// the method will attempt to set it to the FromEmailAddress from the AppSettings, if the setting
        /// is not availabel it will set the value to noreply@prommis.com
        /// </summary>
        /// <param name="fromEmail">The address to use for who is sending the email.</param>
        /// <param name="toEmail">The address to send the email to.</param>
        /// <param name="subject">The subject line of the email.</param>
        /// <param name="body">The contents of the email message itself.</param>
        /// <param name="attachments">Any attachments to go along with the email.</param>
        /// <param name="isBodyHTML">Whether the email message contains html tags.</param>
        public static void SendEmail(
            string fromEmail
            , string toEmail
            , string subject
            , string body
            , List<Attachment> attachments
            , bool isBodyHTML)
        {
            if (toEmail == null
                || toEmail == string.Empty)
            {
                throw new ArgumentException("Must supply a to email address. Cannot send an email without a recipient.", "toEmail");
            }
            if (ConfigurationManager.AppSettings["EmailHost"] == "")
            {
                throw new ApplicationException("EmailHost AppSetting is not set");
            }

            // Setting a default for the from email if unspecified by caller.
            if (fromEmail == "")
            {
                if (ConfigurationManager.AppSettings["FromEmailAddress"] != "")
                {
                    fromEmail = ConfigurationManager.AppSettings["FromEmailAddress"];
                }
                else
                {
                    fromEmail = "noreply@prommis.com";
                }
            }

            MailMessage message = new MailMessage(
                fromEmail
                , toEmail
                , subject
                , body);

            message.IsBodyHtml = isBodyHTML;

            if (attachments != null)
            {
                foreach (Attachment attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }
            }

            SmtpClient emailClient = new SmtpClient(ConfigurationManager.AppSettings["EmailHost"]);
            try
            {
                emailClient.Send(message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Unable to send email. Exception: {0}", ex.Message)
                    + Environment.NewLine
                    + "Parameters"
                    + Environment.NewLine
                    + "--------------"
                    + Environment.NewLine
                    + "Email Host: " + ConfigurationManager.AppSettings["EmailHost"]
                    + Environment.NewLine
                    + "From Email Address: " + (fromEmail == "" ? "noreply@prommis.com" : fromEmail)
                    + Environment.NewLine
                    + "To Email Address: " + toEmail
                    + Environment.NewLine
                    + "Subject: " + subject
                    + Environment.NewLine
                    + "Body: " + body
                    + Environment.NewLine
                    + "Attachments: " + (attachments == null ? "<None>" : attachments.Count.ToString())
                    + Environment.NewLine
                    + "Is Body in HTML: " + isBodyHTML.ToString());
            }
        }
    }
}