using System;
using System.Collections.Generic;

namespace ringba_test
{
    public class Prefix
    {
        public int NumberOfOccurrences { get;private set; }
        public string PrefixCharacters { get;private set; }
        public List<string> WordsWithThisPrefix { get; set; }

        public Prefix(string prefix, string word)
        {
            this.NumberOfOccurrences = 1;
            this.PrefixCharacters = prefix;
            this.WordsWithThisPrefix = new List<string>();
            addToWordsWithThisPrefix(word);
        }

        public void addToWordsWithThisPrefix(string word)
        {
            if (!this.WordsWithThisPrefix.Contains(word.ToLower()))
            {
                this.WordsWithThisPrefix.Add(word.ToLower());
            }
        }

        public void addToNumberOfThisPrefix(){
            this.NumberOfOccurrences++;
        }
    }
}