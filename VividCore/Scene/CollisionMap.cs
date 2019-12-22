using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
namespace Vivid.Scene
{
    public class CollisionMap
    {

        public List<CollisionEntity> Entities = new List<CollisionEntity>();
        public void AddCube(int x, int y, int w, int h)
        {
            var ne = new CollisionCube();
            ne.X = x;
            ne.Y = y;
            ne.W = w;
            ne.H = h;
            Entities.Add(ne);
        }

        public void AddLine(float x, float y, float x2, float y2,float nx,float ny)
        {
            var ne = new CollisionLine();
            ne.X1 = x;
            ne.Y1 = y;
            ne.X2 = x2;
            ne.Y2 = y2;
            ne.NX = nx;
            ne.NY = ny;
            Entities.Add(ne);
        }

        public RayHit RayCast(float x, float y, float x2, float y2)
        {
            var rh = new RayHit();
            float sd = 200000;
            RayHit near = null;
            foreach (var ent in Entities)
            {
                rh = ent.RayCast(x, y, x2, y2);
                if (rh != null)
                {
                    if (rh.Dis < sd)
                    {
                        sd = rh.Dis;
                        near = rh;

                        float xd = x2 - rh.HitX;
                        float yd = y2 - rh.HitY;
                        float IS = (float)Math.Sqrt(xd * xd + yd * yd);
                        rh.IS = IS;

                    }
                }
            }




            return near;
        }

    }

    public class RayHit
    {
        public float HitX, HitY;
        public float Dis;
        public float IS = 0;

    }

    public class CollisionEntity
    {
        public virtual RayHit RayCast(float x, float y, float x2, float y2)
        {
            return null;
        }

  

        
    }
    public class CollisionCube : CollisionEntity
    {
        public float X, Y, W, H;
    }
    public class CollisionLine : CollisionEntity
    {
        public float X1, Y1, X2, Y2;
        public float NX, NY;

        public override RayHit RayCast(float x, float y, float x2, float y2)
        {

            var hit = new RayHit();


            Line l1 = new Line()
            {
                x1 = x,
                y1 = y,
                x2 = x2,
                y2 = y2
            };
            Line l2 = new Line()
            {
                x1 = X1,
                y1 = Y1,
                x2 = X2,
                y2 = Y2
            };

            Point rp = LineIntersection.FindIntersection(l1, l2);
            
            float xd = (float)rp.x - x;
            float yd = (float)rp.y - y;
            float d = (float)Math.Sqrt(xd * xd + yd * yd);
            if (rp.x == 0 && rp.y == 0)
            {
                 return null;
            };


            return new RayHit() { HitX = (float)rp.x, HitY = (float)rp.y,Dis = d };
            //LineEquation l1 = new LineEquation(new Point(x, y), new Point(x2, y2));
           // LineEquation l2 = new LineEquation(new Point(X1, Y1), new Point(X2, Y2));

//            Point hp = new Point(0, 0);

  //          if (l1.IntersectsWithLine(l2, out hp))
            {
    //            hit.HitX = (float)hp.X;
      //          hit.HitY = (float)hp.Y;
        //        return hit;
            }



            return null;

            //return base.RayCast(x, y, x2, y2);

        }
    }


    public struct Line
    {
        public double x1 { get; set; }
        public double y1 { get; set; }

        public double x2 { get; set; }
        public double y2 { get; set; }
    }

    public struct Point
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class LineIntersection
    {
        //  Returns Point of intersection if do intersect otherwise default Point (null)
        public static Point FindIntersection(Line lineA, Line lineB, double tolerance = 0.001)
        {
            double x1 = lineA.x1, y1 = lineA.y1;
            double x2 = lineA.x2, y2 = lineA.y2;

            double x3 = lineB.x1, y3 = lineB.y1;
            double x4 = lineB.x2, y4 = lineB.y2;

            // equations of the form x = c (two vertical lines)
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance && Math.Abs(x1 - x3) < tolerance)
            {
                 return default(Point);
                //throw new Exception("Both lines overlap vertically, ambiguous intersection points.");
            }

            //equations of the form y=c (two horizontal lines)
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance && Math.Abs(y1 - y3) < tolerance)
            {
                throw new Exception("Both lines overlap horizontally, ambiguous intersection points.");
            }

