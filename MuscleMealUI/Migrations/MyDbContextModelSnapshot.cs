﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MuscleMealUI;

#nullable disable

namespace MuscleMealUI.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("MuscleMealUI.Models.Ingredient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Amount")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("RecipeID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Unit")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("RecipeID");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("MuscleMealUI.Models.Recipe", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Instruction")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("OwnerID");

                    b.ToTable("Recipe");
                });

            modelBuilder.Entity("MuscleMealUI.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bio")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Password")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("BLOB");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RecipeUser", b =>
                {
                    b.Property<int>("FavoriteByID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FavoritesID")
                        .HasColumnType("INTEGER");

                    b.HasKey("FavoriteByID", "FavoritesID");

                    b.HasIndex("FavoritesID");

                    b.ToTable("RecipeUser");
                });

            modelBuilder.Entity("MuscleMealUI.Models.Ingredient", b =>
                {
                    b.HasOne("MuscleMealUI.Models.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeID");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("MuscleMealUI.Models.Recipe", b =>
                {
                    b.HasOne("MuscleMealUI.Models.User", "Owner")
                        .WithMany("Recipes")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("RecipeUser", b =>
                {
                    b.HasOne("MuscleMealUI.Models.User", null)
                        .WithMany()
                        .HasForeignKey("FavoriteByID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MuscleMealUI.Models.Recipe", null)
                        .WithMany()
                        .HasForeignKey("FavoritesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MuscleMealUI.Models.Recipe", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("MuscleMealUI.Models.User", b =>
                {
                    b.Navigation("Recipes");
                });
#pragma warning restore 612, 618
        }
    }
}
