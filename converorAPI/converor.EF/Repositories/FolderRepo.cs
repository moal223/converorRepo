
using converor.Core.Models;
using converor.EF.Data;
using converor.EF.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace converor.EF.Repositories
{
    public class FolderRepo : IFolderRepo
    {
        private readonly AppDBContext _context;
        public FolderRepo(AppDBContext context){
            _context = context;
        }
        public Task<int> DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteMultyAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Folder>> GetAllAsync(int parentId)
        {
            var folders = await _context.Folders.Where(f => f.ParentFolderId == parentId).ToListAsync();
            return folders;
        }

        public async Task<Folder> GetByIdAsync(int id)
        {
            var folder = await _context.Folders.Include(f => f.SubFiles).Include(f => f.SubFolders).FirstOrDefaultAsync(f => f.Id == id);
            return folder;
        }

        public async Task<int> InsertAsync(Folder model)
        {
            await _context.Folders.AddAsync(model);
            var folder = await _context.SaveChangesAsync(); 
            return folder;
        }

        public Task<Folder> UpdateAsync()
        {
            throw new NotImplementedException();
        }
    }
}