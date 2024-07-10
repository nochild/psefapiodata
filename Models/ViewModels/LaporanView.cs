﻿using System;

namespace PsefApiOData.Models
{
    /// <summary>
    /// Represents a Laporan.
    /// </summary>
    public class LaporanView
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Laporan.
        /// </summary>
        /// <value>The Pemohon's unique identifier.</value>
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the Permohonan submit date.
        /// </summary>
        /// <value>The Permohonan's submit date.</value>
        public DateTime SubmittedAt { get; set; }

        /// <summary>
        /// Gets or sets the id for the Laporan Status.
        /// </summary>
        /// <value>The Laporan Status</value>
        public string keterangan { get; set; }

        /// <summary>
        /// Gets or sets the Laporan document url.
        /// </summary>
        /// <value>The Laporan's document url.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Laporan CompanyName.
        /// </summary>
        /// <value>The Laporan's document CompanyName.</value>
        public string CompanyName { get; set; }
    }
}
