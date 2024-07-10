using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PsefApiOData.Models
{
    /// <summary>
    /// Represents a Checklist List.
    /// </summary>
    public class MasterChecklist
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Checklist.
        /// </summary>
        /// <value>The Checklist's unique identifier.</value>
        public ushort Id { get; set; }

        /// <summary>
        /// Gets or sets the Kelengkapan.
        /// </summary>
        /// <value>The Kelengkapan</value>
        public string Kelengkapan { get; set; }

        /// <summary>
        /// Gets or sets the Keterangan.
        /// </summary>
        /// <value>The Keterangan</value>
        public string Keterangan { get; set; }

        /// <summary>
        /// Gets or sets the Parent.
        /// </summary>
        /// <value>The Parent.</value>
        public ushort Parent { get; set; }

        /// <summary>
        /// Gets or sets the Input.
        /// </summary>
        /// <value>The Input.</value>
        public ushort Input { get; set; }

    }
    
    public class MasterChecklistDTO
    {
        public int Id { get; set; }
        public string Kelengkapan { get; set; }
        public string Keterangan { get; set; }
        public int Total { get; set; }
    }

}

