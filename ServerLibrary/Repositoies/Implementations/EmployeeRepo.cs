using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositoies.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repositoies.Implementations
{
    public class EmployeeRepo : IGenericRepository<Employee>
    {
        private readonly AppDbContext dbContext;
        public EmployeeRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<GeneralResponse> DeleteById(int id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee is null) return NotFound();
            dbContext.Employees.Remove(employee);
            await Commit();
            return Success();
        }

        public async Task<List<Employee>> GetAll()
        {
            var employees = await dbContext.Employees.AsNoTracking()
                .Include(i => i.Town)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Country)
                .Include(i => i.Branch)
                .ThenInclude(i => i.Department)
                .ThenInclude(i => i.GeneralDepartment)
                .ToListAsync();
            return employees;
        }

        public async Task<Employee> GetById(int id)
        {
            return await dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<GeneralResponse> Insert(Employee entity)
        {
            if (await DuplicateDepartment(entity.Name)) return new GeneralResponse(false, "employee already added");
            dbContext.Employees.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Employee entity)
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(w => w.Id == entity.Id);
            if (employee is null) return NotFound();
            employee = entity;
            await Commit();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry employee not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> DuplicateDepartment(string name)
        {
            return await dbContext.Employees.AnyAsync(w => w.Name.ToLower() == name.ToLower());
        }

       
    }
}
