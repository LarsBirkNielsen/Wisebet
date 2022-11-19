using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Wisebet.Models;

namespace Wisebet.Authorization
{
    public class PredictionAdminAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Prediction>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Prediction prediction)
        {

            if (context.User == null || prediction == null)
                return Task.CompletedTask;


            if (context.User.IsInRole(Constants.PredictionAdminRole))
                context.Succeed(requirement);

            return Task.CompletedTask;

        }
    }
}
