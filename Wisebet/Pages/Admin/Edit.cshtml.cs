using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wisebet.Authorization;
using Wisebet.Data;
using Wisebet.Models;
using Wisebet.Pages.Predictions;

namespace Wisebet.Pages.Admin
{
    [Authorize]

    public class EditModel : DI_BasePageModel
    {
        //We get all the dependency Injection we need by our base class ( DI_BasePageModel ) 
        public EditModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Prediction Prediction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Prediction = await Context.Prediction.FirstOrDefaultAsync(m => m.Id == id);

            if (Prediction == null)
            {
                return NotFound();
            }


            //This way we cant edit invoices that we haven't created our self
            var isAuthorized = User.IsInRole(Constants.PredictionAdminRole);

            if (isAuthorized == false)
                return Forbid();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {

            //We get the invoice with the id
            var prediction = await Context.Prediction.AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);

            if (prediction == null)
                return NotFound();



            //If we are authorized we are able to edit it
            var isAuthorized = User.IsInRole(Constants.PredictionAdminRole);

            if (isAuthorized == false)
                return Forbid();


            Context.Attach(Prediction).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(Prediction.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InvoiceExists(int id)
        {
            return Context.Prediction.Any(e => e.Id == id);
        }
    }
}
