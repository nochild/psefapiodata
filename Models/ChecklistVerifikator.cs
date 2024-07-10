using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PsefApiOData.Models
{
    /// <summary>
    /// Represents a Checklist Verifikator List.
    /// </summary>
    public class ChecklistVerifikator
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Checklist Verifikator.
        /// </summary>
        /// <value>The Checklist Verifikator's unique identifier.</value>
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the associated Checklist Verifikator Permohonan Id.
        /// </summary>
        /// <value>The associated Checklist Verifikator Permohonan Id.</value>
        public uint PermohonanId { get; set; }

        /// <summary>
        /// Gets or sets the associated Checklist Verifikator Checklist Id.
        /// </summary>
        /// <value>The associated Checklist Verifikator Checklist Id.</value>
        public ushort ChecklistId { get; set; }

        /// <summary>
        /// Gets or sets the associated Checklist Verifikator Checklist Value.
        /// </summary>
        /// <value>The associated Checklist Verifikator Checklist Value.</value>
        public byte CheckValue { get; set; }

        /// <summary>
        /// Gets or sets the Checklist Verifikator Note Verifikator.
        /// </summary>
        /// <value>The Checklist Verifikator's Note Verifikator.</value>
        public string NoteVerifikator { get; set; }

        /// <summary>
        /// Gets or sets the Checklist Verifikator Note Supervisor.
        /// </summary>
        /// <value>The Checklist Verifikator's Note Supervisor.</value>
        public string NoteSupervisor { get; set; }

        /// <summary>
        /// Gets or sets the Checklist Verifikator last update.
        /// </summary>
        /// <value>The Checklist Verifikator's last update.</value>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the Checklist Verifikator updated by.
        /// </summary>
        /// <value>The Checklist Verifikator's updated by.</value>
        public string UpdatedBy { get; set; }
    }
}

