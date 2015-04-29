using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagementTool.ViewModels.Login;
using RocketPos.Common.Foundation;

namespace ManagementTool.ViewModels.RoleAdmin
{
    public class RoleAdminVm : ViewModel, IViewModel
    {
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
