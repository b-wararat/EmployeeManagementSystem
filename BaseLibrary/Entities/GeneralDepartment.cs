
namespace BaseLibrary.Entities
{
    public class GeneralDepartment : BaseEntity
    {
        //one to many relationship with department
        public List<Department>? Departments { get; set; }
    }
}
