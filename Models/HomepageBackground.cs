namespace PsefApiOData.Models
{
    /// <summary>
    /// Represents a Homepage Background.
    /// </summary>
    public partial class HomepageBackground
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Homepage Background.
        /// </summary>
        /// <value>The Homepage Background's unique identifier.</value>
        public ushort Id { get; set; }

        /// <summary>
        /// Gets or sets the Homepage Background url.
        /// </summary>
        /// <value>The Homepage Background's url.</value>
        public string Url { get; set; }
    }
}
