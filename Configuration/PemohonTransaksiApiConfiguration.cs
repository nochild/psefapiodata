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
    public class PemohonTransaksiApiConfiguration : IModelConfiguration
    {
        /// <summary>
        /// Applies model configurations using the provided builder for the specified API version.
        /// </summary>
        /// <param name="builder">The <see cref="ODataModelBuilder">builder</see> used to apply configurations.</param>
        /// <param name="apiVersion">The <see cref="ApiVersion">API version</see> associated with the <paramref name="builder"/>.</param>
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            builder.ComplexType<PemohonTransaksiApiUpdate>();
            EntityTypeConfiguration<PemohonTransaksiApiView> pemohonapi = builder
               .EntitySet<PemohonTransaksiApiView>(nameof(PemohonTransaksiApi))
               .EntityType;

            pemohonapi.Collection
                .Function(nameof(PemohonTransaksiApiController.UserApi))
                .Returns<PemohonTransaksiApiView>();

            pemohonapi.HasKey(p => p.Id);
            pemohonapi
                .Filter()
                .OrderBy()
                .Page(50, 50)
                .Select();
        }
    }
}

