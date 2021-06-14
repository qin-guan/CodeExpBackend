using AutoMapper;
using CodeExpBackend.DTOs.Quizzes;
using CodeExpBackend.Models;

namespace CodeExpBackend.Profiles
{
    public class QuizzesProfile: Profile
    {
        public QuizzesProfile()
        {
            CreateMap<CreateQuizRequest, Quiz>();
            CreateMap<Quiz, QuizResponse>();
        }
    }
}