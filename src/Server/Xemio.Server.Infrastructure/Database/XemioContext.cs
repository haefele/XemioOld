using Microsoft.EntityFrameworkCore;
using Xemio.Server.Infrastructure.Entities.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Database
{
    public class XemioContext : DbContext
    {
        public XemioContext(DbContextOptions<XemioContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Folder>(f =>
            {
                f.HasKey(d => d.Id);
                f.HasIndex(d => d.UserId);
                f.Property(d => d.Name).HasMaxLength(200).IsRequired();
                f.Property(d => d.UserId).IsRequired();
                f.Property(d => d.ETag).IsRowVersion();
                f.HasMany(d => d.SubFolders).WithOne(d => d.ParentFolder);
            });
        }

        public DbSet<Folder> Folders { get; set; }
    }
}
