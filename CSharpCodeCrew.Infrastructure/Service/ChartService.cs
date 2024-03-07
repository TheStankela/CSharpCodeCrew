using CSharpCodeCrew.Domain.Interfaces;
using CSharpCodeCrew.Domain.Models;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;

namespace CSharpCodeCrew.Application.Service
{
    public class ChartService : IChartService
    {
        public async Task<byte[]> GenerateChart(string jsonData)
        {
            var employeeData = JsonConvert.DeserializeObject<List<Employee>>(jsonData);

            Bitmap chart = new Bitmap(1200, 800);
            Graphics graphics = Graphics.FromImage(chart);

            RectangleF rectangle = new RectangleF(10, 10, 300, 300);
            float startAngle = 0.0f;

            List<string> legendLabels = new List<string>();
            List<Color> legendColors = new List<Color>();
            List<decimal> legendPercentages = new List<decimal>();

            var totalWorkHours = TotalWorkHours(employeeData);

            foreach (var data in employeeData)
            {
                float sweepAngle = (float)((decimal)360.0 * (data.TotalTime / totalWorkHours));

                using (SolidBrush brush = new SolidBrush(GetRandomColor()))
                {
                    graphics.FillPie(brush, rectangle, startAngle, sweepAngle);

                    legendLabels.Add(data.Name);
                    legendColors.Add(brush.Color);

                    var percentage = (data.TotalTime / totalWorkHours) * 100;
                    legendPercentages.Add(percentage);
                }

                startAngle += sweepAngle;
            }

            DrawLegend(graphics, legendLabels, legendColors, legendPercentages, new PointF(350, 10));

            graphics.Dispose();

            using (MemoryStream stream = new MemoryStream())
            {
                chart.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
        private void DrawLegend(Graphics graphics, List<string> labels, List<Color> colors, List<decimal> percentages, PointF location)
        {
            const int legendItemWidth = 20;
            const int legendItemHeight = 15;
            const int legendItemSpacing = 5;

            for (int i = 0; i < labels.Count; i++)
            {
                PointF legendItemLocation = new PointF(location.X, location.Y + i * (legendItemHeight + legendItemSpacing));

                graphics.FillRectangle(new SolidBrush(colors[i]), legendItemLocation.X, legendItemLocation.Y, legendItemWidth, legendItemHeight);

                string legendText = $"{labels[i]} ({percentages[i]:F2}%)";
                graphics.DrawString(legendText, new Font("Arial", 10), Brushes.White, legendItemLocation.X + legendItemWidth + legendItemSpacing, legendItemLocation.Y);
            }
        }
        private decimal TotalWorkHours(List<Employee> employeeData)
        {
            return employeeData.Sum(e => e.TotalTime);
        }
        private Color GetRandomColor()
        {
            Random random = new Random();
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }
    }
}
