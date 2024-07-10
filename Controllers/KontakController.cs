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
    /// Represents a RESTful service of Kontak.
    /// </summary>
    [Authorize]
    [ApiVersion(V1_0)]
    [ODataRoutePrefix(nameof(Kontak))]
    public class KontakController : ODataController
    {
        /// <summary>
        /// Kontak REST service.
        /// </summary>
        /// <param name="context">Database context.</param>
        public KontakController(PsefMySqlContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all Kontak.
        /// </summary>
        /// <remarks>
        /// *Min role: None*
        /// </remarks>
        /// <returns>All available Kontak.</returns>
        /// <response code="200">Kontak successfully retrieved.</response>
        [AllowAnonymous]
        [ODataRoute]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<Kontak>>), Status200OK)]
        [EnableQuery]
        public IQueryable<Kontak> Get()
        {
            return _context.Kontak;
        }

        /// <summary>
        /// Gets a single Kontak.
        /// </summary>
        /// <remarks>
        /// *Min role: None*
        /// </remarks>
        /// <param name="id">The requested Kontak identifier.</param>
        /// <returns>The requested Kontak.</returns>
        /// <response code="200">The Kontak was successfully retrieved.</response>
        /// <response code="404">The Kontak does not exist.</response>
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(Kontak), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select)]
        public SingleResult<Kontak> Get([FromODataUri] uint id)
        {
            return SingleResult.Create(
                _context.Kontak.Where(e => e.Id == id));
        }

        /// <summary>
        /// Updates an existing Kontak.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The requested Kontak identifier.</param>
        /// <param name="delta">The partial Kontak to update.</param>
        /// <returns>The updated Kontak.</returns>
        /// <response code="200">The Kontak was successfully updated.</response>
        /// <response code="204">The Kontak was successfully updated.</response>
        /// <response code="400">The Kontak is invalid.</response>
        /// <response code="404">The Kontak does not exist.</response>
        /// <response code="422">The Kontak identifier is specified on delta and its value is different from id.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(Kontak), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status422UnprocessableEntity)]
        public async Task<IActionResult> Patch(
            [FromODataUri] uint id,
            [FromBody] Delta<Kontak> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _context.Kontak.FindAsync(id);

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
        /// Deletes a Kontak.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The Kontak to delete.</param>
        /// <returns>None</returns>
        /// <response code="204">The Kontak was successfully deleted.</response>
        /// <response code="404">The Kontak does not exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Delete([FromODataUri] uint id)
        {
            var delete = await _context.Kontak.FindAsync(id);

            if (delete == null)
            {
                return NotFound();
            }

            _context.Kontak.Remove(delete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Updates an existing Kontak.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The requested Kontak identifier.</param>
        /// <param name="update">The Kontak to update.</param>
        /// <returns>The updated Kontak.</returns>
        /// <response code="200">The Kontak was successfully updated.</response>
        /// <response code="204">The Kontak was successfully updated.</response>
        /// <response code="400">The Kontak is invalid.</response>
        /// <response code="404">The Kontak does not exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(Kontak), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromODataUri] uint id,
            [FromBody] Kontak update)
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

        private bool Exists(uint id)
        {
            return _context.Kontak.Any(e => e.Id == id);
        }

        private readonly PsefMySqlContext _context;
    }
}
