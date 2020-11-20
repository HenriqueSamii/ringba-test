using System;
using System.Collections.Generic;
using System.Linq;

namespace ringba_test
{
    class Program
    {
        private static readonly string URL_OF_THE_TEST_FILE = "http://ringba-test-html.s3-website-us-west-1.amazonaws.com/TestQuestions/output.txt";
        private static readonly List<string> TWO_CHARACTER_PREFIXES =
                new List<string> { "bi", "co", "de", "em", "im", "in", "on", "re", "un", "up" };
        private static readonly List<string> MORE_THAN_TWO_CHARACTER_PREFIXES =
                new List<string>
                                {
                                    "ambi","anti","astro","con","dis","extra","fore",
                                    "hetero","hind","homo","inter","mal","mid","mis",
                                    "mono","non","pan","ped","post","pre","pro","semi",
                                    "sub","sur","trans","tri","twi","ultra","uni","under","bio"
                                };
        private static List<Letter> lettersAndThereOccurrences = new List<Letter>();
        private static int numberOfCapitalizedLetters = 0;
        private static List<Word> wordsAndThereOccurrences = new List<Word>();
        private static List<Prefix> twoCharacterPrefixAndThereOccurrences = new List<Prefix>();
        private static List<Prefix> moreThanTwoCharacterAndThereOccurrences = new List<Prefix>();

        static void Main(string[] args)
        {
            downloadUtputFromUrlToTxtFile();
            var stringOutput = setStringOutput();
            int increment = 10000;
            //TODO: Make Divide to Conquer function multi-thread capable
            //Divide to Conquer function is used to divide the main collection of character in to
            //smaller collections so that it can be processed in a shorter amount of time  and it doesn’t overload the system
            divideToConquer(stringOutput, increment);
            printProcessedData();
        }

        private static void divideToConquer(string stringOutput, int increment)
        {
            int indexStringOutputStart = 0;
            var indexStringOutputEnd = increment;
            do
            {
                while (indexStringOutputEnd < stringOutput.Count() && !char.IsUpper(stringOutput[indexStringOutputEnd + 1]))
                {
                    indexStringOutputEnd++;
                }
                outputIteration(stringOutput, indexStringOutputStart, indexStringOutputEnd);
                if (indexStringOutputStart + indexStringOutputEnd + 1 < stringOutput.Count())
                {
                    indexStringOutputStart += indexStringOutputEnd + 1;
                }
                else
                {
                    indexStringOutputStart = stringOutput.Count();
                }
                if (indexStringOutputEnd + increment < stringOutput.Count())
                {
                    indexStringOutputEnd += increment;
                }
                else
                {
                    indexStringOutputEnd = stringOutput.Count();
                }
                //System.Console.WriteLine(indexStringOutputEnd);
            } while (indexStringOutputEnd < stringOutput.Count());
        }

        private static void printProcessedData()
        {
            printLettersAndThereOccurrences();
            marginBottom();
            printNumberOfCapitalizedLetters();
            marginBottom();
            printMostCommonWordAndTheNumberOfTimesItHasBeenSeen();
            marginBottom();
            printMostCommon2CharacterPrefixAndTheNumberOfOccurrences();
            marginBottom();
            printMostCommonCharacterPrefixAndTheNumberOfOccurrencesLengthGreaterThan1();
        }

        private static void marginBottom()
        {
            System.Console.WriteLine("\n");
        }

        private static void printMostCommonCharacterPrefixAndTheNumberOfOccurrencesLengthGreaterThan1()
        {
            var allPrfixesOccurrences = new List<Prefix>();
            allPrfixesOccurrences.AddRange(moreThanTwoCharacterAndThereOccurrences);
            allPrfixesOccurrences.AddRange(twoCharacterPrefixAndThereOccurrences);
            System.Console.WriteLine(pedixText("The common and complex prefix of any length greater than 1", allPrfixesOccurrences));
        }

        private static void printMostCommon2CharacterPrefixAndTheNumberOfOccurrences()
        {
            System.Console.WriteLine(pedixText("The most common 2 character prefix", twoCharacterPrefixAndThereOccurrences));
        }

        private static string pedixText(string v, List<Prefix> prefixList)
        {
            if (prefixList.Count <= 0)
            {
                return $"+++ {v} was not encountered on this file";
            }
            var prefixToPrint = prefixList[0];
            foreach (var prefix in prefixList)
            {
                if (prefix.NumberOfOccurrences > prefixToPrint.NumberOfOccurrences)
                {
                    prefixToPrint = prefix;
                }
            }
            var returnS = $"+++ {v} was '{prefixToPrint.PrefixCharacters}' with {prefixToPrint.NumberOfOccurrences} occurrences";
            returnS += "\nAppearing on:\n";
            foreach (var word in prefixToPrint.WordsWithThisPrefix)
            {
                returnS += $"{word}, ";
            }
            returnS = returnS.Remove(returnS.Length - 2);
            return returnS;
        }

