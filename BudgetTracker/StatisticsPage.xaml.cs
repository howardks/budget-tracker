// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.UI;

namespace BudgetTracker
{
    public sealed partial class StatisticsPage : Page
    {
        // Variables for pie charts from Finances page
        private FinancesPage fPage = MainWindow.fPage;
        private List<PieData> expensePieces = new();
        private List<PieData> incomePieces = new();

        // Variables for pie charts from Goals page
        private GoalsPage gPage = MainWindow.gPage;
        private List<PieData> goalPieces = new();

        public StatisticsPage()
        {
            this.InitializeComponent();
        }

        public void GenerateCharts()
        {
            // Generate all available pie charts
            GenerateFundsPieChart();
            GenerateIncomePieChart();
            GenerateExpensePieChart();
            GenerateGoalPieChart();
        }

        public void PopulateExpenses()
        {
            // Clear expense pie chart pieces
            expensePieces.Clear();

            // Populate pie chart pieces for expenses
            for (int i = 0; i < fPage.ExpenseValues.Count; i++)
            {
                if (i < 24)
                {
                    expensePieces.Add(new PieData(fPage.ExpenseHeaders[i], fPage.ExpenseValues[i] / fPage.Expenses * 100, fPage.ExpenseValues[i], i));
                }
                else
                {
                    expensePieces.Add(new PieData(fPage.ExpenseHeaders[i], fPage.ExpenseValues[i] / fPage.Expenses * 100, fPage.ExpenseValues[i]));
                }
            }
        }

        public void PopulateIncome()
        {
            // Clear income pie chart pieces
            incomePieces.Clear();

            // Populate pie chart pieces for income
            for (int i = 0; i < fPage.IncomeValues.Count; i++)
            {
                if (i < 24)
                {
                    incomePieces.Add(new PieData(fPage.IncomeHeaders[i], fPage.IncomeValues[i] / fPage.Income * 100, fPage.IncomeValues[i], i));
                } 
                else
                {
                    incomePieces.Add(new PieData(fPage.IncomeHeaders[i], fPage.IncomeValues[i] / fPage.Income * 100, fPage.IncomeValues[i]));
                }
            }
        }

        public void PopulateGoals()
        {
            // Clear goal pie chart pieces
            goalPieces.Clear();

            // Populate pie chart pieces for goal expenses
            for (int i = 0; i < gPage.GoalExpenses.Count; i++)
            {
                if (i < 12)
                {
                    goalPieces.Add(new PieData(gPage.GoalHeaders[i][..^8] + " Needed: ", (gPage.GoalExpenses[i] - gPage.GoalSavings[i]) / gPage.Goal * 100, gPage.GoalExpenses[i] - gPage.GoalSavings[i], i));
                }
                else
                {
                    goalPieces.Add(new PieData(gPage.GoalHeaders[i][..^8] + " Needed: ", (gPage.GoalExpenses[i] - gPage.GoalSavings[i]) / gPage.Goal * 100, gPage.GoalExpenses[i] - gPage.GoalSavings[i]));
                }
            }

            // Populate pie chart pieces for goal savings
            for (int i = 0; i < gPage.GoalSavings.Count; i++)
            {
                if (i < 12)
                {
                    goalPieces.Add(new PieData(gPage.SavingsHeaders[i], gPage.GoalSavings[i] / gPage.Goal * 100, gPage.GoalSavings[i], 12 + i));
                }
                else
                {
                    goalPieces.Add(new PieData(gPage.SavingsHeaders[i], gPage.GoalSavings[i] / gPage.Goal * 100, gPage.GoalSavings[i]));
                }
            }
        }

        public void GenerateGoalPieChart()
        {
            // Adjust appropriate visibilities, populate goals pie data, and generate goals pie chart if Goals page has data
            if (gPage.Goal > 0)
            {
                goalTitle.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                goalPanel.Visibility = Microsoft.UI.Xaml.Visibility.Visible;

                PopulateGoals();
                GeneratePieChart(goalItemsControl, goalCanvas, goalPieces);
            } 
            else
            {
                goalTitle.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                goalPanel.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            }
            ControlMissingFinancesTitleVisibility();
        }

        public void GenerateIncomePieChart()
        {
            // Adjust appropriate visibilities, populate income pie data, and generate income pie chart if Finances page has income data
            if (fPage.Income > 0)
            {
                incomeTitle.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                incomePanel.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                incomeLine.Visibility = Microsoft.UI.Xaml.Visibility.Visible;

                PopulateIncome();
                GeneratePieChart(incomeItemsControl, incomeCanvas, incomePieces);
            }
            else
            {
                incomeTitle.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                incomePanel.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                incomeLine.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            }
            ControlMissingFinancesTitleVisibility();
        }

