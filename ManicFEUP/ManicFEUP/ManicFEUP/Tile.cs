using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ManicFEUP
{
    enum TileCollision
    {
        Passable = 0,
        Impassable = 1,
        Platform = 2,
    }

    struct Tile
    {
        public int tileX, tileY;
        public TileCollision Collision;
        public bool visible;

        public Tile(int tileX, int tileY, TileCollision collision)
        {
            this.tileX = tileX;
            this.tileY = tileY;
            this.visible = true;
            this.Collision = collision;
        }

        public Tile(TileCollision collision)
        {
            this.tileX = -1;
            this.tileY = -1;
            this.visible = false;
            this.Collision = collision;
        }
    }

    class TileSet
    {
        public Texture2D Texture;
        public int Width;
        public int Height;
        public Vector2 Size;

        public TileSet(Texture2D texture, int Width, int Height)
        {
            this.Texture = texture;
            this.Width = Width;
            this.Height = Height;
            Size = new Vector2(Width, Height);
        }
    }
}
