using Xamarin.Forms;

namespace AppFactoryThankYou
{
	public partial class MessageView : ContentView
	{
		public static BindableProperty TextProperty = BindableProperty.Create(
			nameof(Text), typeof(string), typeof(MessageView),
			propertyChanged: OnTextChanged);

		public MessageView()
		{
			InitializeComponent();
		}

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is MessageView messageView)
			{
				messageView.messageLabel.Text = newValue as string;
			}
		}
	}
}
