using System;
using ManagementTool.ViewModels.Login;
using RocketPos.Common.Foundation;

namespace ManagementTool.RoleAdmin.ViewModels
{
    public class RoleAdminVm : ViewModel, IViewModel
    {
        public static readonly Guid Token = Guid.NewGuid();         //So others know messages came from this instance
        
        private string _roleName;
        public string RoleName {
            get { return _roleName; }
            set
            {
                _roleName = value;
                OnPropertyChanged();
            } 
        }
        
        
        private ActionCommand _addRoleCommand;
        public ActionCommand AddRoleCommand
        {
            get
            {
                return new ActionCommand(p => AddRole());
            }
        }

        private void AddRole()
        {
            //Call AddRole method in controller
        }
    }
}
