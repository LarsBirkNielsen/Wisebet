using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Wisebet.Authorization;
using Wisebet.Data;
using Wisebet.Models;
using Wisebet.Pages.Predictions;
using Microsoft.AspNetCore.Identity;

namespace Wisebet.Pages.Admin
{
    [Authorize]

    public class CreateModel : DI_BasePageModel
    {
        //We get all the dependency Injection we need by our base class ( DI_BasePageModel ) 
        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {

            var isAuthorized = User.IsInRole(Constants.PredictionAdminRole);

            //Only Admin can get to the create page
            if (isAuthorized == false)
            {
                return Forbid();
            }

            return Page();
        }

        [BindProperty]
        public Prediction Prediction { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            var isAdmin = User.IsInRole(Constants.PredictionAdminRole);

            //If you are not Admin we forbid you to post
            if (isAdmin == false)
                return Forbid();

            //Adding the invoice to the database
            Context.Prediction.Add(Prediction);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
