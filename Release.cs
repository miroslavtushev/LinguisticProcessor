using System;

namespace SEEL.LinguisticProcessor
{
	/// <summary>
    /// Represents releases of a  project
    /// </summary>
    public class Release
    {
		/// <summary>
        /// The unique id of a release
        /// </summary>
        /// <value>The identifier</value>
		public int Id { get; set; }
        /// <summary>
        /// The date when the release was published
        /// </summary>
        /// <value>The published at.</value>
        public DateTimeOffset PublishedAt { get; set; }
        /// <summary>
		/// The date when the development of the release started (approximation)
        /// </summary>
        /// <value>The start development.</value>
        public DateTimeOffset StartDevelopment { get; set; }

		public Release(int id, DateTimeOffset startDevelopment, DateTimeOffset publishedAt)
        {
			Id = id;
			StartDevelopment = startDevelopment;
			PublishedAt = publishedAt;
        }

        /// <summary>
        /// Prints out the contents of the release
        /// </summary>
        public void Print()
		{
			Console.WriteLine($@"{Id} is valid from {StartDevelopment} thru {PublishedAt}");
		}
    }
}
