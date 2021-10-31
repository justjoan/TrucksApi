using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrucksApi.Models;
using static TrucksApi.Models.Enums;

namespace TrucksApi.Controllers
{
    [Route("api/trucks")]
    [ApiController]
    public class TrucksController : ControllerBase
    {
        private readonly TrucksContext _context;

        public TrucksController(TrucksContext context)
        {
            _context = context;
        }

        // GET: api/Trucks?zone=str&zoneType=block
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Truck>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Truck>>> GetTrucks([FromQuery] string zone, [FromQuery] string zoneType = "block" )
        {
            //Return BadRequest if ZoneType or Zone are missing
            if (string.IsNullOrWhiteSpace(zoneType) || string.IsNullOrWhiteSpace(zone))
            {
                return BadRequest();
            }

            //Return BadRequest if unrecognied ZoneType
            ZoneTypes type;
            if (!Enum.TryParse<ZoneTypes>(zoneType, true, out type))
            {
                return BadRequest();
            }

            //Return InternalServerError if ZoneType not mapped
            switch (type)
            {
                case ZoneTypes.Block:
                    return await _context.Trucks.Where(t => t.Block.Equals(zone, StringComparison.OrdinalIgnoreCase)).ToListAsync();
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
    }
}
