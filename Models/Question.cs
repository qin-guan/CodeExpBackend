using System;

namespace CodeExpBackend.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; }
    }
}