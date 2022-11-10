using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VirtualGameStore.Data;

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
        return File(Encoding.ASCII.GetBytes("potato"), "text/csv");
    }
}