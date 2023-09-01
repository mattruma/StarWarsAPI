using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using StarWarsAPI.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace StarWarsAPI
{
    public static class CharacterList
    {
        [FunctionName(nameof(CharacterList))]
        [OpenApiOperation(operationId: nameof(CharacterList), tags: new[] { "Character" }, Summary = "Gets a list of characters.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Character>), Description = "Returns a list of characters.")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "characters")] HttpRequest req,
            [Sql(commandText: "SELECT * FROM Character ORDER BY name",
                commandType: System.Data.CommandType.Text,
                connectionStringSetting: "SqlConnectionString")] IEnumerable<CharacterData> characters)
        {
            return new OkObjectResult(characters.Select(x => new Character(x)));
        }
    }
}
