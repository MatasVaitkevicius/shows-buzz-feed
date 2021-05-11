using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.Question
{
    public class UpdateQuestionCommand
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int Question_No { get; set; }

        public int Row_No { get; set; }
        public int QuizId { get; set; }
    }
}
