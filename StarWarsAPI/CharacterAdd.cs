using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using StarWarsAPI.Shared;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace StarWarsAPI
{
    public static class CharacterAdd
    {
        [OpenApiOperation(operationId: nameof(CharacterAdd), tags: new[] { "Character" }, Summary = "Adds a character.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Character), Description = "Returns a character.")]
        [FunctionName(nameof(CharacterAdd))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "characters")] HttpRequest req,
            [Sql(commandText: "Character",
                connectionStringSetting: "SqlConnectionString")] IAsyncCollector<CharacterData> charactersAdded)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var characterAddOptions = JsonConvert.DeserializeObject<CharacterAddOptions>(requestBody);

            var characterToAdd = new CharacterData()
            {
                CharacterID = characterAddOptions.Id,
                Name = characterAddOptions.Name,
                Url = string.Format("https://swapi.dev/api/people/{0}", characterAddOptions.Id)
            };

            await charactersAdded.AddAsync(characterToAdd);
            await charactersAdded.FlushAsync();

            return new CreatedResult(string.Format($"{req.Host}/api/characters/{characterToAdd.CharacterID}"), new Character(characterToAdd));
        }
    }
}
