using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using StarWarsAPI.Shared;
using System.Collections.Generic;

namespace StarWarsAPI
{
    public static class CharacterDeleteById
    {
        [OpenApiOperation(operationId: nameof(CharacterDeleteById), tags: new[] { "Character" }, Description = "Deletes character by the unique identifier.")]
        [FunctionName(nameof(CharacterDeleteById))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "characters/{id}")] HttpRequest req,
            [Sql(commandText: "DELETE FROM Character WHERE CharacterID = @CharacterID",
                commandType: System.Data.CommandType.Text,
                parameters: "@CharacterID={id}",
                connectionStringSetting: "SqlConnectionString")] IEnumerable<CharacterData> charactersToDelete)
        {
            return new OkResult();
        }
    }
}
