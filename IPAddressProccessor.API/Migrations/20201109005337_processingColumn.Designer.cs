﻿// <auto-generated />
using System;
using IPAddressProccessor.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IPAddressProccessor.API.Migrations
{
    [DbContext(typeof(IPAddressesContext))]
    [Migration("20201109005337_processingColumn")]
    partial class processingColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IPAddressProccessor.API.Data.Entities.BatchUpdateJob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("BatchUpdateJobs");
                });

            modelBuilder.Entity("IPAddressProccessor.API.Data.Entities.BatchUpdateJobItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IpToUpdate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Processing")
                        .HasColumnType("bit");

                    b.Property<bool>("UpdateComplete")
                        .HasColumnType("bit");

                    b.Property<Guid?>("UpdateJobId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UpdateJobId");

                    b.ToTable("BatchUpdateJobItems");
                });

            modelBuilder.Entity("IPAddressProccessor.API.Data.Entities.IPAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Continent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("IPAddresses");
                });

            modelBuilder.Entity("IPAddressProccessor.API.Data.Entities.BatchUpdateJobItem", b =>
                {
                    b.HasOne("IPAddressProccessor.API.Data.Entities.BatchUpdateJob", "UpdateJob")
                        .WithMany("JobItems")
                        .HasForeignKey("UpdateJobId");
                });
#pragma warning restore 612, 618
        }
    }
}
