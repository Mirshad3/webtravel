using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace webtravel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Privacy()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult SendMail(string username, String mail, String subject, String body)
        {
            bool isSendSuccess = false;
            try
            {
                SendMail2(body, subject, username, mail);
            }
            catch (Exception ex)
            {
                throw (new Exception("Mail send failed to loginId " + mail + ", though registration done." + ex.ToString() + "\n" + ex.StackTrace));
            }
            return Json("success");

        }

        public static bool SendMail2(String body, String subject, string username, String mail)
        {
            bool isSendSuccess = false;
            try
            {
                var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                var toEmailAddress = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
                var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();


                MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName),
                    new MailAddress(toEmailAddress, username));
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = mail +","+ body;

                var client = new SmtpClient();
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
                client.Host = smtpHost;
                client.EnableSsl = false;
                client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
                client.Send(message);
                isSendSuccess = true;
            }
            catch (Exception ex)
            {
                throw (new Exception("Mail send failed to loginId " + mail + ", though registration done." + ex.ToString() + "\n" + ex.StackTrace));
            }

            return isSendSuccess;
        }
    }
}