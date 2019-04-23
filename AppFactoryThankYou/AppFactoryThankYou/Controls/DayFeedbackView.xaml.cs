using System;
using System.Linq;
using Xamarin.Forms;

namespace AppFactoryThankYou
{
	public partial class DayFeedbackView : ContentView
	{
		public static BindableProperty MessageProperty = BindableProperty.Create(
			nameof(Message), typeof(string), typeof(DayFeedbackView),
			propertyChanged: OnMessageChanged);

		public static BindableProperty FeedbackProperty = BindableProperty.Create(
			nameof(Feedback), typeof(DayFeedback), typeof(DayFeedbackView),
			propertyChanged: OnFeedbackChanged);

		public DayFeedbackView()
		{
			InitializeComponent();
		}

		public string Message
		{
			get => (string)GetValue(MessageProperty);
			set => SetValue(MessageProperty, value);
		}

		public DayFeedback Feedback
		{
			get => (DayFeedback)GetValue(FeedbackProperty);
			set => SetValue(FeedbackProperty, value);
		}

		private static void OnMessageChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DayFeedbackView feedbackView)
			{
				feedbackView.messageLabel.Text = newValue as string;
			}
		}

		private static void OnFeedbackChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DayFeedbackView feedbackView)
			{
				feedbackView.rootLayout.BindingContext = feedbackView.Feedback;
			}
		}
	}
}
