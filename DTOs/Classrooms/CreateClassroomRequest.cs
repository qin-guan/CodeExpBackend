using System;
using System.ComponentModel.DataAnnotations;

namespace CodeExpBackend.DTOs.Classrooms
{
    public class CreateClassroomRequest
    {
        [Required] public Guid RequestorId { get; set; }
        [Required] public string Name { get; set; }
    }
}