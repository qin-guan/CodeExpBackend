using System.Collections.Generic;

namespace CodeExpBackend.Models
{
    public class McqQuestion: Question
    {
        public IEnumerable<McqQuestionChoice> McqQuestionChoices { get; set; }
        public int AnswerIndex { get; set; }
    }
}