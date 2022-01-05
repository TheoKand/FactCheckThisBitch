using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DockerVpnAndCURL
{
    public static class Extensions
    {
        public static string NordVpnContainerLog(this string containerLog)
        {
            var match = Regex.Match(containerLog, "You are connected to ([^!]*)!");
            if (match.Success)
            {
                return match.Groups[0].Value;
            } else
            {
                if (containerLog.Contains("[Error] logging in: default api: Too Many Requests"))
                {
                    return "Too Many Requests";
                } else
                {
                    return "FAILED TO CONNECT";
                }
                
            }
        }
    }
}