        public void GenerateExpensePieChart()
        {
            // Adjust appropriate visibilities, populate expense pie data, and generate expense pie chart if Finances page has expense data
            if (fPage.Expenses > 0)
            {
                expenseTitle.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                expensePanel.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                expenseLine.Visibility = Microsoft.UI.Xaml.Visibility.Visible;

                PopulateExpenses();
                GeneratePieChart(expenseItemsControl, expenseCanvas, expensePieces);
            } else
            {
                expenseTitle.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                expensePanel.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                expenseLine.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            }
            ControlMissingFinancesTitleVisibility();
        }

        public void GenerateFundsPieChart()
        {
            // Adjust appropriate visibilities, populate funds pie data, and generate funds pie chart if Finances page has income and expense data
            if (fPage.Expenses > 0 && fPage.Income > 0)
            {
                fundsTitle.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                fundsPanel.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                fundsLine.Visibility = Microsoft.UI.Xaml.Visibility.Visible;

                // Funds data is total income and total expenses
                List<PieData> fundsData = new();
                double expensePercent = 100, remainingPercent = 0;

                if (fPage.Income - fPage.Expenses > 0)
                {
                    expensePercent = (fPage.Expenses / fPage.Income) * 100;
                    remainingPercent = 100 - expensePercent;
                }
                fundsData.Add(new("Expenses:", expensePercent, fPage.Expenses, new SolidColorBrush(Color.FromArgb(255, 255, 0, 0))));
                fundsData.Add(new("Remaining Funds:", remainingPercent, fPage.Income - fPage.Expenses, new SolidColorBrush(Color.FromArgb(255, 0, 0, 255))));

                GeneratePieChart(fundsItemsControl, fundsCanvas, fundsData);
            } 
            else
            {
                fundsTitle.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                fundsPanel.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                fundsLine.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            }
            ControlMissingFinancesTitleVisibility();
        }

        public void GeneratePieChart(ItemsControl itemsControl, Canvas canvas, List<PieData> pieData)
        {
            // Reset canvas and items control
            canvas.Children.Clear();
            itemsControl.ItemsSource = null;
            itemsControl.ItemsSource = pieData;

            double pieWidth = canvas.Width, pieHeight = canvas.Height, centerX = pieWidth / 2, centerY = pieHeight / 2, radius = pieWidth / 2;

            if (pieData.Count > 1 && pieData[0].Percentage < 100 && pieData[1].Percentage < 100)
            {
                // Create each pie piece and add it to canvas
                double angle = 0, prevAngle = 0;
                foreach (PieData piece in pieData)
                {
                    // Draw pie piece
                    double line1X = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                    double line1Y = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                    angle = piece.Percentage * 360 / 100 + prevAngle;

                    double arcX = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                    double arcY = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                    var line1Segment = new LineSegment()
                    {
                        Point = new Point(line1X, line1Y)
                    };
                    double arcWidth = radius, arcHeight = radius;
                    bool isLargeArc = piece.Percentage > 50;
                    var arcSegment = new ArcSegment()
                    {
                        Size = new Size(arcWidth, arcHeight),
                        Point = new Point(arcX, arcY),
                        SweepDirection = SweepDirection.Clockwise,
                        IsLargeArc = isLargeArc,
                    };
                    var line2Segment = new LineSegment()
                    {
                        Point = new Point(centerX, centerY)
                    };

                    var pathFigure = new PathFigure()
                    {
                        StartPoint = new Point(centerX, centerY),
                        Segments = new PathSegmentCollection()
                    {
                        line1Segment,
                        arcSegment,
                        line2Segment,
                    },
                        IsClosed = true
                    };

                    var pathGeometry = new PathGeometry();
                    pathGeometry.Figures.Add(pathFigure);
                    var path = new Path()
                    {
                        Fill = piece.color,
                        Data = pathGeometry,
                    };
                    canvas.Children.Add(path);

                    // Update prevAngle for next pie piece
                    prevAngle = angle;

                    // Draw outlines around pie piece
                    Brush outlineBrush = Resources["SystemControlBackgroundChromeMediumLowBrush"] as Brush;
                    var outline1 = new Line()
                    {
                        X1 = centerX,
                        Y1 = centerY,
                        X2 = line1Segment.Point.X,
                        Y2 = line1Segment.Point.Y,
                        Stroke = outlineBrush,
                        StrokeThickness = 5,
                    };
                    var outline2 = new Line()
                    {
                        X1 = centerX,
                        Y1 = centerY,
                        X2 = arcSegment.Point.X,
                        Y2 = arcSegment.Point.Y,
                        Stroke = outlineBrush,
                        StrokeThickness = 5,
                    };

                    canvas.Children.Add(outline1);
                    canvas.Children.Add(outline2);
                }
            } 
            else
            {
                // When there is only one pie piece, display a full circle
                foreach (PieData piece in pieData)
                {
                    if (piece.Percentage == 100)
                    {
                        Ellipse circle = new()
                        {
                            Width = pieWidth,
                            Height = pieHeight,
                            Fill = piece.color
                        };

                        canvas.Children.Add(circle);
                    }
                }
            }
        }

