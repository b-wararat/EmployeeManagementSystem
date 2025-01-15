
namespace BaseLibrary.Entities
{
    public class Town : BaseEntity
    {
        //relationship one to many with employee
        public List<Employee>? Employees { get; set; }

        //many to onw relationship with city
        public City? City { get; set; }
        public int CityId { get; set; }
    }
}
