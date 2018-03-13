﻿using Syncfusion.UI.Xaml.NavigationDrawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NavigationDrawerExercise.Views
{
    /// <summary>
    /// Interaction logic for HelperBoard.xaml
    /// </summary>
    public partial class HelperBoard : UserControl
    {
        public SfNavigationDrawer NavigationDrawer { get; set; }
        public HelperBoard()
        {
            InitializeComponent();
        }
    }
}
