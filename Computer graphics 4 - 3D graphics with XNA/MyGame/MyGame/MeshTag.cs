using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MyGame
{
    public class MeshTag
    {
        public Vector3 Color;
        public Texture2D Texture;
        public float SpecularPower;
        public Effect CachedEffect = null;
        public MeshTag(Vector3 Color, Texture2D Texture,
        float SpecularPower)
        {
            this.Color = Color;
            this.Texture = Texture;
            this.SpecularPower = SpecularPower;
        }
    }

}
