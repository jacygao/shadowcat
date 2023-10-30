using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Azure.Cosmos;

namespace ShadowCatFileAPI
{
    public class Files
    {
        private readonly CosmosClient cosmosClient;
        private readonly Container container;

        public Files(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
            this.container = this.cosmosClient.GetContainer("correlations", "container1");
        }

        [FunctionName("Files")]
        public static async Task<IActionResult> Run(
                    [HttpTrigger(AuthorizationLevel.Function, "get", "patch", Route = "files")] HttpRequest req,
                    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a GetFiles request.");

            if (req.Headers.ContainsKey("Authorization") && req.Headers["Authorization"][0].StartsWith("Bearer "))
            {
                var token = req.Headers["Authorization"][0].Substring("Bearer ".Length);
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                var user = jwtSecurityToken.Claims.First(claim => claim.Type == "email").Value;

                switch (req.Method)
                {
                    case "GET":
                        this.container.GetItemLinqQueryable<List<Correlation>>();
                }


                return new OkResult();
            }
            return new ForbidResult();
        }
    }
}
