using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositoies.Contracts;

namespace ServerLibrary.Repositoies.Implementations
{
    public class CityRepo : IGenericRepository<City>
    {
        private readonly AppDbContext dbContext;
        public CityRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<List<City>> GetAll()
        {
            return await dbContext.Cities.AsNoTracking()
                .Include(i => i.Country).ToListAsync();
        }

        public async Task<City> GetById(int id)
        {
            return await dbContext.Cities.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<GeneralResponse> Insert(City item)
        {
            if (await DuplicateCity(item.Name)) return new GeneralResponse(false, "city already added");
            dbContext.Cities.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(City item)
        {
            var city = await dbContext.Cities.FirstOrDefaultAsync(w => w.Id == item.Id);
            if (city is null) return NotFound();
            city.Name = item.Name;
            city.CountryId = item.CountryId;
            await Commit();
            return Success();
        }
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var city = await dbContext.Cities.FindAsync(id);
            if (city is null) return NotFound();
            dbContext.Cities.Remove(city);
            await Commit();
            return Success();
        }


        private static GeneralResponse NotFound() => new(false, "Sorry city not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> DuplicateCity(string name)
        {
            return await dbContext.Countries.AnyAsync(w => w.Name.ToLower() == name.ToLower());
        }
    }
}
