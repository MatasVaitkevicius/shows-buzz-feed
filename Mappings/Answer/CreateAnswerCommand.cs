using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.Answer
{
    public class CreateAnswerCommand
    {
        public int Weight { get; set; }
        public string Text { get; set; }

        public int QuestionId { get; set; }
    }
}
