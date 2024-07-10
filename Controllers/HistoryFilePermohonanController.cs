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
    /// Represents a RESTful service of History File Permohonan.
    /// </summary>
    [Authorize]
    [ApiVersion(V0_1)]
    [ODataRoutePrefix(nameof(HistoryFilePermohonan))]
    public class HistoryFilePermohonanController : ODataController
    {
        /// <summary>
        /// History File Permohonan REST service.
        /// </summary>
        /// <param name="context">Database context.</param>
        public HistoryFilePermohonanController(PsefMySqlContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves History File Permohonan total count.
        /// </summary>
        /// <remarks>
        /// *Min role: Verifikator*
        /// </remarks>
        /// <returns>History File Permohonan total count.</returns>
        /// <response code="200">Total count of History File Permohonan retrieved.</response>
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
            return await _context.HistoryFilePermohonan.LongCountAsync();
        }

        /// <summary>
        /// Retrieves all History File Permohonan.
        /// </summary>
        /// <remarks>
        /// *Min role: Verifikator*
        /// </remarks>
        /// <returns>All available History File Permohonan.</returns>
        /// <response code="200">History File Permohonan successfully retrieved.</response>
        [MultiRoleAuthorize(
            ApiRole.Verifikator,
            ApiRole.Validator,
            ApiRole.Supervisor,
            ApiRole.Timja,
            ApiRole.Dirpenyanfar,
            ApiRole.Dirjen,
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<HistoryFilePermohonan>>), Status200OK)]
        [EnableQuery]
        public IQueryable<HistoryFilePermohonan> Get()
        {
            return _context.HistoryFilePermohonan;
        }

        /// <summary>
        /// Gets a single History File Permohonan.
        /// </summary>
        /// <remarks>
        /// *Min role: Verifikator*
        /// </remarks>
        /// <param name="id">The requested History File Permohonan identifier.</param>
        /// <returns>The requested History File Permohonan.</returns>
        /// <response code="200">The History File Permohonan was successfully retrieved.</response>
        /// <response code="404">The History File Permohonan does not exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Verifikator,
            ApiRole.Validator,
            ApiRole.Supervisor,
            ApiRole.Timja,
            ApiRole.Dirpenyanfar,
            ApiRole.Dirjen,
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(HistoryFilePermohonan), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select)]
        public SingleResult<HistoryFilePermohonan> Get([FromODataUri] ulong id)
        {
            return SingleResult.Create(
                _context.HistoryFilePermohonan.Where(e => e.Id == id));
        }

        /// <summary>
        /// Creates a new History File Permohonan.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="create">The History File Permohonan to create.</param>
        /// <returns>The created History File Permohonan.</returns>
        /// <response code="201">The History File Permohonan was successfully created.</response>
        /// <response code="204">The History File Permohonan was successfully created.</response>
        /// <response code="400">The History File Permohonan is invalid.</response>
        /// <response code="409">The History File Permohonan with supplied id already exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(HistoryFilePermohonan), Status201Created)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] HistoryFilePermohonan create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.HistoryFilePermohonan.Add(create);

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
        /// Updates an existing History File Permohonan.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The requested History File Permohonan identifier.</param>
        /// <param name="delta">The partial History File Permohonan to update.</param>
        /// <returns>The updated History File Permohonan.</returns>
        /// <response code="200">The History File Permohonan was successfully updated.</response>
        /// <response code="204">The History File Permohonan was successfully updated.</response>
        /// <response code="400">The History File Permohonan is invalid.</response>
        /// <response code="404">The History File Permohonan does not exist.</response>
        /// <response code="422">The History File Permohonan identifier is specified on delta and its value is different from id.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(HistoryFilePermohonan), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status422UnprocessableEntity)]
        public async Task<IActionResult> Patch(
            [FromODataUri] ulong id,
            [FromBody] Delta<HistoryFilePermohonan> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _context.HistoryFilePermohonan.FindAsync(id);

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
        /// Deletes a History File Permohonan.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The History File Permohonan to delete.</param>
        /// <returns>None</returns>
        /// <response code="204">The History File Permohonan was successfully deleted.</response>
        /// <response code="404">The History File Permohonan does not exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Delete([FromODataUri] ulong id)
        {
            var delete = await _context.HistoryFilePermohonan.FindAsync(id);

            if (delete == null)
            {
                return NotFound();
            }

            _context.HistoryFilePermohonan.Remove(delete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Updates an existing History File Permohonan.
        /// </summary>
        /// <remarks>
        /// *Min role: Admin*
        /// </remarks>
        /// <param name="id">The requested History File Permohonan identifier.</param>
        /// <param name="update">The History File Permohonan to update.</param>
        /// <returns>The updated History File Permohonan.</returns>
        /// <response code="200">The History File Permohonan was successfully updated.</response>
        /// <response code="204">The History File Permohonan was successfully updated.</response>
        /// <response code="400">The History File Permohonan is invalid.</response>
        /// <response code="404">The History File Permohonan does not exist.</response>
        [MultiRoleAuthorize(
            ApiRole.Admin,
            ApiRole.SuperAdmin)]
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(HistoryFilePermohonan), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromODataUri] ulong id,
            [FromBody] HistoryFilePermohonan update)
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
        /// Retrieves all History File Permohonan for the specified Permohonan.
        /// </summary>
        /// <remarks>
        /// *Min role: Verifikator*
        /// </remarks>
        /// <param name="permohonanId">The requested Permohonan identifier.</param>
        /// <returns>All available History File Permohonan for the specified Permohonan.</returns>
        /// <response code="200">List of History File Permohonan successfully retrieved.</response>
        /// <response code="404">The list of History File Permohonan does not exist.</response>
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
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<HistoryFilePermohonan>>), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery]
        public IQueryable<HistoryFilePermohonan> ByPermohonan(uint permohonanId)
        {
            return _context.HistoryFilePermohonan.Where(e => e.PermohonanId == permohonanId);
        }

        private bool Exists(ulong id)
        {
            return _context.HistoryFilePermohonan.Any(e => e.Id == id);
        }

        private readonly PsefMySqlContext _context;
    }
}
