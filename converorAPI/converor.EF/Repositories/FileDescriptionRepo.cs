using converor.Core.Models;
using converor.EF.Data;
using converor.EF.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace converor.EF.Repositories
{
    public class FileDescriptionRepo : IFileDescriptionRepo
    {
        private readonly AppDBContext _context;
        public FileDescriptionRepo(AppDBContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(int id)
        {
            var file = await _context.FileDescriptions.FindAsync(id);
            if (file != null)
                _context.FileDescriptions.Remove(file);
        }

        public async Task<List<FileDescription>> GetAllAsync(string userId)
        {
            return await _context.FileDescriptions.Include(f => f.Content).Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task<FileDescription?> GetByIdAsync(int id)
        {
            return await _context.FileDescriptions.Include(f => f.Content).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task InsertAsync(FileDescription entity)
        {
            await _context.FileDescriptions.AddAsync(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FileDescription entity)
        {
            _context.FileDescriptions.Update(entity);
        }
    }
}