        public void ControlMissingFinancesTitleVisibility()
        {
            // Display missingFinancesTitle only when there are no pie charts to display
            if (fPage.Expenses > 0 || fPage.Income > 0 || gPage.Goal > 0)
            {
                missingFinancesTitle.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            } 
            else
            {
                missingFinancesTitle.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            }
        }
    }

    // Class for pie pieces and populating ItemsControl.ItemsTemplate
    public class PieData
    {
        private String name;
        public String Name { get { return name; } set { name = value; } }

        private double percentage;
        public double Percentage { get { return percentage; } set { percentage = value; } }

        private double amount;
        public double Amount { get { return amount; } set { amount = value; } }

        public SolidColorBrush color; 
        public string formattedOutput;

        // Lists of colors
        private static List<SolidColorBrush> colorList = new()
        {
            // Light Colors begin at index 0
            new SolidColorBrush(Color.FromArgb(255, 102, 255, 255)),
            new SolidColorBrush(Color.FromArgb(255, 102, 102, 255)),
            new SolidColorBrush(Color.FromArgb(255, 255, 102, 255)),
            new SolidColorBrush(Color.FromArgb(255, 255, 102, 102)),
            new SolidColorBrush(Color.FromArgb(255, 255, 255, 102)),
            new SolidColorBrush(Color.FromArgb(255, 102, 255, 102)),
            new SolidColorBrush(Color.FromArgb(255, 102, 178, 255)),
            new SolidColorBrush(Color.FromArgb(255, 178, 102, 255)),
            new SolidColorBrush(Color.FromArgb(255, 255, 102, 178)),
            new SolidColorBrush(Color.FromArgb(255, 255, 178, 102)),
            new SolidColorBrush(Color.FromArgb(255, 178, 255, 102)),
            new SolidColorBrush(Color.FromArgb(255, 102, 255, 178)),
            // Dark Colors begin at index 12
            new SolidColorBrush(Color.FromArgb(255, 0, 153, 153)),
            new SolidColorBrush(Color.FromArgb(255, 0, 0, 153)),
            new SolidColorBrush(Color.FromArgb(255, 153, 0, 153)),
            new SolidColorBrush(Color.FromArgb(255, 153, 0, 0)),
            new SolidColorBrush(Color.FromArgb(255, 153, 153, 0)),
            new SolidColorBrush(Color.FromArgb(255, 0, 153, 0)),
            new SolidColorBrush(Color.FromArgb(255, 0, 76, 153)),
            new SolidColorBrush(Color.FromArgb(255, 76, 0, 153)),
            new SolidColorBrush(Color.FromArgb(255, 153, 0, 76)),
            new SolidColorBrush(Color.FromArgb(255, 153, 76, 0)),
            new SolidColorBrush(Color.FromArgb(255, 76, 153, 0)),
            new SolidColorBrush(Color.FromArgb(255, 0, 153, 76))
        };

        // Constructor that provides random color if no color or index is specified
        public PieData(string name, double percentage, double amount)
        {
            Name = name;
            Percentage = percentage;
            Amount = amount;
            GenerateRandomColor();
            formattedOutput = String.Format("{0,-22} {1:P2}   {2:C2}", Name, Percentage/100, Amount);
        }

        // Constructor with specified color
        public PieData(string name, double percentage, double amount, SolidColorBrush color)
        {
            Name = name;
            Percentage = percentage;
            Amount = amount;
            this.color = color;
            formattedOutput = String.Format("{0,-22} {1:P2}   {2:C2}", Name, Percentage/100, Amount);
        }

        // Constructor with color based on index
        public PieData(string name, double percentage, double amount, int index)
        {
            Name = name; 
            Percentage = percentage;
            Amount = amount;
            this.color = colorList[index];
            formattedOutput = String.Format("{0,-22} {1:P2}   {2:C2}", Name, Percentage / 100, Amount);
        }

        private void GenerateRandomColor() 
        {
            // Random color generator
            Random rnd = new();
            color = new SolidColorBrush(Color.FromArgb(255, (byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
        }

    }
}