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
    public class DetailsModel : PageModel
    {
        private readonly Wisebet.Data.ApplicationDbContext _context;

        public DetailsModel(Wisebet.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Prediction Prediction { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Prediction == null)
            {
                return NotFound();
            }

            var prediction = await _context.Prediction.FirstOrDefaultAsync(m => m.Id == id);
            if (prediction == null)
            {
                return NotFound();
            }
            else 
            {
                Prediction = prediction;
            }
            return Page();
        }
    }
}
