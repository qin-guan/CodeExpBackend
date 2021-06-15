using System;

namespace CodeExpBackend.DTOs.Teams
{
    public class TeamResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
    }
}