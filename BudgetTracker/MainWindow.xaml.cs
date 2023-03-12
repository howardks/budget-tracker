// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BudgetTracker
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private ObservableCollection<NavLink> _navLinks = new()
        {
            new() { Label = "Finances", Symbol = Symbol.List },
            new() { Label = "Goals", Symbol = Symbol.SolidStar },
            new() { Label = "Statistics", Symbol = Symbol.FourBars },
            new() { Label = "Settings", Symbol = Symbol.Setting } // Might not keep settings option
        };

        public ObservableCollection<NavLink> NavLinks
        {
            get { return _navLinks; }
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {
            pageHeader.Text = (e.ClickedItem as NavLink).Label.ToUpper();
            pageContent.Content = (e.ClickedItem as NavLink).Label + " Page";
        }

        public MainWindow()
        {
            this.InitializeComponent();
            NavLinksList.SelectedItem = NavLinks[0];
            pageHeader.Text = NavLinks[0].Label.ToUpper();
            pageContent.Content = NavLinks[0].Label + " Page";
        }
    }

    public class NavLink
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
    }
}
