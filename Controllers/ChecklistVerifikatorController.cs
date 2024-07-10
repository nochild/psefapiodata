using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Drawing;
using System.Data;
using System.Data.SqlClient;
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
    [ODataRoutePrefix(nameof(ChecklistVerifikator))]
    public class ChecklistVerifikatorController : ODataController
    {
        /// <summary>
        /// Master Checklist REST service.
        /// </summary>
        /// <param name="context">Database context.</param>
        public ChecklistVerifikatorController(PsefMySqlContext context)
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
            return await _context.ChecklistVerifikator.LongCountAsync();
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
        [ProducesResponseType(typeof(ODataValue<IEnumerable<ChecklistVerifikator>>), Status200OK)]
        [EnableQuery]
        public IQueryable<ChecklistVerifikator> Get()
        {
            return _context.ChecklistVerifikator;
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
        [ProducesResponseType(typeof(ChecklistVerifikator), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select)]
        public SingleResult<ChecklistVerifikator> Get([FromODataUri] ulong id)
        {
            return SingleResult.Create(
                _context.ChecklistVerifikator.Where(e => e.Id == id));
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
        
        [AllowAnonymous]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(List<Dictionary<string, object>>), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] List<Dictionary<string, object>> create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (create != null && create.Any())
                {
                    // if (create[0].TryGetValue("permohonanId", out var permohonanIdObj) && uint.TryParse(permohonanIdObj.ToString(), out permohonanId))
                    // {
                        foreach (var item in create)
                        {
                            foreach (var keyValuePair in item)
                            {
                                // if (keyValuePair.Key.StartsWith("permohonanId[") && keyValuePair.Value is ushort permohonanId)
                                if (keyValuePair.Key.StartsWith("permohonanId["))
                                {
                                    // uint permohonanId;
                                    // uint checkValue;
                                    // string noteVerifikator;
                                    // string noteSupervisor;
                                    // Dapatkan nomor indeks dari keyValuePair.Key
                                    int startIndex = keyValuePair.Key.IndexOf('[');
                                    int endIndex = keyValuePair.Key.IndexOf(']');
                                    string indexStr = keyValuePair.Key.Substring(startIndex + 1, endIndex - startIndex - 1);
                                    if (int.TryParse(indexStr, out int index))
                                    {
                                        object verifikatorObj;
                                        object supervisorObj;
                                        string noteVerifikator;
                                        string noteSupervisor;
                                        uint permohonanId = item.TryGetValue($"permohonanId[{index}]", out var permohonanIdObj) && uint.TryParse(permohonanIdObj.ToString(), out permohonanId) ? (byte)permohonanId : (byte)0;
                                        uint checkValue = item.TryGetValue($"checklist[{index}]", out var checkValueObj) && uint.TryParse(checkValueObj.ToString(), out checkValue) ? (byte)checkValue : (byte)0;
                                        // string noteVerifikator = item.TryGetValue($"verifikator[{index}]", out var verifikator) ? verifikator.ToString() : null;
                                        // string noteSupervisor = item.TryGetValue($"supervisor[{index}]", out var supervisor) ? supervisor.ToString() : null;
                                        if (item.TryGetValue($"verifikator[{index}]", out verifikatorObj) && verifikatorObj != DBNull.Value)
                                        {
                                            noteVerifikator = verifikatorObj.ToString();
                                        }
                                        else
                                        {
                                            noteVerifikator = null;
                                        }

                                        if (item.TryGetValue($"supervisor[{index}]", out supervisorObj) && supervisorObj != DBNull.Value)
                                        {
                                            noteSupervisor = supervisorObj.ToString();
                                        }
                                        else
                                        {
                                            noteSupervisor = null;
                                        }
                                        var existingEntity = _context.ChecklistVerifikator.FirstOrDefault(e =>
                                            e.PermohonanId == permohonanId &&
                                            e.ChecklistId == (byte)index);
                                            
                                        if (existingEntity != null)
                                        {
                                            existingEntity.CheckValue = (byte)checkValue;
                                            existingEntity.NoteVerifikator = noteVerifikator;
                                            existingEntity.NoteSupervisor = noteSupervisor;
                                            existingEntity.UpdatedBy = ApiHelper.GetUserName(HttpContext.User);
                                        }
                                        else
                                        {
                                            var checklistEntity = new ChecklistVerifikator
                                            {
                                                PermohonanId = permohonanId, // Sesuaikan dengan kebutuhan Anda
                                                ChecklistId = (byte)index,
                                                CheckValue = (byte)checkValue,
                                                NoteVerifikator = noteVerifikator,
                                                NoteSupervisor = noteSupervisor,
                                                UpdatedBy = ApiHelper.GetUserName(HttpContext.User)
                                                // Set other properties as needed
                                            };

                                            _context.ChecklistVerifikator.Add(checklistEntity);
                                        }
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }

                    // }
                }
                else
                {
                    return NoContent(); // Tidak ada item untuk diproses
                }
            }
            catch (DbUpdateException)
            {
                return Conflict(); // Konflik karena penyisipan konkuren
            }

            return Ok(create);
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
        [ProducesResponseType(typeof(ChecklistVerifikator), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status422UnprocessableEntity)]
        public async Task<IActionResult> Patch(
            [FromODataUri] ulong id,
            [FromBody] Delta<ChecklistVerifikator> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _context.ChecklistVerifikator.FindAsync(id);

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
        public async Task<IActionResult> Delete([FromODataUri] ulong id)
        {
            var delete = await _context.ChecklistVerifikator.FindAsync(id);

            if (delete == null)
            {
                return NotFound();
            }

            _context.ChecklistVerifikator.Remove(delete);
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
        [ProducesResponseType(typeof(ChecklistVerifikator), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromODataUri] ulong id,
            [FromBody] ChecklistVerifikator update)
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
        /// <param name="permohonanId">The requested Permohonan identifier.</param>
        /// <returns>All available Master Checklist for the specified Permohonan.</returns>
        /// <response code="200">List of Master Checklist successfully retrieved.</response>
        /// <response code="404">The list of Master Checklist does not exist.</response>
        
        [AllowAnonymous]
        [HttpGet]
        [Produces(JsonOutput)]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<ChecklistVerifikator>>), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery]
        public IQueryable<ChecklistVerifikator> ByPermohonan(uint permohonanId)
        {
            return _context.ChecklistVerifikator.Where(e => e.PermohonanId == permohonanId);
        }
        
        [AllowAnonymous]
        [HttpGet("exportPdf")]
        public async Task<IActionResult> ExportPdf(int permohonanId)
        {
            try
            {
                // string[] dataprint;
                var no = 1;
                List<Dictionary<string, object>> dataprint = new List<Dictionary<string, object>>();
                string rawQuery = "SELECT * FROM masterchecklist WHERE Parent = 0 ORDER BY Id ASC";

                // Jalankan query dengan parameter
                var dataFromQuery = await _context.MasterChecklist.FromSqlRaw(rawQuery).ToListAsync();

                if (dataFromQuery == null || dataFromQuery.Count == 0)
                {
                    return NoContent(); // Tidak ada data yang ditemukan
                }

                foreach (var item in dataFromQuery)
                {
                    if(item.Input == 0){
                        var checklistEntity = new Dictionary<string, object>
                        {
                            { "No", no++ }, // Sesuaikan dengan kebutuhan Anda
                            { "Kelengkapan Dokumen", item.Kelengkapan },
                            { "Checklist", "" },
                            { "Keterangan", item.Keterangan },
                            { "Note Verifikator", "" },
                            { "Note Supervisor", "" },
                            // Add other properties as needed
                        };
                        dataprint.Add(checklistEntity);

                        string rawQuerys = "SELECT * FROM masterchecklist WHERE Parent = "+item.Id+" ORDER BY Id ASC";

                        // Jalankan query dengan parameter
                        var dataFromQuerys = await _context.MasterChecklist.FromSqlRaw(rawQuerys).ToListAsync();
                        foreach (var items in dataFromQuerys)
                        {
                            var existingEntity = _context.ChecklistVerifikator.FirstOrDefault(e =>
                                            e.PermohonanId == permohonanId &&
                                            e.ChecklistId == items.Id);

                            var checklistEntitys = new Dictionary<string, object>
                            {
                                { "No", "" }, // Sesuaikan dengan kebutuhan Anda
                                { "Kelengkapan Dokumen", items.Kelengkapan },
                                { "Checklist", existingEntity.CheckValue == 1 ? "Ada":"Tidak" },
                                { "Keterangan", items.Keterangan },
                                { "Note Verifikator", existingEntity.NoteVerifikator },
                                { "Note Supervisor", existingEntity.NoteSupervisor },
                                // Add other properties as needed
                            };
                            dataprint.Add(checklistEntitys);
                        }
                    }else{
                        var existingEntity = _context.ChecklistVerifikator.FirstOrDefault(e =>
                                        e.PermohonanId == permohonanId &&
                                        e.ChecklistId == item.Id);

                        var checklistEntitys = new Dictionary<string, object>
                        {
                            { "No", no++ }, // Sesuaikan dengan kebutuhan Anda
                            { "Kelengkapan Dokumen", item.Kelengkapan },
                            { "Checklist", existingEntity.CheckValue == 1 ? "Ada":"Tidak" },
                            { "Keterangan", item.Keterangan },
                            { "Note Verifikator", existingEntity.NoteVerifikator },
                            { "Note Supervisor", existingEntity.NoteSupervisor },
                            // Add other properties as needed
                        };
                        dataprint.Add(checklistEntitys);
                    }
                }
                DataTable dataTable = new DataTable();
                foreach (var key in dataprint.First().Keys)
                {
                    dataTable.Columns.Add(key);
                }

                foreach (var dict in dataprint)
                {
                    var values = dict.Values.ToArray();
                    dataTable.Rows.Add(values);
                }
                // Membuat dokumen PDF baru
                PdfDocument doc = new PdfDocument();
                // Set the page size.
                doc.PageSettings.Size = PdfPageSize.A4;
                //Change the page orientation to landscape
                doc.PageSettings.Orientation = PdfPageOrientation.Landscape;

                PdfPage page = doc.Pages.Add();
                PdfGraphics graphics = page.Graphics;

                //Create string format.
                PdfStringFormat format = new PdfStringFormat();
                format.Alignment = PdfTextAlignment.Center;
                format.LineAlignment = PdfVerticalAlignment.Middle;
                
                // Membuat tabel dengan kolom-kolom yang sesuai
                PdfGrid pdfGrid = new PdfGrid();
                pdfGrid.DataSource = dataTable;
                // //Enable the horizontal overflow property to set table width based on the text width
                // pdfGrid.Style.AllowHorizontalOverflow = true;

                //Declare and define the grid style.
                PdfGridStyle gridStyle = new PdfGridStyle();
                PdfGridStyle gridStyle1 = new PdfGridStyle();
                //Set cell padding, which specifies the space between the border and content of the cell.
                gridStyle.CellPadding = new PdfPaddings(4, 4, 4, 4);
                //Set cell spacing, which specifies the space between the adjacent cells.
                // gridStyle.CellSpacing = 2;
                //Enable to adjust PDF table row width based on the text length.
                gridStyle1.AllowHorizontalOverflow = true;
                //Apply style.

                // Define a custom font and size
                PdfFont customFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                gridStyle.Font = customFont;
                gridStyle1.Font = customFont;
                
                pdfGrid.Style = gridStyle;

                // PdfGridCellStyle cellStyle = new PdfGridCellStyle
                // {
                //     Borders = new PdfBorders(), // No borders
                //     CellPadding = new PdfPaddings(4, 4, 4, 4)
                // };

                PdfGridCellStyle cellStyle = new PdfGridCellStyle();
                cellStyle.Borders.Bottom.Color = new PdfColor(Color.Transparent);
                cellStyle.Borders.Top.Color = new PdfColor(Color.Transparent);
                cellStyle.Borders.Left.Color = new PdfColor(Color.Transparent);
                cellStyle.Borders.Right.Color = new PdfColor(Color.Transparent);
                cellStyle.CellPadding = new PdfPaddings(4, 4, 4, 4);

                PdfGrid table = new PdfGrid();
                table.Style = gridStyle1;
                //Create a DataTable.
                DataTable dataTables = new DataTable();
                //Add columns to the DataTable
                dataTables.Columns.Add("ID");
                dataTables.Columns.Add("Name");
                dataTables.Columns.Add("tes");
                //Add rows to the DataTable.
                // return Ok(
                // _context.Permohonan.Where(e => e.Id == permohonanId));
                var perm = _context.Permohonan.SingleOrDefault(e => e.Id == permohonanId);
                // return Ok(perm);
                dataTables.Rows.Add(new object[] { "Tanggal Pengajuan", ":", perm.SubmittedAt.ToString("dd MMMM yyyy")});
                dataTables.Rows.Add(new object[] { "Nomor Permohonan", ":", perm.PermohonanNumber });
                dataTables.Rows.Add(new object[] { "OSS ID Izin", ":", perm.IdIzin});
                dataTables.Rows.Add(new object[] { "Nama Sistem", ":", perm.SystemName});
                dataTables.Rows.Add(new object[] { "Alamat Domain", ":", perm.Domain});
                // dataTables.Rows.Add(new object[] { "Nama Pimpinan Perusahaan", ":" });
                // dataTables.Rows.Add(new object[] { "Nama Perusahaan", ":" });
                // dataTables.Rows.Add(new object[] { "Nama APJ", ":" });
                // dataTables.Rows.Add(new object[] { "Nomor HP", ":" });
                dataTables.Rows.Add(new object[] { "", "", "" });
                // //Assign data source.
                table.DataSource = dataTables;
                table.BeginCellLayout += PdfGrid_BeginCellLayout;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Rows[i].Cells.Count; j++)
                    {
                        //Apply style
                        table.Rows[i].Cells[j].Style = cellStyle;                                 
                    }
                }
                // table.Rows[0].Cells[0].Style = cellStyle;

                graphics.DrawString("SELF-ASSESSMENT CHECKLIST DOKUMEN PERIZINAN PSEF", customFont, PdfBrushes.Black, new PointF(400, 10), format);
                // Menggambar tabel pada dokumen PDF
                table.Draw(page, new PointF(10, 30));
                // Menggambar tabel pada dokumen PDF
                // pdfGrid.Draw(page, new PointF(10, 80));
                pdfGrid.Draw(page, new PointF(10, 190));

                // Menyimpan dokumen PDF ke MemoryStream
                MemoryStream memoryStream = new MemoryStream();
                doc.Save(memoryStream);
                doc.Close();

                // Mengubah MemoryStream menjadi byte array
                byte[] pdfBytes = memoryStream.ToArray();

                // Mengembalikan file PDF sebagai respons
                
                return File(pdfBytes, "application/pdf", "exported_data.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        private static void PdfGrid_BeginCellLayout(object sender, PdfGridBeginCellLayoutEventArgs args)
        {
            //Draw multiple fonts for a particular table cell.
            if(args.IsHeaderRow)
            {
                args.Skip = true;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<MasterChecklistDTO>>), Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [EnableQuery]
        public IActionResult CountNo(int chart)
        {
            List<Dictionary<string, object>> dataprint = new List<Dictionary<string, object>>();
            var data = _context.MasterChecklist.Where(e => e.Input == 1).OrderBy(e => e.Id).ToList();
            foreach (var items in data)
            {
                var existingEntity = (int)_context.ChecklistVerifikator.Where(e => e.CheckValue == 0 && e.ChecklistId == items.Id).LongCount();
                var checklistEntitys = new Dictionary<string, object>
                {
                    { "Id", items.Id },
                    { "Kelengkapan", items.Kelengkapan },
                    { "Keterangan", items.Keterangan },
                    { "Total", existingEntity },
                    { "tes", ApiHelper.GetUserRole(HttpContext.User) },
                    // Add other properties as needed
                };
                dataprint.Add(checklistEntitys);
            }
            var orderedObjects = dataprint
            .OrderByDescending(obj => (int)obj["Total"])
            .ThenBy(obj => (ushort)obj["Id"])
            .ToList();

            if(chart == 1){
                var limitedDataprint = orderedObjects.Take(5).ToList();
                return Ok(limitedDataprint);
            }else{
                return Ok(orderedObjects);
            }
            // string rawQuery = "SELECT Kelengkapan, Keterangan, " +
            //                 "(SELECT COUNT(checklistverifikator.ChecklistId) " +
            //                 "FROM checklistverifikator " +
            //                 "WHERE checklistverifikator.CheckValue = 0 " +
            //                 "AND checklistverifikator.ChecklistId = masterchecklist.Id) AS Total " +
            //                 "FROM masterchecklist " +
            //                 "WHERE Input = 1 " +
            //                 "ORDER BY Total DESC, masterchecklist.Id ASC;";

            // var result = await _context.MasterChecklistDTO.FromSqlRaw(rawQuery).ToListAsync();

            // if (result != null)
            // {
            //     return Ok(result);
            // }
            // else
            // {
            //     return NotFound();
            // }
        }

        private bool Exists(ulong id)
        {
            return _context.ChecklistVerifikator.Any(e => e.Id == id);
        }

        private readonly PsefMySqlContext _context;
    }
}
