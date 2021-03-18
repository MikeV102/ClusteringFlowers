using System;
using System.Collections.Generic;

namespace Nai4
{
    public class Euclidean
    {
        public static double Distance(Point p1, Point p2)
        {
            double distance = -1;
            double sumOfDifferences = 0;
            List<double> differences = new List<double>();

            if (p1._list.Count != p2._list.Count)
            {
               throw  new ArgumentException("points passed as parameters have different number of coordinates");
            }

            for (int i = 0; i < p1._list.Count; i++)
            {
                differences.Add(Math.Pow((p1._list[i] - p2._list[i]),2));
            }
            
            
            foreach (var difference in differences)
            {
                sumOfDifferences += difference;
            }

            distance = Math.Sqrt(sumOfDifferences);
            
            
            return distance;
        }
    }
}