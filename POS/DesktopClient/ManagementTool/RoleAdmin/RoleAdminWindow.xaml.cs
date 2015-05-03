using System.Security.Permissions;
using System.Windows;
using ManagementTool.ViewModels.Login;
using ManagementTool.Views;

namespace ManagementTool.RoleAdmin
{
    /// <summary>
    /// Interaction logic for RoleAdminWindow.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
    public partial class RoleAdminWindow : Window, IView
    {
        public RoleAdminWindow()
        {
            InitializeComponent();
        }

        #region IView Members
        public IViewModel ViewModel
        {
            get
            {
                return DataContext as IViewModel;
            }
            set
            {
                DataContext = value;
            }
        }
        #endregion
    }
}
