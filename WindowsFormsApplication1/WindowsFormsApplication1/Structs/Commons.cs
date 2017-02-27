using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structs
{
    class Commons
    {
        public struct POINT
        {
            public decimal posX;
            public decimal posY;

            public POINT(decimal x, decimal y)
            {
                this.posX = x;
                this.posY = y;
            }

            public static POINT operator -(POINT a, POINT b)
            {
                decimal X = a.posX - b.posX;
                decimal Y = a.posY - b.posY;
                return new POINT(X, Y);
            }
            public static POINT operator +(POINT a, POINT b)
            {
                decimal X = a.posX + b.posX;
                decimal Y = a.posY + b.posY;
                return new POINT(X, Y);
            }

            public static bool operator ==(POINT a, POINT b)
            {
                if (a.posX == b.posX && a.posY == b.posY) return true;
                return false;
            }

            public static bool operator !=(POINT a, POINT b)
            {
                return !(a == b);
            }

            public override bool Equals(object obj)
            {
                if (obj is POINT)
                {
                    return this == (POINT)obj;
                }
                else return false;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
