using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public abstract class SpacialObject
    {
        public Point Location;
        public int X { get { return Location.X; } }
        public int Z { get { return Location.Z; } }

        public SpacialObject(int x, int z)
            : this(new Point(x, z))
        {
        }

        public SpacialObject(Point location)
        {
            Location = location;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", X, Z);
        }
    }
}