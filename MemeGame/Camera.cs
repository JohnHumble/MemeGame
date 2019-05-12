using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeGame
{
    class Camera
    {
        public Vector2 location;
        public float scale;
        private Matrix transform;

        private readonly int screenWidth, screenHeight;

        // initalize
        public Camera(Vector2 loc)
        {
            location = loc;
            scale = 1;
            transform = new Matrix();
        }
        public Camera(Vector2 loc, float scl)
        {
            location = loc;
            scale = scl;
            transform = new Matrix();
        }
        public Camera(float x, float y, float scl = 1)
        {
            location = new Vector2(x, y);
            scale = scl;
            transform = new Matrix();
        }
        public Camera(float x, float y, float scl, int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            location = new Vector2(-x + screenWidth/2f, -y + screenHeight/2f);
            scale = scl;
            transform = new Matrix();
        }

        public Point transformMouse(int x, int y)
        {
            return new Point((int)(x / scale - location.X), (int)(y / scale - location.Y));
        }

        public Vector2 getMid()
        {
            float midX = -location.X + (screenWidth / 2) / scale;
            float midY = -location.Y + (screenHeight / 2) / scale;
            return new Vector2(midX, midY);
            //return location;
        }

        private Vector2 adjustVec(Vector2 vector)
        {
            return new Vector2(-vector.X + screenWidth / 2 / scale, -vector.Y + screenHeight / 2 / scale);
        }

        // change location.
        public void setLocation(Vector2 loc)
        {
            location = adjustVec(loc);
        }
        public void setLocation(float x, float y)
        {
            setLocation(new Vector2(x,y));
        }

        const float DAMPEN = 32;
        // the camera eases into the location
        public void trackTo(Vector2 loc)
        {
            loc = adjustVec(loc);
            Vector2 distance = loc - location;
            distance.X /= DAMPEN;
            distance.Y /= DAMPEN;

            location += distance;
        }
        // tracks with scale
        const int SIDE_BUFFER = 300;
        const float MIN_SCALE = .5f;
        public void trackTo(Vector2 loc, Vector2 difference, Vector2 worldMid, Vector2 worldDifference)
        {
            difference.X = Math.Min(difference.X, worldDifference.X);
            difference.Y = Math.Min(difference.Y, worldDifference.Y);
            
            if (Tool.distance(loc,worldMid) > Math.Abs(worldDifference.X) + Math.Abs(worldDifference.Y))
            {
                loc = worldMid;
            }

            if (difference.Y == 0)
                difference.Y = 1;
            if (difference.X == 0)
                difference.X = 1;

            difference.X += SIDE_BUFFER * 2;
            difference.Y += SIDE_BUFFER * 2;
           
            float target = Math.Max(MIN_SCALE, Math.Min(1, Math.Min(screenWidth / difference.X, screenHeight / difference.Y)));

            scale += (target - scale) / DAMPEN;

            trackTo(loc);
        }

        // change scale
        public void setScale(float Scale)
        {
            scale = Scale;
        }

        // give matrix. use this in draw method
        public Matrix getTransformation()
        {
            transform = Matrix.CreateTranslation(new Vector3(location.X, location.Y, 0)) * Matrix.CreateScale(scale);
            return transform;
        }
    }
}
