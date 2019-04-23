using Newtonsoft.Json;

namespace AppFactoryThankYou
{
	public class DayFeedback
	{
		[JsonProperty("day")]
		public int Day { get; set; }

		[JsonProperty("rating")]
		public int Rating { get; set; }

		[JsonProperty("comments")]
		public string Comments { get; set; }
	}
}
