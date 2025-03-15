
using converor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace converor.EF.Configurations
{
    public class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder){
            // sub-folders relationship
            builder.HasOne(f => f.ParentFolder)
                    .WithMany(f => f.SubFolders)
                    .HasForeignKey(f => f.ParentFolderId)
                    .OnDelete(DeleteBehavior.Restrict);
            
            // folder-file relationship
            builder.HasMany(f => f.SubFiles)
                    .WithOne(fi => fi.ParentFolder)
                    .HasForeignKey(fi => fi.ParentFolderId)
                    .OnDelete(DeleteBehavior.Cascade);

            // user-folder relationship
            builder.HasOne(f => f.ApplicationUser)
                    .WithMany(u => u.Folders)
                    .HasForeignKey(f => f.ApplicationUserId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}