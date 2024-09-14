using Ordernary.Models.DTOs;

namespace Ordernary.Repositories.Interface
{
    public interface IReportInterface
    {
        Task<DailyReportResponseDTO> GetDailyReportAsync(DateTime date);
    }
}
