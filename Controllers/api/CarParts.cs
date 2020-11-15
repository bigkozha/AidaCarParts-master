using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AidaCarParts.Models;
using Microsoft.EntityFrameworkCore;

namespace AidaCarParts.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarParts : ControllerBase
    {
        private readonly Context _context;

        public CarParts()
        {
            _context = new Context();
        }

        [HttpGet]
        [Route("GetPartsByPageNumber")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPartsByPageNumberAsync(
        [FromQuery] int pageSize = 16,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int sectionIndex = 2)
        {
            var totalItems = await _context.Parts
            .Where(p => p.SectionAndSubsectionId == sectionIndex)
            .LongCountAsync();

            var itemsOnPage = await _context.Parts
                .Where(p => p.SectionAndSubsectionId == sectionIndex)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                totalItems,
                itemsOnPage
            };

            return new JsonResult(result);
        }

    }
}
