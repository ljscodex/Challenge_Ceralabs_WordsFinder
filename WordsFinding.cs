namespace Challenge
{
using System;
using System.Collections.Generic;
using System.Linq;
    using System.Text;
    using Challenge.Models;

    public class WordsFinder : iWordsFinder
{
    private HashSet<string> _matrixWords;
    private int _rows;
    private int _cols;

    public string ValidationMessage { get; private set; }

    public WordsFinder(IEnumerable<string> matrix)
    {
        //The WordFinder constructor receives a set of strings which represents a character matrix.
        _matrixWords = new HashSet<string>();
        _rows = matrix.Count();

        // a few validations before we start to consume memory.
        if ( _rows < 1 )
        {
             ValidationMessage= "Matrix is Empty!";
             return;
        }
        //all strings contain the same number of characters
        _cols = matrix.First().Length;

        //The matrix size does not exceed 64x64
        if ( _rows > 64 && _cols > 64 )
        {
            ValidationMessage =   "Matrix is Outside the limits!";
            return;
          
        }
        // Extract words from the matrix
        // We use the Parallel class to improve performance based on we dont need any order to find the words.
        Parallel.For (0,  _rows,  i =>
            {
                string row = matrix.ElementAt(i);
                _matrixWords.Add(row); // Horizontal words

                // Vertical words
                if (i == 0)
                {
                    for (int j = 0; j < _cols; j++)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int k = 0; k < _rows; k++)
                        {
                            sb.Append(matrix.ElementAt(k)[j]);
                        }
                        _matrixWords.Add(sb.ToString());
                    }
                }
            });
            //If no words are found, the "Find" method should return an empty set of strings.

    }

    public IEnumerable<string> Find(IEnumerable<string> wordstream)
    {
        var foundWords = new HashSet<string>();
        var wordCount = new Dictionary<string, int>();


        foreach (var word in wordstream)
        {
            bool exists = SubstringExistsInMatrix(_matrixWords, word);
            if (exists)
            {
                //If any word in the word stream is found more than once within the stream, the search results should count it only once
                foundWords.Add(word);
                wordCount[word] = 1;
            }
        }
        //The "Find" method should return the top 10 most repeated words from the word stream found in the matrix
        return wordCount.Keys.OrderBy(w => w).Take(10);
    }

    private static bool SubstringExistsInMatrix(IEnumerable<string> matrix, string substring)
    {
        // search for words inside the matrix element using Linq
        bool exists = matrix.Any(s => s.Contains(substring, StringComparison.OrdinalIgnoreCase));
        return  exists;
    }

}

}