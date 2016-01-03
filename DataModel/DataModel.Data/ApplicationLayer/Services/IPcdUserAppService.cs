using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using DataModel.Data.ApplicationLayer.DTO;

namespace DataModel.Data.ApplicationLayer.Services
{
    public interface IPcdUserAppService : IApplicationService
    {
        IEnumerable<UserDto> GetAllUsers();

        UserDto GetActiveUserOrNull(string emailAddress, string password);

        GetUserOutput GetUser(GetUserInput input);

        Task<RegisterUserOutput> RegisterUser(RegisterUserInput registerUser);

        RegisterExternalUserOutput RegisterExternalUser(RegisterExternalUserInput input);
        
        ConfirmEmailOutput ConfirmEmail(ConfirmEmailInput input);

        GetCurrentUserInfoOutput GetCurrentUserInfo(GetCurrentUserInfoInput input);

        Task<ChangePasswordOutput> ChangePassword(ChangePasswordInput input);

        Task<SendPasswordResetLinkOutput> SendPasswordResetLink(SendPasswordResetLinkInput input);

        Task<SendConfirmationOutput> SendConfirmation(SendConfirmationInput input);
        
        Task<UpdateRegisterUserOutput> UpdateRegisterUser(UpdateRegisterUserInput input);

        GetUserByUsernameOutput GetUserByUsername(GetUserByUsernameInput input);

        Task<LoginWithFormOutput> LoginWithForm(LoginWithFormInput input);

        Task<UpdateUserOutput> UpdateUser(UpdateUserInput input);

        Task<VerifyResetCodeOutput> VerifyResetCode(VerifyResetCodeInput input);

        Task<ChangeForgotPasswordOutput> ChangeForgotPassword(ChangeForgotPasswordInput input);
    }
}
