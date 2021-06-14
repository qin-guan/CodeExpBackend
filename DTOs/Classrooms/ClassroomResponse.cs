using System;

namespace CodeExpBackend.DTOs.Classrooms
{
    public class ClassroomResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}