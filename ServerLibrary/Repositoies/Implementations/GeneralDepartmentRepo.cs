using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositoies.Contracts;

namespace ServerLibrary.Repositoies.Implementations
{
    public class GeneralDepartmentRepo : IGenericRepository<GeneralDepartment>
    {
        private readonly AppDbContext dbContext;
        public GeneralDepartmentRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;   
        }
        

        public async Task<List<GeneralDepartment>> GetAll()
        {
            return await dbContext.GeneralDepartments.AsNoTracking().ToListAsync();
        }

        public async Task<GeneralDepartment> GetById(int id)
        {
            return await dbContext.GeneralDepartments.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<GeneralResponse> Insert(GeneralDepartment item)
        {
            if (await DuplicateDepartment(item.Name)) return new GeneralResponse(false, "department already added");
            dbContext.GeneralDepartments.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(GeneralDepartment item)
        {
            var department = await dbContext.GeneralDepartments.FirstOrDefaultAsync(w => w.Id == item.Id);
            if(department is null) return NotFound();
            department.Name = item.Name;
            await Commit();
            return Success();
        }
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var department = await dbContext.GeneralDepartments.FindAsync(id);
            if (department is null) return NotFound();
            dbContext.GeneralDepartments.Remove(department);
            await Commit();
            return Success();
        }


        private static GeneralResponse NotFound() => new(false, "Sorry department not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> DuplicateDepartment(string name) {
            return await dbContext.GeneralDepartments.AnyAsync(w => w.Name.ToLower() == name.ToLower());
        }
    }
}
