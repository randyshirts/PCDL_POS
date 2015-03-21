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
        IEnumerable<UserDto> GetAllUsers();

        UserDto GetActiveUserOrNull(string emailAddress, string password);

        GetUserOutput GetUser(GetUserInput input);

        Task<RegisterUserOutput> RegisterUser(RegisterUserInput registerUser);

        RegisterExternalUserOutput RegisterExternalUser(RegisterExternalUserInput input);
        
        ConfirmEmailOutput ConfirmEmail(ConfirmEmailInput input);

        GetCurrentUserInfoOutput GetCurrentUserInfo(GetCurrentUserInfoInput input);

        void ChangePassword(ChangePasswordInput input);

        void SendPasswordResetLink(SendPasswordResetLinkInput input);

        void ResetPassword(ResetPasswordInput input);

        Task<SendConfirmationOutput> SendConfirmation(SendConfirmationInput input);
        
        Task<UpdateRegisterUserOutput> UpdateRegisterUser(UpdateRegisterUserInput input);

        GetUserByUsernameOutput GetUserByUsername(GetUserByUsernameInput input);
    }
}
