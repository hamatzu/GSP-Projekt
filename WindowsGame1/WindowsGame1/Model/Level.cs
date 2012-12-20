﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using WindowsGame1.View;

namespace WindowsGame1.Model
{
    class Level
    {
        public static int LEVEL_WIDTH;
        public static int LEVEL_HEIGHT;

        public const int TILE_WIDTH = 1;
        public const int TILE_HEIGHT = 1;

        internal Tile[,] levelTiles;
        private List<Enemy> enemyList = new List<Enemy>();

        private List<string> allLevels = new List<string>();
        string currentLevelName;
        int currentLevelNr = 1;
        private bool exitLevel = false;
        private Player player;
        private float scale;
        private Camera camera;

        internal Level(Player a_player, Camera a_camera)
        {
            camera = a_camera;
            allLevels.Add("Level 1 - Where is the disco?");
            allLevels.Add("Level 2 - What was that?");
            allLevels.Add("Level 3 - There can only be one!");
            player = a_player;

            currentLevelName = allLevels.ElementAt(currentLevelNr - 1);
            LoadTiles("Content/Levels/level_" + currentLevelNr + ".txt");
        }

        public void nextLevel()
        {
            exitLevel = false;
            currentLevelNr++;
            LoadTiles("Content/Levels/level_" + currentLevelNr + ".txt");
        }

        private void LoadTiles(string fileStream)
        {
            // Load the level and ensure all of the lines are the same length.
            int width = 0;

            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;

                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            LEVEL_WIDTH = width;
            LEVEL_HEIGHT = lines.Count();
            levelTiles = new Tile[LEVEL_WIDTH, LEVEL_HEIGHT];
            camera.setGameScale();
            currentLevelName = allLevels.ElementAt(currentLevelNr - 1);
            int y = 0;
            foreach (string row in lines)
            {
                int x = 0;
                foreach (char aChar in row)
                {
                    if(aChar.ToString().Equals("#"))
                        levelTiles[x, y] = Tile.createBlocked();

                    if (aChar.ToString().Equals("."))
                        levelTiles[x, y] = Tile.createEmpty();

                    if (aChar.ToString().Equals("T"))
                        levelTiles[x, y] = Tile.createTrap();

                    if (aChar.ToString().Equals("1"))
                    {
                        levelTiles[x, y] = Tile.createEmpty();
                        player.setPlayerPosition(new Vector2(x, y));
                    }

                    if (aChar.ToString().Equals("X"))
                        levelTiles[x, y] = Tile.createExit();

                    if (aChar.ToString().Equals("E"))
                    {
                        levelTiles[x, y] = Tile.createEmpty();
                        Vector2 enemyPos = new Vector2(x, y);
                        enemyList.Add(new Enemy(enemyPos, camera.getScaleX()));
                    }

                    levelTiles[x, y].setTilePosition(new Vector2(x, y));

                    Console.Write(aChar);
                    x++;
                }
                Console.Write("\n");
                y++;
            }  

        }

        internal string getLevelName()
        {
            return currentLevelName;
        }

        public List<Enemy> getLevelEnemies()
        {
            return enemyList;
        }

        internal bool isExitLevel()
        {
            return exitLevel;
        }

        //Check if player is colliding with a tile
        internal bool IsCollidingAt(FloatRectangle a_rect)
        {
            Vector2 tileSize = new Vector2(1f, 1f);
            for (int x = 0; x < LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    FloatRectangle rect = FloatRectangle.createFromTopLeft(new Vector2(x, y), tileSize);
                    if (a_rect.isIntersecting(rect))
                    {
                        if (levelTiles[x, y].isBlocked())
                        {
                            player.setAllowedJump(true);
                            return true;
                        }

                        if (levelTiles[x, y].isTrap())
                        {
                            levelTiles[x, y].setWalkedOn(true);
                            player.setAllowedJump(false);
                            return true;
                        }

                        if (levelTiles[x, y].isExit())
                            exitLevel = true;
                    }
                }
            }
            return false;
        }

        internal void ToggleTile(FloatRectangle a_mouse)
        {
            Vector2 tileSize = new Vector2(1f, 1f);
            for (int x = 0; x < LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    FloatRectangle rect = FloatRectangle.createFromTopLeft(new Vector2(x, y), tileSize);
                    if (a_mouse.isIntersecting(rect))
                    {
                        levelTiles[x, y] = Tile.createBlocked();
                        Console.WriteLine(x + "," + y);
                    }
                }
            }
        }
    }
}
