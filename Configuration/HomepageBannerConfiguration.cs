﻿using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using PsefApiOData.Models;

namespace PsefApiOData.Configuration
{
    /// <summary>
    /// Represents the model configuration for Homepage Banner.
    /// </summary>
    public class HomepageBannerConfiguration : IModelConfiguration
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

            EntityTypeConfiguration<HomepageBanner> banner = builder
                .EntitySet<HomepageBanner>(nameof(HomepageBanner))
                .EntityType;

            banner.HasKey(p => p.Id);
            banner
                .Expand(SelectExpandType.Disabled)
                .Filter()
                .OrderBy()
                .Page(50, 50)
                .Select();
        }
    }
}