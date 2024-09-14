using Ordernary.Data;
using Ordernary.Services.ServiceInterfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Ordernary.Models.DTOs;

namespace Ordernary.Services
{
    public class DailyReportService : IDailyReportService
    {
        private readonly Context _context;

        public DailyReportService(Context context)
        {
            _context = context;
        }

        // Method to get daily report with total quantity, price, and article-level details
        private async Task<DailyReportResponseDTO> GetDailyReportAsync(DateTime date)
        {
            var articleReport = await _context.OrderArticles
                .Where(oa => oa.Order.CreatedAt.Date == date.Date)
                .GroupBy(oa => new { oa.ArticleId, oa.Article.Name })
                .Select(g => new DailyReportDTO
                {
                    ArticleName = g.Key.Name,
                    TotalQuantity = g.Sum(oa => oa.Quantity),
                    TotalPrice = g.Sum(oa => oa.Quantity * oa.Article.Price)
                })
                .ToListAsync();

            var totalEarnings = articleReport.Sum(r => r.TotalPrice);

            return new DailyReportResponseDTO
            {
                ArticleReport = articleReport,
                TotalEarnings = totalEarnings
            };
        }

        public async Task SendDailyReport()
        {
            var today = DateTime.UtcNow.Date;

            // Retrieve daily report from repository
            var dailyReport = await GetDailyReportAsync(today);

            // Format the report as an HTML table
            string reportHtml = FormatReportAsHtml(dailyReport, today);

            // Create a temporary .doc file for the report (if still needed for some purposes)
            string filePath = CreateDocFile(reportHtml, today);

            // Send the report via email
            await SendEmailWithAttachmentAsync("steamzamenabycomi@gmail.com", "Daily Report", reportHtml, filePath);
        }

        // Method to format the report as an HTML table
        private string FormatReportAsHtml(DailyReportResponseDTO report, DateTime date)
        {
            var sb = new StringBuilder();

            // Add HTML table structure
            sb.AppendLine("<html><body>");
            sb.AppendLine($"<h1>Daily Report for {date.ToShortDateString()}</h1>");
            sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
            sb.AppendLine("<tr><th>Article Name</th><th>Total Quantity</th><th>Total Price</th></tr>");

            foreach (var article in report.ArticleReport)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{article.ArticleName}</td>");
                sb.AppendLine($"<td>{article.TotalQuantity}</td>");
                sb.AppendLine($"<td>{article.TotalPrice:C}</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine($"<h2>Total Earnings: {report.TotalEarnings:C}</h2>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }

        // Same CreateDocFile method
        private string CreateDocFile(string content, DateTime date)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");
            string filePath = Path.Combine(Path.GetTempPath(), $"DailyReport_{formattedDate}.doc");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(content);
            }

            return filePath;
        }

        private async Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentFilePath)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("steamzamenabycomi@gmail.com", "cmmt cgwt qino ebam"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("steamzamenabycomi@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true, // Enable HTML content for the table
            };
            mailMessage.To.Add(to);

            // Add attachment if necessary
            if (File.Exists(attachmentFilePath))
            {
                var attachment = new Attachment(attachmentFilePath);
                mailMessage.Attachments.Add(attachment);
            }

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            finally
            {
                // Clean up
                foreach (var attachment in mailMessage.Attachments)
                {
                    attachment.Dispose();
                }
                if (File.Exists(attachmentFilePath))
                {
                    File.Delete(attachmentFilePath); // Delete the temporary file after sending the email
                }
            }
        }
    }
}
