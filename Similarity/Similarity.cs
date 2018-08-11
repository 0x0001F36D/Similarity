// Author: Viyrex(aka Yuyu)
// Contact: mailto:viyrex.aka.yuyu@gmail.com
// Github: https://github.com/0x0001F36D

using System;
using System.Collections.Generic;
using System.Linq;



/// <summary>
/// 相似度
/// </summary>
public interface ISimilarity
{

    /// <summary>
    /// 差分比對，回傳介於 0.0 至 1.0 的相似值，值越高表示越相似。
    /// </summary> 
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="cmp"></param>
    /// <returns></returns>
    double Diff<T>(IEnumerable<T> src, IEnumerable<T> cmp);
}

internal sealed class Similarity : ISimilarity
{
    #region Methods

    public double Diff<T>(IEnumerable<T> src, IEnumerable<T> cmp)
    {
        if (src is null || cmp is null)
            return 0;

        double all = src.Count() + cmp.Count();
        double union = src.Union(cmp).Count();
        double intersection = src.Intersect(cmp).Count();
        var jaccard = intersection / union;

        var a = union / all;
        var b = intersection / all;
        var kappa = CohensKappa(a, b, jaccard);
        return Math.Exp(kappa - 1.0);
    }

    private double CohensKappa(double a, double b, double po)
    {
        var pe = ((1.0 - b) * a + (1.0 - a) * b);
        var kappa = (po - pe) / (1.0 - pe);
        return kappa;
    }

    #endregion Methods
}


public static class SimilarityExtension
{
    private static ISimilarity s_similarity;

    /// <summary>
    /// 預設的相似度評估物件
    /// </summary>
    public static ISimilarity Default { get; }

    /// <summary>
    /// 取得或設定相似度評估物件
    /// </summary>
    public static ISimilarity Similarity
    {
        get => s_similarity;
        set => s_similarity = value ?? throw new ArgumentNullException();
    }

    static SimilarityExtension()
    {
        Default = Similarity = new Similarity();
    }


    /// <summary>
    /// 差分比對，回傳介於 0.0 至 1.0 的相似值，值越高表示越相似。
    /// </summary> 
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="cmp"></param>
    /// <returns></returns>
    public static double Diff<T>(this IEnumerable<T> src, IEnumerable<T> cmp)
    {
        return Similarity.Diff(src, cmp);
    }


}