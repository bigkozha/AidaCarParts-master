using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AidaCarParts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
        [HttpGet]
        [Route("GetPartsByPageNumber")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPartsByPageNumberAsync(
        [FromQuery] int pageSize = 15,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int? sectionIndex = null,
        [FromQuery] string searchWord = null)
        {
            var itemsOnPage = _context.Parts.AsQueryable();

            if (sectionIndex != null)
            {
                itemsOnPage = itemsOnPage
                    .Where(p => p.SectionAndSubsectionId == sectionIndex);
            }

            if (!string.IsNullOrEmpty(searchWord))
            {
                itemsOnPage = itemsOnPage
                    .Where(i => i.PartName.Contains(searchWord));
            }

            var totalItems = await itemsOnPage
                .LongCountAsync();

            var resultCollection = await itemsOnPage
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                totalItems = totalItems,
                itemsOnPage = resultCollection
            };

            return new JsonResult(result);
        }

    }
}
