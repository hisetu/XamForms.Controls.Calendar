using System;
using Xamarin.Forms;
using TextAlign = XamForms.Controls.TextAlign;

namespace XamForms.Controls
{
    public class CalendarButton : Button
    {        
		public static readonly BindableProperty DateProperty =
            BindableProperty.Create(nameof(Date), typeof(DateTime?), typeof(CalendarButton), null);

        public DateTime? Date
        {
            get { return (DateTime?)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(CalendarButton), false);

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly BindableProperty IsOutOfMonthProperty =
            BindableProperty.Create(nameof(IsOutOfMonth), typeof(bool), typeof(CalendarButton), false);

        public bool IsOutOfMonth
        {
            get { return (bool)GetValue(IsOutOfMonthProperty); }
            set { SetValue(IsOutOfMonthProperty, value); }
        }

        public static readonly BindableProperty TextWithoutMeasureProperty =
            BindableProperty.Create(nameof(TextWithoutMeasure), typeof(string), typeof(Button), null);

        public string TextWithoutMeasure
        {
            get
            {
                var text = (string)GetValue(TextWithoutMeasureProperty);
                return string.IsNullOrEmpty(text) ? Text : text;
            }
            set { SetValue(TextWithoutMeasureProperty, value); }
        }

		public static readonly BindableProperty BackgroundPatternProperty =
			BindableProperty.Create(nameof(BackgroundPattern), typeof(BackgroundPattern), typeof(Button), null);

		public BackgroundPattern BackgroundPattern
		{
			get { return (BackgroundPattern)GetValue(BackgroundPatternProperty); }
			set { SetValue(BackgroundPatternProperty, value); }
		}

		public static readonly BindableProperty BackgroundImageProperty =
			BindableProperty.Create(nameof(BackgroundImage), typeof(FileImageSource), typeof(Button), null);

		public FileImageSource BackgroundImage
		{
			get { return (FileImageSource)GetValue(BackgroundImageProperty); }
			set { SetValue(BackgroundImageProperty, value); }	
		}

        public static readonly BindableProperty ShapeDateProperty =
            BindableProperty.Create(nameof(ShapeDate), typeof(EnumShapeDate), typeof(Button), EnumShapeDate.None, BindingMode.TwoWay);
        
        public EnumShapeDate ShapeDate
        {
            get => (EnumShapeDate)GetValue(ShapeDateProperty);
            set
            {
                SetValue(ShapeDateProperty, value);
            }        
        }

        public static readonly BindableProperty PatternStyleProperty =
            BindableProperty.Create(nameof(PatternStyle) , typeof(EnumPatternStyle) , typeof(Button),  EnumPatternStyle.Strokes);

        public EnumPatternStyle PatternStyle
        {
            get => (EnumPatternStyle)GetValue(PatternStyleProperty);
            set => SetValue(PatternStyleProperty, value);
        }

        public static readonly BindableProperty ButtonTextAlignProperty =
            BindableProperty.Create(nameof(TextAlignment), typeof(TextAlign), typeof(CalendarButton), TextAlign.Middle);

        public TextAlign ButtonTextAlign
        {
            get => (TextAlign)GetValue(ButtonTextAlignProperty);
            set => SetValue(ButtonTextAlignProperty, value);
        }
    }
}

