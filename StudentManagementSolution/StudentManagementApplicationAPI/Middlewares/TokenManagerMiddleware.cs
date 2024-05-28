using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudentManagementApplicationAPI.Interfaces.Service.TokenService;


namespace StudentManagementApplicationAPI.Middlewares
{
    //public class TokenManagerMiddleware : IMiddleware
    //{
    //    private readonly List<Claim> _logoutClaims = new List<Claim>();

    //    public TokenManagerMiddleware()
    //    {

    //    }

    //    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    //    {

    //        if (context.User.Claims == null || (context.User.Claims != null && !IsLoggedOut(context.User.Claims)))
    //        {
    //            await next(context);
    //        }
    //        else
    //        {
    //            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    //        }
    //    }

    //    private bool IsLoggedOut(IEnumerable<Claim> currentClaims)
    //    {
    //        foreach (var currentClaim in currentClaims)
    //        {
    //            if (currentClaim != null && _logoutClaims.Any(claim => claim.Type == currentClaim.Type && claim.Value == currentClaim.Value))
    //            {
    //                Debug.WriteLine("okay");
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

    //    public void AddLoggedOutClaim(Claim claim)
    //    {
    //        _logoutClaims.Add(claim);
    //    }
    //}

}
