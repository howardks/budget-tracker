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
    public sealed partial class StatisticsPage : Page
    {
        private FinancesPage fPage = new();
        private List<PieData> expenses = new();

        public StatisticsPage()
        {
            this.InitializeComponent();
            PopulateExpenses();
        }

        public void PopulateExpenses()
        {
            foreach (NumberBox n in fPage.ExpenseBoxes)
            {
                if (!n.Text.Equals(""))
                {
                    expenses.Add(new PieData(n.Header.ToString(), (int)(Double.Parse(n.Text) / fPage.Expenses)));
                }
            }

        }
    }

    public class PieData
    {
        private String name;
        public String Name { get { return name; } set { name = value; } }

        private int number;
        public int Number { get { return number; } set { number = value; } }

        public PieData(string name, int number)
        {
            Name = name;
            Number = number;
        }
    }
}
