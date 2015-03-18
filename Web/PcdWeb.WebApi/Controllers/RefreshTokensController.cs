using System.Threading.Tasks;
using System.Web.Http;
using DataModel.Data.ApplicationLayer.Services;

namespace PcdWeb.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController : ApiController
    {

        //private AuthRepository _repo = null;
        private readonly IPcdWebUserAppService _userAppService;
        public RefreshTokensController()
        {
            _userAppService = new PcdWebUserAppService();
        }

        //[Authorize(Users="Admin")]
        //[Route("")]
        //public IHttpActionResult Get()
        //{
        //    return Ok(_userAppService.GetAllRefreshTokens());
        //}

        ////[Authorize(Users = "Admin")]
        //[AllowAnonymous]
        //[Route("")]
        //public async Task<IHttpActionResult> Delete(string tokenId)
        //{
        //    var result = await _userAppService.RemoveRefreshToken(tokenId);
        //    if (result)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest("Token Id does not exist");
            
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    //if (disposing)
        //    //{
        //    //    _userAppService.Dispose();
        //    //}

        //    base.Dispose(disposing);
        //}
    }
}
