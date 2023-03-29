// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BudgetTracker
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static FinancesPage fPage = new FinancesPage();
        public static GoalsPage gPage = new GoalsPage();
        public static StatisticsPage sPage = new StatisticsPage();

        private ObservableCollection<NavLink> _navLinks = new()
        {
            new() { Label = "Finances", Symbol = Symbol.List, Page = fPage },
            new() { Label = "Goals", Symbol = Symbol.SolidStar, Page = gPage },
            new() { Label = "Statistics", Symbol = Symbol.FourBars, Page = sPage },
        };

        public ObservableCollection<NavLink> NavLinks
        {
            get { return _navLinks; }
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {
            pageHeader.Text = (e.ClickedItem as NavLink).Label.ToUpper();
            pageContent.Content = (e.ClickedItem as NavLink).Page;
        }

        public MainWindow()
        {
            this.InitializeComponent();
            NavLinksList.SelectedItem = NavLinks[0];
            pageHeader.Text = NavLinks[0].Label.ToUpper();
            pageContent.Content = NavLinks[0].Page;
        }
    }

    public class NavLink
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public Page Page { get; set; }
    }
}
