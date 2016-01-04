using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Models.Parameters;

namespace OdooRpc.CoreCLR.Client.Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var p = new Program();
            p.LoginToOdoo().Wait();
            p.GetPartners().Wait();
            p.SearchPartners().Wait();

            Console.WriteLine("Done! Press a key to exit...");
            Console.ReadKey();
        }

        private OdooConnectionInfo OdooConnection;
        private IOdooRpcClient OdooRpcClient;

        public Program()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                var settings = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("appsettings.json"));
                this.OdooConnection = settings["OdooConnection"].ToObject<OdooConnectionInfo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading app settings: {0}", ex.Message);
            }
        }

        public async Task LoginToOdoo()
        {
            try
            {
                this.OdooRpcClient = new OdooRpcClient(this.OdooConnection);

                var odooVersion = await this.OdooRpcClient.GetOdooVersion();

                Console.WriteLine("Odoo Version: {0} - {1}", odooVersion.ServerVersion, odooVersion.ProtocolVersion);

                await this.OdooRpcClient.Authenticate();

                if (this.OdooRpcClient.SessionInfo.IsLoggedIn)
                {
                    Console.WriteLine("Login successful => User Id: {0}", this.OdooRpcClient.SessionInfo.UserId);
                }
                else
                {
                    Console.WriteLine("Login failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to Odoo: {0}", ex.Message);
            }
        }

        public async Task GetPartners()
        {
            try
            {
                var reqParams = new OdooGetParameters("hr.department");
                reqParams.Ids.Add(6);
                //reqParams.Ids.Add(7);

                reqParams.Fields.Add("name");
                reqParams.Fields.Add("company_id");

                var partners = await this.OdooRpcClient.Get<JObject[]>(reqParams);

                Console.WriteLine(partners.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting partners from Odoo: {0}", ex.Message);
            }
        }

        public async Task SearchPartners()
        {
            try
            {
                var reqParams = new OdooSearchParameters(
                    "hr.department", 
                    new OdooDomainFilter().Filter("name", "like", "SIC"),
                    new OdooPaginationParameters(0, 1)
                );

                var partners = await this.OdooRpcClient.Search<long[]>(reqParams);

                Console.WriteLine(partners.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting partners from Odoo: {0}", ex.Message);
            }
        }
    }
}