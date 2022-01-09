using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bi_grams
{
    class Program
    {
        private static int NgramType=2;
        static void Main(string[] args)
        {
            var corpus = GetCorpus();
            corpus = NormalizeCorpus(corpus);
            var nGrams = GetNGrams(corpus,NgramType);
            List<(string,int)> nGramsCountMatrix = GetNGramCountMatrix(nGrams);
            PrintNGramsMatrix(nGramsCountMatrix);
        }

        private static List<(string, int)> GetNGramCountMatrix(List<string> biGrams)
        {
            var values = biGrams.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => new { Element = y.Key, Counter = y.Count() })
                .Select(x=> (x.Element,x.Counter))
                .ToList();
            
            return values;
        }

        private static void PrintNGramsMatrix(List<(string, int)> nGramsCountMatrix)
        {
            Console.WriteLine("N-Grams: N = "+NgramType);
            foreach (var ngrams in nGramsCountMatrix)
            {
                Console.WriteLine($"{ngrams.Item1}: Count = {ngrams.Item2}");
            }
        }

        private static List<string> GetNGrams(string corpus,int nGramType)
        {
            var allWords = corpus.Split(" ",StringSplitOptions.RemoveEmptyEntries);

            var nGramList = new List<string>();

            
            for (int i = 0; i < allWords.Length; i++)
            {
                var nGram = string.Empty;
                
                if(i+nGramType>=allWords.Length) continue;
                
                for (int j = i; j <  i+nGramType; j++)
                {
                    nGram += allWords[j] + " ";
                }
                nGramList.Add(nGram.Trim());
            }
            return nGramList;

        }

        private static string NormalizeCorpus(string text)
        {
            //remove all special characters
            var str = text.Replace(".", "")
                .Replace(",", "")
                .Replace("#", "")
                .Replace("@", "")
                .Replace("'", "")
                .Replace("?","")
                //convert to lower caps
                .ToLower();

            return str;
        }

        static string GetCorpus()
        {
            var text =  File.ReadAllText("corpus.txt");
            text = text.Replace(Environment.NewLine, " ");
            return text;
        }
    }
}