using System;
using OdooRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client
{
    internal static class OdooEndpoints
    {
        public static Uri GetAuthenticationUri(OdooConnectionInfo connectionInfo)
        {
            return GetEndpointUri(connectionInfo, "/web/session/authenticate");
        }
        
        private static Uri GetEndpointUri(OdooConnectionInfo connectionInfo, string endpoint)
        {
            return new Uri(string.Format("{0}://{1}:{2}{3}",
                GetConnectionProtocol(connectionInfo),
                connectionInfo.Host,
                connectionInfo.Port,
                endpoint
            ));
        }
        
        private static string GetConnectionProtocol(OdooConnectionInfo connectionInfo)
        {
            return connectionInfo.IsSSL ? "https" : "http";
        }
    }
}