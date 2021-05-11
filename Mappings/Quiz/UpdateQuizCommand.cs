using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.Quiz
{
    public class UpdateQuizCommand
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DateCreated { get; set; }
		public int TimesCompleted { get; set; }
		public int UserId { get; set; }
	}
}
