using ReactiveUI;
using System.IO;
using System.Text.Json;
using WpfToolsCoding.Models;
using WpfToolsCoding.Models.Shapes;
using DynamicData;
using Line = WpfToolsCoding.Models.Shapes.Line;
using System.Windows.Media;
using WpfToolsCoding.Enums;
using System.Linq;
using System.Collections.Generic;


namespace WpfToolsCoding.ViewModels
{
    public class VmShape : ShapeBase
    {
        public VmShape(string path)
        {
            AllShapes = new List<ShapeBase>();
            ReadJSON(path);
        }

        public void ReadJSON(string path)
        {
            JSONtext = File.ReadAllText(path);

            var result = JsonSerializer.Deserialize<List<ShapeBase>>(JSONtext, new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true });

            if (result == null)
                return;


            var shapeLists = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(JSONtext);

            if (shapeLists == null )  
                return;

            foreach (var shape in shapeLists)
            {
                if (shape == null) 
                    continue;

                if (shape.First().Value.ToString() == ShapesEnums.line.ToString())
                {
                        Line line = new()
                        {
                            Type = ShapesEnums.line.ToString(),
                            Filled = bool.TryParse(shape.GetValueOrDefault(ShapesPropertyEnums.filled.ToString())?.ToString(), out bool filledValue) ? filledValue : false,
                            Color = shape.GetValueOrDefault(ShapesPropertyEnums.color.ToString())?.ToString(),
                            ARGBcolor = System.Windows.Media.Color.FromArgb(127, 0, 0, 0),
                            a = shape.GetValueOrDefault(ShapesPropertyEnums.a.ToString())?.ToString(),
                            b = shape.GetValueOrDefault(ShapesPropertyEnums.b.ToString())?.ToString(),
                        };
                    LinesShapes.Add(line);
                 }
                else if (shape.First().Value.ToString() == ShapesEnums.triangle.ToString())
                {
                    Triangle triangle = new()
                    {
                        Type = ShapesEnums.triangle.ToString(),
                        Filled = bool.TryParse(shape.GetValueOrDefault(ShapesPropertyEnums.filled.ToString())?.ToString(), out bool filledValue) ? filledValue : false,
                        Color = shape.GetValueOrDefault(ShapesPropertyEnums.color.ToString())?.ToString(),
                        ARGBcolor = System.Windows.Media.Color.FromArgb(127, 0, 0, 0),
                        a = shape.GetValueOrDefault(ShapesPropertyEnums.a.ToString())?.ToString(),
                        b = shape.GetValueOrDefault(ShapesPropertyEnums.b.ToString())?.ToString(),
                        c = shape.GetValueOrDefault(ShapesPropertyEnums.c.ToString())?.ToString(),
                    };
                    TriangleShapes.Add(triangle);
                }
                else if (shape.First().Value.ToString() == ShapesEnums.circle.ToString())
                {
                    Circle circle = new()
                    {
                        Type = ShapesEnums.circle.ToString(),
                        Filled = bool.TryParse(shape.GetValueOrDefault(ShapesPropertyEnums.filled.ToString())?.ToString(), out bool filledValue) ? filledValue : false,
                        Color = shape.GetValueOrDefault(ShapesPropertyEnums.color.ToString())?.ToString(),
                        ARGBcolor = System.Windows.Media.Color.FromArgb(127, 0, 0, 0),
                        center = shape.GetValueOrDefault(ShapesPropertyEnums.center.ToString())?.ToString(),
                        radius = double.Parse(shape.GetValueOrDefault(ShapesPropertyEnums.radius.ToString()).ToString()),
                    };
                    CircleShapes.Add(circle);
                }
            }
            DrawLines();
            DrawTriangles();
            DrawCircles();

            AllShapes.Add(LinesShapes);
            AllShapes.Add(TriangleShapes);
            AllShapes.Add(CircleShapes);

        }

        private void DrawCircles()
        {
            foreach (var circle in CircleShapes)
            {
                MapColors(circle);

                string[] centerString = circle.center.Split(';').ToArray();
                double radius = circle.radius;

                circle.radius = radius*2;
            }
        }

        private void DrawLines(){
            foreach (var line in LinesShapes){
                MapColors(line);

                List<string> pointsString = line.a.Split(';').Concat(line.b.Split(';')).ToList();
                List<double> points = pointsString.Select(point => double.Parse(point.Trim())).ToList();

                line.X1 = points.ElementAt(0);
                line.Y1 = points.ElementAt(1);
                line.X2 = points.ElementAt(2);
                line.Y2 = points.ElementAt(3);
            }
        }

        private void DrawTriangles()
        {
            foreach (var triangle in TriangleShapes){
                MapColors(triangle);

                string[] pointsString = triangle.a.Split(';').Concat(triangle.b.Split(';')).Concat(triangle.c.Split(';')).ToArray();
                pointsString = pointsString.Select(o => o.Replace(',', '.'))
                               .SelectMany((o, index) =>
                               {
                                   if (index % 2 != 1)
                                   {
                                       o = o.Insert(o.Length, ",");
                                   }
                                   return new[] { o };
                               }).ToArray();
            
                string result = string.Join(" ", pointsString);
                triangle.Points = result;
            }
        }

        private void MapColors(ShapeBase shape)
        {
            string[] colorsString = shape.Color.Split(';').ToArray();
            byte a = byte.Parse(colorsString[0]);
            byte r = byte.Parse(colorsString[1]);
            byte g = byte.Parse(colorsString[2]);
            byte b = byte.Parse(colorsString[3]);

            shape.ARGBcolor = System.Windows.Media.Color.FromArgb(a, r, g, b);
            shape.BrushColor = new SolidColorBrush(shape.ARGBcolor);

        }

        public List<ShapeBase> AllShapes{ get; set; } = new();
        public List<Line> LinesShapes { get; set; } = new();
        public List<Triangle> TriangleShapes { get; set; } = new();
        public List<Circle> CircleShapes { get; set; } = new();


        public string JSONtext { get; set; }


    }
}
