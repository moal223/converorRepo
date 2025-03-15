using converor.Core.Models;

namespace converor.EF.Repositories.Interfaces
{
    public interface IFileDescriptionRepo
    {
        Task<List<FileDescription>> GetAllWithContentAsync(string userId);
        Task<List<FileDescription>> GetAllAsync(string userId);
        Task<FileDescription?> GetByIdAsync(int id);
        Task InsertAsync(FileDescription entity);
        Task UpdateAsync(FileDescription entity);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
