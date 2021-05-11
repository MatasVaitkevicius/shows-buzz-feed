/**
 * @(#) Klausimas.cs
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Models
{
	public class Question
	{
		[Key]
		public int Id { get; set; }
		public string Content { get; set; }

		public int Question_No { get; set; }

		public int Row_No { get; set; }

		public Quiz Quiz { get; set; }
		//public string QuizName { get; set; }
		//Klausimynas Quiz { get; set; }

		//Answer Answer { get; set; }

	}

}
