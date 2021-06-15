using System;

namespace CodeExpBackend.DTOs.Users
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public string TeamId { get; set; }
    }
}