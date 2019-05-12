using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MemeGame
{
    class Builder
    {
        private WallCollection walls;

        private int radius;

        public Builder(WallCollection walls)
        {
            this.walls = walls;
            radius = 64;
        }

        public void saveMap(string fileName)
        {
            Map saveMap = new Map(walls);
            string appPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            appPath +="\\"+ fileName;
            
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(appPath, FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, saveMap);
            stream.Close();
        }

       

        public void loadMap(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();

            string appPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            appPath +="\\"+ fileName;

            if (File.Exists(appPath))
            {

            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Map load = (Map)formatter.Deserialize(stream);


            stream.Close();
            walls.loadFromMap(load);
            }
        }

        public void building(MouseState mouse, Camera camera)
        {
            Point mousePos = camera.transformMouse(mouse.X, mouse.Y);
            // create tiles with left click
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                walls.createBlock(mousePos.X - radius / 2, mousePos.Y - radius / 2, radius, radius);
            }

            // remove tiles with right click
            if (mouse.RightButton == ButtonState.Pressed)
            {
                for (int i = 0; i < walls.Count; i++)
                {
                    Wall wall = walls[i];
                    if (wall.distance(mousePos) < radius){
                        walls.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}
