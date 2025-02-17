using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using PsefApiOData.Models;
using PsefApiOData.Models.ViewModels;

namespace PsefApiOData.Misc
{
    internal class PermohonanHelper
    {
        public PermohonanHelper(PsefMySqlContext context)
        {
            _context = context;
        }

        public IQueryable<Permohonan> Verifikator()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId == PermohonanStatus.Diajukan.Id ||
                    c.StatusId == PermohonanStatus.DikembalikanKepalaSeksi.Id);
        }

        public IQueryable<Permohonan> Kasi()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId == PermohonanStatus.DisetujuiVerifikator.Id ||
                    c.StatusId == PermohonanStatus.DikembalikanKepalaSubDirektorat.Id);
        }

        public IQueryable<Permohonan> Kasubdit()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId == PermohonanStatus.DisetujuiKepalaSeksi.Id ||
                    c.StatusId == PermohonanStatus.DikembalikanDirekturPelayananFarmasi.Id);
        }

        public IQueryable<Permohonan> Diryanfar()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId == PermohonanStatus.DisetujuiKepalaSubDirektorat.Id ||
                    c.StatusId == PermohonanStatus.DikembalikanDirekturJenderal.Id);
        }

        public IQueryable<Permohonan> Dirjen()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId == PermohonanStatus.DisetujuiDirekturPelayananFarmasi.Id);
        }

        public IQueryable<Permohonan> DirjenSetujui()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId == PermohonanStatus.DisetujuiDirekturJenderal.Id);
        }

        public IQueryable<Permohonan> Validator()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId == PermohonanStatus.DisetujuiDirekturJenderal.Id);
        }

        public IQueryable<Permohonan> NonRumusan()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId != PermohonanStatus.Dibuat.Id &&
                    c.StatusId != PermohonanStatus.DikembalikanVerifikator.Id);
        }

        public IQueryable<Permohonan> DalamProses()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId != PermohonanStatus.Dibuat.Id &&
                    c.StatusId != PermohonanStatus.DikembalikanVerifikator.Id &&
                    c.StatusId != PermohonanStatus.Selesai.Id &&
                    c.StatusId != PermohonanStatus.Ditolak.Id);
        }


        public IQueryable<Permohonan> Selesai()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId == PermohonanStatus.Selesai.Id);
        }

        public IQueryable<Permohonan> Ditolak()
        {
            return _context.Permohonan
                .Where(c =>
                    c.StatusId == PermohonanStatus.Ditolak.Id);
        }

        public HistoryPermohonan CreateHistory(
            Permohonan permohonan,
            PermohonanSystemUpdate update,
            HttpContext httpContext)
        {
            return new HistoryPermohonan
            {
                PermohonanId = permohonan.Id,
                StatusId = permohonan.StatusId,
                Reason = update.Reason ?? string.Empty,
                UpdatedAt = DateTime.Now,
                UpdatedBy = ApiHelper.GetUserName(httpContext.User)
            };
        }

        public HistoryFilePermohonan CreateHistoryFile(
            Permohonan permohonan,
            PermohonanSystemUpdate update,
            HttpContext httpContext)
        {
            return new HistoryFilePermohonan
            {
                PermohonanId = permohonan.Id,
                StraUrl = permohonan.StraUrl ?? string.Empty,
                SuratPermohonanUrl = permohonan.SuratPermohonanUrl ?? string.Empty,
                ProsesBisnisUrl = permohonan.ProsesBisnisUrl ?? string.Empty,
                DokumenApiUrl = permohonan.DokumenApiUrl ?? string.Empty,
                DokumenPseUrl = permohonan.DokumenPseUrl ?? string.Empty,
                IzinUsahaUrl = permohonan.IzinUsahaUrl ?? string.Empty,
                KomitmenKerjasamaApotekUrl = permohonan.KomitmenKerjasamaApotekUrl ?? string.Empty,
                SpplUrl = permohonan.SpplUrl ?? string.Empty,
                IzinLokasiUrl = permohonan.IzinLokasiUrl ?? string.Empty,
                ImbUrl = permohonan.ImbUrl ?? string.Empty,
                PembayaranPnbpUrl = permohonan.PembayaranPnbpUrl ?? string.Empty,
                PernyataanKeaslianDokumenUrl = permohonan.PernyataanKeaslianDokumenUrl ?? string.Empty,
                LastUpdate = DateTime.Now,
                UpdatedBy = ApiHelper.GetUserName(httpContext.User),
                StatusId = permohonan.StatusId
            };
        }

        public GeneratePdfResult GenerateAndSignPdf(
            TandaDaftarHelper helper,
            OssFullInfo ossFullInfo,
            Pemohon pemohon,
            Permohonan permohonan,
            Perizinan perizinan)
        {
            GeneratePdfResult result = helper.GeneratePdf(ossFullInfo, pemohon, permohonan, perizinan);
            result.SignResult = new ElectronicSignatureResult
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                FailureContent = string.Empty
            };

            return result;
        }

        private readonly PsefMySqlContext _context;
    }
}