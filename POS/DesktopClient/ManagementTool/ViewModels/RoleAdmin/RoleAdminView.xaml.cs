using System.Security.Permissions;
using System.Windows;
using ManagementTool.ViewModels.Login;
using ManagementTool.Views;

namespace ManagementTool.ViewModels.RoleAdmin
{
    /// <summary>
    /// Interaction logic for RoleAdminView.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
    public partial class RoleAdminView : Window, IView
    {
        public RoleAdminView()
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
