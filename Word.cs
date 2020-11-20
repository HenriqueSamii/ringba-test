namespace ringba_test
{
    public class Word
    {
        public int NumberOfOccurrences { get;private set; }
        public string NameOfWord { get;private set; }
        public Word(string word)
        {
            this.NumberOfOccurrences = 1;
            this.NameOfWord = word;
        }
        public void addToNumberOfThisWord(){
            this.NumberOfOccurrences++;
        }
    }
}