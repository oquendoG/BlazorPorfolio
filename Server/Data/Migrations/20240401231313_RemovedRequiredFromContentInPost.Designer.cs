﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240401231313_RemovedRequiredFromContentInPost")]
    partial class RemovedRequiredFromContentInPost
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("Shared.Models.Blog.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ThumbnailImage")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("74c25577-7dd8-47b1-a021-ef808ad0cd24"),
                            Description = "Description of category 1",
                            Name = "Category 1",
                            ThumbnailImage = "uploads/placeholder.jpg"
                        },
                        new
                        {
                            Id = new Guid("cca6df3e-c22a-4586-8211-124e6df4970c"),
                            Description = "Description of category 2",
                            Name = "Category 2",
                            ThumbnailImage = "uploads/placeholder.jpg"
                        },
                        new
                        {
                            Id = new Guid("662e4d35-71cc-4b78-8e10-d819bf6c3b7d"),
                            Description = "Description of category 3",
                            Name = "Category 3",
                            ThumbnailImage = "uploads/placeholder.jpg"
                        });
                });

            modelBuilder.Entity("Shared.Models.Blog.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .HasMaxLength(65536)
                        .HasColumnType("TEXT");

                    b.Property<string>("Excerpt")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("PublishDate")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Published")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Thumbnailimage")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Posts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("4f4281ee-5106-48cd-a4c3-5bd0f510cf6f"),
                            Author = "Wilson OQuendo",
                            CategoryId = new Guid("74c25577-7dd8-47b1-a021-ef808ad0cd24"),
                            Content = "",
                            Excerpt = "Este es un extracto del post 1.",
                            PublishDate = "01/04/2024 11:13",
                            Published = false,
                            Thumbnailimage = "uploads/placeholder.jpg",
                            Title = "Primer Post"
                        },
                        new
                        {
                            Id = new Guid("38c5baa2-c59b-41c0-96b9-c18464a9e345"),
                            Author = "Wilson OQuendo",
                            CategoryId = new Guid("cca6df3e-c22a-4586-8211-124e6df4970c"),
                            Content = "",
                            Excerpt = "Este es un extracto del post 2.",
                            PublishDate = "01/04/2024 11:13",
                            Published = false,
                            Thumbnailimage = "uploads/placeholder.jpg",
                            Title = "Segundo Post"
                        },
                        new
                        {
                            Id = new Guid("8d020f5c-0ee7-4616-abfc-da99eafd74d3"),
                            Author = "Wilson OQuendo",
                            CategoryId = new Guid("662e4d35-71cc-4b78-8e10-d819bf6c3b7d"),
                            Content = "",
                            Excerpt = "Este es un extracto del post 3.",
                            PublishDate = "01/04/2024 11:13",
                            Published = false,
                            Thumbnailimage = "uploads/placeholder.jpg",
                            Title = "Tercer Post"
                        },
                        new
                        {
                            Id = new Guid("3f500016-6c13-4eeb-8040-4c632a1e5851"),
                            Author = "Wilson OQuendo",
                            CategoryId = new Guid("74c25577-7dd8-47b1-a021-ef808ad0cd24"),
                            Content = "",
                            Excerpt = "Este es un extracto del post 4.",
                            PublishDate = "01/04/2024 11:13",
                            Published = false,
                            Thumbnailimage = "uploads/placeholder.jpg",
                            Title = "Cuarto Post"
                        },
                        new
                        {
                            Id = new Guid("c1a61afd-4462-44b5-9cde-804d8ae451b3"),
                            Author = "Wilson OQuendo",
                            CategoryId = new Guid("cca6df3e-c22a-4586-8211-124e6df4970c"),
                            Content = "",
                            Excerpt = "Este es un extracto del post 5.",
                            PublishDate = "01/04/2024 11:13",
                            Published = false,
                            Thumbnailimage = "uploads/placeholder.jpg",
                            Title = "Quinto Post"
                        },
                        new
                        {
                            Id = new Guid("21481a83-7763-40b2-9613-a78d6dc6f459"),
                            Author = "Wilson OQuendo",
                            CategoryId = new Guid("662e4d35-71cc-4b78-8e10-d819bf6c3b7d"),
                            Content = "",
                            Excerpt = "Este es un extracto del post 6.",
                            PublishDate = "01/04/2024 11:13",
                            Published = false,
                            Thumbnailimage = "uploads/placeholder.jpg",
                            Title = "Sexto Post"
                        });
                });

            modelBuilder.Entity("Shared.Models.Blog.Post", b =>
                {
                    b.HasOne("Shared.Models.Blog.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Shared.Models.Blog.Category", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
