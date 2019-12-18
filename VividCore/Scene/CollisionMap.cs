using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid.Scene
{
    public class CollisionMap
    {

        public List<CollisionEntity> Entities = new List<CollisionEntity>();
        public void AddCube(int x,int y,int w,int h)
        {
            var ne = new CollisionCube();
            ne.X = x;
            ne.Y = y;
            ne.W = w;
            ne.H = h;
            Entities.Add(ne);
        }

        public void AddLine(float x,float y,float x2,float y2)
        {
            var ne = new CollisionLine();
            ne.X1 = x;
            ne.Y1 = y;
            ne.X2 = x2;
            ne.Y2 = y2;
            Entities.Add(ne);
        }

        public RayHit RayCast(float x,float y,float x2,float y2)
        {
            var rh = new RayHit();

            foreach(var ent in Entities)
            {
                rh = ent.RayCast(x, y, x2, y2);
            }

            return rh;
        }

    }

    public class RayHit
    {
        public float HitX, HitY;
        public float Dis;

    }

    public class CollisionEntity
    {
        public virtual RayHit RayCast(float x,float y,float x2,float y2)
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
        public override RayHit RayCast(float x, float y, float x2, float y2)
        {

            var hit = new RayHit();


            return hit;

            //return base.RayCast(x, y, x2, y2);

        }
    }
}
