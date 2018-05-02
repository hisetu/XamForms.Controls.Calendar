using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Graphics.Drawable;
using Android.Views;
using System;

namespace XamForms.Controls.Droid
{
    public static class ExtensionMethodsUtils
    {
        public static ShapeType ToShapeType(this EnumShapeDate enumShapeDate)
        {            
            switch(enumShapeDate)
            {
                case EnumShapeDate.Oval:
                    return ShapeType.Oval;

                case EnumShapeDate.Rectangle:
                    return ShapeType.Rectangle;

                default:
                    return ShapeType.Rectangle;
            }            
        }

        public static Bitmap RoundBitmap(this Bitmap bitmap)
        {
            if (bitmap != null)
            {
                try
                {
                    int maxValue = Math.Max(bitmap.Width, bitmap.Height);
                    Bitmap output = Bitmap.CreateBitmap(maxValue, maxValue, Bitmap.Config.Argb8888);
                    var canvas = new Canvas(output);

                    var paint = new Paint();
                    var rect = new Rect(0, 0, bitmap.Width, bitmap.Height);
                    var rectF = new RectF(0, 0, maxValue, maxValue);

                    paint.AntiAlias = true;
                    canvas.DrawARGB(0, 0, 0, 0);
                    canvas.DrawOval(rectF, paint);
                    paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));

                    canvas.DrawBitmap(bitmap, rect, rectF, paint);
                   bitmap.Dispose();
                    return output;
                }
                catch 
                {
                    //I track exception and not OutMemoryException, because throw the OutMemoryException from Java.                    
                }
            }

            return null;
        }

        public static Bitmap ToBitmap(this Drawable drawable)
        {
            int width = 0;
            int height = 0;
            Bitmap bitmap;
            
            if(drawable.Bounds.IsEmpty)
            {
                width = drawable.IntrinsicWidth;
                height = drawable.IntrinsicHeight;
            }
            else
            {
                width = drawable.Bounds.Width();
                height = drawable.Bounds.Height();
            }

            if(!(width > 0 && height > 0))
            {
                width = 100;
                height = 100;
            }
            
            bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(bitmap);
            drawable.SetBounds(0, 0, width, height);
            drawable.Draw(canvas);
            return bitmap;
        }

        public static Drawable ToDrawable(this Bitmap bitmap)
        {
            return new BitmapDrawable(bitmap);
        }

        public static RoundedBitmapDrawable ToRoundedBitmap(this Bitmap bitmap, Context context)
        {
            return RoundedBitmapDrawableFactory.Create(context.Resources, bitmap);
        }

        public static GravityFlags ToAndroid(this XamForms.Controls.TextAlign textAlignment)
        {
            switch(textAlignment)
            {
                case TextAlign.CenterBottom:
                    return GravityFlags.Bottom | GravityFlags.Center;
                case TextAlign.CenterTop:
                    return GravityFlags.Top | GravityFlags.Center;

                case TextAlign.LeftBottom:
                    return GravityFlags.Left | GravityFlags.Bottom;

                case TextAlign.LeftCenter:
                    return GravityFlags.Left | GravityFlags.Center;

                case TextAlign.LeftTop:
                    return GravityFlags.Left | GravityFlags.Top;

                case TextAlign.Middle:
                    return GravityFlags.Center;

                case TextAlign.RightBottom:
                    return GravityFlags.Right | GravityFlags.Bottom;

                case TextAlign.RightCenter:
                    return GravityFlags.Right | GravityFlags.Center;

                case TextAlign.RightTop:
                    return GravityFlags.Right | GravityFlags.Top;

                default:
                    return GravityFlags.Center;
            }
        }
    }
}