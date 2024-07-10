using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using PsefApiOData.Controllers;
using PsefApiOData.Models;

namespace PsefApiOData.Configuration
{
    /// <summary>
    /// Represents the model configuration for Checklist Verifikator .
    /// </summary>
    public class ChecklistVerifikatorConfiguration : IModelConfiguration
    {
        /// <summary>
        /// Applies model configurations using the provided builder for the specified API version.
        /// </summary>
        /// <param name="builder">The <see cref="ODataModelBuilder">builder</see> used to apply configurations.</param>
        /// <param name="apiVersion">The <see cref="ApiVersion">API version</see> associated with the <paramref name="builder"/>.</param>
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            if (apiVersion < ApiInfo.Ver1_0)
            {
                return;
            }
            EntityTypeConfiguration<ChecklistVerifikator> history = builder
                .EntitySet<ChecklistVerifikator>(nameof(ChecklistVerifikator))
                .EntityType;

            history.Collection
                .Function(nameof(ChecklistVerifikatorController.TotalCount))
                .Returns<long>();
            history.Collection
                .Function(nameof(ChecklistVerifikatorController.CountNo))
                .ReturnsFromEntitySet<MasterChecklistDTO>(nameof(MasterChecklistDTO));
            history.Collection
                .Function(nameof(ChecklistVerifikatorController.ByPermohonan))
                .ReturnsFromEntitySet<ChecklistVerifikator>(nameof(ChecklistVerifikator))
                .Parameter<uint>("permohonanId");
            history.Collection
                .Function(nameof(ChecklistVerifikatorController.ExportPdf))
                .ReturnsFromEntitySet<ChecklistVerifikator>(nameof(ChecklistVerifikator))
                .Parameter<uint>("permohonanId");

            history.HasKey(p => p.Id);
            history
                .Expand(SelectExpandType.Disabled)
                .Filter()
                .OrderBy()
                // .Page(50, 50)
                .Select();
        }
    }
}