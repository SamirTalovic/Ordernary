using Ordernary.Models;

namespace Ordernary.Repositories.Interface
{
    public interface ITableRepository
    {
        Task<IEnumerable<Table>> GetAllAsync();       
        Task<Table> GetByIdAsync(int tableId);        
        Task AddAsync(Table table);                   
        Task UpdateAsync(Table table);                
        Task DeleteAsync(int tableId);                
        Task SaveChangesAsync();
    }
}
