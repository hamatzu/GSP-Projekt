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

        public Tile[,] levelTiles;
        private List<Enemy> enemyList = new List<Enemy>();
        private List<Gem> gemList = new List<Gem>();

        private List<string> allLevels = new List<string>();
        string currentLevelName;
        int currentLevelNr = 1;

        private Player player;
        private Camera camera;

        private Random randMovement = new Random();
        private bool exitLevel = false;
        private bool lastLevelFinished = false;

        internal Level(Player a_player, Camera a_camera)
        {
            camera = a_camera;
            allLevels.Add("Level 1 - Where is the disco?");
            allLevels.Add("Level 2 - Dance baby, dance!");
            allLevels.Add("Level 3 - What was that?!");
            player = a_player;

            currentLevelName = allLevels.ElementAt(currentLevelNr - 1);
            LoadTiles("Content/Levels/level_" + currentLevelNr + ".txt");
        }


        /*************************************
         * Load next level
         *************************************/
        public void nextLevel()
        {
            exitLevel = false;
            currentLevelNr++;
            enemyList.Clear();
            gemList.Clear();

            if (currentLevelNr > allLevels.Count)
            {
                lastLevelFinished = true;
            }
            else
            {
                lastLevelFinished = false;
                LoadTiles("Content/Levels/level_" + currentLevelNr + ".txt");
            }
        }

        /*************************************************
         * Load tiles from file and populate levelTiles
         *************************************************/
        public void LoadTiles(string fileStream)
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
                    //Block tile
                    if(aChar.ToString().Equals("#"))
                        levelTiles[x, y] = Tile.createBlocked();

                    //Empty tile
                    if (aChar.ToString().Equals("."))
                        levelTiles[x, y] = Tile.createEmpty();

                    //Trap I.E Disco tile
                    if (aChar.ToString().Equals("T"))
                        levelTiles[x, y] = Tile.createTrap();

                    //Player start position
                    if (aChar.ToString().Equals("1"))
                    {
                        levelTiles[x, y] = Tile.createEmpty();
                        player.setPlayerPosition(new Vector2(x, y));
                    }

                    //Exit tile
                    if (aChar.ToString().Equals("X"))
                        levelTiles[x, y] = Tile.createExit();

                    //Heart Tile/gem
                    if (aChar.ToString().Equals("L"))
                    {
                        levelTiles[x, y] = Tile.createEmpty();
                        Vector2 gemPos = new Vector2(x, y);
                        Gem aGem = new Gem(gemPos);
                        aGem.setGemType(Gem.GemType.Life);
                        gemList.Add(aGem);
                    }

                    // Enemy - Rasta
                    if (aChar.ToString().Equals("R"))
                    {
                        levelTiles[x, y] = Tile.createEmpty();
                        Vector2 enemyPos = new Vector2(x, y);
                        Enemy theEnemy = new Enemy(enemyPos, camera.getScaleX());
                        theEnemy.setEnemyType(Enemy.EnemyType.Rasta);
                        enemyList.Add(theEnemy);
                    }

                    // Enemy - HipHopper
                    if (aChar.ToString().Equals("H"))
                    {
                        levelTiles[x, y] = Tile.createEmpty();
                        Vector2 enemyPos = new Vector2(x, y);
                        Enemy theEnemy = new Enemy(enemyPos, camera.getScaleX());
                        theEnemy.setEnemyType(Enemy.EnemyType.HipHopper);
                        enemyList.Add(theEnemy);
                    }

                    // Enemy - Ghost
                    if (aChar.ToString().Equals("G"))
                    {

                        levelTiles[x, y] = Tile.createEmpty();
                        Vector2 enemyPos = new Vector2(x, y);
                        Enemy theEnemy = new Enemy(enemyPos, camera.getScaleX());
                        theEnemy.setEnemySpeed(new Vector2(2.0f * ((float)randMovement.NextDouble() / (float)1f) - 1.0f, (float)randMovement.NextDouble()));
                        theEnemy.setEnemyType(Enemy.EnemyType.Ghost);
                        enemyList.Add(theEnemy);
                    }

                    levelTiles[x, y].setTilePosition(new Vector2(x, y));

                    x++;
                }
                y++;
            }  

        }


        /*************************************************
         * Check if object is coliding with a tile
         *************************************************/
        internal bool IsCollidingAt(FloatRectangle a_rect, Vector2 a_centerBottomPosition)
        {
            Vector2 topLeft = new Vector2(a_centerBottomPosition.X - 2, a_centerBottomPosition.Y);
            Vector2 topRight = new Vector2(a_centerBottomPosition.X + 2, a_centerBottomPosition.Y);

            if (topRight.X > Model.Level.LEVEL_WIDTH)
                topRight.X = Model.Level.LEVEL_WIDTH;

            if (topLeft.X < 0)
                topLeft.X = 0;

            Vector2 tileSize = new Vector2(1f, 1f);
            for (int x = (int)topLeft.X; x < topRight.X; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    FloatRectangle rect = FloatRectangle.createFromTopLeft(new Vector2(x, y), tileSize);
                    if (a_rect.isIntersecting(rect))
                    {
                        if (levelTiles[x, y].isBlocked())
                        {
                            player.setAllowedJump(true);
                            player.setMustBoogie(false);
                            return true;
                        }

                        if (levelTiles[x, y].isTrap())
                        {
                            levelTiles[x, y].setWalkedOn(true);
                            player.setAllowedJump(false);
                            player.setMustBoogie(true);
                            return true;
                        }

                        if (levelTiles[x, y].isExit())
                            exitLevel = true;
                    }
                }
            }
            return false;
        }

        internal List<Gem> getLevelGems()
        {
            return gemList;
        }

        internal int getCurrentLevel()
        {
            return currentLevelNr;
        }
    
        internal bool finishedLastLevel()
        {
 	        return lastLevelFinished;
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
    }
}
