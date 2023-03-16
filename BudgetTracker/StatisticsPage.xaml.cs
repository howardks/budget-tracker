// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
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
        private FinancesPage fPage = MainWindow.fPage;
        private List<PieData> expenses = new();

        public StatisticsPage()
        {
            this.InitializeComponent();
        }

        public void PopulateExpenses()
        {
            expenses.Clear();
            foreach (NumberBox n in fPage.ExpenseBoxes)
            {
                if (!n.Text.Equals(""))
                {
                    expenses.Add(new PieData(n.Header.ToString(), (Double.Parse(n.Text) / fPage.Expenses) * 100));
                }
            }

        }

        public void GenerateExpensePieChart()
        {
            if (fPage.Expenses > 0)
            {
                expenseTitle.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                expensePanel.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                PopulateExpenses();
                GeneratePieChart(expenseItemsControl, expenseCanvas, expenses);
            } else
            {
                expenseTitle.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                expensePanel.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            }
        }

        public void GenerateIncomeExpensePieChart()
        {
            List<PieData> incomeExpenseData = new();
            double expensePercent = 100, remainingPercent = 0;

            if (fPage.Income - fPage.Expenses > 0)
            {
                expensePercent = (fPage.Expenses / fPage.Income) * 100;
                remainingPercent = 100 - expensePercent;
            } 
            incomeExpenseData.Add(new("Expenses:", expensePercent, new SolidColorBrush(Color.FromArgb(255, 255, 0, 0))));
            incomeExpenseData.Add(new("Remaining Funds:", remainingPercent, new SolidColorBrush(Color.FromArgb(255, 0, 0, 255))));

            GeneratePieChart(incomeExpenseitemsControl, incomeExpenseCanvas, incomeExpenseData);
        }

        public void GeneratePieChart(ItemsControl itemsControl, Canvas canvas, List<PieData> pieData)
        {
            canvas.Children.Clear();
            itemsControl.ItemsSource = null;
            itemsControl.ItemsSource = pieData;

            double pieWidth = canvas.Width, pieHeight = canvas.Height, centerX = pieWidth / 2, centerY = pieHeight / 2, radius = pieWidth / 2;

            if (pieData.Count > 1 && pieData[0].Percentage < 100 && pieData[1].Percentage < 100)
            {
                // Draw pie chart
                double angle = 0, prevAngle = 0;
                foreach (PieData piece in pieData)
                {
                    double line1X = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                    double line1Y = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                    angle = piece.Percentage * 360 / 100 + prevAngle;
                    Debug.WriteLine(angle);

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

                    prevAngle = angle;


                    // draw outlines
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
            } else
            {
                foreach (PieData piece in pieData)
                {
                    if (piece.Percentage == 100)
                    {
                        Ellipse circle = new Ellipse();
                        circle.Width = pieWidth;
                        circle.Height = pieHeight;
                        circle.Fill = piece.color;

                        canvas.Children.Add(circle);
                    }
                }
            }
        }
    }



    public class PieData
    {
        private String name;
        public String Name { get { return name; } set { name = value; } }

        private double percentage;
        public double Percentage { get { return percentage; } set { percentage = value; } }

        public SolidColorBrush color; // Make private later
        public string formattedOutput;

        public PieData(string name, double percentage)
        {
            Name = name;
            Percentage = percentage;
            GenerateRandomColor();
            formattedOutput = String.Format("{0,-22} {1:P2}", Name, Percentage/100);
        }

        public PieData(string name, double percentage, SolidColorBrush color)
        {
            Name = name;
            Percentage = percentage;
            this.color = color;
            formattedOutput = String.Format("{0,-22} {1:P2}", Name, Percentage/100);
        }

        private void GenerateRandomColor() // Random colors look terrible, maybe allow user to select colors
        {
            Random rnd = new Random();
            color = new SolidColorBrush(Color.FromArgb(255, (byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
        }
    }
}