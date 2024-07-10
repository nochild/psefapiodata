using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PsefApiOData.Models
{
    /// <summary>
    /// Represents a Pemohon Transaksi Api.
    /// </summary>
    public class PemohonTransaksiApiDetailView
    {

        /// <summary>
        /// Gets or sets the unique identifier for the Pemohon Transaksi API.
        /// </summary>
        /// <value>The Pemohon's unique identifier.</value>
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the associated Pemohon identifier.
        /// </summary>
        /// <value>The associated Pemohon identifier.</value>
        public uint PemohonId { get; set; }

        /// <summary>
        /// Gets or sets the Pemohon Transaksi API Tanggal Transaksi.
        /// </summary>
        /// <value>The Pemohon's Transaksi API Tanggal Transaksi.</value>
        public DateTime TanggalTransaksi { get; set; }

        /// <summary>
        /// Gets or sets the token for the API Nomor Transaksi.
        /// </summary>
        /// <value>The Nomor Transaksi</value>
        public string NomorTransaksi { get; set; }

        /// <summary>
        /// Gets or sets the token for the API Sarana Transaksi.
        /// </summary>
        /// <value>The Sarana Transaksi</value>
        public string SaranaTransaksi { get; set; }

        /// <summary>
        /// Gets or sets the token for the API Sediaan Transaksi.
        /// </summary>
        /// <value>The Sediaan Transaksi</value>
        public string SediaanTransaksi { get; set; }

        /// <summary>
        /// Gets or sets the token for the API Golongan Obat.
        /// </summary>
        /// <value>The Golongan Obat</value>
        public string GolonganObat { get; set; }

        /// <summary>
        /// Gets or sets the token for the API Jumlah Sediaan.
        /// </summary>
        /// <value>The Jumlah Sediaan</value>
        public string JumlahSediaan { get; set; }

        /// <summary>
        /// Gets or sets the token for the API Nama Pasien.
        /// </summary>
        /// <value>The Nama Pasien</value>
        public string NamaPasien { get; set; }

        /// <summary>
        /// Gets or sets the token for the API Resep Obat.
        /// </summary>
        /// <value>The Resep Obat</value>
        public string ResepObat { get; set; }

        /// <summary>
        /// Gets or sets CompanyName.
        /// </summary>
        /// <value>The CompanyName.</value>
        public string CompanyName { get; set; }
    }
}
