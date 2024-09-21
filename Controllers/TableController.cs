using Microsoft.AspNetCore.Mvc;
using Ordernary.Models;
using Ordernary.Models.DTOs;
using Ordernary.Repositories.Interface;

namespace Ordernary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableRepository _tableRepository;
        public TableController(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }
        [HttpGet("alltables")]
        public async Task<ActionResult<IEnumerable<Table>>> GetTables()
        {
            var tables = await _tableRepository.GetAllAsync();
            return Ok(tables);
        }
        [HttpGet("table{id}")]
        public async Task<ActionResult<Table>> GetTable(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table);
        }
        [HttpPost("createtable")]
        public async Task<ActionResult<Table>> PostTable(TableDTO tableDto)
        {
            var table = new Table
            {
                Occupied = false,
                WeiterCall = false,
                Number = tableDto.Number
            };

            await _tableRepository.AddAsync(table);
            await _tableRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTable), new { id = table.TableId }, table);
        }
        [HttpPut("update{id}")]
        public async Task<IActionResult> PutTable(int id, TableDTO tableDto)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            // Update the existing table with the DTO values
            table.Occupied = tableDto.Occupied;
            table.WeiterCall = tableDto.WeiterCall;
            table.Number = tableDto.Number;

            await _tableRepository.UpdateAsync(table);
            await _tableRepository.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("delete{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            await _tableRepository.DeleteAsync(id);
            await _tableRepository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("assign")]
        public async Task<IActionResult> AssignTablesToWaiter([FromBody] AssignTablesDto request)
        {
            try
            {
                await _tableRepository.AssignTablesToWaiterAsync(request.WaiterId, request.TableIds);
                return Ok("Tables assigned to the waiter successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
