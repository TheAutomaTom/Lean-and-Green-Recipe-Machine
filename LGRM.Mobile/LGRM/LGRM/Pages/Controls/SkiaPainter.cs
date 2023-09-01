using LGRM.Model;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LGRM.XamF.Pages
{
    public class SkiaPainter
    {
        public Kind kind;
        SKColor A1;
        SKColor A2;

        SKColor GetAppSkColor(String s)
        {
            try
            {
                Application.Current.Resources.TryGetValue(s, out var resourceValue);
                return ((Color)resourceValue).ToSKColor();
            }
            catch
            {
                return Color.HotPink.ToSKColor();
            }
        }

        public SkiaPainter()
        {
            A1 = GetAppSkColor("NeutralBG1");            
            //A1 = Color.White.ToSKColor();
            A2 = GetAppSkColor("NeutralBG2");
        }

        public SkiaPainter(Kind kind) 
        {
            this.kind = kind;
            switch (kind)
            {
                case Kind.Lean:
                    A1 = GetAppSkColor("LeansA1");
                    A2 = GetAppSkColor("LeansA2");
                    break;
                case Kind.Green:
                    A1 = GetAppSkColor("GreensA1");
                    A2 = GetAppSkColor("GreensA2");
                    break;
                case Kind.HealthyFat:
                    A1 = GetAppSkColor("HealthyFatsA1");
                    A2 = GetAppSkColor("HealthyFatsA2");
                    break;
                case Kind.Condiment:
                    A1 = GetAppSkColor("CondimentsA1");
                    A2 = GetAppSkColor("CondimentsA2");
                    break;                    
            }


        }






        public void OnCanvasPaint_Open2Title(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            var info = e.Info;

            SKPath path = new SKPath();
            path.MoveTo(0.0f * info.Width, 1.0f * info.Height);  //Bottom Left 
            path.LineTo(0.1f * info.Width, 0.97f * info.Height); //Bottom Left 1
            path.LineTo(0.2f * info.Width, 0.92f * info.Height); //Bottom Left 2
            path.LineTo(0.3f * info.Width, 0.82f * info.Height); //Bottom Left 3

            path.LineTo(0.7f * info.Width, 0.18f * info.Height); //Top Right 3
            path.LineTo(0.8f * info.Width, 0.08f * info.Height); //Top Right 2
            path.LineTo(0.9f * info.Width, 0.03f * info.Height); //Top Right 1
            path.LineTo(1.0f * info.Width, 0.0f * info.Height);  //Top Right 
            path.LineTo(1.0f * info.Width, 1.0f * info.Height);  //Bottom Right (End)
            path.Close();

            SKPaint fillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = A1
            };
            canvas.DrawPath(path, fillPaint);


        }

        public void OnCanvasPaint_Title2Open(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            var info = e.Info;


            SKPath path = new SKPath();
            path.MoveTo(0.0f * info.Width, 0.0f * info.Height);  //Top Left 
            path.LineTo(0.1f * info.Width, 0.03f * info.Height); //Top Left 1
            path.LineTo(0.2f * info.Width, 0.08f * info.Height); //Top Left 2
            path.LineTo(0.3f * info.Width, 0.18f * info.Height); //Top Left 3

            path.LineTo(0.7f * info.Width, 0.82f * info.Height); //Bottom Right 3
            path.LineTo(0.8f * info.Width, 0.92f * info.Height); //Bottom Right 2
            path.LineTo(0.9f * info.Width, 0.97f * info.Height); //Bottom Right 1
            path.LineTo(1.0f * info.Width, 1.0f * info.Height);  //Bottom Right 
            path.LineTo(0.0f * info.Width, 1.0f * info.Height);  //Bottom Left (End)
            path.Close();

            SKPaint fillPaint1 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = A1
            };
            canvas.DrawPath(path, fillPaint1);



        }


        public void OnCanvasPaint_Title2Sub(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            var info = e.Info;

            SKPath path2 = new SKPath();

            //Square to fill in second tier tab, 0.2 shorter than primary tab
            path2.MoveTo(0.0f * info.Width, 0.2f * info.Height);   //Top Left Corner
            path2.LineTo(1.100f * info.Width, 0.2f * info.Height); //Straight to the Right
            path2.LineTo(1.100f * info.Width, 1.0f * info.Height); //Bottom Right
            path2.LineTo(0.0f * info.Width, 1.0f * info.Height);   //Bottom Left
            path2.Close();

            SKPaint fillPaint2 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = A2
            };
            canvas.DrawPath(path2, fillPaint2);

            SKPath path = new SKPath();
            path.MoveTo(0.0f * info.Width, 0.0f * info.Height);  //Top Left 
            path.LineTo(0.1f * info.Width, 0.03f * info.Height); //Top Left 1
            path.LineTo(0.2f * info.Width, 0.08f * info.Height); //Top Left 2
            path.LineTo(0.3f * info.Width, 0.18f * info.Height); //Top Left 3

            path.LineTo(0.7f * info.Width, 0.82f * info.Height); //Bottom Right 3
            path.LineTo(0.8f * info.Width, 0.92f * info.Height); //Bottom Right 2
            path.LineTo(0.9f * info.Width, 0.97f * info.Height); //Bottom Right 1
            path.LineTo(1.0f * info.Width, 1.0f * info.Height);  //Bottom Right 
            path.LineTo(0.0f * info.Width, 1.0f * info.Height);  //Bottom Left (End)
            path.Close();

            SKPaint fillPaint1 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = A1
            };
            canvas.DrawPath(path, fillPaint1);



        }


        public void OnCanvasPaint_Sub2Open(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            var info = e.Info;

            SKPath path = new SKPath();
            path.MoveTo(0.0f * info.Width, 0.2f * info.Height);  //Top Left 
            path.LineTo(0.1f * info.Width, 0.23f * info.Height); //Top Left 1
            path.LineTo(0.2f * info.Width, 0.28f * info.Height); //Top Left 2
            path.LineTo(0.3f * info.Width, 0.38f * info.Height); //Top Left 3

            path.LineTo(0.7f * info.Width, 0.82f * info.Height); //Bottom Right 3
            path.LineTo(0.8f * info.Width, 0.92f * info.Height); //Bottom Right 2
            path.LineTo(0.9f * info.Width, 0.97f * info.Height); //Bottom Right 1
            path.LineTo(1.0f * info.Width, 1.0f * info.Height);  //Bottom Right 
            path.LineTo(0.0f * info.Width, 1.0f * info.Height);  //Bottom Left (End)
            path.Close();

            SKPaint fillPaint3 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = A2
            };
            canvas.DrawPath(path, fillPaint3);
        }
    }
}
