using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Wisebet.Authorization;
using Wisebet.Data;
using Wisebet.Models;
using Wisebet.Pages.Predictions;

namespace Wisebet.Pages.Admin
{
    [Authorize]

    public class DeleteModel : DI_BasePageModel
    {

        //We get all the dependency Injection we need by our base class ( DI_BasePageModel ) 
        public DeleteModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Prediction Prediction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.Prediction == null)
            {
                return NotFound();
            }

            Prediction = await Context.Prediction.FirstOrDefaultAsync(m => m.Id == id);

            if (Prediction == null)
            {
                return NotFound();
            }

            var isAuthorized = User.IsInRole(Constants.PredictionAdminRole);

            if (isAuthorized == false)
                return Forbid();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Prediction = await Context.Prediction.FindAsync(id);

            if (Prediction == null)
                return NotFound();

            var isAuthorized = User.IsInRole(Constants.PredictionAdminRole);

            if (isAuthorized == false)
                return Forbid();


            Context.Prediction.Remove(Prediction);
            await Context.SaveChangesAsync();


            return RedirectToPage("./Index");
        }
    }
}
