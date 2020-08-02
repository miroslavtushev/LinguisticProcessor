using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SEEL.LinguisticProcessor.Splitters
{
    public class NoDelimiterSplitter : ISplitter
    {
        private static List<string> NaturalLanguageWordsByFreq { get; set; } = new List<string>();
        private static int MaxWordLen { get; set; }
        private string Input { get; set; }
        private static Dictionary<string, double> WordsCosts = new Dictionary<string, double>();

        static NoDelimiterSplitter()
        {
            #region Loading resources
            // get the English Stop Words
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName1 = "SEEL.LinguisticProcessor.Resources.NaturalLanguageWordsByFrequency.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName1))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    NaturalLanguageWordsByFreq.Add(line);
                }
            }
            #endregion

            MaxWordLen = NaturalLanguageWordsByFreq.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
            // build cost array
            var len = NaturalLanguageWordsByFreq.Count;
            for (int i = 0; i < len; i++)
            {
                if (!WordsCosts.ContainsKey(NaturalLanguageWordsByFreq[i]))
                    WordsCosts.Add(NaturalLanguageWordsByFreq[i], Math.Log((double)(i + 1) * Math.Log((double)len)));
            }
        }

        public string Split(string input)
        {
            Input = input;
           
            var cost = new List<double>();
            cost.Add(0);
            for (int i = 1; i <= Input.Length; i++)
            {
                Tuple<double, int> c = Bestmatch(i, cost);
                cost.Add(c.Item1);
            }

            int j = Input.Length;
            List<string> ret = new List<string>();
            while (j > 0)
            {
                var res = Bestmatch(j, cost);
                ret.Add(Input.Substring(j - res.Item2, res.Item2));
                j -= res.Item2;
            }
            ret.Reverse();
            return String.Join(" ", ret.ToArray());
        }

        private Tuple<double, int> Bestmatch(int i, List<double> cost)
        {
            var t = cost.GetRange(0, i);
            var reversed = Enumerable.Reverse(t).ToList();
            int c = 1;
            List<double> matches = new List<double>();
            for (int j = 0; j < reversed.Count; j++)
            {
                var sub = Input.Substring(i - j - 1, c);
                double match;
                if (WordsCosts.ContainsKey(sub))
                    match = WordsCosts[sub];
                else
                    match = float.MaxValue;
                match = match + reversed[j];
                matches.Add(match);
                c++;
            }
            return new Tuple<double, int>(matches.Min(), matches.IndexOf(matches.Min()) + 1);
        }
    }
}
