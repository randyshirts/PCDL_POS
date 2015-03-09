using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using DataModel.Data.ApplicationLayer.DTO;
using Microsoft.AspNet.Identity;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IPcdWebUserAppService : IApplicationService
    {

        //ChangeProfileImageOutput ChangeProfileImage(ChangeProfileImageInput input);

        //GetUserProfileOutput GetUserProfile(GetUserProfileInput input);

        IEnumerable<UserDto> GetAllUsers();

        UserDto GetActiveUserOrNull(string emailAddress, string password);

        GetUserOutput GetUser(GetUserInput input);

        void RegisterUser(RegisterUserInput registerUser);

        Task<IdentityResult> ConfirmEmail(ConfirmEmailInput input);

        GetCurrentUserInfoOutput GetCurrentUserInfo(GetCurrentUserInfoInput input);

        void ChangePassword(ChangePasswordInput input);

        void SendPasswordResetLink(SendPasswordResetLinkInput input);

        void ResetPassword(ResetPasswordInput input);

    }
}
