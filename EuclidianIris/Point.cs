using System.Collections.Generic;
using System.Text;

namespace Nai4
{
    public class Point
    {
        public List<double> _list; // list of coordinates
        public string classification; //name of the flower

        public Point()
        {
            _list = new List<double>();   
        }

        public Point(List<double> coordinates)
        {
            _list = coordinates;
        }
        public Point(List<double> coordinates,string classification)
        {
            _list = coordinates;
            this.classification = classification;
        }

       

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(this, null)) return false;
            if (ReferenceEquals(this, null)) return false;
            if (this.GetType() != obj.GetType()) return false;
                
            foreach (var d in this._list)
            {
                bool equal = false;
                foreach (var d1 in ((Point)obj)._list)
                {
                    if (d.Equals(d1))
                    {
                        equal = true;
                        break;
                    }

                }
                    if (equal == false)
                    {
                        return false;
                    }
            }

            return true;
        
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            
            foreach (var d in this._list)
            {
                builder.Append(d + "  |  ");
            }

            builder.Append(classification);
            return builder.ToString();
        }
    }
    
        
}