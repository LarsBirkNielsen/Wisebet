using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Wisebet.Models;

namespace Wisebet.Authorization
{
    public class PredictionCreatorAuthorizationHandler :
       AuthorizationHandler<OperationAuthorizationRequirement, Prediction>
    {

        UserManager<IdentityUser> _userManager;

        public PredictionCreatorAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Prediction prediction)
        {
            //Checking if no user is logged in at all OR if we have no prediction given
            if (context.User == null || prediction == null)
                return Task.CompletedTask;

            //Checking if the requirement is NOT any of the CRUD operations we specified in the predictionOperation class
            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            ////Checking if the person who created the prediction is the same as the one who is logged in
            //if (prediction.CreatorId == _userManager.GetUserId(context.User))
            //    context.Succeed(requirement);

            //If none of the above is true. 
            return Task.CompletedTask;

        }
    }
}
