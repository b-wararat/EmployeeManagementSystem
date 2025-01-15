namespace BaseLibrary.Entities
{
    public class Country : BaseEntity
    {
        //one to many relationship with city
        public List<City>? Cities { get; set; }
    }
}
