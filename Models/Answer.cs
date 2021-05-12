using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace shows_buzz_feed.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public int Weight { get; set; }
        public string Text { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
