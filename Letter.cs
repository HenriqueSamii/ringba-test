namespace ringba_test
{
    public class Letter
    {
        public int NumberOfOccurrences { get;private set; }
        public char NameOfLetter { get;private set; }

        public Letter(char letter)
        {
            this.NumberOfOccurrences = 1;
            this.NameOfLetter = letter;
        }
        public void addToNumberOfThisLetter(){
            this.NumberOfOccurrences++;
        }
    }
}