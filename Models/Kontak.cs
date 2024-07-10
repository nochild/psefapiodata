using System;

namespace PsefApiOData.Models
{
    /// <summary>
    /// Represents a Kontak.
    /// </summary>
    public partial class Kontak
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Kontak.
        /// </summary>
        /// <value>The Kontak's unique identifier.</value>
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the Kontak Email.
        /// </summary>
        /// <value>The Kontak's Email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Kontak Alamat.
        /// </summary>
        /// <value>The Kontak's Alamat.</value>
        public string Alamat { get; set; }
    }
}