            //equations of the form x=c (two vertical lines)
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance)
            {
                return default(Point);
            }

            //equations of the form y=c (two horizontal lines)
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance)
            {
                return default(Point);
            }

            //general equation of line is y = mx + c where m is the slope
            //assume equation of line 1 as y1 = m1x1 + c1 
            //=> -m1x1 + y1 = c1 ----(1)
            //assume equation of line 2 as y2 = m2x2 + c2
            //=> -m2x2 + y2 = c2 -----(2)
            //if line 1 and 2 intersect then x1=x2=x & y1=y2=y where (x,y) is the intersection point
            //so we will get below two equations 
            //-m1x + y = c1 --------(3)
            //-m2x + y = c2 --------(4)

            double x, y;

            //lineA is vertical x1 = x2
            //slope will be infinity
            //so lets derive another solution
            if (Math.Abs(x1 - x2) < tolerance)
            {
                //compute slope of line 2 (m2) and c2
                double m2 = (y4 - y3) / (x4 - x3);
                double c2 = -m2 * x3 + y3;

                //equation of vertical line is x = c
                //if line 1 and 2 intersect then x1=c1=x
                //subsitute x=x1 in (4) => -m2x1 + y = c2
                // => y = c2 + m2x1 
                x = x1;
                y = c2 + m2 * x1;
            }
            //lineB is vertical x3 = x4
            //slope will be infinity
            //so lets derive another solution
            else if (Math.Abs(x3 - x4) < tolerance)
            {
                //compute slope of line 1 (m1) and c2
                double m1 = (y2 - y1) / (x2 - x1);
                double c1 = -m1 * x1 + y1;

                //equation of vertical line is x = c
                //if line 1 and 2 intersect then x3=c3=x
                //subsitute x=x3 in (3) => -m1x3 + y = c1
                // => y = c1 + m1x3 
                x = x3;
                y = c1 + m1 * x3;
            }
            //lineA & lineB are not vertical 
            //(could be horizontal we can handle it with slope = 0)
            else
            {
                //compute slope of line 1 (m1) and c2
                double m1 = (y2 - y1) / (x2 - x1);
                double c1 = -m1 * x1 + y1;

                //compute slope of line 2 (m2) and c2
                double m2 = (y4 - y3) / (x4 - x3);
                double c2 = -m2 * x3 + y3;

                //solving equations (3) & (4) => x = (c1-c2)/(m2-m1)
                //plugging x value in equation (4) => y = c2 + m2 * x
                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;

                //verify by plugging intersection point (x, y)
                //in orginal equations (1) & (2) to see if they intersect
                //otherwise x,y values will not be finite and will fail this check
                if (!(Math.Abs(-m1 * x + y - c1) < tolerance
                    && Math.Abs(-m2 * x + y - c2) < tolerance))
                {
                    return default(Point);
                }
            }

            //x,y can intersect outside the line segment since line is infinitely long
            //so finally check if x, y is within both the line segments
            if (IsInsideLine(lineA, x, y) &&
                IsInsideLine(lineB, x, y))
            {
                return new Point { x = x, y = y };
            }

            //return default null (no intersection)
            return default(Point);

        }

        // Returns true if given point(x,y) is inside the given line segment
        private static bool IsInsideLine(Line line, double x, double y)
        {
            return (x >= line.x1 && x <= line.x2
                        || x >= line.x2 && x <= line.x1)
                   && (y >= line.y1 && y <= line.y2
                        || y >= line.y2 && y <= line.y1);
        }
    }

}