using System;
using System.Collections.Generic;
using System.IO;

namespace Nai4
{
    public static class DataReader
    {
        public static void ReadData(FileInfo fileInfo, List<Point> list)
        {
            using (var streamTraining = new StreamReader(fileInfo.OpenRead())) //TODO: reading file
            {
                string line = null;    
                while ((line = streamTraining.ReadLine()) != null)
                {
                    //TODO: handling document spacing
                    string[] columns = line.Split("   \t", StringSplitOptions.RemoveEmptyEntries);
                    Point point = new Point();
                    for (int i = 0; i < columns.Length - 1; i++)
                    {
                        point._list.Add(Double.Parse(columns[i]));
                    }

                    point.classification = columns[columns.Length - 1];

                   list.Add(point);
                }
            }
        }
    }
}