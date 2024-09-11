using Microsoft.EntityFrameworkCore;
using Ordernary.Data;
using Ordernary.Models;
using Ordernary.Repositories.Interface;

namespace Ordernary.Repositories.Implementation
{
    public class TableRepository : ITableRepository
    {
        private readonly Context _context;

        public TableRepository(Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Table>> GetAllAsync()
        {
            return await _context.Tables.ToListAsync();
        }
        public async Task<Table> GetByIdAsync(int tableId)
        {
            return await _context.Tables.FindAsync(tableId);
        }
        public async Task AddAsync(Table table)
        {
            await _context.Tables.AddAsync(table);
        }
        public async Task UpdateAsync(Table table)
        {
            _context.Tables.Update(table);
        }
        public async Task DeleteAsync(int tableId)
        {
            var table = await GetByIdAsync(tableId);
            if (table != null)
            {
                _context.Tables.Remove(table);
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
