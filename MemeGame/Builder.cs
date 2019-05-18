using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    enum BuildState
    {
        Blocks,
        StartLocations,
        Guns,
        Pan
    }

    class Builder
    {
        private WallCollection walls;
        private readonly int radius;

        private List<Point> startLocation, gunLocations;
        private Button menu;

        private Texture2D playerTexture, gunTexture;
        private int playerWidth, playerHeight, gunWidth, gunHeight;

        private BuildState state;
        private Point last_pressed;
        private Vector2 last_camera;

        public Builder(WallCollection walls, SpriteFont buttonFont, Texture2D buttonTexture, Texture2D playerTexture, int playerWidth, int playerHeight, Texture2D gunTexture, int gunWidth, int gunHeight)
        {
            startLocation = new List<Point>();
            gunLocations = new List<Point>();

            this.playerHeight = playerHeight;
            this.playerWidth = playerWidth;
            this.gunHeight = gunHeight;
            this.gunWidth = gunWidth;

            this.playerTexture = playerTexture;
            this.gunTexture = gunTexture;

            this.walls = walls;
            radius = 64;

            menu = new Button(100, 100, 100, 40, "Menu", buttonFont, buttonTexture);

            state = BuildState.Pan;
            last_pressed = new Point(0, 0);
        }

        public void saveMap(string fileName)
        {
            Map saveMap = new Map(walls,startLocation,gunLocations);
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
                startLocation = loadPointDataList(load.startLocations);
                gunLocations = loadPointDataList(load.gunLocations);
            }
        }

        private List<Point> loadPointDataList(List<PointData> dataList)
        {
            List<Point> tmp = new List<Point>();
            
            foreach (var data in dataList)
            {
                tmp.Add(new Point(data.x, data.y));
            }

            return tmp;
        }

        public Screen Update(MouseState mouse, Camera camera, string file)
        {
            if (menu.IsPressed(mouse))
            {
                saveMap(file);
                return Screen.Menu;
            }

            if (state == BuildState.Blocks)
            {
                building(mouse, camera);
            }

            if (state == BuildState.Pan)
            {
                paning(mouse, camera);
            }

            return Screen.Build;
        }

        public void paning(MouseState mouse, Camera camera)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Point distance = new Point(last_pressed.X - mouse.X, last_pressed.Y - mouse.Y);
                Vector2 loc = last_camera;
                loc.X -= distance.X;
                loc.Y -= distance.Y;
                camera.location = loc;
            }
            else
            {
                last_pressed = new Point(mouse.X, mouse.Y);
                last_camera = camera.location;
            }
        }

        public void building(MouseState mouse, Camera camera)
        {
            Point mousePos = camera.transformMouse(mouse.X, mouse.Y);
            // create tiles with left click
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                walls.CreateBlock(mousePos.X - radius / 2, mousePos.Y - radius / 2, radius, radius);
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

        public void DrawLocations(SpriteBatch spriteBatch)
        {
            foreach (var playerLoc in startLocation)
            {
                Rectangle render = new Rectangle(playerLoc.X,playerLoc.Y,playerWidth,playerHeight);
                Rectangle source = new Rectangle(0,0,playerWidth,playerHeight);
                spriteBatch.Draw(playerTexture, render, source, Color.White);
            }

            foreach (var gunLoc in gunLocations)
            {
                Rectangle render = new Rectangle(gunLoc.X, gunLoc.Y, gunWidth, gunHeight);
                Rectangle source = new Rectangle(0, 0, gunWidth, gunHeight);
                spriteBatch.Draw(gunTexture, render, source, Color.White);
            }
        }

        public void Drawhud(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }
    }
}
