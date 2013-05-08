using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace Global.Utilities
{
    public static class Fax
    {
        public static void SendFax(
            string toFaxNumber
            , string subject
            , string faxCoverPage
            , List<Attachment> attachments)
        {
            if (ConfigurationManager.AppSettings["FromFaxAddress"] == "")
            {
                throw new ApplicationException("FromFaxAddress AppSetting is not set");
            }

            // Remove all non-digit characters from toFaxNumber
            StringBuilder faxsb = new StringBuilder();
            foreach (char c in toFaxNumber)
            {
                if (char.IsDigit(c)) { faxsb.Append(c); }
            }

            // Prepend a 1 if 10 digits
            if (faxsb.Length == 10) { faxsb.Insert(0, 1); }

            toFaxNumber = faxsb.ToString();

            if (toFaxNumber.Length != 11)
            {
                throw new ApplicationException("toFaxNumber was not the expected length (11 digits). The country code may be missing.");
            }

            Email.SendEmail(
                ConfigurationManager.AppSettings["FromFaxAddress"]
                , toFaxNumber + "@myfax.com"
                , subject
                , faxCoverPage
                , attachments
                , true);
        }
    }
}
