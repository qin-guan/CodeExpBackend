using System;
using System.Collections.Generic;

namespace CodeExpBackend.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public List<User> Users { get; set; }
    }
}