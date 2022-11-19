using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Wisebet.Data;
using Wisebet.Models;

namespace Wisebet.Pages.Predictions
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly Wisebet.Data.ApplicationDbContext _context;

        public IndexModel(Wisebet.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Prediction> Prediction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Prediction != null)
            {
                Prediction = await _context.Prediction.ToListAsync();
            }
        }
    }
}
