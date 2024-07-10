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
    /// Represents a RESTful service of Master Checklist.
    /// </summary>
    [Authorize]
    [ApiVersion(V1_0)]
    [ODataRoutePrefix(nameof(MasterChecklist))]
    public class MasterChecklistController : ODataController
    {
        /// <summary>
        /// Master Checklist REST service.
        /// </summary>
        /// <param name="context">Database context.</param>
        public MasterChecklistController(PsefMySqlContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves Master Checklist total count.
        /// </summary>
        /// <remarks>
        /// *Min role: Verifikator*
        /// </remarks>
        /// <returns>Master Checklist total count.</returns>
        /// <response code="200">Total count of Master Checklist retrieved.</response>
        [MultiRoleAuthorize(
            ApiRole.Verifikator,
            ApiRole.Validator,
            ApiRole.Supervisor,
            ApiRole.Timja,
            ApiRole.Dirpenyanfar,
            ApiRole.Dirjen,
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [HttpGet]
        [ODataRoute(nameof(TotalCount))]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(long), Status200OK)]
        public async Task<long> TotalCount()
        {
            return await _context.MasterChecklist.LongCountAsync();
        }

        /// <summary>
        /// Retrieves all Master Checklist.
        /// </summary>
        /// <remarks>
        /// *Min role: Verifikator*
        /// </remarks>
        /// <returns>All available Master Checklist.</returns>
        /// <response code="200">Master Checklist successfully retrieved.</response>
        
        [AllowAnonymous]
        [ODataRoute]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<MasterChecklist>>), Status200OK)]
        [EnableQuery]
        public IQueryable<MasterChecklist> Get()
        {
            return _context.MasterChecklist;
        }

        /// <summary>
        /// Gets a single Master Checklist.
        /// </summary>
        /// <remarks>
        /// *Min role: Verifikator*
        /// </remarks>
        /// <param name="id">The requested Master Checklist identifier.</param>
        /// <returns>The requested Master Checklist.</returns>
        /// <response code="200">The Master Checklist was successfully retrieved.</response>
        /// <response code="404">The Master Checklist does not exist.</response>
        
        [AllowAnonymous]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(MasterChecklist), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select)]
        public SingleResult<MasterChecklist> Get([FromODataUri] ushort id)
        {
            return SingleResult.Create(
                _context.MasterChecklist.Where(e => e.Id == id));
        }

        /// <summary>
        /// Creates a new Master Checklist.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="create">The Master Checklist to create.</param>
        /// <returns>The created Master Checklist.</returns>
        /// <response code="201">The Master Checklist was successfully created.</response>
        /// <response code="204">The Master Checklist was successfully created.</response>
        /// <response code="400">The Master Checklist is invalid.</response>
        /// <response code="409">The Master Checklist with supplied id already exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(MasterChecklist), Status201Created)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] MasterChecklist create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MasterChecklist.Add(create);

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
        /// Updates an existing Master Checklist.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The requested Master Checklist identifier.</param>
        /// <param name="delta">The partial Master Checklist to update.</param>
        /// <returns>The updated Master Checklist.</returns>
        /// <response code="200">The Master Checklist was successfully updated.</response>
        /// <response code="204">The Master Checklist was successfully updated.</response>
        /// <response code="400">The Master Checklist is invalid.</response>
        /// <response code="404">The Master Checklist does not exist.</response>
        /// <response code="422">The Master Checklist identifier is specified on delta and its value is different from id.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(MasterChecklist), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status422UnprocessableEntity)]
        public async Task<IActionResult> Patch(
            [FromODataUri] ushort id,
            [FromBody] Delta<MasterChecklist> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _context.MasterChecklist.FindAsync(id);

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
        /// Deletes a Master Checklist.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The Master Checklist to delete.</param>
        /// <returns>None</returns>
        /// <response code="204">The Master Checklist was successfully deleted.</response>
        /// <response code="404">The Master Checklist does not exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Delete([FromODataUri] ushort id)
        {
            var delete = await _context.MasterChecklist.FindAsync(id);

            if (delete == null)
            {
                return NotFound();
            }

            _context.MasterChecklist.Remove(delete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Updates an existing Master Checklist.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The requested Master Checklist identifier.</param>
        /// <param name="update">The Master Checklist to update.</param>
        /// <returns>The updated Master Checklist.</returns>
        /// <response code="200">The Master Checklist was successfully updated.</response>
        /// <response code="204">The Master Checklist was successfully updated.</response>
        /// <response code="400">The Master Checklist is invalid.</response>
        /// <response code="404">The Master Checklist does not exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(MasterChecklist), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromODataUri] ushort id,
            [FromBody] MasterChecklist update)
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

        /// <summary>
        /// Retrieves all Master Checklist for the specified Permohonan.
        /// </summary>
        /// <remarks>
        /// *Min role: Verifikator*
        /// </remarks>
        /// <param name="parentId">The requested Permohonan identifier.</param>
        /// <returns>All available Master Checklist for the specified Permohonan.</returns>
        /// <response code="200">List of Master Checklist successfully retrieved.</response>
        /// <response code="404">The list of Master Checklist does not exist.</response>
        
        [AllowAnonymous]
        [HttpGet]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<MasterChecklist>>), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery]
        public IQueryable<MasterChecklist> ByParent(ushort parentId)
        {
            return _context.MasterChecklist.Where(e => e.Parent == parentId).OrderBy(e => e.Id);
        }

        private bool Exists(ushort id)
        {
            return _context.MasterChecklist.Any(e => e.Id == id);
        }

        private readonly PsefMySqlContext _context;
    }
}
