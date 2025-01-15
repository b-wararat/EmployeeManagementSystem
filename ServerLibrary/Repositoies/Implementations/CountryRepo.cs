using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositoies.Contracts;

namespace ServerLibrary.Repositoies.Implementations
{
    public class CountryRepo : IGenericRepository<Country>
    {
        private readonly AppDbContext dbContext;
        public CountryRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<List<Country>> GetAll()
        {
            return await dbContext.Countries.AsNoTracking().ToListAsync();
        }

        public async Task<Country> GetById(int id)
        {
            return await dbContext.Countries.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<GeneralResponse> Insert(Country item)
        {
            if (await DuplicateCountry(item.Name)) return new GeneralResponse(false, "country already added");
            dbContext.Countries.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Country item)
        {
            var country = await dbContext.Countries.FirstOrDefaultAsync(w => w.Id == item.Id);
            if (country is null) return NotFound();
            country.Name = item.Name;
            await Commit();
            return Success();
        }
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var country = await dbContext.Countries.FindAsync(id);
            if (country is null) return NotFound();
            dbContext.Countries.Remove(country);
            await Commit();
            return Success();
        }


        private static GeneralResponse NotFound() => new(false, "Sorry Country not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> DuplicateCountry(string name)
        {
            return await dbContext.Countries.AnyAsync(w => w.Name.ToLower() == name.ToLower());
        }
    }
}
