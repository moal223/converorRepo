
using converor.Core.Models;

namespace converor.EF.Repositories.Interfaces
{
    public interface IFolderRepo
    {
        Task<int> InsertAsync(Folder model);
        Task<int> DeleteAsync();
        Task<int> DeleteMultyAsync();
        Task<List<Folder>> GetAllAsync(int parentId);
        Task<Folder> GetByIdAsync(int id);
        Task<Folder> UpdateAsync(); 
    }
}