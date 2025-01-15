namespace BaseLibrary.Entities
{
    public class City : BaseEntity
    {
        //Many to one relationship with country
        public Country? Country { get; set; }
        public int CountryId { get; set; }

        //one to many relationship with town
        public List<Town>? Towns { get; set; }
    }
}
