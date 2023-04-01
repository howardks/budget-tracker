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

namespace BudgetTracker
{
    public sealed partial class GoalsPage : Page
    {
        // Variables for tracking the newest grid row location
        private int goalGridRows = -1;

        // Variables for tracking goals and savings
        private static double goal = 0;
        public double Goal { get { return goal; } }
        private static double savings = 0;
        public double Savings { get { return savings; } }

        // Lists for grid row nodes
        private List<NumberBox> _goalBoxes = new();
        public List<NumberBox> GoalBoxes { get { return _goalBoxes; } }
        private List<NumberBox> _savingsBoxes = new();
        public List<NumberBox> SavingsBoxes { get { return _savingsBoxes; } }
        private List<Button> _goalButtons = new();
        public List<Button> GoalButtons { get { return _goalButtons; } }

        // Lists of headers, goals, and savings for sharing with Statistics page
        private List<string> _goalHeaders = new();
        public List<string> GoalHeaders { get { return _goalHeaders; } }
        private List<double> _goalExpenses = new();
        public List<double> GoalExpenses { get { return _goalExpenses; } }
        private List<string> _savingsHeaders = new();
        public List<string> SavingsHeaders { get { return _savingsHeaders; } }
        private List<double> _goalSavings = new();
        public List<double> GoalSavings { get { return _goalSavings; } }

        public GoalsPage()
        {
            this.InitializeComponent();
        }

        private void addGoalButton_Click(object sender, RoutedEventArgs e)
        {
            // Add a new row definition to goalGrid
            RowDefinition newRow = new();
            newRow.Height = GridLength.Auto;
            goalGrid.RowDefinitions.Add(newRow);

            // Create new NumberBoxes with custom Headers
            goalGridRows++;
            NumberBox newGoalBox = new();
            newGoalBox.PlaceholderText = "0.00";
            newGoalBox.Margin = new Thickness(0,0,5,0);
            NumberBox newSavingsBox = new();
            newSavingsBox.PlaceholderText = "0.00";
            newSavingsBox.Margin = new Thickness(5,0,0,0);
            if (goalName.Text.Equals(""))
            {
                newGoalBox.Header = "Goal Total: ";
                newSavingsBox.Header = "Goal Progress: ";
            } else
            {
                newGoalBox.Header = goalName.Text.Trim() + " Total: ";
                newSavingsBox.Header = goalName.Text.Trim() + " Progress: ";
                goalName.Text = "";
            }
            _goalBoxes.Add(newGoalBox);
            _savingsBoxes.Add(newSavingsBox);

            // Create new remove Button
            Button removeGoalButton = new();
            removeGoalButton.Content = "-";
            removeGoalButton.Width = 40;
            removeGoalButton.Height = 40;
            removeGoalButton.Margin = new Thickness(10, 18, 0, 0);
            removeGoalButton.BorderBrush = addGoalButton.BorderBrush;
            removeGoalButton.BorderThickness = addGoalButton.BorderThickness;
            removeGoalButton.Click += RemoveGoalButton_Click;
            _goalButtons.Add(removeGoalButton);

            // Add newGoalBox, newSavingsBox and removeGoalButton to grid
            goalGrid.Children.Add(newGoalBox);
            Grid.SetRow(newGoalBox, goalGridRows);
            goalGrid.Children.Add(newSavingsBox);
            Grid.SetRow(newSavingsBox, goalGridRows);
            Grid.SetColumn(newSavingsBox, 1);
            goalGrid.Children.Add(removeGoalButton);
            Grid.SetRow(removeGoalButton, goalGridRows);
            Grid.SetColumn(removeGoalButton, 2);
        }

        private void RemoveGoalButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove specified NumberBoxes, Button, and RowDefinition
            int index = _goalButtons.IndexOf(sender as Button);
            NumberBox removedGoalBox = _goalBoxes[index];
            NumberBox removedSavingsBox = _savingsBoxes[index];
            Button removedGoalButton = _goalButtons[index];
            goalGrid.Children.Remove(removedGoalBox);
            goalGrid.Children.Remove(removedSavingsBox);
            goalGrid.Children.Remove(removedGoalButton);
            goalGrid.RowDefinitions.RemoveAt(index);
            _goalBoxes.RemoveAt(index);
            _savingsBoxes.RemoveAt(index);
            _goalButtons.RemoveAt(index);
            goalGridRows--;

            // Reposition goalGrid elements
            for (int i = 0; i < GoalBoxes.Count; i++)
            {
                Grid.SetRow(GoalBoxes[i], i);
                Grid.SetRow(SavingsBoxes[i], i);
                Grid.SetColumn(SavingsBoxes[i], 1);
                Grid.SetRow(GoalButtons[i], i);
                Grid.SetColumn(GoalButtons[i], 2);
            }
        }

        private void GoalButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset goal, savings, headers, goalExpenses and goalSavings
            goal = 0;
            savings = 0;
            _goalHeaders.Clear();
            _goalExpenses.Clear();
            _savingsHeaders.Clear();
            _goalSavings.Clear();

            // Calculate total goal and total savings, populate headers and values lists
            for (int i = 0; i < GoalBoxes.Count; i++)
            {
                double goalVal = double.Parse(GoalBoxes[i].Text);
                double savingsVal = double.Parse(SavingsBoxes[i].Text);
                if (goalVal >= savingsVal)
                {
                    goal += goalVal;
                    savings += savingsVal;
                    _goalHeaders.Add(GoalBoxes[i].Header.ToString());
                    _goalExpenses.Add(goalVal);
                    _savingsHeaders.Add(SavingsBoxes[i].Header.ToString());
                    _goalSavings.Add(savingsVal);
                }
            }

            // Output goal, savings, and remaining goal
            totalGoal.Text = String.Format("{0:C2}", goal);
            totalSaved.Text = String.Format("{0:C2}", savings);
            remainingGoal.Text = String.Format("{0:C2}", goal - savings);

            // Generate pie charts on Statistics page
            MainWindow.sPage.GenerateCharts();
        }
    }
}
