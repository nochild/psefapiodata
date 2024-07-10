using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsefApiOData.Misc;
using PsefApiOData.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static PsefApiOData.ApiInfo;

namespace PsefApiOData.Controllers
{
    /// <summary>
    /// Represents a RESTful service of Tentang.
    /// </summary>
    [Authorize]
    [ApiVersion(V1_0)]
    [ODataRoutePrefix(nameof(Tentang))]
    public class TentangController : ODataController
    {
        /// <summary>
        /// Tentang REST service.
        /// </summary>
        /// <param name="context">Database context.</param>
        public TentangController(PsefMySqlContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all Tentang.
        /// </summary>
        /// <remarks>
        /// *Min role: None*
        /// </remarks>
        /// <returns>All available Tentang.</returns>
        /// <response code="200">Tentang successfully retrieved.</response>
        [AllowAnonymous]
        [ODataRoute]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<Tentang>>), Status200OK)]
        [EnableQuery]
        public IQueryable<Tentang> Get()
        {
            return _context.Tentang;
        }

        /// <summary>
        /// Gets a single Tentang.
        /// </summary>
        /// <remarks>
        /// *Min role: None*
        /// </remarks>
        /// <param name="id">The requested Tentang identifier.</param>
        /// <returns>The requested Tentang.</returns>
        /// <response code="200">The Tentang was successfully retrieved.</response>
        /// <response code="404">The Tentang does not exist.</response>
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(Tentang), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select)]
        public SingleResult<Tentang> Get([FromODataUri] ushort id)
        {
            return SingleResult.Create(
                _context.Tentang.Where(e => e.Id == id));
        }

        /// <summary>
        /// Updates an existing Tentang.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The requested Tentang identifier.</param>
        /// <param name="delta">The partial Tentang to update.</param>
        /// <returns>The updated Tentang.</returns>
        /// <response code="200">The Tentang was successfully updated.</response>
        /// <response code="204">The Tentang was successfully updated.</response>
        /// <response code="400">The Tentang is invalid.</response>
        /// <response code="404">The Tentang does not exist.</response>
        /// <response code="422">The Tentang identifier is specified on delta and its value is different from id.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(Tentang), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status422UnprocessableEntity)]
        public async Task<IActionResult> Patch(
            [FromODataUri] ushort id,
            [FromBody] Delta<Tentang> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _context.Tentang.FindAsync(id);

            if (update == null)
            {
                return NotFound();
            }

            delta.Patch(update);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                if (update.Id != id)
                {
                    ModelState.AddModelError(nameof(update.Id), DontSetKeyOnPatch);
                    return UnprocessableEntity(ModelState);
                }

                throw;
            }

            return Updated(update);
        }

        /// <summary>
        /// Deletes a Tentang.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The Tentang to delete.</param>
        /// <returns>None</returns>
        /// <response code="204">The Tentang was successfully deleted.</response>
        /// <response code="404">The Tentang does not exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Delete([FromODataUri] ushort id)
        {
            var delete = await _context.Tentang.FindAsync(id);

            if (delete == null)
            {
                return NotFound();
            }

            _context.Tentang.Remove(delete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Updates an existing Tentang.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The requested Tentang identifier.</param>
        /// <param name="update">The Tentang to update.</param>
        /// <returns>The updated Tentang.</returns>
        /// <response code="200">The Tentang was successfully updated.</response>
        /// <response code="204">The Tentang was successfully updated.</response>
        /// <response code="400">The Tentang is invalid.</response>
        /// <response code="404">The Tentang does not exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(Tentang), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromODataUri] ushort id,
            [FromBody] Tentang update)
        {
            if (id != update.Id)
            {
                return BadRequest();
            }

            _context.Entry(update).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return Updated(update);
        }

        private bool Exists(ushort id)
        {
            return _context.Tentang.Any(e => e.Id == id);
        }

        private readonly PsefMySqlContext _context;
    }
}
