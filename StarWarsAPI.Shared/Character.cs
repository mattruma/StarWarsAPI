namespace StarWarsAPI.Shared
{
    public class Character
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public Character()
        {
        }

        public Character(
            CharacterData characterData)
        {
            this.Id = characterData.CharacterID;
            this.Name = characterData.Name;
            this.Url = characterData.Url;
        }
    }
}
