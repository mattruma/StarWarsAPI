using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using StarWarsAPI.Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StarWarsAPI
{
    public static class CharacterUpdateById
    {
        [OpenApiOperation(operationId: nameof(CharacterUpdateById), tags: new[] { "Character" }, Description = "Updates a character by the unique identifier.")]
        [OpenApiParameter(name: "id", Description = "The unique identifier.", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Character), Description = "Returns a character.")]
        [FunctionName(nameof(CharacterUpdateById))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "characters/{id}")] HttpRequest req,
            [Sql(commandText: "SELECT * FROM Character WHERE CharacterID = @CharacterID",
                commandType: System.Data.CommandType.Text,
                parameters: "@CharacterID={id}",
                connectionStringSetting: "SqlConnectionString")] IEnumerable<CharacterData> charactersToUpdate,
            [Sql(commandText: "Character",
                connectionStringSetting: "SqlConnectionString")] IAsyncCollector<CharacterData> charactersUpdated)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var characterUpdateOptions = JsonConvert.DeserializeObject<CharacterUpdateOptions>(requestBody);

            var updateCharacter = charactersToUpdate.First();

            updateCharacter.Name = characterUpdateOptions.Name;
            updateCharacter.Url = string.Format("https://swapi.dev/api/people/{0}", updateCharacter.CharacterID);

            await charactersUpdated.AddAsync(updateCharacter);
            await charactersUpdated.FlushAsync();

            return new OkObjectResult(new Character(updateCharacter));
        }
    }
}
