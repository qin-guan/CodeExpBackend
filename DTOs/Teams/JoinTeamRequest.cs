using System;
using System.ComponentModel.DataAnnotations;

namespace CodeExpBackend.DTOs.Teams
{
    public class JoinTeamRequest
    {
        [Required] public Guid RequestorId { get; set; }
    }
}