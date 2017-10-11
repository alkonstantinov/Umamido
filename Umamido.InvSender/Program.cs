using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.InvSender
{
    class Program
    {
        static string tempPath = Path.GetTempPath();
        static string GetPdf(int reqId, bool IsOriginal)
        {
            string wkhtmlPath = ConfigurationManager.AppSettings["PATHTOWKHTMLTOPDF"];
            string url = ConfigurationManager.AppSettings["URLTOINV"];
            string printUrl = url + "?invId=" + reqId + "&isOriginal=" + IsOriginal.ToString();
            string pdf_path = Path.Combine(tempPath, reqId.ToString() + (IsOriginal ? "_o" : "_c") + ".pdf");

            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = wkhtmlPath;
            psi.Arguments = "  \"" + printUrl + "\" \"" + pdf_path + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.EnableRaisingEvents = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();

                process.WaitForExit();
                process.Close();
                string result = "";

                if (File.Exists(pdf_path))
                {
                    result = pdf_path;
                    //File.Delete(pdf_path);
                }

                return result;

            }
        }

        static void SendMail(string toEmail, string invFnm)
        {

            try
            {
                MailMessage mm = new MailMessage(ConfigurationManager.AppSettings["FromEmail"], toEmail);
                mm.Subject = "Invoice";
                mm.Body = "";
                Attachment at = new Attachment(invFnm, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = at.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(invFnm);
                disposition.ModificationDate = File.GetLastWriteTime(invFnm);
                disposition.ReadDate = File.GetLastAccessTime(invFnm);
                disposition.FileName = Path.GetFileName(invFnm);
                disposition.Size = new FileInfo(invFnm).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                mm.Attachments.Add(at);

                SmtpClient cl = new SmtpClient();
                cl.Send(mm);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.StackTrace);
                //Console.ReadLine();
                return;
            }

        }

        static void Main(string[] args)
        {
            var dl = new Umamido.DL.DLFuncs();
            var x = dl.GetRestaurant(5);
            foreach (var item in dl.GetReqForInvoice())
            {
                string fnm = GetPdf(item.ReqId, true);
                SendMail(item.Mail, fnm);
                fnm = GetPdf(item.ReqId, false);
                SendMail(ConfigurationManager.AppSettings["AccountantEmail"], fnm);
            }

        }
    }
}

