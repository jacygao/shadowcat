using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace ShadowCatFileAPI
{
    public static class Users
    {
        [FunctionName("Users")]
        public static async Task<IActionResult> Run(
                    [HttpTrigger(AuthorizationLevel.Function, "get, post", Route = "users")] HttpRequest req,
                    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a GetFiles request.");

            if (req.Headers.ContainsKey("Authorization") && req.Headers["Authorization"][0].StartsWith("Bearer "))
            {
                var token = req.Headers["Authorization"][0].Substring("Bearer ".Length);
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                var user = jwtSecurityToken.Claims.First(claim => claim.Type == "email").Value;

                return new OkResult();
            }

            switch (req.Method)
            {
                case "POST":
                    break;
                case "GET":
                    break;
            }

            return new ForbidResult();
        }
    }
}
