﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TextSearchEngine;

#nullable disable

namespace TextSearchEngine.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230404092610_file")]
    partial class file
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.15");

            modelBuilder.Entity("TextSearchEngine.Models.Essay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Keyword")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PostDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Essay");
                });

            modelBuilder.Entity("TextSearchEngine.Models.MarkDown", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("File")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PostDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MarkDown");
                });

            modelBuilder.Entity("TextSearchEngine.Models.UserInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PostDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
