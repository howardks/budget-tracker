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
        private int incomeGridRows = -1;

        private List<NumberBox> _incomeBoxes = new();
        public List<NumberBox> IncomeBoxes { get { return _incomeBoxes; } }

        private List<Button> _incomeButtons = new();
        public List<Button> IncomeButtons { get { return _incomeButtons; } }

        private List<NumberBox> _expenseBoxes = new();
        public List<NumberBox> ExpenseBoxes { get { return _expenseBoxes; } }

        private List<Button> _expenseButtons = new();
        public List<Button> ExpenseButtons { get { return _expenseButtons; } }

        public FinancesPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double income = 0;
            foreach (NumberBox n in IncomeBoxes)
            {
                if (!n.Text.Equals(""))
                {
                    income += double.Parse(n.Text);
                }
            }

            double expenses = 0;
            foreach (NumberBox n in ExpenseBoxes)
            {
                if (!n.Text.Equals(""))
                {
                    expenses += double.Parse(n.Text);
                }
            }

            totalIncome.Text = String.Format("{0:C2}", income);
            totalExpenses.Text = String.Format("{0:C2}", expenses);
            remaining.Text = String.Format("{0:C2}", income - expenses);
        }

        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Add a new row definition to incomeGrid
            RowDefinition newRow = new();
            newRow.Height = GridLength.Auto;
            incomeGrid.RowDefinitions.Add(newRow);
            
            // Create new NumberBox
            incomeGridRows++;
            NumberBox newBox = new();
            if (incomeName.Text.Equals(""))
            {
                newBox.Header = "Income:";
            } else
            {
                newBox.Header = incomeName.Text.Trim() + ":";
                incomeName.Text = "";
            }
            
            newBox.PlaceholderText = "0.00";
            _incomeBoxes.Add(newBox);

            // Create new remove Button
            Button removeButton = new();
            removeButton.Content = "-";
            removeButton.Width = 40;
            removeButton.Height = 40;
            removeButton.Margin = new Thickness(10,18,0,0);
            removeButton.BorderBrush = addIncomeButton.BorderBrush;
            removeButton.BorderThickness = addIncomeButton.BorderThickness;
            removeButton.Click += RemoveIncomeButton_Click;
            _incomeButtons.Add(removeButton);

            // Add newBox and removeButton to incomeGrid
            incomeGrid.Children.Add(newBox);
            Grid.SetRow(newBox, incomeGridRows);
            incomeGrid.Children.Add(removeButton);
            Grid.SetRow(removeButton, incomeGridRows);
            Grid.SetColumn(removeButton, 1);
        }

        private void RemoveIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            int index = _incomeButtons.IndexOf(sender as Button);
            NumberBox removedBox = _incomeBoxes[index];
            Button removedButton = _incomeButtons[index];
            incomeGrid.Children.Remove(removedBox);
            incomeGrid.Children.Remove(removedButton);
            incomeGrid.RowDefinitions.RemoveAt(index);
            _incomeBoxes.RemoveAt(index);
            _incomeButtons.RemoveAt(index);
            incomeGridRows--;
            
            // Reposition incomeGrid elements
            for (int i = 0; i < IncomeBoxes.Count; i++)
            {
                Grid.SetRow(IncomeBoxes[i], i);
                Grid.SetRow(IncomeButtons[i], i);
                Grid.SetColumn(IncomeButtons[i], 1);
            }
        }

        private void addExpenseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveExpenseButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
