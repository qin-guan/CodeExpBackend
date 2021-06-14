using System;
using System.Collections.Generic;

namespace CodeExpBackend.Models
{
    public class Classroom
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public Guid AdminUserId { get; set; }
        public User AdminUser { get; set; }
        public List<Quiz> Quizzes { get; set; }
        public ICollection<User> Users { get; set; }
    }
}