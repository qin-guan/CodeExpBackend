namespace CodeExpBackend.DTOs.Questions
{
    public class CreateMcqQuestionChoiceRequest
    {
        public string Choice { get; set; }
        public bool IsAnswer { get; set; }
    }
}