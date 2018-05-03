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
    class MultiSpotLightMaterial : Material
    {
        public ChaseLight ChaseLight { get; set; }
        public Vector3 AmbientColor { get; set; }
        public Vector3 [] LightPosition { get; set; }
        public Vector3 [] LightColor { get; set; }
        public Vector3 [] LightDirection { get; set; }
        public float [] ConeAngle { get; set; }
        public float [] LightFalloff { get; set; }

        public MultiSpotLightMaterial(ChaseLight cl)
        {
            ChaseLight = cl;

            AmbientColor = new Vector3(0.15f, 0.15f, 0.15f);

            LightPosition = new Vector3[2];
            LightPosition[0] = new Vector3(0, 30000, 30000);
            LightPosition[1] = cl.LightPosition;

            LightColor = new Vector3[2];
            LightColor[0] = new Vector3(.85f, .85f, .85f);
            LightColor[1] = cl.LightColor;

            LightDirection = new Vector3[2];
            LightDirection[0] = new Vector3(0, -1, -1);
            LightDirection[1] = cl.LightDirection;

            ConeAngle = new float[2];
            ConeAngle[0] = 30;
            ConeAngle[1] = 30;

            LightFalloff = new float[2];
            LightFalloff[0] = 200;
            LightFalloff[1] = 200;


        }
        private void UpdateStateOfParameters()
        {
            LightPosition[1] = ChaseLight.LightPosition;
            LightColor[1] = ChaseLight.LightColor;
            LightDirection[1] = ChaseLight.LightDirection;
        }
        public override void SetEffectParameters(Effect effect)
        {
            UpdateStateOfParameters();
            float [] angleInRadians = new float[2];
            for(int i=0;i<ConeAngle.Length;i++)
            {
                angleInRadians[i] = MathHelper.ToRadians(ConeAngle[i] / 2);
            }

            if (effect.Parameters["AmbientColor"] != null)
                effect.Parameters["AmbientColor"].SetValue(AmbientColor);
            if (effect.Parameters["LightPosition"] != null)
                effect.Parameters["LightPosition"].SetValue(LightPosition);
            if (effect.Parameters["LightColor"] != null)
                effect.Parameters["LightColor"].SetValue(LightColor);
            if (effect.Parameters["LightDirection"] != null)
                effect.Parameters["LightDirection"].SetValue(LightDirection);
            if (effect.Parameters["ConeAngle"] != null)
                effect.Parameters["ConeAngle"].SetValue(angleInRadians);
            if (effect.Parameters["LightFalloff"] != null)
                effect.Parameters["LightFalloff"].SetValue(LightFalloff);
        }
    }
}
