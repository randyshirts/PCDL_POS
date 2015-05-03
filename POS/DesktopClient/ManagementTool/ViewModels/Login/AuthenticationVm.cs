using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using ManagementTool.RoleAdmin;
using ManagementTool.RoleAdmin.Views;
using ManagementTool.Views;
using RocketPos.Common.Foundation;

namespace ManagementTool.ViewModels.Login
{
    public interface IViewModel
    {
    }

    public class AuthenticationVm : ViewModel, IViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ActionCommand _loginCommand;
        private readonly ActionCommand _logoutCommand;
        private readonly ActionCommand _showViewCommand;
       

        public AuthenticationVm(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _loginCommand = new ActionCommand(Login, CanLogin);
            _logoutCommand = new ActionCommand(Logout, CanLogout);
            _showViewCommand = new ActionCommand(ShowView, null);

            LoginEnabled = true;
            LogoutEnabled = false;
        }

        #region Properties

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return string.Format("Signed in as {0}. {1}",
                        Thread.CurrentPrincipal.Identity.Name,
                        Thread.CurrentPrincipal.IsInRole("Administrators")
                            ? "You are an administrator!"
                            : "You are NOT a member of the administrators group.");

                return "Not authenticated!";
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private bool _loginEnabled;
        public bool LoginEnabled
        {
            get { return _loginEnabled;}
            set
            {
                _loginEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _logoutEnabled;
        public bool LogoutEnabled
        {
            get { return _logoutEnabled; }
            set
            {
                _logoutEnabled = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        public ActionCommand LoginCommand
        {
            get { return _loginCommand; }
        }

        public ActionCommand LogoutCommand
        {
            get { return _logoutCommand; }
        }

        public ActionCommand ShowViewCommand
        {
            get { return _showViewCommand; }
        }

        #endregion

        private void Login(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            string clearTextPassword = passwordBox.Password;
            try
            {
                //Validate credentials through the authentication service
                User user = _authenticationService.AuthenticateUser(Username, clearTextPassword);

                //Get the current principal object
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException(
                        "The application's default thread principal must be set to a CustomPrincipal object on startup.");

                //Authenticate the user
                customPrincipal.Identity = new CustomIdentity(user.Username, user.Email, user.Roles);

                //Update UI
                OnPropertyChanged("AuthenticatedUser");
                OnPropertyChanged("IsAuthenticated");
                LoginEnabled = false;
                LogoutEnabled = true;
                //_logoutCommand.RaiseCanExecuteChanged();
                Username = string.Empty; //reset
                passwordBox.Password = string.Empty; //reset
                Status = string.Empty;
            }
            catch (UnauthorizedAccessException)
            {
                Status = "Login failed! Please provide some valid credentials.";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        private bool CanLogin(object parameter)
        {
            return !IsAuthenticated;
        }

        private void Logout(object parameter)
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
                OnPropertyChanged("AuthenticatedUser");
                OnPropertyChanged("IsAuthenticated");
                LoginEnabled = true;
                LogoutEnabled = false;
                Status = string.Empty;
            }
        }

        private bool CanLogout(object parameter)
        {
            return IsAuthenticated;
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        private void ShowView(object parameter)
        {
            try
            {
                Status = string.Empty;
                IView view;
                if (parameter == null)
                    view = new SecretWindow();
                else
                    view = new RoleAdminWindow();

                view.Show();
            }
            catch (SecurityException)
            {
                Status = "You are not authorized!";
            }
        }


        //#region INotifyPropertyChanged Members

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void NotifyPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

        //#endregion
    }
}
