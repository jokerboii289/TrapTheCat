using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using UnityEngine;

namespace Model
{

    public struct Point
    {
        public int X, Z;
       
        public Point(int x, int z)
        {
            X = x;
            //Y = Y;
            Z = z;
        }
    }
}