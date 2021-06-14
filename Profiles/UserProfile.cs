using AutoMapper;
using CodeExpBackend.DTOs.Users;
using CodeExpBackend.Models;

namespace CodeExpBackend.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserRequest, UserResponse>();
            CreateMap<User, UserResponse>();
        }
    }
}