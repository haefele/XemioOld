using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Xemio.Server.Infrastructure.Database;

namespace Xemio.Server.Infrastructure.Database.Migrations
{
    [DbContext(typeof(XemioContext))]
    [Migration("20161218035829_FolderNameLengthAndIsRequired")]
    partial class FolderNameLengthAndIsRequired
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Xemio.Server.Infrastructure.Entities.Notes.Folder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("ETag")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<Guid?>("ParentFolderId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ParentFolderId");

                    b.HasIndex("UserId");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("Xemio.Server.Infrastructure.Entities.Notes.Folder", b =>
                {
                    b.HasOne("Xemio.Server.Infrastructure.Entities.Notes.Folder", "ParentFolder")
                        .WithMany("SubFolders")
                        .HasForeignKey("ParentFolderId");
                });
        }
    }
}
