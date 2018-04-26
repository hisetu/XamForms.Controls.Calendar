using Android.Graphics;
using Android.Graphics.Drawables;
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
                    return output;
                }
                catch 
                {
                    //I track exception and not OutMemoryException, because throw the OutMemoryException from Java.                    
                }
            }

            return null;
        }
    }
}