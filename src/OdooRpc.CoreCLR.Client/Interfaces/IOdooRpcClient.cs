using System.Threading.Tasks;
using OdooRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Interfaces
{
    public interface IOdooRpcClient
    {
        OdooSessionInfo SessionInfo { get; }

        Task<OdooVersionInfo> GetOdooVersion();
        Task Authenticate();
        void SetUserId(int userId);
        Task<T> Get<T>(string model, long id);
        Task<T> Get<T>(OdooGetParameters parameters);
    }
}