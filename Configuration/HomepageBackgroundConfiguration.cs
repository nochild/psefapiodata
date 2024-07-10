using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using PsefApiOData.Models;

namespace PsefApiOData.Configuration
{
    /// <summary>
    /// Represents the model configuration for Homepage Background.
    /// </summary>
    public class HomepageBackgroundConfiguration : IModelConfiguration
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

            EntityTypeConfiguration<HomepageBackground> bg = builder
                .EntitySet<HomepageBackground>(nameof(HomepageBackground))
                .EntityType;

            bg.HasKey(p => p.Id);
            bg
                .Expand(SelectExpandType.Disabled)
                .Filter()
                .OrderBy()
                .Page(50, 50)
                .Select();
        }
    }
}