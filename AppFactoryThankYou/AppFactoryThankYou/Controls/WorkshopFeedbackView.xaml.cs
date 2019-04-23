using Xamarin.Forms;

namespace AppFactoryThankYou
{
	public partial class WorkshopFeedbackView : ContentView
	{
		public static BindableProperty MessageProperty = BindableProperty.Create(
			nameof(Message), typeof(string), typeof(WorkshopFeedbackView),
			propertyChanged: OnMessageChanged);

		public static BindableProperty FeedbackProperty = BindableProperty.Create(
			nameof(Feedback), typeof(WorkshopFeedback), typeof(WorkshopFeedbackView),
			propertyChanged: OnFeedbackChanged);

		public WorkshopFeedbackView()
		{
			InitializeComponent();
		}

		public string Message
		{
			get => (string)GetValue(MessageProperty);
			set => SetValue(MessageProperty, value);
		}

		public WorkshopFeedback Feedback
		{
			get => (WorkshopFeedback)GetValue(FeedbackProperty);
			set => SetValue(FeedbackProperty, value);
		}

		private static void OnMessageChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is WorkshopFeedbackView feedbackView)
			{
				feedbackView.messageLabel.Text = newValue as string;
			}
		}

		private static void OnFeedbackChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is WorkshopFeedbackView feedbackView)
			{
				feedbackView.rootLayout.BindingContext = feedbackView.Feedback;
			}
		}
	}
}
