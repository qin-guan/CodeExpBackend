using System;
using System.Collections.Generic;

namespace CodeExpBackend.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }

        public List<Classroom> AdminClassrooms { get; set; }
        public ICollection<Classroom> Classrooms { get; set; }

        public User(string name)
        {
            Name = name;
        }
    }
}