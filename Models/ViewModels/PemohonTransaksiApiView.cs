namespace PsefApiOData.Models
{
    /// <summary>
    /// Represents a Pemohon Transaksi Api.
    /// </summary>
    public class PemohonTransaksiApiView
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
        /// Gets or sets the Pemohon Transaksi API url.
        /// </summary>
        /// <value>The Pemohon's Transaksi API url.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the token for the API Token.
        /// </summary>
        /// <value>The API Token</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets CompanyName.
        /// </summary>
        /// <value>The CompanyName.</value>
        public string CompanyName { get; set; }
    }
}
