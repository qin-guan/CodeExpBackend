using System;

namespace CodeExpBackend.Models
{
    public class McqQuestionChoice
    {
        public Guid Id { get; set; }
        public string Choice { get; set; }
        public bool IsAnswer { get; set; }
        
        public Guid McqQuestionId { get; set; }
        public McqQuestion McqQuestion { get; set; }
    }
}