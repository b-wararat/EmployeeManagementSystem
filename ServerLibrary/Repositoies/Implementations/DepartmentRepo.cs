using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositoies.Contracts;


namespace ServerLibrary.Repositoies.Implementations
{
    public class DepartmentRepo : IGenericRepository<Department>
    {
        private readonly AppDbContext dbContext;
        public DepartmentRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<List<Department>> GetAll()
        {
            return await dbContext.Departments.AsNoTracking().ToListAsync();
        }

        public async Task<Department> GetById(int id)
        {
            return await dbContext.Departments.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<GeneralResponse> Insert(Department item)
        {
            if (await DuplicateDepartment(item.Name)) return new GeneralResponse(false, "department already added");
            dbContext.Departments.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Department item)
        {
            var department = await dbContext.Departments.FirstOrDefaultAsync(w => w.Id == item.Id);
            if (department is null) return NotFound();
            department.Name = item.Name;
            await Commit();
            return Success();
        }
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var department = await dbContext.Departments.FindAsync(id);
            if (department is null) return NotFound();
            dbContext.Departments.Remove(department);
            await Commit();
            return Success();
        }


        private static GeneralResponse NotFound() => new(false, "Sorry department not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> DuplicateDepartment(string name)
        {
            return await dbContext.Departments.AnyAsync(w => w.Name.ToLower() == name.ToLower());
        }
    }
}
