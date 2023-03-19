// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BudgetTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FinancesPage : Page
    {
        // Variables for tracking the newest grid row location
        private int incomeGridRows = -1, expenseGridRows = -1;

        // Variables for tracking expenses and income
        private static double expenses = 0;
        public double Expenses { get { return expenses; } }
        private static double income = 0;
        public double Income { get { return income; } }

        // Dictionary of expenses for sharing with Statistics page
        private static Dictionary<string, double> _expenseValues = new();
        public Dictionary<string, double> ExpenseValues { get { return _expenseValues; } }

        // List for schedule drop down buttons contents
        private List<string> _scheduleList = new()
        {
            "Once", "Weekly", "Bi-monthly", "Monthly"
        };

        // Lists for income rows
        private List<NumberBox> _incomeBoxes = new();
        public List<NumberBox> IncomeBoxes { get { return _incomeBoxes; } }
        private List<Button> _incomeButtons = new();
        public List<Button> IncomeButtons { get { return _incomeButtons; } }
        private List<DropDownButton> _incomeSchedules = new();

        // Lists for expense rows
        private List<NumberBox> _expenseBoxes = new();
        public List<NumberBox> ExpenseBoxes { get { return _expenseBoxes; } }

        private List<Button> _expenseButtons = new();
        public List<Button> ExpenseButtons { get { return _expenseButtons; } }
        private List<DropDownButton> _expenseSchedules = new();

        public FinancesPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            income = 0;
            for (int i = 0; i < IncomeBoxes.Count; i++)
            {
                if (!IncomeBoxes[i].Text.Equals(""))
                {
                    switch (_incomeSchedules[i].Content.ToString())
                    {
                        case "Weekly":
                            income += 4 * double.Parse(IncomeBoxes[i].Text);
                            break;
                        case "Bi-monthly":
                            income += 2 * double.Parse(IncomeBoxes[i].Text);
                            break;
                        case "Once":
                        case "Monthly":
                        default:
                            income += double.Parse(IncomeBoxes[i].Text);
                            break;
                    }
                }
            }

            expenses = 0;
            _expenseValues.Clear();
            for (int i = 0; i < ExpenseBoxes.Count; i++)
            {
                if (!ExpenseBoxes[i].Text.Equals(""))
                {
                    double expenseVal = 0;
                    switch (_expenseSchedules[i].Content.ToString())
                    {
                        case "Weekly":
                            expenseVal = 4 * double.Parse(ExpenseBoxes[i].Text);
                            break;
                        case "Bi-monthly":
                            expenseVal = 2 * double.Parse(ExpenseBoxes[i].Text);
                            break;
                        case "Once":
                        case "Monthly":
                        default:
                            expenseVal = double.Parse(ExpenseBoxes[i].Text);
                            break;
                    }
                    expenses += expenseVal;
                    _expenseValues.Add(ExpenseBoxes[i].Header.ToString(), expenseVal);
                }
            }

            totalIncome.Text = String.Format("{0:C2}", income);
            totalExpenses.Text = String.Format("{0:C2}", expenses);
            remaining.Text = String.Format("{0:C2}", income - expenses);
            MainWindow.sPage.GenerateExpensePieChart();
            MainWindow.sPage.GenerateFundsPieChart();
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

            // Create new schedule DropDownButton
            ListView lv = new() { ItemsSource = _scheduleList };
            Flyout sFlyout = new() { Content = lv };
            DropDownButton scheduleButton = new DropDownButton();
            scheduleButton.Content = "Schedule";
            scheduleButton.Flyout = sFlyout;
            scheduleButton.Height = 40;
            scheduleButton.Width = 124;
            scheduleButton.Margin = new Thickness(10, 18, 0, 0);
            scheduleButton.BorderBrush = addIncomeButton.BorderBrush;
            scheduleButton.BorderThickness = addIncomeButton.BorderThickness;
            scheduleButton.HorizontalAlignment = HorizontalAlignment.Center;
            _incomeSchedules.Add(scheduleButton);

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
            incomeGrid.Children.Add(scheduleButton);
            Grid.SetRow(scheduleButton, incomeGridRows);
            Grid.SetColumn(scheduleButton, 1);
            incomeGrid.Children.Add(removeButton);
            Grid.SetRow(removeButton, incomeGridRows);
            Grid.SetColumn(removeButton, 2);
            
            lv.SelectionChanged += (o, args) =>
                {
                    Lv_SelectionChanged(lv, scheduleButton);
                };
        }

        private void Lv_SelectionChanged(ListView sender, Button ddb)
        {
            ddb.Content = sender.SelectedItem.ToString();
        }

        private void RemoveIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove specified NumberBox, Button, and RowDefinition
            int index = _incomeButtons.IndexOf(sender as Button);
            NumberBox removedBox = _incomeBoxes[index];
            Button removedButton = _incomeButtons[index];
            DropDownButton removedSchedule = _incomeSchedules[index];
            incomeGrid.Children.Remove(removedBox);
            incomeGrid.Children.Remove(removedButton);
            incomeGrid.Children.Remove(removedSchedule);
            incomeGrid.RowDefinitions.RemoveAt(index);
            _incomeBoxes.RemoveAt(index);
            _incomeButtons.RemoveAt(index);
            _incomeSchedules.RemoveAt(index);
            incomeGridRows--;
            
            // Reposition incomeGrid elements
            for (int i = 0; i < IncomeBoxes.Count; i++)
            {
                Grid.SetRow(IncomeBoxes[i], i);
                Grid.SetRow(_incomeSchedules[i], i);
                Grid.SetColumn(_incomeSchedules[i], 1);
                Grid.SetRow(IncomeButtons[i], i);
                Grid.SetColumn(IncomeButtons[i], 2);
            }
        }

        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
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

            // Create new schedule DropDownButton
            ListView lv = new() { ItemsSource = _scheduleList };
            Flyout sFlyout = new() { Content = lv };
            DropDownButton scheduleButton = new DropDownButton();
            scheduleButton.Content = "Schedule";
            scheduleButton.Flyout = sFlyout;
            scheduleButton.Height = 40;
            scheduleButton.Width = 124;
            scheduleButton.Margin = new Thickness(10, 18, 0, 0);
            scheduleButton.BorderBrush = addIncomeButton.BorderBrush;
            scheduleButton.BorderThickness = addIncomeButton.BorderThickness;
            scheduleButton.HorizontalAlignment = HorizontalAlignment.Center;
            _expenseSchedules.Add(scheduleButton);

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

            // Add newBox and removeButton to incomeGrid
            expenseGrid.Children.Add(newBox);
            Grid.SetRow(newBox, expenseGridRows);
            expenseGrid.Children.Add(scheduleButton);
            Grid.SetRow(scheduleButton, expenseGridRows);
            Grid.SetColumn(scheduleButton, 1);
            expenseGrid.Children.Add(removeButton);
            Grid.SetRow(removeButton, expenseGridRows);
            Grid.SetColumn(removeButton, 2);

            lv.SelectionChanged += (o, args) =>
            {
                Lv_SelectionChanged(lv, scheduleButton);
            };
        }

        private void RemoveExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove specified NumberBox, Button, and RowDefinition
            int index = _expenseButtons.IndexOf(sender as Button);
            NumberBox removedBox = _expenseBoxes[index];
            Button removedButton = _expenseButtons[index];
            DropDownButton removedSchedule = _expenseSchedules[index];
            expenseGrid.Children.Remove(removedBox);
            expenseGrid.Children.Remove(removedButton);
            expenseGrid.Children.Remove(removedSchedule);
            expenseGrid.RowDefinitions.RemoveAt(index);
            _expenseBoxes.RemoveAt(index);
            _expenseButtons.RemoveAt(index);
            _expenseSchedules.RemoveAt(index);
            expenseGridRows--;

            // Reposition incomeGrid elements
            for (int i = 0; i < ExpenseBoxes.Count; i++)
            {
                Grid.SetRow(ExpenseBoxes[i], i);
                Grid.SetRow(_expenseSchedules[i], i);
                Grid.SetColumn(_expenseButtons[i], 1);
                Grid.SetRow(ExpenseButtons[i], i);
                Grid.SetColumn(ExpenseButtons[i], 2);
            }
        }
    }
}
