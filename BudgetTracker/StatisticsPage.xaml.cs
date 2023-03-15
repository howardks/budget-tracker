// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BudgetTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        private FinancesPage fPage = new();
        private List<PieData> expenses = new();

        public StatisticsPage()
        {
            this.InitializeComponent();
            GeneratePieChart();
        }

        public void PopulateExpenses()
        {
            foreach (NumberBox n in fPage.ExpenseBoxes)
            {
                if (!n.Text.Equals(""))
                {
                    expenses.Add(new PieData(n.Header.ToString(), Double.Parse(n.Text) / fPage.Expenses));
                }
            }

        }

        public void GeneratePieChart()
        {
            PopulateExpenses();

            // TODO: Figure out this whole situation
        }
    }
}


public class PieData
{
    private String name;
    public String Name { get { return name; } set { name = value; } }

    private double percentage;
    public double Percentage { get { return percentage; } set { percentage = value; } }

    public SolidColorBrush color;

    public PieData(string name, double percentage)
    {
        Name = name;
        Percentage = percentage;
        GenerateRandomColor();
    }

    private void GenerateRandomColor()
    {
        Random rnd = new Random();
        color = new SolidColorBrush(Color.FromArgb(0, (byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
    }
}
