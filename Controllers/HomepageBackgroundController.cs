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
    /// Represents a RESTful service of Homepage Background.
    /// </summary>
    [Authorize]
    [ApiVersion(V1_0)]
    [ODataRoutePrefix(nameof(HomepageBackground))]
    public class HomepageBackgroundController : ODataController
    {
        /// <summary>
        /// Homepage Background REST service.
        /// </summary>
        /// <param name="context">Database context.</param>
        public HomepageBackgroundController(PsefMySqlContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all Homepage Background.
        /// </summary>
        /// <remarks>
        /// *Anonymous Access*
        /// </remarks>
        /// <returns>All available Homepage Background.</returns>
        /// <response code="200">Homepage Background successfully retrieved.</response>
        [AllowAnonymous]
        [ODataRoute]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<HomepageBackground>>), Status200OK)]
        [EnableQuery]
        public IQueryable<HomepageBackground> Get()
        {
            return _context.HomepageBackground;
        }

        /// <summary>
        /// Gets a single Homepage Background.
        /// </summary>
        /// <remarks>
        /// *Min role: None*
        /// </remarks>
        /// <param name="id">The requested Homepage Background identifier.</param>
        /// <returns>The requested Homepage Background.</returns>
        /// <response code="200">The Homepage Background was successfully retrieved.</response>
        /// <response code="404">The Homepage Background does not exist.</response>
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(HomepageBackground), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select)]
        public SingleResult<HomepageBackground> Get([FromODataUri] ushort id)
        {
            return SingleResult.Create(
                _context.HomepageBackground.Where(e => e.Id == id));
        }

        /// <summary>
        /// Creates a new Homepage Background.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="create">The Homepage Background to create.</param>
        /// <returns>The created Homepage Background.</returns>
        /// <response code="201">The Homepage Background was successfully created.</response>
        /// <response code="204">The Homepage Background was successfully created.</response>
        /// <response code="400">The Homepage Background is invalid.</response>
        /// <response code="409">The Homepage Background with supplied id already exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(HomepageBackground), Status201Created)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] HomepageBackground create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.HomepageBackground.Add(create);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Exists(create.Id))
                {
                    return Conflict();
                }

                throw;
            }

            return Created(create);
        }

        /// <summary>
        /// Updates an existing Homepage Background.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The requested Homepage Background identifier.</param>
        /// <param name="delta">The partial Homepage Background to update.</param>
        /// <returns>The updated Homepage Background.</returns>
        /// <response code="200">The Homepage Background was successfully updated.</response>
        /// <response code="204">The Homepage Background was successfully updated.</response>
        /// <response code="400">The Homepage Background is invalid.</response>
        /// <response code="404">The Homepage Background does not exist.</response>
        /// <response code="422">The Homepage Background identifier is specified on delta and its value is different from id.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(HomepageBackground), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status422UnprocessableEntity)]
        [HttpPatch]
        public async Task<IActionResult> Patch(
            [FromODataUri] ushort id,
            [FromBody] Delta<HomepageBackground> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _context.HomepageBackground.FindAsync(id);

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
        /// Deletes a Homepage Background.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The Homepage Background to delete.</param>
        /// <returns>None</returns>
        /// <response code="204">The Homepage Background was successfully deleted.</response>
        /// <response code="404">The Homepage Background does not exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Delete([FromODataUri] ushort id)
        {
            var delete = await _context.HomepageBackground.FindAsync(id);

            if (delete == null)
            {
                return NotFound();
            }

            _context.HomepageBackground.Remove(delete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool Exists(ushort id)
        {
            return _context.HomepageBackground.Any(e => e.Id == id);
        }

        private readonly PsefMySqlContext _context;
    }
}