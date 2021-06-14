using AutoMapper;
using CodeExpBackend.DTOs.Questions;
using CodeExpBackend.Models;

namespace CodeExpBackend.Profiles
{
    public class QuestionProfile: Profile
    {
        public QuestionProfile()
        {
            CreateMap<CreateQuestionRequest, Question>();
            CreateMap<Question, QuestionResponse>();
        }
    }
}