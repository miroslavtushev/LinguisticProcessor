using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEEL.LinguisticProcessor.Spellcheck
{
    /// <summary>
    /// Computes LevenshteinDistance with optional <see cref="MaxAllowedDistance"/> parameter
    /// </summary>
    public class DamerauLevenshteinDistance
    {
        public int MaxAllowedDistance { get; set; } = int.MaxValue;
        private int[,] _distance;

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        /// <remarks>Change <see cref="MaxAllowedDistance"/> to achieve O(m*k) time complexity</remarks>
        public int? RunAlgorithm(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            _distance = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; _distance[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; _distance[0, j] = j++)
            {
            }

            // Step 3
            int curMin;
            for (int i = 1; i <= n; i++)
            {
                curMin = int.MaxValue;
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    _distance[i, j] = Math.Min(
                        Math.Min(_distance[i - 1, j] + 1, _distance[i, j - 1] + 1),
                        _distance[i - 1, j - 1] + cost);

                    if (i > 1 && j > 1 && s[i-1] == t[j - 2] && s[i - 2] == t[j - 1])
                        _distance[i, j] = Math.Min(_distance[i, j],
                                       _distance[i - 2, j - 2] + cost);  // transposition

                    // because the distance will only stay same or go higher next row
                    if (_distance[i, j] < curMin) curMin = _distance[i, j];
                }
                // when finished calculating the row, check if we've exceeded the boundary
                if (curMin > MaxAllowedDistance) return null;
            }
            // Step 7
            return _distance[n, m] > MaxAllowedDistance ? null : (int?)_distance[n, m];
        }

        /// <summary>
        /// Given maximum # of allowed operations, check if 1 word is a misspelling of another
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns>True or False</returns>
        public bool IsMisspelling(string word1, string word2)
        {
            if (RunAlgorithm(word1, word2).HasValue) return true;
            else return false;
        }
    }
}
