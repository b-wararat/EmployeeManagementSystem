using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositoies.Contracts;

namespace ServerLibrary.Repositoies.Implementations
{
    public class BranchRepo : IGenericRepository<Branch>
    {
        private readonly AppDbContext dbContext;
        public BranchRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<List<Branch>> GetAll()
        {
            return await dbContext.Branches.AsNoTracking()
                .Include(i => i.Department).ToListAsync();
        }

        public async Task<Branch> GetById(int id)
        {
            return await dbContext.Branches.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<GeneralResponse> Insert(Branch item)
        {
            if (await DuplicateBranch(item.Name)) return new GeneralResponse(false, "Branch already added");
            dbContext.Branches.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Branch item)
        {
            var branch = await dbContext.Branches.FirstOrDefaultAsync(w => w.Id == item.Id);
            if (branch is null) return NotFound();
            branch.Name = item.Name;
            branch.DepartmentId = item.DepartmentId;
            await Commit();
            return Success();
        }
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var branch = await dbContext.Branches.FindAsync(id);
            if (branch is null) return NotFound();
            dbContext.Branches.Remove(branch);
            await Commit();
            return Success();
        }


        private static GeneralResponse NotFound() => new(false, "Sorry branch not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> DuplicateBranch(string name)
        {
            return await dbContext.Branches.AnyAsync(w => w.Name.ToLower() == name.ToLower());
        }
    }
}
