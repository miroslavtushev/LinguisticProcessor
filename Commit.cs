using System;

namespace SEEL.LinguisticProcessor
{
    public class Commit
    {
		/// <summary>
        /// The commiter's id
        /// </summary>
        /// <value>The identifier</value>
		public int Id { get; set; }
        /// <summary>
        /// Date and time of the commit
        /// </summary>
        /// <value>The date of commit</value>
		public DateTimeOffset DateOfCommit { get; set; }

		public Commit(int id, DateTimeOffset dateOfCommit)
        {
			Id = id;
			DateOfCommit = dateOfCommit;
        }

        /// <summary>
        /// Prints the commiter's id and the date
        /// </summary>
        public void Print()
		{
			Console.WriteLine($@"{Id} commited on {DateOfCommit}");
		}
    }
}
