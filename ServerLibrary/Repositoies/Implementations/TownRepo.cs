using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositoies.Contracts;

namespace ServerLibrary.Repositoies.Implementations
{
    public class TownRepo : IGenericRepository<Town>
    {
        private readonly AppDbContext dbContext;
        public TownRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<List<Town>> GetAll()
        {
            return await dbContext.Towns.AsNoTracking()
                .Include(i => i.City).ToListAsync();
        }

        public async Task<Town> GetById(int id)
        {
            return await dbContext.Towns.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<GeneralResponse> Insert(Town item)
        {
            if (await DuplicateCountry(item.Name)) return new GeneralResponse(false, "town already added");
            dbContext.Towns.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Town item)
        {
            var town = await dbContext.Towns.FirstOrDefaultAsync(w => w.Id == item.Id);
            if (town is null) return NotFound();
            town.Name = item.Name;
            town.CityId = item.CityId;
            await Commit();
            return Success();
        }
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var town = await dbContext.Towns.FindAsync(id);
            if (town is null) return NotFound();
            dbContext.Towns.Remove(town);
            await Commit();
            return Success();
        }


        private static GeneralResponse NotFound() => new(false, "Sorry town not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> DuplicateCountry(string name)
        {
            return await dbContext.Countries.AnyAsync(w => w.Name.ToLower() == name.ToLower());
        }
    }
}
