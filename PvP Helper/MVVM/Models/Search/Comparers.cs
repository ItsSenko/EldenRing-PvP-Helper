using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Search
{
    public class Comparers
    {
        public static int LevenshteinDistance(string x, string y)
        {
            if (string.IsNullOrEmpty(x))
            {
                if (!string.IsNullOrEmpty(y))
                {
                    return y.Length;
                }
                return 0;
            }

            if (string.IsNullOrEmpty(y))
            {
                if (!string.IsNullOrEmpty(x))
                {
                    return x.Length;
                }
                return 0;
            }

            int cost;
            int[,] d = new int[x.Length + 1, y.Length + 1];
            int min1;
            int min2;
            int min3;

            for (int i = 0; i <= d.GetUpperBound(0); i += 1)
            {
                d[i, 0] = i;
            }

            for (int i = 0; i <= d.GetUpperBound(1); i += 1)
            {
                d[0, i] = i;
            }

            for (int i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (int j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    cost = (x[i - 1] != y[j - 1]) ? 1 : 0;

                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }
    }
}