        private static void printMostCommonWordAndTheNumberOfTimesItHasBeenSeen()
        {
            if (wordsAndThereOccurrences.Count > 0)
            {
                bool isThereNoRepeatingWords = false;
                var wordToPrint = wordsAndThereOccurrences[0];
                if (wordsAndThereOccurrences.Count == 1)
                {
                    isThereNoRepeatingWords = true;
                }
                else
                {
                    foreach (var word in wordsAndThereOccurrences)
                    {
                        if (word.NumberOfOccurrences > wordToPrint.NumberOfOccurrences)
                        {
                            wordToPrint = word;
                            isThereNoRepeatingWords = true;
                        }
                    }
                }
                if (isThereNoRepeatingWords)
                {
                    System.Console.WriteLine($"+++ The most common word was '{wordToPrint.NameOfWord}' with {wordToPrint.NumberOfOccurrences} occurrences");
                }
                else
                {
                    System.Console.WriteLine("+++ All words in this file are single and there is no more than 1 of each word");
                }
            }
            else
            {
                System.Console.WriteLine("+++ Couldn't find a word in this file");
            }
        }

        private static void printNumberOfCapitalizedLetters()
        {
            System.Console.WriteLine($"+++ Number of capitalized letter: {numberOfCapitalizedLetters}");
        }

        private static void printLettersAndThereOccurrences()
        {
            System.Console.WriteLine("+++ How many of each letter are in the file");
            foreach (var letter in lettersAndThereOccurrences)
            {
                System.Console.WriteLine($"-- Letter: {letter.NameOfLetter} - Number of occurrences: {letter.NumberOfOccurrences}");
            }
        }

        private static void outputIteration(string output, int beginning, int end)
        {
            string wordHolder = "";
            for (int i = beginning; i < end; i++)
            {
                wordHolder += output[i];

                lettersAndThereOccurrencesAdd(output[i]);

                if (char.IsUpper(output[i]))
                {
                    numberOfCapitalizedLetters++;
                }

                if (i + 1 == output.Length || char.IsUpper(output[i + 1]))
                {
                    wordsAndThereOccurrencesAdd(wordHolder);
                    characterPrefixAndThereOccurrencesAdd(wordHolder);
                    wordHolder = "";
                }
            }
        }

        private static bool characterPrefixAndThereOccurrencesAdd(string word)
        {
            int indexOfPrefixDictionary = MORE_THAN_TWO_CHARACTER_PREFIXES.FindIndex(a => word.ToLower().StartsWith(a.ToLower()));
            if (indexOfPrefixDictionary != -1 && word.Length > MORE_THAN_TWO_CHARACTER_PREFIXES[indexOfPrefixDictionary].Length)
            {
                int indexOfMoreThanTwoCharacterAndThereOccurrences = moreThanTwoCharacterAndThereOccurrences.
                    FindIndex(
                        a => a.PrefixCharacters == MORE_THAN_TWO_CHARACTER_PREFIXES[indexOfPrefixDictionary]
                    );
                if (indexOfMoreThanTwoCharacterAndThereOccurrences == -1)
                {
                    moreThanTwoCharacterAndThereOccurrences.Add(new Prefix(MORE_THAN_TWO_CHARACTER_PREFIXES[indexOfPrefixDictionary],word));
                }
                else
                {
                    moreThanTwoCharacterAndThereOccurrences[indexOfMoreThanTwoCharacterAndThereOccurrences].addToNumberOfThisPrefix();
                    moreThanTwoCharacterAndThereOccurrences[indexOfMoreThanTwoCharacterAndThereOccurrences].addToWordsWithThisPrefix(word);
                }
                return true;
            }

            indexOfPrefixDictionary = TWO_CHARACTER_PREFIXES.FindIndex(a => word.ToLower().StartsWith(a.ToLower()));
            if (indexOfPrefixDictionary != -1 && word.Length > TWO_CHARACTER_PREFIXES[indexOfPrefixDictionary].Length)
            {
                int indexOfTwoCharacterPrefixAndThereOccurrences = twoCharacterPrefixAndThereOccurrences.
                    FindIndex(
                        a => a.PrefixCharacters == TWO_CHARACTER_PREFIXES[indexOfPrefixDictionary]
                    );
                if (indexOfTwoCharacterPrefixAndThereOccurrences == -1)
                {
                    twoCharacterPrefixAndThereOccurrences.Add(new Prefix(TWO_CHARACTER_PREFIXES[indexOfPrefixDictionary],word));
                }
                else
                {
                    twoCharacterPrefixAndThereOccurrences[indexOfTwoCharacterPrefixAndThereOccurrences].addToNumberOfThisPrefix();
                    twoCharacterPrefixAndThereOccurrences[indexOfTwoCharacterPrefixAndThereOccurrences].addToWordsWithThisPrefix(word);
                }
                return true;
            }

            return false;
        }

        private static void wordsAndThereOccurrencesAdd(string word)
        {
            int index = wordsAndThereOccurrences.FindIndex(a => a.NameOfWord.ToLower() == word.ToLower());
            if (index == -1)
            {
                wordsAndThereOccurrences.Add(new Word(word));
            }
            else
            {
                wordsAndThereOccurrences[index].addToNumberOfThisWord();
            }
        }

        private static void lettersAndThereOccurrencesAdd(char letter)
        {
            letter = Char.ToLower(letter);
            int index = lettersAndThereOccurrences.FindIndex(a => a.NameOfLetter == letter);
            if (index == -1)
            {
                lettersAndThereOccurrences.Add(new Letter(letter));
            }
            else
            {
                lettersAndThereOccurrences[index].addToNumberOfThisLetter();
            }
        }

        private static string setStringOutput()
        {
            return System.IO.File.ReadAllText(@"..\output.txt");
        }

        private static void downloadUtputFromUrlToTxtFile()
        {
            var wc = new System.Net.WebClient();
            wc.DownloadFile(URL_OF_THE_TEST_FILE, @"..\output.txt");
        }
    }
}