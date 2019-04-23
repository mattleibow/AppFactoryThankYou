using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppFactoryThankYou
{
	public class FeedbackViewModel : BindableObject
	{
		private static readonly Uri DbUri = new Uri("https://appfactory-feedback.documents.azure.com:443/");
		private const string DbKey = "NF56a7KaFtlCEFq1o3oF9jUm8DXIlRvTF1XftHIK23efq6zaZ1QXS3ya7GeDL1wxc2zMAppOEhMlakNuqert3Q==";
		private const string DbDatabase = "feedback-db";
		private const string DbCollection = "feedback";

		private const string EmbeddedDataPath = "AppFactoryThankYou.Data.Messages.json";
		private const string CurrentPageKey = "CurrentPage";
		private static readonly string SavedDataPath = Path.Combine(FileSystem.AppDataDirectory, "saved.json");

		private Task<DocumentClient> initTask;

		private string statusMessage;
		private int currentPage;
		private WorkshopFeedback feedback;
		private MessagesJson messages;

		public FeedbackViewModel(int days)
		{
			Feedback = new WorkshopFeedback(days);
			LoadEmbeddedMessages();
		}

		public string StatusMessage
		{
			get { return statusMessage; }
			set
			{
				statusMessage = value;
				OnPropertyChanged();
			}
		}

		public int CurrentPage
		{
			get { return currentPage; }
			set
			{
				currentPage = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(HasPreviousDays));
				OnPropertyChanged(nameof(HasNextDays));
				OnPropertyChanged(nameof(SlideType));

				if (!HasNextDays)
					UploadData();
			}
		}

		public WorkshopFeedback Feedback
		{
			get { return feedback; }
			set
			{
				feedback = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(HasPreviousDays));
				OnPropertyChanged(nameof(HasNextDays));
				OnPropertyChanged(nameof(SlideType));
			}
		}

		public MessagesJson Messages
		{
			get { return messages; }
			set
			{
				messages = value;
				OnPropertyChanged();
			}
		}

		public bool HasPreviousDays => CurrentPage > -1;

		public bool HasNextDays => CurrentPage < Feedback.Days.Count + 1;

		public SlideType SlideType
		{
			get
			{
				if (CurrentPage < 0)
					return SlideType.Intro;
				else if (CurrentPage >= Feedback.Days.Count + 1)
					return SlideType.Exit;
				else if (CurrentPage >= Feedback.Days.Count)
					return SlideType.WorkshopFeedback;
				else
					return SlideType.DayFeedback;
			}
		}

		public void SavedCurrentData()
		{
			// serialize the feedback form
			var json = JsonConvert.SerializeObject(Feedback);

			// save the data to a local file
			File.WriteAllText(SavedDataPath, json);

			// save the current page
			Preferences.Set(CurrentPageKey, CurrentPage);
		}

		public void LoadSavedData()
		{
			// check if there is local data
			if (File.Exists(SavedDataPath))
			{
				// read the local data
				var json = File.ReadAllText(SavedDataPath);

				// deserialize the data
				var feedback = JsonConvert.DeserializeObject<WorkshopFeedback>(json);

				// update the view model with the local data
				feedback.EnsureCorrectDays(Feedback.Days.Count);
				Feedback = feedback;
			}

			// load the current page
			CurrentPage = Preferences.Get(CurrentPageKey, -1);
		}

		private void LoadEmbeddedMessages()
		{
			// read the messages from the embedded data
			var assembly = typeof(FeedbackViewModel).Assembly;
			using (var stream = assembly.GetManifestResourceStream(EmbeddedDataPath))
			using (var reader = new StreamReader(stream))
			{
				// deserialize the messages
				Messages = JsonConvert.DeserializeObject<MessagesJson>(reader.ReadToEnd());
			}
		}

		private async void UploadData()
		{
			StatusMessage = "Uploading feedback...";

			var client = await CreateClientAsync();
			var uri = UriFactory.CreateDocumentCollectionUri(DbDatabase, DbCollection);

			// see if we have already uploaded something
			var existingCount = 0;
			try
			{
				existingCount = await client.CreateDocumentQuery<WorkshopFeedback>(uri)
					.Where(f => f.Id == Feedback.Id)
					.CountAsync();
			}
			catch (Exception ex)
			{
				StatusMessage = "There was an error: " + ex.Message;
			}

			// now upload
			try
			{
				if (existingCount == 0)
				{
					var doc = await client.CreateDocumentAsync(uri, Feedback);
					Feedback.Id = doc.Resource.Id;
				}
				else
				{
					var extUri = UriFactory.CreateDocumentUri(DbDatabase, DbCollection, Feedback.Id);
					await client.ReplaceDocumentAsync(extUri, Feedback);
				}

				StatusMessage = "Upload complete!";
			}
			catch (Exception ex)
			{
				StatusMessage = "There was an error: " + ex.Message;
			}
		}

		private Task<DocumentClient> CreateClientAsync() =>
			initTask ?? (initTask = Task.Run(() => new DocumentClient(DbUri, DbKey)));
	}
}
