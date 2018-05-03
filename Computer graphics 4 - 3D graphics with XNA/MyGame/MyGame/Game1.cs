using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TApplication = System.Windows.Forms.Application;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyGame
{
    using Cameras;
    using Materials;
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;

        ChaseCamera cam1;
        TargetCamera cam2;
        TargetCamera cam3;

        ChaseLight chaseLight;

        Material mat;
        Effect effect, PhongEffect, GouraudEffect;

        bool BlinnModel = false;

        int cameraChosen = 1;
        
        MouseState lastMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            models.Add(new CModel(Content.Load<Model>("Ship"),
             new Vector3(1000, 1000, 1000), Vector3.Zero, new Vector3(0.4f),
             GraphicsDevice));

            models.Add(new CModel(Content.Load<Model>("jula"),
             new Vector3(1000, 1000, 1000), Vector3.Zero, new Vector3(0.4f),
             GraphicsDevice));

            models.Add(new CModel(Content.Load<Model>("ground"),
             new Vector3(0, 0, 0), Vector3.Zero, new Vector3(10f),
             GraphicsDevice));
           
            PhongEffect = Content.Load<Effect>("MultiSpotLightEffect");
            GouraudEffect = Content.Load<Effect>("MultiSpotLightEffectGouraud");
            effect = PhongEffect;
                        
            chaseLight = new ChaseLight(new Vector3(0, 200, -100),
             new Vector3(0, 200, 0),
             new Vector3(0, 0, 0));

            mat = new MultiSpotLightMaterial(chaseLight);

            Random rand = new Random();

            for (int i = -1; i < 2; i += 2)
                for (int j = -1; j < 2; j += 2)
                {
                    Vector3 additionalRotation = Vector3.Zero;
                    if (i > 0)
                        additionalRotation += new Vector3(0, 1, 0) * (rand.Next() % 100 * .025f);
                    else
                        additionalRotation += new Vector3(0, -1, 0) * (rand.Next() % 100 * .025f);

                    models.Add(new CModel(Content.Load<Model>("deer"),
                    new Vector3(0 + i * 4000, 0, 0 + j *4000), Vector3.Zero + additionalRotation, Vector3.One, GraphicsDevice));
                }

            models.Add(new CModel(Content.Load<Model>("moon"),
             new Vector3(0, 10000, 10000), new Vector3(1.05f, 0, 1), new Vector3(500f),
             GraphicsDevice));

            models.Add(new CModel(Content.Load<Model>("fireplace"),
             new Vector3(1000, 100, 1000), new Vector3(1.05f,0,1), new Vector3(50f),
             GraphicsDevice));

            for (int i = -1; i < 2; i += 2)
                for (int j = -1; j < 2; j += 2)
                {
                    models.Add(new CModel(Content.Load<Model>("ball"),
                     new Vector3(2000*1, 100, 2000*j), new Vector3(1.05f, 0, 1), new Vector3(500f),
                     GraphicsDevice));
                }


            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    Vector3 additionalRotation = Vector3.Zero;
                    float scale = 2;

                    if (i > 0)
                        additionalRotation += new Vector3(0, 1, 0) * (rand.Next() % 100 * .025f);
                    else
                        additionalRotation += new Vector3(0, -1, 0) * (rand.Next() % 100 * .025f);

                    models.Add(new CModel(Content.Load<Model>("AlanTree"),
                    new Vector3(0 + i * 6000, 0, 0 + j * 6000), Vector3.Zero + additionalRotation, new Vector3(scale, scale, scale), GraphicsDevice));
                }

            foreach (CModel cm in models)
            {
                cm.Material = mat;
                cm.SetModelEffect(effect, true);
            }
            

            cam1 = new ChaseCamera(new Vector3(0, 200, 2500),
             new Vector3(0, 200, 0),
             new Vector3(0, 0, 0), GraphicsDevice);

            cam2 = new TargetCamera(
                    new Vector3(100, 15000, 100),
                    Vector3.Zero, GraphicsDevice);

            cam3 = new TargetCamera(
                    new Vector3(100, 15000, 100),
                    Vector3.Zero, GraphicsDevice);

            switch (cameraChosen)
            {
                case 1:
                    camera = cam1;
                    break;
                case 2:
                    camera = cam2;
                    break;
                case 3:
                    camera = cam3;
                    break;
                default:
                    throw new InvalidDataException("Z³y numer kamery");
            }

            lastMouseState = Mouse.GetState();
        }
        

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape))
                this.Exit();

            if (keyState.IsKeyDown(Keys.Z))
                foreach (CModel cm in models)
                {
                    cm.SetModelEffect(PhongEffect, true);
                }
            else if (keyState.IsKeyDown(Keys.X))
                foreach (CModel cm in models)
                {
                    cm.SetModelEffect(GouraudEffect, true);
                }       

            if (keyState.IsKeyDown(Keys.P))
                {
                BlinnModel = false;
                }
            if (keyState.IsKeyDown(Keys.B))
                {
                BlinnModel = true;
                }

            if (keyState.IsKeyDown(Keys.D1))
            {
                camera = cam1;
                cameraChosen = 1;
            }
            else if (keyState.IsKeyDown(Keys.D2))
            {
                camera = cam2;
                cameraChosen = 2;
            } else if (keyState.IsKeyDown(Keys.D3))
            {
                camera = cam3;
                cameraChosen = 3;
            }
            

            updateModel(gameTime);
            updateChaseLight(gameTime);
            updateCamera(gameTime);
            base.Update(gameTime);
        }
        void updateModel(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector3 rotChange = new Vector3(0, 0, 0);

            
            if (keyState.IsKeyDown(Keys.W))
                rotChange += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.S))
                rotChange += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.A))
                rotChange += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.D))
                rotChange += new Vector3(0, -1, 0);
            models[0].Rotation += rotChange * .025f;

            if (!keyState.IsKeyDown(Keys.Space))
                return;

            Matrix rotation = Matrix.CreateFromYawPitchRoll(
            models[0].Rotation.Y, models[0].Rotation.X,
            models[0].Rotation.Z);

            models[0].Position += Vector3.Transform(Vector3.Forward,
            rotation) * (float)gameTime.ElapsedGameTime.TotalMilliseconds *
            4;
        }

        void updateChaseLight(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector3 rotChange = new Vector3(0, 0, 0);

            if (keyState.IsKeyDown(Keys.Up))
                rotChange += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.Down))
                rotChange += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.Left))
                rotChange += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.Right))
                rotChange += new Vector3(0, -1, 0);
            chaseLight.Rotate(rotChange * .025f);
            chaseLight.Move(models[0].Position,
                    models[0].Rotation);
            chaseLight.Update();
            mat.SetEffectParameters(effect);
        }

        void updateCamera(GameTime gameTime)
        {
            switch(cameraChosen)
            {
                case 1:
                    ((ChaseCamera)camera).Move(models[0].Position,
                    models[0].Rotation);
                    cam3.Target = models[0].Position;
                    break;
                case 2:
                    cam1.Move(models[0].Position,
                    models[0].Rotation);
                    cam3.Target = models[0].Position;
                    break;
                case 3:
                    cam1.Move(models[0].Position,
                    models[0].Rotation);
                    cam3.Target = models[0].Position;
                    break;
                default:
                    throw new InvalidDataException("Z³y numer kamery");
            }


            camera.Update();
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                {
                    if(cameraChosen == 1)
                        model.Draw(camera.View, camera.Projection, ((ChaseCamera)camera).Position, BlinnModel);
                    else if (cameraChosen == 2 || cameraChosen == 3)
                        model.Draw(camera.View, camera.Projection, ((TargetCamera)camera).Position, BlinnModel);
                }

            base.Draw(gameTime);
        }


    }
}
