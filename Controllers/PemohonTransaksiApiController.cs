using System;
using System.Net.Mail;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PsefApiOData.Misc;
using PsefApiOData.Models;
using PsefApiOData.Models.ViewModels;
using WorkDaysCalculator;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static PsefApiOData.ApiInfo;
using Microsoft.OData.Edm;

namespace PsefApiOData.Controllers
{
    /// <summary>
    /// Represents a RESTful service of PemohonTransaksiApi.
    /// </summary>
    [Authorize]
    [ApiVersion(V0_1)]
    [ODataRoutePrefix(nameof(PemohonTransaksiApi))]
    public class PemohonTransaksiApiController : ODataController
    {
        /// <summary>
        /// Pemohon REST service.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="mapper">AutoMapper mapping profile.</param>
        public PemohonTransaksiApiController(
            IMapper mapper,
            PsefMySqlContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Retrieves all Api Permohonan.
        /// </summary>
        /// <remarks>
        /// *Min role: Verifikator*
        /// </remarks>
        /// <returns>All available Permohonan.</returns>
        /// <response code="200">Permohonan successfully retrieved.</response>
        [MultiRoleAuthorize(
            ApiRole.Bpom,
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
        [ProducesResponseType(typeof(ODataValue<IEnumerable<PemohonTransaksiApiView>>), Status200OK)]
        [EnableQuery]
        public IQueryable<PemohonTransaksiApiView> Get()
        {
            List<PemohonTransaksiApi> pemohonApis = _context.PemohonTransaksiApi.ToList();

            List<PemohonTransaksiApiView> result = new List<PemohonTransaksiApiView>();

            foreach(PemohonTransaksiApi data in pemohonApis)
            {
                PemohonTransaksiApiView dt = new PemohonTransaksiApiView();

                dt.Id = data.Id;
                dt.PemohonId = data.PemohonId;
                dt.Url = data.Url;
                dt.Token = data.Token;
                dt.CompanyName = _context.Pemohon.FirstOrDefault(e => e.Id == data.PemohonId).CompanyName;

                result.Add(dt);
            }

            return result.AsQueryable();
        }

        /// <summary>
        /// Gets a single Pemohon.
        /// </summary>
        /// <remarks>
        /// *Min role: None*
        /// </remarks>
        /// <param name="id">The requested Pemohon identifier.</param>
        /// <returns>The requested Pemohon.</returns>
        /// <response code="200">The Pemohon was successfully retrieved.</response>
        /// <response code="404">The Pemohon does not exist.</response>
        [ODataRoute(IdRoute)]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(PemohonTransaksiApi), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select)]
        public async Task<SingleResult<PemohonTransaksiApi>> Get([FromODataUri] uint id)
        {
            List<PemohonTransaksiApi> result = new List<PemohonTransaksiApi>();
            PemohonTransaksiApi pemohon = await _context.PemohonTransaksiApi.FirstOrDefaultAsync(c => c.Id == id);

            if (pemohon != null)
            {
                result.Add(pemohon);
            }

            return SingleResult.Create(result.AsQueryable());
        }

        /// <summary>
        /// Retrieves Data API By User.
        /// </summary>
        /// <remarks>
        /// *Role: None*
        /// </remarks>
        /// <returns>Data API By User.</returns>
        /// <response code="200">Data API By User.</response>
        [HttpGet]
        [Produces(JsonOutput)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(typeof(PemohonTransaksiApi), Status200OK)]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select)]
        public async Task<SingleResult<PemohonTransaksiApi>> UserApi()
        {

            Pemohon pemohon = await _context.Pemohon
                .FirstOrDefaultAsync(c =>
                    c.UserId == ApiHelper.GetUserId(HttpContext.User));

            PemohonTransaksiApi data = await _context.PemohonTransaksiApi.FirstOrDefaultAsync(c => c.PemohonId == pemohon.Id);

            List<PemohonTransaksiApi> result = new List<PemohonTransaksiApi>();

            if(data != null)
            {
                result.Add(data);
            }

            return SingleResult.Create(result.AsQueryable());
        }

        /// <summary>
        /// Creates a new API Pemohon.
        /// </summary>
        /// <remarks>
        /// *Min role: None*
        /// </remarks>
        /// <param name="create">The API Pemohon to create.</param>
        /// <returns>The created Permohonan.</returns>
        /// <response code="201">The API Pemohon was successfully created.</response>
        /// <response code="204">The API Pemohon was successfully created.</response>
        /// <response code="400">The API Pemohon is invalid.</response>
        /// <response code="409">The API Pemohon with supplied id already exist.</response>
        [ODataRoute]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(PemohonTransaksiApi), Status201Created)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] PemohonTransaksiApi create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            Pemohon pemohon = await _context.Pemohon
                .FirstOrDefaultAsync(c =>
                    c.UserId == ApiHelper.GetUserId(HttpContext.User));

            if (pemohon == null)
            {
                return BadRequest("Pemohon not found");
            }

            create.PemohonId = pemohon.Id;

            PemohonTransaksiApi update = await _context.PemohonTransaksiApi
                .FirstOrDefaultAsync(c =>
                    c.PemohonId == pemohon.Id);

            if(update == null)
            {
                _context.PemohonTransaksiApi.Add(create);
            } else
            {
                update.Url = create.Url;
                update.Token = create.Token;
            }

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

            if (update == null)
            {
                return Created(create);
            } else
            {
                return Updated(create);
            }
        }

        /// <summary>
        /// Updates an existing API Pemohon for the current user.
        /// </summary>
        /// <remarks>
        /// *Min role: None*
        /// </remarks>
        /// <param name="data">The partial Permohonan to update.</param>
        /// <returns>The updated Permohonan.</returns>
        /// <response code="200">The API Pemohon was successfully updated.</response>
        /// <response code="204">The API Pemohon was successfully updated.</response>
        /// <response code="400">The API Pemohon is invalid.</response>
        /// <response code="404">The API Pemohon does not exist.</response>
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(PemohonTransaksiApi), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status422UnprocessableEntity)]
        public async Task<IActionResult> Patch(
            [FromBody] PemohonTransaksiApi data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string currentUserId = ApiHelper.GetUserId(HttpContext.User);

            Pemohon pemohon = await _context.Pemohon
                .FirstOrDefaultAsync(c =>
                    c.UserId == ApiHelper.GetUserId(HttpContext.User));

            PemohonTransaksiApi update = await _context.PemohonTransaksiApi
                .FirstOrDefaultAsync(c =>
                    c.PemohonId == pemohon.Id);

            if (update == null)
            {
                return NotFound();
            }

            update.Url = data.Url;
            update.Token = data.Token;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {

                throw;
            }

            return Updated(update);
        }

        private bool Exists(uint? id)
        {
            return _context.PemohonTransaksiApi.Any(e => e.Id == id);
        }

        private readonly PsefMySqlContext _context;
        private readonly IMapper _mapper;
    }
}