using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WPFUIItems;
using TestStack.White.UIItems.TableItems;
using RocketPos.Common;
using System.Windows.Controls;


namespace WhiteTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            Application application = Application.Launch(@"C:\Users\Randy\Documents\Visual Studio 2013\Projects\RocketPOS\DesktopClient\Build\Inventory.exe");
            
            //Verify window was opened
            Window window = application.GetWindow("Inventory Window");
            
            //Ensure window is active before proceeding
            window.WaitWhileBusy();

            var button = window.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("ConsignorViewButton"));
                            button.Click();

            var datagrid = window.Get<TestStack.White.UIItems.ListView>(SearchCriteria.ByAutomationId("ConsignorDataGrid"));
            var contents = datagrid.Rows;
            
        }
    }
}
