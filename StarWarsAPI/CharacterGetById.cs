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
    public static class CharacterGetById
    {
        [OpenApiOperation(operationId: nameof(CharacterGetById), tags: new[] { "Character" }, Description = "Gets a character by the unique identifier.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Character), Description = "Returns a character.")]
        [FunctionName(nameof(CharacterGetById))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "characters/{id}")] HttpRequest req,
            [Sql(commandText: "SELECT * FROM Character WHERE CharacterID = @CharacterID",
                commandType: System.Data.CommandType.Text,
                parameters: "@CharacterID={id}",
                connectionStringSetting: "SqlConnectionString")] IEnumerable<CharacterData> characters)
        {
            var character = characters.FirstOrDefault();

            if (character == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            return new OkObjectResult(new Character(character));
        }
    }
}
