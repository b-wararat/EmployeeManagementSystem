using BaseLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers;
using ServerLibrary.Repositoies.Contracts;

namespace Server.Properties
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController(IGenericRepository<City> genericRepository)
        : GenericController<City>(genericRepository)
    {
    }
}
