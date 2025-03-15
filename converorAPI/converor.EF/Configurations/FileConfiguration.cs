using Microsoft.EntityFrameworkCore;
using converor.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace converor.EF.Configurations
{
    public class FileConfiguration : IEntityTypeConfiguration<FileDescription>
    {
        public void Configure(EntityTypeBuilder<FileDescription> builder){
            // filedescription-folder relationship [one to many]
            builder.HasOne(fi => fi.ParentFolder)
                    .WithMany(f => f.SubFiles)
                    .HasForeignKey(fi => fi.ParentFolderId);
        }
    }
}