
namespace BaseLibrary.Entities
{
    public class Branch : BaseEntity
    {
        //many to one relationship with department
        public Department? Department { get; set; }
        public int DepartmentId { get; set; }

        //one to many relationship with employee
        public List<Employee>? Employees { get; set; }
    }
}
