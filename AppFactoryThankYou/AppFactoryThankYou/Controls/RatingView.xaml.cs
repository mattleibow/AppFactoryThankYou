using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFactoryThankYou
{
	public partial class RatingView : ContentView
	{
		public static BindableProperty RatingProperty = BindableProperty.Create(
			nameof(Rating), typeof(int), typeof(RatingView),
			defaultBindingMode: BindingMode.TwoWay,
			propertyChanged: OnRatingChanged);

		public RatingView()
		{
			InitializeComponent();

			SizeChanged += OnSizeChanged;
		}

		public int Rating
		{
			get => (int)GetValue(RatingProperty);
			set => SetValue(RatingProperty, value);
		}

		private static void OnRatingChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is RatingView ratingView)
			{
				ratingView.UpdateRatingIndicator();
			}
		}

		private void OnSetRating(object sender, EventArgs e)
		{
			if (sender is Button button && button.CommandParameter is string param && int.TryParse(param, out var rating))
			{
				Rating = rating;
				UpdateRatingIndicator();
			}
		}

		private void OnSizeChanged(object sender, EventArgs e)
		{
			UpdateRatingIndicator();
		}

		private void UpdateRatingIndicator()
		{
			var ratingString = Rating.ToString();
			var buttons = ratingGrid.Children.OfType<Button>();
			var button = buttons.FirstOrDefault(b => ratingString.Equals(b.CommandParameter as string, StringComparison.OrdinalIgnoreCase));

			if (button == null)
			{
				ratingSelectionBox.Opacity = 0;
			}
			else
			{
				ratingSelectionBox.FadeTo(1, 500);
				ratingSelectionBox.LayoutTo(button.Bounds, 500);
			}
		}
	}
}
