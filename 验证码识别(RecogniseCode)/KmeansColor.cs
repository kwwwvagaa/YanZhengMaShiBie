using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RecogniseCode
{
    public static class KmeansColor
    {
        /*
  * 聚类函数主体。
  * 针对一维 double 数组。指定聚类数目 k。
  * 将数据聚成 k 类。
  */
        public static Color[][] cluster(Color[] p, int k)
        {
            int intRunCount = 0;
        start:
            intRunCount++;
            // 存放聚类旧的聚类中心
            Color[] c = new Color[k];
            // 存放新计算的聚类中心
            Color[] nc = new Color[k];
            // 存放放回结果
            Color[][] g;
            // 初始化聚类中心
            // 经典方法是随机选取 k 个
            // 本例中采用前 k 个作为聚类中心
            // 聚类中心的选取不影响最终结果
            Random r = new Random();

            int intValue = -16777216 / k;
            for (int i = 0; i < k; i++)
            {
                c[i] = Color.FromArgb(r.Next(1, 255), r.Next(1, 255), r.Next(1, 255));
            }
            // 循环聚类，更新聚类中心
            // 到聚类中心不变为止
            while (true)
            {
                // 根据聚类中心将元素分类
                g = group(p, c);
                // 计算分类后的聚类中心
                for (int i = 0; i < g.Length; i++)
                {
                    nc[i] = center(g[i]);
                }
                // 如果聚类中心不同
                if (!equal(nc, c))
                {
                    // 为下一次聚类准备
                    c = nc;
                    nc = new Color[k];
                }
                else // 聚类结束
                {
                    foreach (var item in g)
                    {
                        if (intRunCount < 10000 && item.Length <= 50)
                        {
                            goto start;
                        }
                    }
                    break;
                }
            }
            // 返回聚类结果
            return g;
        }
        /*
         * 聚类中心函数
         * 简单的一维聚类返回其算数平均值
         * 可扩展
         */
        public static Color center(Color[] p)
        {
            if (p.Length <= 0)
            {
                Random r = new Random();
                return Color.FromArgb(r.Next(1, 255), r.Next(1, 255), r.Next(1, 255));
            }
            int intSumR = 0, intSumG = 0, intSumB = 0;
            foreach (var item in p)
            {
                intSumR += item.R;
                intSumG += item.G;
                intSumB += item.B;
            }
            int intLength = p.Length;
            return Color.FromArgb(intSumR / intLength, intSumG / intLength, intSumB / intLength);
        }
        /*
         * 给定 double 型数组 p 和聚类中心 c。
         * 根据 c 将 p 中元素聚类。返回二维数组。
         * 存放各组元素。
         */
        public static Color[][] group(Color[] p, Color[] c)
        {
            // 中间变量，用来分组标记
            int[] gi = new int[p.Length];
            // 考察每一个元素 pi 同聚类中心 cj 的距离
            // pi 与 cj 的距离最小则归为 j 类
            for (int i = 0; i < p.Length; i++)
            {
                // 存放距离
                double[] d = new double[c.Length];
                // 计算到每个聚类中心的距离
                for (int j = 0; j < c.Length; j++)
                {
                    d[j] = distance(p[i], c[j]);
                }
                // 找出最小距离
                int ci = min(d);
                // 标记属于哪一组
                gi[i] = ci;
            }
            // 存放分组结果
            Color[][] g = new Color[c.Length][];
            // 遍历每个聚类中心，分组
            for (int i = 0; i < c.Length; i++)
            {
                // 中间变量，记录聚类后每一组的大小
                int s = 0;
                // 计算每一组的长度
                for (int j = 0; j < gi.Length; j++)
                    if (gi[j] == i)
                        s++;
                // 存储每一组的成员
                g[i] = new Color[s];
                s = 0;
                // 根据分组标记将各元素归位
                for (int j = 0; j < gi.Length; j++)
                    if (gi[j] == i)
                    {
                        g[i][s] = p[j];
                        s++;
                    }
            }
            // 返回分组结果
            return g;
        }

        /*
         * 计算两个点之间的距离， 这里采用最简单得一维欧氏距离， 可扩展。
         */
        public static double distance(Color x, Color y)
        {

            return Math.Sqrt(Math.Pow(x.R - y.R, 2) + Math.Pow(x.G - y.G, 2) + Math.Pow(x.B - y.B, 2));
            // return Math.Abs(x.R - y.R) + Math.Abs(x.G - y.G) + Math.Abs(x.B - y.B);
        }

        /*
         * 返回给定 double 数组各元素之和。
         */
        public static double sum(double[] p)
        {
            double sum = 0.0;
            for (int i = 0; i < p.Length; i++)
                sum += p[i];
            return sum;
        }

        /*
         * 给定 double 类型数组，返回最小值得下标。
         */
        public static int min(double[] p)
        {
            int i = 0;
            double m = p[0];
            for (int j = 1; j < p.Length; j++)
            {
                if (p[j] < m)
                {
                    i = j;
                    m = p[j];
                }
            }
            return i;
        }

        /*
         * 判断两个 double 数组是否相等。 长度一样且对应位置值相同返回真。
         */
        public static bool equal(Color[] a, Color[] b)
        {
            if (a.Length != b.Length)
                return false;
            else
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i].Name != b[i].Name)
                        return false;
                }
            }
            return true;
        }
    }
}
