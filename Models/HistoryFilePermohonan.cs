using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PsefApiOData.Models
{
    /// <summary>
    /// Represents a History File Permohonan.
    /// </summary>
    public partial class HistoryFilePermohonan
    {
        /// <summary>
        /// Gets or sets the unique identifier for the History File Permohonan.
        /// </summary>
        /// <value>The History File Permohonan's unique identifier.</value>
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the associated History File Permohonan identifier.
        /// </summary>
        /// <value>The associated History File Permohonan identifier.</value>
        public uint? PermohonanId { get; set; }

        /// <summary>
        /// Gets or sets the associated History File Permohonan Status identifier.
        /// </summary>
        /// <value>The associated History File Permohonan Status identifier.</value>
        public byte StatusId { get; set; }

        /// <summary>
        /// (Read Only) Gets the associated History File Permohonan Status name.
        /// </summary>
        /// <value>The associated History File Permohonan Status name.</value>
        [NotMapped]
        public string StatusName
        {
            get => PermohonanStatus.List.Find(e => e.Id == StatusId).Name;
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the History File Permohonan apoteker STRA document url.
        /// </summary>
        /// <value>The History File Permohonan's apoteker STRA document url.</value>
        public string StraUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan Surat Permohonan document url.
        /// </summary>
        /// <value>The History File Permohonan's Surat Permohonan document url.</value>
        public string SuratPermohonanUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan Proses Bisnis document url.
        /// </summary>
        /// <value>The History File Permohonan's Proses Bisnis document url.</value>
        public string ProsesBisnisUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan Dokumen Api document url.
        /// </summary>
        /// <value>The History File Permohonan's Dokumen Api document url.</value>
        public string DokumenApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan Dokumen PSE Kominfo document url.
        /// </summary>
        /// <value>The History File Permohonan's Dokumen PSE Kominfo document url.</value>
        public string DokumenPseUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan izin usaha document url.
        /// </summary>
        /// <value>The History File Permohonan's izin usaha document url.</value>
        public string IzinUsahaUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan komitmen kerjasama apotek document url.
        /// </summary>
        /// <value>The History File Permohonan's komitmen kerjasama apotek document url.</value>
        public string KomitmenKerjasamaApotekUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan SPPL document url.
        /// </summary>
        /// <value>The History File Permohonan's SPPL document url.</value>
        public string SpplUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan Izin Lokasi document url.
        /// </summary>
        /// <value>The History File Permohonan's Izin Lokasi document url.</value>
        public string IzinLokasiUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan IMB document url.
        /// </summary>
        /// <value>The History File Permohonan's IMB document url.</value>
        public string ImbUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan Pembayaran PNBP document url.
        /// </summary>
        /// <value>The History File Permohonan's Pembayaran PNBP document url.</value>
        public string PembayaranPnbpUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan Pernyataan Keaslian Dokumen document url.
        /// </summary>
        /// <value>The History File Permohonan's Pernyataan Keaslian Dokumen document url.</value>
        public string PernyataanKeaslianDokumenUrl { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan last update.
        /// </summary>
        /// <value>The History File Permohonan's last update.</value>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the History File Permohonan updated by.
        /// </summary>
        /// <value>The History File Permohonan's updated by.</value>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets Permohonan associated with the History File Permohonan.
        /// </summary>
        /// <value>The associated History File Permohonan.</value>
        [IgnoreDataMember]
        public virtual Permohonan Permohonan { get; set; }
    }
}
