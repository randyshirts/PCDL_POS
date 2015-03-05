using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Inventory;
using POS;
//using MvvmFoundation.Wpf;
using RocketPos.Inventory;
using RocketPos.POS;
using RocketPos.Common.Foundation;


namespace RocketPos.ViewModels
{
    public class MainPageVM
    {
        //protected RelayCommand _openPosWindowCommand;
        //protected RelayCommand _openInvWindowCommand;

        public MainPageVM()
        {


        }

        public ICommand OpenPosWindowCommand
        {
            get{return new ActionCommand(p => this.OpenPosWindow());}
     
        }
        
        public ICommand OpenInvWindowCommand
        {
            get{return new ActionCommand(p => this.OpenInvWindow());}
          
        }
        
        // Insert code required on object creation below this point.

        void OpenPosWindow()
        { 
            var newWindow = new PosWindow();
            newWindow.Show();
        }

        void OpenInvWindow()
        { 
            var newWindow = new InvWindow();
            newWindow.Show();
        }

    }
        
}