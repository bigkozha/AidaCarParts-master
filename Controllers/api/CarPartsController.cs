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
    public class CarPartsController : ControllerBase
    {
        private readonly Context _context;

        public CarPartsController(Context context)
        {
            _context = context;
        }

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

        [HttpDelete]
        [Route("DeletePartById")]
        public async Task<IActionResult> DeletePartById([FromQuery] int id)
        {
            var partToDelete = _context.Parts.FirstOrDefault(p => p.Id == id);

            if (partToDelete == null)
            {
                return new NotFoundResult();
            }

            try
            {
                _context.Parts.Remove(partToDelete);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("UpdatePart")]
        public async Task<IActionResult> UpdatePart([FromBody] Part partToUpdate)
        {
            var part = _context.Parts.FirstOrDefault(p => p.Id == partToUpdate.Id);

            if (part == null)
            {
                return new NotFoundResult();
            }

            try
            {
                part.Availibility = partToUpdate.Availibility;
                part.Cost = partToUpdate.Cost;
                part.Note = partToUpdate.Note;
                part.OEM = partToUpdate.OEM;
                part.PartCode = partToUpdate.PartCode;
                part.Section = partToUpdate.Section;
                part.SectionAndSubsectionId = partToUpdate.SectionAndSubsectionId;
                part.UnitOfMeasure = partToUpdate.UnitOfMeasure;
                part.Volume = partToUpdate.Volume;
                part.Weight = partToUpdate.Weight;
                part.PartName = partToUpdate.PartName;

                _context.Update(part);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("CreatePart")]
        public async Task<IActionResult> CreatePart([FromBody] Part partToCreate)
        {
            try
            {
                await _context.Parts.AddAsync(partToCreate);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
