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
        public async Task AssignTablesToWaiterAsync(int waiterId, List<int> tableIds)
        {
            var waiter = await _context.AppUsers.FindAsync(waiterId);
            if (waiter == null || waiter.Role != Role.WEITER)
                throw new Exception("Waiter not found or invalid role.");

            var tables = await _context.Tables.Where(t => tableIds.Contains(t.TableId)).ToListAsync();
            foreach (var table in tables)
            {
                table.WeiterId = waiterId; 
            }

            await _context.SaveChangesAsync();
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
