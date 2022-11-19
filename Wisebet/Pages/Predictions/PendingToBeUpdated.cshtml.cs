using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Wisebet.Models;

namespace Wisebet.Pages.Predictions
{
    public class PendingModelToBeUpdated : PageModel
    {
        private readonly Wisebet.Data.ApplicationDbContext _context;

        public PendingModelToBeUpdated(Wisebet.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Prediction> Prediction { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Prediction != null)
            {
                Prediction = await _context.Prediction.Where(x => x.KickOff < DateTime.Now && x.Status == "Pending").ToListAsync();
            }
        }
    }
}
