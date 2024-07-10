using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;
using PsefApiOData.Models;
using PsefApiOData.Controllers;
using System;

namespace PsefApiOData.Configuration
{
    /// <summary>
    /// Represents the model configuration for Pemohon Transaksi Api.
    /// </summary>
    public class PemohonTransaksiApiDetailConfiguration : IModelConfiguration
    {
        /// <summary>
        /// Applies model configurations using the provided builder for the specified API version.
        /// </summary>
        /// <param name="builder">The <see cref="ODataModelBuilder">builder</see> used to apply configurations.</param>
        /// <param name="apiVersion">The <see cref="ApiVersion">API version</see> associated with the <paramref name="builder"/>.</param>
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            builder.ComplexType<PemohonTransaksiApiDetail>();
            EntityTypeConfiguration<PemohonTransaksiApiDetailView> pemohonapi = builder
               .EntitySet<PemohonTransaksiApiDetailView>(nameof(PemohonTransaksiApiDetail))
               .EntityType;

            pemohonapi.HasKey(p => p.Id);
            pemohonapi
                .Filter()
                .OrderBy()
                .Page(50, 50)
                .Select();
            pemohonapi.Collection
                .Function(nameof(PemohonTransaksiApiDetailController.ByParent))
                .ReturnsFromEntitySet<PemohonTransaksiApiDetailView>(nameof(PemohonTransaksiApiDetail))
                .Parameter<uint>("parentId");
        }
    }
}

