using AutoMapper;
using CodeExpBackend.DTOs.Classrooms;
using CodeExpBackend.Models;

namespace CodeExpBackend.Profiles
{
    public class ClassroomProfile: Profile
    {
        public ClassroomProfile()
        {
            CreateMap<CreateClassroomRequest, Classroom>();
            CreateMap<Classroom, ClassroomResponse>();
        }
    }
}