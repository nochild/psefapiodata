using System;

namespace PsefApiOData.Models
{
    /// <summary>
    /// Represents a Tentang Banner.
    /// </summary>
    public partial class Tentang
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Tentang.
        /// </summary>
        /// <value>The Tentang's unique identifier.</value>
        public ushort Id { get; set; }

        /// <summary>
        /// Gets or sets the Tentang Deskripsi.
        /// </summary>
        /// <value>The Tentang's Deskripsi.</value>
        public string Deskripsi { get; set; }
    }
}
