namespace CodeExpBackend.Models
{
    public class OpenEndedQuestion: Question
    {
        public bool PhotoOnly { get; set; }
        public bool NumbersOnly { get; set; }
        public int MinWordRequirement { get; set; }
        public int Points { get; set; }
        public string Answer { get; set; }
    }
}