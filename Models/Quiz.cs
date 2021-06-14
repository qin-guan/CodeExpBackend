using System;
using System.Collections.Generic;

namespace CodeExpBackend.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public List<Question> Questions { get; set; }
    }
}