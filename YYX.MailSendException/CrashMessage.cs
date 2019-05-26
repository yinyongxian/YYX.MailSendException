using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YYX.MailSendException
{
    public static class CrashMessage
    {
        /// <summary>
        /// 软件名称
        /// </summary>
        public static string ProductName
        {
            get { return Application.ProductName; }
        }

        /// <summary>
        /// 软件版本
        /// </summary>
        public static string ProductVersion
        {
            get { return Application.ProductVersion; }
        }

        /// <summary>
        /// 软件编译时间
        /// </summary>
        public static DateTime LastWriteTime
        {
            get
            {
                var fileInfo = new FileInfo(Application.ExecutablePath);
                return fileInfo.LastWriteTime;
            }
        }

        /// <summary>
        /// 机器名称
        /// </summary>
        public static string MachineName
        {
            get { return Environment.MachineName; }
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static string UserName
        {
            get { return Environment.UserName; }
        }

        /// <summary>
        /// 内网IPAddress
        /// </summary>
        public static string InternalNetworkIpAddress
        {
            get
            {
                var iPAddresses =
                    Dns
                        .GetHostAddresses(Dns.GetHostName())
                        .Where(ip =>
                            ip.GetAddressBytes().Length == 4
                        )
                        .Select(iPAddress =>
                            iPAddress
                        );

                return string.Join(", ", iPAddresses);
            }
        }

        /// <summary>
        /// 公网IPAddress
        /// </summary>
        public static string ExternalNetworkIpAddress
        {
            get
            {
                return GetExternalNetworkIpAddress();
            }
        }

        private static string GetExternalNetworkIpAddress()
        {
            const string message = "获取失败";
            const string url = "https://cmyip.com";
            try
            {
                var webRequest = WebRequest.Create(url);
                webRequest.Timeout = 10000;
                var stream = webRequest.GetResponse().GetResponseStream();
                if (stream != null)
                {
                    var streamReader = new StreamReader(stream);
                    var readValue = streamReader.ReadToEnd();
                    const string pattern = @"([\d]{1,3}\.){3}[\d]{1,3}";
                    var match = Regex.Match(readValue, pattern);
                    return match.Success ? match.Value : message;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return message;
        }

        /// <summary>
        /// 系统信息
        /// </summary>
        public static OperatingSystem OsVersion
        {
            get { return Environment.OSVersion; }
        }

        /// <summary>
        /// 系统平台
        /// </summary>
        public static PlatformID Platform
        {
            get { return OsVersion.Platform; }
        }

        /// <summary>
        /// 处理器数量
        /// </summary>
        public static int ProcessorCount
        {
            get { return Environment.ProcessorCount; }
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        public static DateTime DateTimeNow
        {
            get { return DateTime.Now; }
        }
    }
}