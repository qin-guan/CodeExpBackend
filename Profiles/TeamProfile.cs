using AutoMapper;
using CodeExpBackend.DTOs.Teams;
using CodeExpBackend.Models;

namespace CodeExpBackend.Profiles
{
    public class TeamProfile: Profile
    {
        public TeamProfile()
        {
            CreateMap<Team, TeamResponse>();
        }
    }
}