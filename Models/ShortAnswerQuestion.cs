namespace CodeExpBackend.Models
{
    public class ShortAnswerQuestion: Question
    {
        public bool OneWordOnly { get; set; }
        public bool NumbersOnly { get; set; }
        public int Points { get; set; }
        public string Answer { get; set; }
    }
}