using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace AppFactoryThankYou
{
	[DesignTimeVisible(true)]
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			BindingContext = ViewModel;
		}

		public FeedbackViewModel ViewModel { get; } = new FeedbackViewModel(3);

		protected override void OnAppearing()
		{
			base.OnAppearing();

			MessagingCenter.Subscribe<App>(this, App.SleepMessage, OnSleep);

			ViewModel.LoadSavedData();

			UpdateCurrentPage(true);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			ViewModel.SavedCurrentData();

			MessagingCenter.Unsubscribe<App>(this, App.SleepMessage);
		}

		private void OnSleep(App app)
		{
			ViewModel.SavedCurrentData();
		}

		private void OnPreviousClicked(object sender, EventArgs e)
		{
			if (!ViewModel.HasPreviousDays)
				return;

			ViewModel.CurrentPage--;
			UpdateCurrentPage(false);
		}

		private void OnNextClicked(object sender, EventArgs e)
		{
			if (!ViewModel.HasNextDays)
				return;

			ViewModel.CurrentPage++;
			UpdateCurrentPage(true);
		}

		private async void UpdateCurrentPage(bool isForward)
		{
			// animate the slide out
			if (slideContainer.Children.Count > 0)
				await slideContainer.TranslateTo(isForward ? -Width : Width, 0, 500, Easing.SpringIn);

			// prepare the container
			slideContainer.Children.Clear();
			slideContainer.TranslationX = isForward ? Width : -Width;

			// load the next slide
			switch (ViewModel.SlideType)
			{
				case SlideType.Intro:
					slideContainer.Children.Add(new MessageView
					{
						Text = ViewModel.Messages.Intro
					});
					break;
				case SlideType.Exit:
					slideContainer.Children.Add(new MessageView
					{
						Text = ViewModel.Messages.Exit
					});
					break;
				case SlideType.WorkshopFeedback:
					slideContainer.Children.Add(new WorkshopFeedbackView
					{
						Feedback = ViewModel.Feedback,
						Message = ViewModel.Messages.Summary
					});
					break;
				case SlideType.DayFeedback:
					slideContainer.Children.Add(new DayFeedbackView
					{
						Feedback = ViewModel.Feedback.Days[ViewModel.CurrentPage],
						Message = ViewModel.Messages.Messages[ViewModel.CurrentPage]
					});
					break;
			}

			// animate the slide in
			await slideContainer.TranslateTo(0, 0, 500, Easing.SpringOut);
		}
	}
}
