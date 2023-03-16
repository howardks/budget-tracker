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

        public void GeneratePieChart()
        {
            PopulateExpenses();

            float pieWidth = 600, pieHeight = 600, centerX = pieWidth / 2, centerY = pieHeight / 2, radius = pieWidth / 2;
            mainCanvas.Width = pieWidth;
            mainCanvas.Height = pieHeight;

            // draw pie
            float angle = 0, prevAngle = 0;
            foreach (var category in expenses)
            {
                double line1X = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double line1Y = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                angle = (float)category.Percentage * (float)360 / 100 + prevAngle;
                Debug.WriteLine(angle);

                double arcX = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double arcY = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                var line1Segment = new LineSegment()
                {
                    Point = new Point(line1X, line1Y)
                };
                double arcWidth = radius, arcHeight = radius;
                bool isLargeArc = category.Percentage > 50;
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
                    Fill = category.color,
                    Data = pathGeometry,
                };
                mainCanvas.Children.Add(path);

                prevAngle = angle;


                // draw outlines
                var outline1 = new Line()
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = line1Segment.Point.X,
                    Y2 = line1Segment.Point.Y,
                    Stroke = new SolidColorBrush(Color.FromArgb(0, 255, 0, 0)),
                    StrokeThickness = 5,
                };
                var outline2 = new Line()
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = arcSegment.Point.X,
                    Y2 = arcSegment.Point.Y,
                    Stroke = new SolidColorBrush(Color.FromArgb(0, 100, 255, 0)),
                    StrokeThickness = 5,
                };

                mainCanvas.Children.Add(outline1);
                mainCanvas.Children.Add(outline2);
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
