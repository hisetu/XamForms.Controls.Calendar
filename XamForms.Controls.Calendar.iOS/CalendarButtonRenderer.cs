﻿using XamForms.Controls;
using XamForms.Controls.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
#if __UNIFIED__
using Foundation;
#else
using MonoTouch.Foundation;
#endif

[assembly: Xamarin.Forms.ExportRenderer(typeof(CalendarButton), typeof(CalendarButtonRenderer))]
namespace XamForms.Controls.iOS
{
	[Preserve(AllMembers = true)]
    public class CalendarButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control == null || Element == null)
                return;

            SetTextAlignment((Element as CalendarButton).ButtonTextAlign);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = Element as CalendarButton;
            if (e.PropertyName == nameof(element.TextWithoutMeasure) || e.PropertyName == "Renderer")
            {
                Control.SetTitle(element.TextWithoutMeasure, UIControlState.Normal);
				Control.SetTitle(element.TextWithoutMeasure, UIControlState.Disabled);
            }
			if (e.PropertyName == nameof(element.TextColor) || e.PropertyName == "Renderer")
			{
				Control.SetTitleColor(element.TextColor.ToUIColor(), UIControlState.Disabled);
				Control.SetTitleColor(element.TextColor.ToUIColor(), UIControlState.Normal);
			}
			if (e.PropertyName == nameof(element.BackgroundPattern))
			{ 
				DrawBackgroundPattern();
			}
			if (e.PropertyName == nameof(element.BackgroundImage))
			{ 
				DrawBackgroundImage();
			}

            if (e.PropertyName == nameof(Element.BorderWidth) || e.PropertyName == nameof(Element.BorderColor) || e.PropertyName == nameof(Element.BackgroundColor) || e.PropertyName == "Renderer")
            {
                SetNeedsDisplay();
            }

            if(e.PropertyName == CalendarButton.ButtonTextAlignProperty.PropertyName)
            {
                SetTextAlignment(element.ButtonTextAlign);
            }
        }
        
        public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			Control.SetBackgroundImage(null, UIControlState.Normal);
			Control.SetBackgroundImage(null, UIControlState.Disabled);
            Control.BackgroundColor = Element.BackgroundColor.ToUIColor();
            SetShape();
			DrawBackgroundImage();
			DrawBackgroundPattern();
		}

        private void SetShape()
        {
            Control.Layer.CornerRadius = 20;
        }

        protected async void DrawBackgroundImage()
		{
			var element = Element as CalendarButton;
			if (element == null || element.BackgroundImage == null) return;
			var image = await GetImage(element.BackgroundImage);
			Control.SetBackgroundImage(image, UIControlState.Normal);
			Control.SetBackgroundImage(image, UIControlState.Disabled);
		}

		protected void DrawBackgroundPattern()
		{            
			var element = Element as CalendarButton;
			if (element == null || element.BackgroundPattern == null || Control.Frame.Width == 0) return;

            UIImage image = null;

            if(element.PatternStyle == EnumPatternStyle.Strokes)
            {
                image = CreateStrokesPatterns(element);
            }
            else if (element.PatternStyle == EnumPatternStyle.Circles)
            {
                image = CreateCirclePatterns(element);
            }
			
			Control.SetBackgroundImage(image, UIControlState.Normal);
			Control.SetBackgroundImage(image, UIControlState.Disabled);
		}
        
        private UIImage CreateStrokesPatterns(CalendarButton element)
        {            
            UIImage image;
            UIGraphics.BeginImageContext(Control.Frame.Size);
            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                for (var i = 0; i < element.BackgroundPattern.Pattern.Count; i++)
                {
                    var p = element.BackgroundPattern.Pattern[i];
                    g.SetFillColor(p.Color.ToCGColor());
                    var l = (int)Math.Ceiling(Control.Frame.Width * element.BackgroundPattern.GetLeft(i));
                    var t = (int)Math.Ceiling(Control.Frame.Height * element.BackgroundPattern.GetTop(i));
                    var w = (int)Math.Ceiling(Control.Frame.Width * element.BackgroundPattern.Pattern[i].WidthPercent);
                    var h = (int)Math.Ceiling(Control.Frame.Height * element.BackgroundPattern.Pattern[i].HightPercent);
                    var r = new CGRect { X = l, Y = t, Width = w, Height = h };
                    g.FillRect(r);
                    DrawText(g, p, r);
                }

                image = UIGraphics.GetImageFromCurrentImageContext();
            }
            UIGraphics.EndImageContext();

            return image;
        }

        private UIImage CreateCirclePatterns(CalendarButton element)
        {
            UIImage image = null;
            UIGraphics.BeginImageContext(Control.Frame.Size);
            int height = 32;
            int startX = 12;

            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                for(int i = 0; i < element.BackgroundPattern.Pattern.Count; i++)
                {
                    var backGroundPattern = element.BackgroundPattern.Pattern[i];
                    g.SetFillColor(backGroundPattern.Color.ToCGColor());
                    g.SetStrokeColor(Color.White.ToCGColor());
                    g.FillEllipseInRect(new CGRect(startX, height, 7, 7));

                    startX += 4;
                }

                image = UIGraphics.GetImageFromCurrentImageContext();
            }

            UIGraphics.EndImageContext();

            return image;
        }

        Task<UIImage> GetImage(FileImageSource image)
		{
			var handler = new FileImageSourceHandler();
			return handler.LoadImageAsync(image);
		}

		protected void DrawText(CGContext g, Pattern p, CGRect r)
		{
			if (string.IsNullOrEmpty(p.Text)) return;
			var bounds = p.Text.StringSize(UIFont.FromName("Helvetica",p.TextSize));
			var al = (int)p.TextAlign;
			var x = r.X;;

			if ((al & 2) == 2) // center
			{
				x = r.X + (int)Math.Round(r.Width/2.0) - (int)Math.Round(bounds.Width / 2.0);
			}
			else if ((al & 4) == 4) // right
			{
				x = (r.X + r.Width) - bounds.Width - 2;
			}
			var y = r.Y + (int)Math.Round(bounds.Height/ 2.0) + 2;
			if ((al & 16) == 16) // middle
			{
				y = r.Y + (int)Math.Ceiling(r.Height / 2.0) + (int)Math.Round(bounds.Height / 5.0);
			}
			else if ((al & 32) == 32) // bottom
			{
				y = (r.Y + r.Height) - 2;
			}
			g.SaveState();
			g.TranslateCTM(0, Bounds.Height);
    		g.ScaleCTM(1,-1);
			g.SetFillColor(p.TextColor.ToCGColor());
			g.SetTextDrawingMode(CGTextDrawingMode.Fill);
			g.SelectFont("Helvetica", p.TextSize, CGTextEncoding.MacRoman);
			g.ShowTextAtPoint(x, Bounds.Height - y, p.Text);
			g.RestoreState();
		}

        private void SetTextAlignment(TextAlign textAlign)
        {
            switch (textAlign)
            {
                case TextAlign.CenterBottom:
                    Control.VerticalAlignment = UIControlContentVerticalAlignment.Bottom;
                    Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
                    break;
                case TextAlign.CenterTop:
                    Control.VerticalAlignment = UIControlContentVerticalAlignment.Top;
                    Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
                    break;
                case TextAlign.LeftBottom:
                    Control.VerticalAlignment = UIControlContentVerticalAlignment.Bottom;
                    Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Leading;
                    break;
                case TextAlign.LeftCenter:
                    Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
                    Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
                    break;
                case TextAlign.LeftTop:
                    Control.VerticalAlignment = UIControlContentVerticalAlignment.Top;
                    Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
                    break;
                case TextAlign.Middle:
                    Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
                    Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
                    break;
                case TextAlign.RightBottom:
                    Control.VerticalAlignment = UIControlContentVerticalAlignment.Bottom;
                    Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
                    break;
                case TextAlign.RightCenter:
                    Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
                    Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
                    break;
                case TextAlign.RightTop:
                    Control.VerticalAlignment = UIControlContentVerticalAlignment.Top;
                    Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
                    break;                        
                default:
                    Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
                    Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
                    break;
            }
        }
    }

	public static class Calendar
	{
		public static void Init()
		{
			var d = "";
		}
	}
}

