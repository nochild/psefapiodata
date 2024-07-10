using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;
using PsefApiOData.Models;

namespace PsefApiOData.Configuration
{
    /// <summary>
    /// Represents the model configuration for Permohonan Klinik.
    /// </summary>
    public class PermohonanKlinikConfiguration : IModelConfiguration
    {
        /// <summary>
        /// Applies model configurations using the provided builder for the specified API version.
        /// </summary>
        /// <param name="builder">The <see cref="ODataModelBuilder">builder</see> used to apply configurations.</param>
        /// <param name="apiVersion">The <see cref="ApiVersion">API version</see> associated with the <paramref name="builder"/>.</param>
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            EntityTypeConfiguration<PermohonanKlinik> permohonan = builder
                .EntitySet<PermohonanKlinik>(nameof(PermohonanKlinik))
                .EntityType;

            permohonan.HasKey(p => p.PermohonanId);
            permohonan
                .Filter()
                .OrderBy()
                .Page(50, 50)
                .Select();
        }
    }
}