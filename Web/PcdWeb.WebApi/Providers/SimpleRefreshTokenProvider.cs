using System;
using System.Threading.Tasks;
using DataModel.Data.ApplicationLayer.Services;
using Microsoft.Owin.Security.Infrastructure;

namespace PcdWeb.Providers
{
    //public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    //{
    //    //private AuthRepository _repo = null;
    //    private readonly IPcdWebUserAppService _userAppService;
    //    public SimpleRefreshTokenProvider()
    //    {
    //        _userAppService = new PcdWebUserAppService();
    //    }

    //    public async Task CreateAsync(AuthenticationTokenCreateContext context)
    //    {
    //        var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

    //        if (string.IsNullOrEmpty(clientid))
    //        {
    //            return;
    //        }

    //        var refreshTokenId = Guid.NewGuid().ToString("n");

            
    //        var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime"); 
               
    //        var token = new RefreshToken() 
    //        { 
    //            Id = Helper.GetHash(refreshTokenId),
    //            ClientId = clientid, 
    //            Subject = context.Ticket.Identity.Name,
    //            IssuedUtc = DateTime.UtcNow,
    //            ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime)) 
    //        };

    //        context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
    //        context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;
                
    //        token.ProtectedTicket = context.SerializeTicket();

    //        var result = await _userAppService.AddRefreshToken(token);

    //        if (result)
    //        {
    //            context.SetToken(refreshTokenId);
    //        }
             
    //    }
        

    //    public class RefreshToken
    //    {
    //    }

    //public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
    //    {

    //        var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
    //        context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

    //        string hashedTokenId = Helper.GetHash(context.Token);


    //        var refreshToken = await _userAppService.FindRefreshToken(hashedTokenId);

    //        if (refreshToken != null )
    //        {
    //            //Get protectedTicket from refreshToken class
    //            context.DeserializeTicket(refreshToken.ProtectedTicket);
    //            var result = await _userAppService.RemoveRefreshToken(hashedTokenId);
    //        }
            
    //    }

    //    public void Create(AuthenticationTokenCreateContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Receive(AuthenticationTokenReceiveContext context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}