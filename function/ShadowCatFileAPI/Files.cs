using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json;
using ShadowCatFileAPI.Contracts;

namespace ShadowCatFileAPI
{
    public class Files
    {
        const string unusedUsername = "unused";

        private readonly CosmosClient cosmosClient;
        private readonly Container container;

        public Files(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
            this.container = cosmosClient.GetContainer("correlations", "container1");
        }

        [FunctionName("Files")]
        public async Task<IActionResult> Run(
                    [HttpTrigger(AuthorizationLevel.Function, "get", "patch", Route = "files")] HttpRequest req,
                    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a Files request.");

            if (req.Headers.ContainsKey("Authorization") && req.Headers["Authorization"][0].StartsWith("Bearer "))
            {
                var token = req.Headers["Authorization"][0].Substring("Bearer ".Length);
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                var user = jwtSecurityToken.Claims.First(claim => claim.Type == "email").Value;

                if (string.IsNullOrEmpty(user)) {
                    return new NotFoundResult();
                }

                switch (req.Method)
                {
                    case "GET":

                        return new OkObjectResult(GetUserCorrelationsAsync(user).Result);

                    case "PATCH":

                        var payload = await new StreamReader(req.Body).ReadToEndAsync();

                        PatchCorrelationsRequest requestBody = JsonConvert.DeserializeObject<PatchCorrelationsRequest>(payload);

                        try
                        {
                            var replacedItem = await UpdateUserCorrelationAsync(user, requestBody.Region, requestBody.Description);
                        } 
                        catch (Exception ex)
                        {
                            return new UnprocessableEntityObjectResult(ex.Message);
                        }

                        return new NoContentResult();
                }


                return new OkResult();
            }
            return new ForbidResult();
        }

        public async Task<List<Correlation>> GetUserCorrelationsAsync(string user)
        {
            List<Correlation> result = new();

            var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM correlations c WHERE c.username = @user"
            ).WithParameter("@user", user);

            // Query multiple correlations from container
            using (FeedIterator<Correlation> feed = container.GetItemQueryIterator<Correlation>(
                queryDefinition: parameterizedQuery
            ))
            {
                // Iterate query result pages
                while (feed.HasMoreResults)
                {
                    // Iterate query results
                    foreach (var item in await feed.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        public async Task<Correlation> UpdateUserCorrelationAsync(string user, string region, string desc)
        {
            List<Correlation> unusedCorrelations = new();

            // Get an unused client
            var parameterizedQuery = new QueryDefinition(
                query: "SELECT * FROM correlations c WHERE c.username = @user OFFSET 0 LIMIT 1"
            ).WithParameter("@user", unusedUsername);

            // Query multiple correlations from container
            using FeedIterator<Correlation> feed = container.GetItemQueryIterator<Correlation>(
                queryDefinition: parameterizedQuery
            );

            while (feed.HasMoreResults)
            {
                FeedResponse<Correlation> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (Correlation item in response)
                {
                    unusedCorrelations.Add(item);
                }
            }

            if (unusedCorrelations.Count == 0)
            {
                throw new TaskCanceledException("no avaialble client for the given region");
            }

            var correlation = unusedCorrelations.First();

            return await container.ReplaceItemAsync(
                item: correlation,
                id: correlation.Id,
                partitionKey: new PartitionKey(unusedUsername)
            );
        }
    }
}
