namespace PsefApiOData.Models
{
    /// <summary>
    /// Represents a Pemohon Transaksi.
    /// </summary>
    public class PemohonTransaksiApiUpdate
    {
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
    }
}
