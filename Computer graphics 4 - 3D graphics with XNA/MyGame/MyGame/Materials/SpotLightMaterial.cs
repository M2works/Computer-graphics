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

namespace MyGame.Materials
{
    class SpotLightMaterial : Material
    {
        public Vector3 AmbientColor { get; set; }
        public Vector3 LightPosition { get; set; }
        public Vector3 LightColor { get; set; }
        public Vector3 LightDirection { get; set; } 
        public float ConeAngle { get; set; }
        public float LightFalloff { get; set; }

        public SpotLightMaterial()
        {
            AmbientColor = new Vector3(0.15f, 0.15f, 0.15f);
            LightPosition = new Vector3(0, 30000, 30000);
            LightColor = new Vector3(.85f, .85f, .85f);
            LightDirection = new Vector3(0, -1, -1);
            ConeAngle = 30;
            LightFalloff = 200;
        }
        public override void SetEffectParameters(Effect effect)
        {
            if (effect.Parameters["AmbientColor"] != null)
                effect.Parameters["AmbientColor"].SetValue(
                AmbientColor);
            if (effect.Parameters["LightPosition"] != null)
                effect.Parameters["LightPosition"].SetValue(LightPosition);
            if (effect.Parameters["LightColor"] != null)
                effect.Parameters["LightColor"].SetValue(LightColor);
            if (effect.Parameters["LightDirection"] != null)
                effect.Parameters["LightDirection"].SetValue(LightDirection);
            if (effect.Parameters["ConeAngle"] != null)
                effect.Parameters["ConeAngle"].SetValue(
                MathHelper.ToRadians(ConeAngle / 2));
            if (effect.Parameters["LightFalloff"] != null)
                effect.Parameters["LightFalloff"].SetValue(LightFalloff);
        }
    }
}
