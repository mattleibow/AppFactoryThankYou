using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AppFactoryThankYou
{
	public class WorkshopFeedback
	{
		public WorkshopFeedback()
		{
			Id = Guid.NewGuid().ToString();
			Days = new List<DayFeedback>();
		}

		public WorkshopFeedback(int days)
			: this()
		{
			EnsureCorrectDays(days);
		}

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("full_name")]
		public string FullName { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("twitter")]
		public string Twitter { get; set; }

		[JsonProperty("days")]
		public List<DayFeedback> Days { get; set; }

		[JsonProperty("rating")]
		public int Rating { get; set; }

		[JsonProperty("comments")]
		public string Comments { get; set; }

		public void EnsureCorrectDays(int days)
		{
			for (var day = Days.Count; day < days; day++)
			{
				Days.Add(new DayFeedback
				{
					Day = day + 1
				});
			}
		}
	}
}
