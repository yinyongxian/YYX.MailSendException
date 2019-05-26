using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace YYX.MailSendException
{
    internal static class CrashHandler
    {
        internal static void SendEmail(string host, string from, string password, string to, string body)
        {
            var sendEmail = true;

            #region RELEASE模式下发送

#if DEBUG
            sendEmail = false;
#endif

            #endregion

            try
            {
                if (sendEmail)
                {
                    Send(host, from, password, to, body);
                }
            }
            catch (Exception)
            {
                //不处理发送失败情况
            }
        }

        private static void Send(string host, string from, string password, string to, string body)
        {
            var subject = GenerateSubject();
            var environmentMessage = GenerateEnvironmentMessage();
            body = environmentMessage + "<br/>" + body;
            var mailMessage = new MailMessage(@from, to, subject, body)
            {
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            var smtpClient = new SmtpClient(host)
            {
                Credentials = new NetworkCredential(@from, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            smtpClient.SendAsync(mailMessage, null);
        }


        private static string GenerateEnvironmentMessage()
        {
            var productName = Format("软件名称", CrashMessage.ProductName);
            var productVersion = Format("软件版本", CrashMessage.ProductVersion);
            var lastWriteTime = Format("编译时间", CrashMessage.LastWriteTime.ToString(CultureInfo.CurrentCulture));
            var machineName = Format("机器名称", CrashMessage.MachineName);
            var userName = Format("当前登录用户", CrashMessage.UserName);
            var internalNetworkIpAddress = Format("内网IPAddress", CrashMessage.InternalNetworkIpAddress);
            var externalNetworkIpAddress = Format("公网IPAddress", CrashMessage.ExternalNetworkIpAddress);
            var osVersionString = Format("系统信息", CrashMessage.OsVersion.ToString());
            var platformString = Format("系统平台", CrashMessage.Platform.ToString());
            var processorCount = Format("处理器数量", CrashMessage.ProcessorCount.ToString());
            var dateTimeNow = Format("发生时间", CrashMessage.DateTimeNow.ToString(CultureInfo.CurrentCulture));
            const string remarks = "<p>来XXX自动发送的电子邮件<p/>";
            var strings = new[]
            {
                productName,
                productVersion,
                lastWriteTime,
                machineName,
                userName,
                internalNetworkIpAddress,
                externalNetworkIpAddress,
                osVersionString,
                platformString,
                processorCount,
                dateTimeNow,
                remarks
            };

            var environmentMessage = string.Concat(strings);
            return environmentMessage;
        }

        private static string Format(string text, string value)
        {
            return string.Format("{0}: {1}<br/>", text, value);
        }

        private static string GenerateSubject()
        {
            return string.Format("{0}-{1}-{2}", CrashMessage.ProductName, CrashMessage.ProductVersion, CrashMessage.DateTimeNow);
        }
    }
}