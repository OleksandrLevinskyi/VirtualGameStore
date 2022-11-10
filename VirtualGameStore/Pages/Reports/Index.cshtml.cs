using System.Net;
using System.Text;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VirtualGameStore.Data;
using VirtualGameStore.Models;

namespace VirtualGameStore.Pages.Reports;

[Authorize(Roles = "Employee")]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
    }

    public async Task<ActionResult> OnPostAsync([Bind] string report)
    {
        byte[] bytes = report switch
        {
            ReportTypes.GameList => await CreateReportGameList(),
            _ => Array.Empty<byte>()
        };
        return File(bytes, "application/octet-stream", report + ".xlsx");
    }

    private async Task<byte[]> CreateReportGameList()
    {
        // Prepping values. First, we need to get all the games for the report with details included.
        var games = await _context.Games
            .Include(g => g.Categories)
            .Include(g => g.Platforms)
            .ToListAsync();
        // These are the columns that the report will have, set up in this weird array
        // to make it difficult to accidentally misalign the columns with their values.
        var reportStyler = new ReportStyler<Game>(new ReportStyler<Game>.Column[]
        {
            new("ID", XLDataType.Number, g => g.Id, "0"),
            new("Name", XLDataType.Text, g => g.Name),
            new("Price", XLDataType.Number, g => g.Price, "$#,##0.00"),
            new("IsDigital", XLDataType.Boolean, g => g.IsDigital),
            new("Stock", XLDataType.Number, g => g.Stock, "#,##0"),
            new("Categories", XLDataType.Text, g => string.Join(", ", g.Categories!.Select(c => c.Name))),
            new("Platforms", XLDataType.Text, g => string.Join(", ", g.Platforms!.Select(p => p.Name))),
        });

        // Make the workbook and add the report to it.
        using var workbook = new XLWorkbook();
        var worksheet = reportStyler.AddReportAsSheet(workbook, ReportTypes.Prefix + ReportTypes.GameList, games);

        // Turning into a file...
        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }

    public struct ReportTypes
    {
        public const string Prefix = "Report - ";
        public const string GameList = "Game List";
        public const string MemberList = "Member List";
        public const string WishList = "Wish List";
        public const string Sales = "Sales";
        public const string EventList = "Event List";
    }
}