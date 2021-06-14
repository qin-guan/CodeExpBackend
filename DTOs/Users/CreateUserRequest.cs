using System.ComponentModel.DataAnnotations;

namespace CodeExpBackend.DTOs.Users
{
    public class CreateUserRequest
    {
        [Required] public string Name { get; set; }
    }
}