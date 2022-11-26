using System.Net;
using System.Text;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<User> _userManager;

    public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public void OnGet()
    {
    }

    public async Task<ActionResult> OnPostAsync([Bind] string report)
    {
        byte[] bytes = report switch
        {
            ReportTypes.GameList => await CreateReportGameList(),
            ReportTypes.MemberList => await CreateReportMemberList(),
            ReportTypes.WishList => await CreateReportWishList(),
            ReportTypes.Sales => await CreateReportSales(),
            ReportTypes.EventList => await CreateReportEventList(),
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
            .OrderBy(g => g.IsDigital)
            .ThenBy(g => g.Name)
            .ToListAsync();
        // These are the columns that the report will have, set up in this weird array
        // to make it difficult to accidentally misalign the columns with their values.
        var reportStyler = new ReportStyler<Game>(new ReportStyler<Game>.Column[]
        {
            new("Name", XLDataType.Text, g => g.Name),
            new("Price", XLDataType.Number, g => g.Price, "$#,##0.00"),
            new("Format", XLDataType.Text, g => g.IsDigital ? "Digital" : "Physical"),
            new("Items in Stock", XLDataType.Number, g => g.IsDigital ? "N/A" : g.Stock, "#,##0"),
            new("Categories", XLDataType.Text, g => string.Join(", ", g.Categories!.Select(c => c.Name).OrderBy(n => n))),
            new("Platforms", XLDataType.Text, g => string.Join(", ", g.Platforms!.Select(p => p.Name).OrderBy(n => n))),
        });

        // Make the workbook and add the report to it.
        using var workbook = new XLWorkbook();
        var worksheet = reportStyler.AddReportAsSheet(workbook, ReportTypes.Prefix + ReportTypes.GameList, games);

        // Turning into a file...
        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }

    private async Task<byte[]> CreateReportMemberList()
    {
        var userIdsWithMember = (await _userManager.GetUsersInRoleAsync("Member")).Select(u => u.Id);
        var members = await _context.Users
            .Where(u => userIdsWithMember.Contains(u.Id))
            .Include(u => u.Orders)
            .ThenInclude(o => o.Items)
            .ThenInclude(i => i.Game)
            .OrderBy(u => u.UserName)
            .ToListAsync();
        var reportStyler = new ReportStyler<User>(new ReportStyler<User>.Column[]
        {
            new("User Name", XLDataType.Text, u => u.UserName),
            new("Full Name", XLDataType.Text, u => FullName(u.FirstName, u.LastName)),
            new("Receive Promo Emails", XLDataType.Text, u => u.IsEmailMarketingEnabled ? "YES" : "NO"),
            new("No. Games Owned", XLDataType.Number,
                u => u.Orders.Sum(o => o.Items.Sum(i => i.Game.IsDigital ? 1 : i.Quantity)),"#,##0")
        });

        // Make the workbook and add the report to it.
        using var workbook = new XLWorkbook();
        var worksheet = reportStyler.AddReportAsSheet(workbook, ReportTypes.Prefix + ReportTypes.MemberList, members);

        // Turning into a file...
        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }

    private async Task<byte[]> CreateReportWishList()
    {
        var memberRole = await _context.Roles.FirstOrDefaultAsync(i => i.Name == "Member");
        var userIdsWithMember = (await _userManager.GetUsersInRoleAsync("Member")).Select(u => u.Id);
        var members = await _context.Users
            .Where(u => userIdsWithMember.Contains(u.Id))
            .Include(u => u.WishList)
            .OrderBy(u => u.UserName)
            .ToListAsync();
        var reportStyler = new ReportStyler<User>(new ReportStyler<User>.Column[]
        {
            new("User Name", XLDataType.Text, u => u.UserName),
            new("No. Games in Wishlist", XLDataType.Number, u => u.WishList.Count, "#,##0"),
            new("Wishlist Value", XLDataType.Number, u => u.WishList.Sum(g => g.Price), "$#,##0.00"),
        });

        // Make the workbook and add the report to it.
        using var workbook = new XLWorkbook();
        var worksheet = reportStyler.AddReportAsSheet(workbook, ReportTypes.Prefix + ReportTypes.WishList, members);

        // Turning into a file...
        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }

    private async Task<byte[]> CreateReportSales()
    {
        var sales = (await _context.OrderItems
                .Include(i => i.Order)
                .Where(i => i.Order.StatusId == 2) // This will no longer be a magic number after we fix status
                .Include(i => i.Game)
                .ThenInclude(g => g.Categories)
                .Include(i => i.Game)
                .ThenInclude(g => g.Platforms)
                .ToListAsync())
            .GroupBy(i => i.Game)
            .OrderBy(g => g.Key.Name)
            .ToList();

        var reportStyler = new ReportStyler<IGrouping<Game, OrderItem>>(
            new ReportStyler<IGrouping<Game, OrderItem>>.Column[]
            {
                new("Game", XLDataType.Text, s => s.Key.Name),
                new("Categories", XLDataType.Text, s => string.Join(", ", s.Key.Categories!.Select(c => c.Name).OrderBy(n => n))),
                new("Platforms", XLDataType.Text, s => string.Join(", ", s.Key.Platforms!.Select(p => p.Name).OrderBy(n => n))),
                new("Quantity Sold", XLDataType.Number, s => s.Sum(i => i.Quantity), "#,##0"),
                new("Total", XLDataType.Number, s => s.Sum(i => i.Total), "$#,##0.00"),
            });

        // Make the workbook and add the report to it.
        using var workbook = new XLWorkbook();
        var worksheet = reportStyler.AddReportAsSheet(workbook, ReportTypes.Prefix + ReportTypes.Sales, sales);

        // Turning into a file...
        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }

    private async Task<byte[]> CreateReportEventList()
    {
        var events = await _context.Events
            .Include(e => e.Registrations)
            .OrderByDescending(e => e.DateTime)
            .ToListAsync();
        var reportStyler = new ReportStyler<Event>(new ReportStyler<Event>.Column[]
        {
            new("Name", XLDataType.Text, e => e.Name),
            new("Date", XLDataType.DateTime, e => e.DateTime.Date),
            new("Time", XLDataType.DateTime, e => e.DateTime.TimeOfDay, "H:mm"),
            new("Registrations", XLDataType.Number, e => e.Registrations.Count, "#,##0"),
        });
        // Make the workbook and add the report to it.
        using var workbook = new XLWorkbook();
        var worksheet = reportStyler.AddReportAsSheet(workbook, ReportTypes.Prefix + ReportTypes.EventList, events);

        // Turning into a file...
        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }

    private static string FullName(string? firstName, string? lastName)
    {
        if (string.IsNullOrEmpty(firstName))
            return string.IsNullOrEmpty(lastName) ? "" : lastName;
        else if (string.IsNullOrEmpty(lastName))
            return firstName;
        else
            return $"{lastName}, {firstName}";
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