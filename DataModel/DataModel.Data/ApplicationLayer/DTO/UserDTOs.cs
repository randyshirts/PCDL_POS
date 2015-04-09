using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DataModel.Data.DataLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace DataModel.Data.ApplicationLayer.DTO
{
    public class UserDto 
    {
        public UserDto(User user)
        {
            //Id = Int32.Parse(user.Id);
            Id = user.Id;
            Name = user.UserName;
            EmailAddress = user.Email;
            Profile = user.PersonUser;
            PhoneNumber = user.PhoneNumber;
            PasswordResetCode = user.PasswordResetCode;
        }

        public string Id { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email address of the user.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Phone Number of the user.
        /// </summary>
        public string PhoneNumber { get; set; }

        public string PasswordResetCode { get; set; }
        /// <summary>
        /// Profile of the user.
        /// </summary>
        public Person Profile { get; set; }

        public User ConvertToUser()
        {            
            return new User
            {
                Id = Id,
                UserName = Name,
                PersonUser = Profile,
                Email = EmailAddress,
                PhoneNumber = PhoneNumber,
                PasswordResetCode = PasswordResetCode
            };
        }
    }

    public class ConfirmEmailInput
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ConfirmationCode { get; set; }
    }

    public class ChangePasswordInput : IInputDto
    {
        [Required]
        [StringLength(30, MinimumLength = 3)] //TODO: Avoid Magic numbers!
        public virtual string CurrentPassword { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public virtual string NewPassword { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }

    public class ChangePasswordOutput : IOutputDto
    {
        public bool Result { get; set; }
    }

    public class GetCurrentUserInfoInput : IInputDto
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }

    public class GetCurrentUserInfoOutput : IOutputDto
    {
        public UserDto User { get; set; }
    }

    public class GetUserInput : IInputDto
    {
        public string UserId { get; set; }
    }

    public class GetUserOutput : IOutputDto
    {
        public UserDto User { get; set; }
    }

    public class UpdateUserInput : IOutputDto
    {
        public string CurrentEmail { get; set; }
        public string NewEmail { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UpdateUserOutput : IOutputDto
    {
        public bool Result { get; set; }
    }

    public class GetUserProfileInput : IInputDto
    {
        [Range(1, int.MaxValue)]
        public long UserId { get; set; }
    }

    public class GetUserProfileOutput : IOutputDto
    {
        public bool CanNotSeeTheProfile { get; set; }

        public bool SentFriendshipRequest { get; set; }

        public UserDto User { get; set; }
    }

    public class RegisterUserInput : IInputDto
    {
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        [Phone]
        public string Phone { get; set; }

        //[Required]
        //[StringLength(30)]
        //public string UserName { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string Password { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Compare("Password", ErrorMessage = "Passwords do no match!")]
        public string PasswordRepeat { get; set; }

        //public string ProfileImage { get; set; }
    }

    public class RegisterUserOutput : IOutputDto
    {
        public IdentityResult Result { get; set; }
    }

    public class UpdateRegisterUserOutput : IOutputDto
    {
        public bool Result { get; set; }
    }
    
    public class UpdateRegisterUserInput : IInputDto
    {
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string Password { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Compare("Password", ErrorMessage = "Passwords do no match!")]
        public string PasswordRepeat { get; set; }

        [Required]
        public PersonDto Person { get; set; }
    }

    public class RegisterExternalUserInput : IInputDto
    {
        public User User { get; set; }
    }

    public class RegisterExternalUserOutput : IOutputDto
    {
        public IdentityResult Result { get; set; }
    }

    public class ConfirmEmailOutput : IOutputDto
    {
        public bool Result { get; set; }
    }

    public class SendConfirmationOutput : IOutputDto
    {
        public bool Result { get; set; }    
    }

    public class SendConfirmationInput : IInputDto
    {
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string EmailAddress { get; set; }

    }

    public class ResetPasswordInput : IInputDto
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string Password { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Compare("Password", ErrorMessage = "Passwords do no match!")]
        public string PasswordRepeat { get; set; }

        public string PasswordResetCode { get; set; }
    }

    public class SendPasswordResetLinkInput : IInputDto
    {   
        public string EmailAddress { get; set; }
    }

    public class SendPasswordResetLinkOutput : IOutputDto
    {
        public bool Result { get; set; }
    }

    public class VerifyResetCodeInput : IInputDto
    {
        public string UserId { get; set; }
        public string ResetCode { get; set; }
    }

    public class VerifyResetCodeOutput : IOutputDto
    {
        public bool Result { get; set; }
    }

    public class ChangeForgotPasswordInput : IInputDto
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class ChangeForgotPasswordOutput : IOutputDto
    {
        public bool Result { get; set; }    
    }

    public class GenerateUserIdentityAsyncOutput : IOutputDto
    {
        public IdentityResult Result { get; set; }
    }

    public class GenerateUserIdentityAsyncInput : IInputDto
    {
        public UserDto User { get; set; }
    }

    public class AddUserToPersonOutput : IOutputDto
    {
        public bool Result { get; set; }
    }

    public class AddUserToPersonInput : IInputDto
    {
        public int PersonId { get; set; }
        public User User { get; set; }
    }

    public class GetUserByUsernameOutput : IOutputDto
    {
        public UserDto User { get; set; }
    }

    public class GetUserByUsernameInput : IInputDto
    {
        public string Email { get; set; } 
    }

    public class LoginWithFormOutput : IOutputDto
    {
        public SignInStatus Result { get; set; }
    }

    public class LoginWithFormInput : IInputDto
    {
        public string Email { get; set; }
        public string Password { get; set; } 
        public bool RememberMe { get; set; }
        public bool ShouldLockout { get; set; }
    }

    public static class UserDtosMapper
    {
        public static void Map()
        {
            //AutoMapper.Mapper.CreateMap<User, UserDto>()
            //    .ForMember(
            //        user => user.ProfileImage,
            //        configuration => configuration.ResolveUsing(
            //            user => user.ProfileImage == null
            //                //TODO: How to implement this?
            //                        ? ""
            //                        : "ProfileImages/" + user.ProfileImage
            //                             )
            //    ).ReverseMap();

            //AutoMapper.Mapper.CreateMap<RegisterUserInput, User>();

            //AutoMapper.Mapper.CreateMap<User, UserDto>().ReverseMap();

            //AutoMapper.Mapper.CreateMap<RegisterUserInput, User>();
        }


    }
}
