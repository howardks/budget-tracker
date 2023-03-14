// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BudgetTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FinancesPage : Page
    {
        private List<NumberBox> _incomeBoxes = new List<NumberBox>();
        private int incomeGridRows = 0;

        public List<NumberBox> IncomeBoxes
        {
            get { return _incomeBoxes; }
        }

        public FinancesPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double income = double.Parse(income1.Text);// + double.Parse(income2.Text);
            double expenses = double.Parse(housing.Text) + double.Parse(utilities.Text) + double.Parse(food.Text) + 
                double.Parse(other1.Text) + double.Parse(other2.Text) + double.Parse(other3.Text);

            totalIncome.Text = "$" + income;
            totalExpenses.Text = "$" + expenses;
            remaining.Text = "$" + (income - expenses);
        }

        private void addIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Add a new row definition to incomeGrid
            RowDefinition newRow = new RowDefinition();
            newRow.Height = GridLength.Auto;
            incomeGrid.RowDefinitions.Add(newRow);
            
            // Create new NumberBox
            incomeGridRows++;
            NumberBox newBox = new NumberBox();
            newBox.Header = "Income " + (incomeGridRows + 1) + ":";

            // Add newBox to incomeGrid
            incomeGrid.Children.Add(newBox);
            Grid.SetRow(newBox, incomeGridRows);
        }

        private void removeIncomeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
