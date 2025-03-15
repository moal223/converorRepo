namespace converor.Core.Models
{
    public class Folder
    {
        public int Id {get; set;}
        public string Name {get; set;}

        // user
        public string ApplicationUserId {get; set;}
        public ApplicationUser ApplicationUser {get; set;}

        // self-referencing for parent folder

        // ForeignKey for parent folder
        public int? ParentFolderId {get; set;}
        public Folder ParentFolder {get; set;}


        public List<Folder> SubFolders {get; set;}
        public List<FileDescription> SubFiles {get; set;}
    }
}