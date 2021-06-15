using AutoMapper;
using CodeExpBackend.DTOs.Questions;
using CodeExpBackend.Models;

namespace CodeExpBackend.Profiles
{
    public class QuestionProfile: Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionResponse>();
            CreateMap<CreateMcqQuestionChoiceRequest, McqQuestionChoice>();
        }
    }
}