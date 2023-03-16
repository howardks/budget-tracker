// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BudgetTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FinancesPage : Page
    {
        private int incomeGridRows = -1, expenseGridRows = -1;

        private List<NumberBox> _incomeBoxes = new();
        public List<NumberBox> IncomeBoxes { get { return _incomeBoxes; } }

        private List<Button> _incomeButtons = new();
        public List<Button> IncomeButtons { get { return _incomeButtons; } }

        private static List<NumberBox> _expenseBoxes = new();
        public List<NumberBox> ExpenseBoxes { get { return _expenseBoxes; } }

        private List<Button> _expenseButtons = new();
        public List<Button> ExpenseButtons { get { return _expenseButtons; } }

        private static double expenses = 0;
        public double Expenses { get { return expenses; } }

        private static double income = 0;
        public double Income { get { return income; } }

        public FinancesPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            income = 0;
            foreach (NumberBox n in IncomeBoxes)
            {
                if (!n.Text.Equals(""))
                {
                    income += double.Parse(n.Text);
                }
            }

            expenses = 0;
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
            MainWindow.sPage.GenerateExpensePieChart();
            MainWindow.sPage.GenerateIncomeExpensePieChart();
        }

        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Add a new row definition to incomeGrid
            RowDefinition newRow = new();
            newRow.Height = GridLength.Auto;
            incomeGrid.RowDefinitions.Add(newRow);
            
            // Create new NumberBox with custom Header
            incomeGridRows++;
            NumberBox newBox = new();
            newBox.PlaceholderText = "0.00";
            if (incomeName.Text.Equals(""))
            {
                newBox.Header = "Income:";
            } else
            {
                newBox.Header = incomeName.Text.Trim() + ":";
                incomeName.Text = "";
            }
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
            // Remove specified NumberBox, Button, and RowDefinition
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
            // Add a new row definition to expenseGrid
            RowDefinition newRow = new();
            newRow.Height = GridLength.Auto;
            expenseGrid.RowDefinitions.Add(newRow);

            // Create new NumberBox with custom Header
            expenseGridRows++;
            NumberBox newBox = new();
            newBox.PlaceholderText = "0.00";
            if (expenseName.Text.Equals(""))
            {
                newBox.Header = "Expense:";
            }
            else
            {
                newBox.Header = expenseName.Text.Trim() + ":";
                expenseName.Text = "";
            }
            _expenseBoxes.Add(newBox);

            // Create new remove Button
            Button removeButton = new();
            removeButton.Content = "-";
            removeButton.Width = 40;
            removeButton.Height = 40;
            removeButton.Margin = new Thickness(10, 18, 0, 0);
            removeButton.BorderBrush = addExpenseButton.BorderBrush;
            removeButton.BorderThickness = addExpenseButton.BorderThickness;
            removeButton.Click += RemoveExpenseButton_Click;
            _expenseButtons.Add(removeButton);

            // Add newBox and removeButton to expenseGrid
            expenseGrid.Children.Add(newBox);
            Grid.SetRow(newBox, expenseGridRows);
            expenseGrid.Children.Add(removeButton);
            Grid.SetRow(removeButton, expenseGridRows);
            Grid.SetColumn(removeButton, 1);
        }

        private void RemoveExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove specified NumberBox, Button, and RowDefinition
            int index = _expenseButtons.IndexOf(sender as Button);
            NumberBox removedBox = _expenseBoxes[index];
            Button removedButton = _expenseButtons[index];
            expenseGrid.Children.Remove(removedBox);
            expenseGrid.Children.Remove(removedButton);
            expenseGrid.RowDefinitions.RemoveAt(index);
            _expenseBoxes.RemoveAt(index);
            _expenseButtons.RemoveAt(index);
            expenseGridRows--;

            // Reposition incomeGrid elements
            for (int i = 0; i < ExpenseBoxes.Count; i++)
            {
                Grid.SetRow(ExpenseBoxes[i], i);
                Grid.SetRow(ExpenseButtons[i], i);
                Grid.SetColumn(ExpenseButtons[i], 1);
            }
        }
    }
}
