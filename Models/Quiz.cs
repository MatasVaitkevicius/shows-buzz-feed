using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace shows_buzz_feed.Models
{
    public class Quiz
    {
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DateCreated { get; set; }
		public int TimesCompleted { get; set; }
		public int UserId { get; set; }

		public Quiz()
		{
			Questions = new List<Question>();
		}

		public List<Question> Questions { get; set; }
	}
}
