using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AforgenetTest
{
    public class Kmeans
    {
        static int SUCCESS = 1;
        static int FAILURE = 0;
        static int TRUE = 1;
        static int FALSE = 0;
        static int MAXVECTDIM = 20;
        static int MAXPATTERN = 20;
        static int MAXCLUSTER = 10;

        class aCluster
        {
            public double[] Center = new double[MAXVECTDIM];
            public int[] Member = new int[Kmeans.MAXPATTERN];  //Index of Vectors belonging to this cluster  
            public int NumMembers;
        }

        class aVector
        {
            public double[] Center = new double[Kmeans.MAXVECTDIM];
            public int Size;
        }


        double[,] Pattern = new double[MAXPATTERN, MAXVECTDIM + 1];
        //map<TPoint, RGBQUAD> m_mapPattern;  
        aCluster[] Cluster = new aCluster[MAXCLUSTER];
        int NumPatterns;          // Number of patterns  
        int SizeVector;           // Number of dimensions in vector  
        int NumClusters;          // Number of clusters  

        public int LoadPatterns()
        {
            int i, j;
            double x = 0;
            NumPatterns = 10; // Read # of patterns  
            SizeVector = 1; // Read dimension of vector   
            NumClusters = 3; // Read # of clusters for K-Means  
            for (i = 0; i < NumPatterns; i++)
            {         // For each vector  
                for (j = 0; j < SizeVector; j++)
                {       // create a pattern  
                    Pattern[i, j] = x;
                    x += i / 2;
                }
            }
            return SUCCESS;
        }

        public void InitClusters()
        {
            int i, j;
            for (i = 0; i < NumClusters; i++)
            {
                Cluster[i].Member[0] = i;
                for (j = 0; j < SizeVector; j++)
                {
                    Cluster[i].Center[j] = Pattern[i, j];
                }
            }
        }

        public void RunKMeans()
        {
            int converged;
            int pass;
            pass = 1;
            converged = FALSE;
            while (converged == FALSE)
            {
                DistributeSamples();
                converged = CalcNewClustCenters();
                ShowClusters();
            }
        }

        private double EucNorm(int p, int c)
        {   // Calc Euclidean norm of vector difference  
            double dist, x;        // between pattern vector, p, and cluster  
            int i;                // center, c.  

            dist = 0;
            for (i = 0; i < SizeVector; i++)
            {
                x = (Cluster[c].Center[i] - Pattern[p, i]) * (Cluster[c].Center[i] - Pattern[p, i]);
                dist += x;
            }
            return dist;
        }

        private int FindClosestCluster(int pat)
        {
            int i, ClustID;
            double MinDist, d;
            MinDist = 9.9e+99;
            ClustID = -1;
            for (i = 0; i < NumClusters; i++)
            {
                d = EucNorm(pat, i);
                //printf("Distance from pattern %d to cluster %d is %f/n/n",pat,i,sqrt(d));  
                if (d < MinDist)
                {
                    MinDist = d;
                    ClustID = i;
                }
            }

            if (ClustID < 0)
            {
                //  printf("Aaargh");  
                //   exit(0);  
            }
            return ClustID;
        }

        private void DistributeSamples()
        {
            int i, pat, Clustid, MemberIndex;
            //Clear membership list for all current clusters  
            for (i = 0; i < NumClusters; i++)
            {
                Cluster[i].NumMembers = 0;
            }

            for (pat = 0; pat < NumPatterns; pat++)
            {
                //Find cluster center to which the pattern is closest  
                Clustid = FindClosestCluster(pat);
                //printf("patern %d assigned to cluster %d/n/n",pat,Clustid);  
                //post this pattern to the cluster  
                MemberIndex = Cluster[Clustid].NumMembers;
                Cluster[Clustid].Member[MemberIndex] = pat;
                Cluster[Clustid].NumMembers++;
            }
        }

        private int CalcNewClustCenters()
        {
            int ConvFlag, VectID, i, j, k;
            double[] tmp = new double[MAXVECTDIM];

            ConvFlag = TRUE;

            for (i = 0; i < NumClusters; i++)
            {              //for each cluster  
                for (j = 0; j < SizeVector; j++)
                {            // clear workspace  
                    tmp[j] = 0.0;
                }

                for (j = 0; j < Cluster[i].NumMembers; j++)
                { //traverse member vectors  
                    VectID = Cluster[i].Member[j];
                    for (k = 0; k < SizeVector; k++)
                    {         //traverse elements of vector  
                        tmp[k] += Pattern[VectID, k];       // add (member) pattern elmnt into temp  
                    }
                }

                for (k = 0; k < SizeVector; k++)
                {            //traverse elements of vector  
                    tmp[k] = tmp[k] / Cluster[i].NumMembers;
                    if (tmp[k] != Cluster[i].Center[k])
                        ConvFlag = FALSE;
                    Cluster[i].Center[k] = tmp[k];
                }

            }
            return ConvFlag;
        }


        public void ShowClusters()
        {
            int cl, i, pi;
            Console.WriteLine();
            for (cl = 0; cl < NumClusters; cl++)
            {
                Console.WriteLine("/n-CLUSTER -%2d--Center: ", cl);

                for (pi = 0; pi < SizeVector; pi++)
                {
                    Console.WriteLine(" %2f", Cluster[cl].Center[pi]);
                }

                Console.WriteLine("----/n ");

                for (i = 0; i < Cluster[cl].NumMembers; i++)
                {
                    for (pi = 0; pi < SizeVector; pi++)
                    {
                        Console.WriteLine(" %2f ", Pattern[Cluster[cl].Member[i], pi]);
                    }
                }

            }
        }
    }
}
