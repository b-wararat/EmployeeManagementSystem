using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class Country : BaseEntity
    {
        //one to many relationship with city
        [JsonIgnore]
        public List<City>? Cities { get; set; }
    }
}
