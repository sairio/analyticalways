﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20200818123306_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Data.Entities.StockTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("date");

                    b.Property<string>("PointOfSale")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150)
                        .IsUnicode(false);

                    b.Property<string>("Product")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150)
                        .IsUnicode(false);

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("StockTable");
                });
#pragma warning restore 612, 618
        }
    }
}
