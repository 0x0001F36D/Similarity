// Author: Viyrex(aka Yuyu)
// Contact: mailto:viyrex.aka.yuyu@gmail.com
// Github: https://github.com/0x0001F36D

namespace StringSimilarity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 相似度
    /// </summary>
    public static class Similarity
    {
        #region Methods

        /// <summary>
        /// 差分比對，回傳介於 0.0 至 1.0 的相似值，值越高表示越相似。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set1"></param>
        /// <param name="set2"></param>
        /// <returns></returns>
        public static double Diff<T>(IEnumerable<T> set1, IEnumerable<T> set2)
        {
            if (set1 is null || set2 is null)
                return 0;

            double all = set1.Count() + set2.Count();
            double union = set1.Union(set2).Count();
            double intersection = set1.Intersect(set2).Count();
            var jaccard = intersection / union;

            var a = union / all;
            var b = intersection / all;
            var kappa = CohensKappa(a, b, jaccard);
            return Math.Exp(kappa - 1.0);
        }

        private static double CohensKappa(double a, double b, double po)
        {
            var pe = ((1.0 - b) * a + (1.0 - a) * b);
            var kappa = (po - pe) / (1.0 - pe);
            return kappa;
        }

        #endregion Methods
    }
}