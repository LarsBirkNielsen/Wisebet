using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Wisebet.Models;

namespace Wisebet.Pages.LeagueHistory
{
    public class Ligue1HistoryModel : PageModel
    {
        private readonly Wisebet.Data.ApplicationDbContext _context;

        public Ligue1HistoryModel(Wisebet.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Prediction> Prediction { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Prediction != null)
            {
                Prediction = await _context.Prediction.Where(x => x.League == "Ligue 1" && x.Status != "Pending").ToListAsync();
            }
        }
    }
}
