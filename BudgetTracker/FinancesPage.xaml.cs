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
        private int incomeGridRows = 0;

        private List<NumberBox> _incomeBoxes = new();
        public List<NumberBox> IncomeBoxes { get { return _incomeBoxes; } }

        private List<Button> _incomeButtons = new();
        public List<Button> IncomeButtons { get { return _incomeButtons; } }

        public FinancesPage()
        {
            this.InitializeComponent();
            _incomeBoxes.Add(income1);
            _incomeButtons.Add(addIncomeButton);
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

            double expenses = double.Parse(housing.Text) + double.Parse(utilities.Text) + double.Parse(food.Text) + 
                double.Parse(other1.Text) + double.Parse(other2.Text) + double.Parse(other3.Text);

            totalIncome.Text = String.Format("{0:C2}", income);
            totalExpenses.Text = String.Format("{0:C2}", expenses);
            remaining.Text = String.Format("{0:C2}", income - expenses);
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
            newBox.Header = "Income:";
            newBox.PlaceholderText = "0.00";
            _incomeBoxes.Add(newBox);

            // Create new remove Button
            Button removeButton = new Button();
            removeButton.Content = "-";
            removeButton.Width = 40;
            removeButton.Height = 40;
            removeButton.Margin = new Thickness(10,18,0,0);
            removeButton.BorderBrush = addIncomeButton.BorderBrush;
            removeButton.BorderThickness = addIncomeButton.BorderThickness;
            removeButton.Click += removeIncomeButton_Click;
            _incomeButtons.Add(removeButton);

            // Add newBox and removeButton to incomeGrid
            incomeGrid.Children.Add(newBox);
            Grid.SetRow(newBox, incomeGridRows);
            incomeGrid.Children.Add(removeButton);
            Grid.SetRow(removeButton, incomeGridRows);
            Grid.SetColumn(removeButton, 1);
        }

        private void removeIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            int index = _incomeButtons.IndexOf(sender as Button);
            NumberBox removedBox = _incomeBoxes[index];
            Button removedButton = _incomeButtons[index];
            incomeGrid.Children.Remove(removedBox);
            incomeGrid.Children.Remove(removedButton);
            incomeGrid.RowDefinitions.RemoveAt(incomeGridRows);
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
    }
}
