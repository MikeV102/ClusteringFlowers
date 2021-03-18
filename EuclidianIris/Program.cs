using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nai4
{
    class Program
    {
        static void Main(string[] args)
        {
          List<Point>  points = new List<Point>();
          
          DataReader.ReadData(new FileInfo(@"Data\iris_test.txt"), points );
          
          Random random = new Random();
          
          List<Point> x = new List<Point>();
          x.Add(new Point(new List<double>(new []{2.1,3.1})));
          x.Add(new Point(new List<double>(new []{5.1,6.1})));
          List<Point> y = new List<Point>();
          y.Add(new Point(new List<double>(new []{6.1,5.1})));
          y.Add(new Point(new List<double>(new []{3.1,2.1})));
          
          Dictionary<Point,List<Point>> dic1= new Dictionary<Point, List<Point>>();
          dic1.Add(new Point(new List<double>(new []{1.2,2.2})),x );
          dic1.Add(new Point(new List<double>(new []{2.4,4.4})),y);

          Dictionary<Point,List<Point>> dic2 = new Dictionary<Point, List<Point>>();
          dic2.Add(new Point(new List<double>(new []{2.4,4.4})), x);
          dic2.Add(new Point(new List<double>(new []{1.2,2.2})), y);
          

          Console.WriteLine(DictionariesTheSame(dic1,dic2));


          int userK = -1;
          while (userK < 1)//TODO: loop until the right k parameter is input
          {
              Console.WriteLine("input k: ");
              try
              {
                  userK = int.Parse(Console.ReadLine());
              }
              catch (FormatException formatException)
              {
                  Console.WriteLine("k is in incorrect format");
              }

              if (userK < 1)
              {
                  Console.WriteLine("k must be > 1");
              }
          }

          
          
          //TODO: ##########################################################################################################
          
          
          
          List<Point> clusterCenters = new List<Point>(userK);
          Dictionary<Point,List<Point>> clusterMembers = new Dictionary<Point, List<Point>>(); //to store cluster <center, members>
          Dictionary<Point,List<Point>> toCompare = new Dictionary<Point, List<Point>>();//to check whether cluster members have changed 
           // List<List<Point>> clustersMembersList = new List<List<Point>>(userK);
           Dictionary<Point,double> pointDistanceFromCenter = new Dictionary<Point, double>(); //to store <point, its distance to center>
          
          for (int i = 0; i < userK; i++) //TODO: randomly choosing cluster centers
          {
             // clusterCenters.Add(new Point(points[random.Next(0,points.Count)]._list));
              clusterCenters.Add(points[random.Next(0,points.Count)]);
              
              clusterMembers.Add(clusterCenters[i],new List<Point>()); // adding randomly chosen cluster centers to the dictionary
          }


          bool pointsChangingClusters = true; // flag indicating whether between iteration the state of points membership has changed
          int iterationCount = 1;
          while (pointsChangingClusters)
          {

              if (iterationCount > 1)
              {
                  clusterMembers.Clear();

                  for (int i = 0; i < userK; i++)
                  {
                      //setting up the dictionary for new clusters 
                      clusterMembers.Add(clusterCenters[i],new List<Point>()); // adding randomly chosen cluster centers to the dictionary

                  }
              }
              
                pointDistanceFromCenter.Clear();
                foreach (var point in points)
              {
                  double minDistance = Euclidean.Distance(clusterCenters[0], point); //TODO: calculates distance from clusterCenter to the point
                  Point cluster = clusterCenters[0]; // TODO: sets starting cluster 
                  for (int i = 0; i < clusterCenters.Count; i++) //TODO: iterates through clusterCenters 
                  {
                      double g;   
                      if ((g = Euclidean.Distance(clusterCenters[i], point)) < minDistance) // TODO: comparing next cluster - our point distance with minDistance
                      {
                          minDistance = Euclidean.Distance(clusterCenters[i], point); // TODO: setting new minDistance
                          cluster = clusterCenters[i]; //TODO: setting the cluster for the smallest distance
                      }

                  }

                  pointDistanceFromCenter.Add(point, minDistance); //TODO: adding minimal distance to point
                  clusterMembers[cluster].Add(point); //TODO: adding point to the list of points for the cluster 

              }

              Console.WriteLine("---------------------------------- Iteration: " + iterationCount++ + " ----------------------------------");
                
              PrintCluster(clusterMembers,pointDistanceFromCenter,false);

              if (DictionariesTheSame(toCompare, clusterMembers))
              {
                  //throw new Exception("Dobrze dziala");
                  PrintCluster(clusterMembers,pointDistanceFromCenter,true);

                 break; 
              }

              // if (ClusterCentersTheSame(toCompare, clusterMembers))
              // {
              //   //  throw new Exception("dobrze dziala");
              // }
                  


              toCompare.Clear();
              foreach (var member in clusterMembers)
              {
                  toCompare.Add(member.Key, member.Value); //TODO: copying all members in order to be able to compare whether sth has changed
              }

              // all cluster -> take the lsit -> for each coordinate -> sum and divide by number of points
              foreach (var member in clusterMembers) //TODO:for each cluster
              {
                  // for (int k = 0; k < clusterCenters[0]._list.Count; k++)
                  // {
                      Point p = new Point(new List<double>(4));

                      for (int i = 0; i < member.Value[0]._list.Count; i++) //TODO: for each coordinate
                      {
                          double sum = 0;

                          foreach (var point in member.Value) //TODO: for each Point in a cluster list
                          {
                              sum += point._list[i]; //TODO: sum coordinates
                          }
                        sum = sum / member.Value.Count; // TODO: divide the sum by number of coordinates 
                        p._list.Add(sum);

                      }
                      // clusterCenters[clusterCenters.IndexOf(member.Key)]._list[k] = sum; // TODO: assign the divided sum as a new coordinate of a cluster point
                     clusterCenters[clusterCenters.IndexOf(member.Key)] = p;
                 // }

                  
              }


          }





        }

        public static bool DictionariesTheSame(Dictionary<Point, List<Point>> d1, Dictionary<Point, List<Point>> d2)
        {
            if (d1.Keys.Count != d2.Keys.Count) // comparing number of centers in dictionaries 
                return false;

            Point[] keys1 = new Point[d1.Keys.Count]; 
            Point[] keys2 = new Point[d2.Keys.Count];
            d1.Keys.CopyTo(keys1,0);//getting keys from dictionaries to arrays
            d2.Keys.CopyTo(keys2,0);

            
            
            foreach (var point in keys1)//checking if sets of cluster centers are the same
            {
                if (!keys2.Contains(point))
                    return false;
            }

            
            for (int i = 0; i < keys1.Length; i++)
            {
                
                if (!ListsEqual(d1[keys1[i]], d2[keys2[i]]))
                    return false;
            }

            // foreach (var point in keys1)
            // {
            //     
            //     
            //     
            //     if (!ListsEqual(d1[point], d2[point]))
            //         return false;
            // }
            
            
            return true;
        }

        public static bool ListsEqual(List<Point> l1, List<Point> l2)
        {
            if (l1.Count != l2.Count)
                return false;

            
            foreach (var l1Item in l1)
            {
                    bool equals = false;
                    
                foreach (var l2Item in l2)
                {
                    if( l1Item.Equals(l2Item))
                    {
                        equals = true;
                        break;
                    }
                }

                if (equals == false)
                    return false;
            }

            return true;
        }

        public static void PrintCluster(Dictionary<Point, List<Point>> dictionary, Dictionary<Point,double> distances, bool last)
        {
            double sum = 0;
            double entropy = 0;
            
            
            foreach (var pair in dictionary)
            {
                    var list = pair.Value;
                if (last == true)
                {
                    Console.WriteLine("FOR SUCH CLUSTER CENTER: " + pair.Key);

                    foreach (var point in list)
                    {
                        Console.WriteLine(point);
                    }
                }


                foreach (var distance in  distances)
                {
                    foreach (var point in list)
                    {
                        if (point.Equals(distance.Key))
                        {
                            sum += distance.Value;
                            entropy -= ( Math.Log2(distance.Value) * distance.Value);
                        }
                    }
                }

                if(last != true)
                Console.WriteLine("the sum of distances: " + sum);
                
                if(last == true)
                Console.WriteLine("the entropy : " + entropy);

            }
        }

        public static bool ClusterCentersTheSame(Dictionary<Point, List<Point>> d1, Dictionary<Point, List<Point>> d2)
        {
            if (d1.Keys.Count != d2.Keys.Count) // comparing number of centers in dictionaries 
                return false;

            Point[] keys1 = new Point[d1.Keys.Count]; 
            Point[] keys2 = new Point[d2.Keys.Count];
            d1.Keys.CopyTo(keys1,0);//getting keys from dictionaries to arrays
            d2.Keys.CopyTo(keys2,0);

            
            
            foreach (var point in keys1)//checking if sets of cluster centers are the same
            {
                if (!keys2.Contains(point))
                    return false;
            }
            
            return true;
        }
    }
}