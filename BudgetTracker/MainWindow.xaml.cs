// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace BudgetTracker
{
    public sealed partial class MainWindow : Window
    {
        // Pages for populating Main Panel
        public static FinancesPage fPage = new();
        public static GoalsPage gPage = new();
        public static StatisticsPage sPage = new();

        // Collection for populating Side Panel
        private ObservableCollection<NavLink> _navLinks = new()
        {
            new() { Label = "Finances", Symbol = Symbol.List, Page = fPage },
            new() { Label = "Goals", Symbol = Symbol.SolidStar, Page = gPage },
            new() { Label = "Statistics", Symbol = Symbol.FourBars, Page = sPage },
        };
        public ObservableCollection<NavLink> NavLinks { get { return _navLinks; } }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Populate main panel based on side panel item clicked
            pageHeader.Text = (e.ClickedItem as NavLink).Label.ToUpper();
            pageContent.Content = (e.ClickedItem as NavLink).Page;
        }

        public MainWindow()
        {
            this.InitializeComponent();

            // Show first page on application opening
            NavLinksList.SelectedItem = NavLinks[0];
            pageHeader.Text = NavLinks[0].Label.ToUpper();
            pageContent.Content = NavLinks[0].Page;
        }
    }

    // Nested class which allows navigation links to display properly using ListView.ItemTemplate
    public class NavLink
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public Page Page { get; set; }
    }
}
