using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using PsefApiOData.Controllers;
using PsefApiOData.Models;

namespace PsefApiOData.Configuration
{
    /// <summary>
    /// Represents the model configuration for History File Permohonan.
    /// </summary>
    public class HistoryFilePermohonanConfiguration : IModelConfiguration
    {
        /// <summary>
        /// Applies model configurations using the provided builder for the specified API version.
        /// </summary>
        /// <param name="builder">The <see cref="ODataModelBuilder">builder</see> used to apply configurations.</param>
        /// <param name="apiVersion">The <see cref="ApiVersion">API version</see> associated with the <paramref name="builder"/>.</param>
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            EntityTypeConfiguration<HistoryFilePermohonan> history = builder
                .EntitySet<HistoryFilePermohonan>(nameof(HistoryFilePermohonan))
                .EntityType;

            history.Property(e => e.StatusName).AddedExplicitly = true;

            history.Collection
                .Function(nameof(HistoryFilePermohonanController.TotalCount))
                .Returns<long>();
            history.Collection
                .Function(nameof(HistoryFilePermohonanController.ByPermohonan))
                .ReturnsFromEntitySet<HistoryFilePermohonan>(nameof(HistoryFilePermohonan))
                .Parameter<uint>("permohonanId");

            history.HasKey(p => p.Id);
            history
                .Expand(SelectExpandType.Disabled)
                .Filter()
                .OrderBy()
                .Page(50, 50)
                .Select();
        }
    }
}