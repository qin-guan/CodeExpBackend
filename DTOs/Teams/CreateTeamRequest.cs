using System;
using System.ComponentModel.DataAnnotations;

namespace CodeExpBackend.DTOs.Teams
{
    public class CreateTeamRequest
    {
        [Required] public string Name { get; set; }
        [Required] public Guid RequestorId { get; set; }
    }
}