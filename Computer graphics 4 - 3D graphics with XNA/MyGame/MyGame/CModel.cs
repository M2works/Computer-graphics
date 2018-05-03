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
        public class CModel
        {
            public Vector3 Position { get; set; }
            public Vector3 Rotation { get; set; }
            public Vector3 Scale { get; set; }
            public Material Material { get; set; }
            public Model Model { get; private set; }
            private Matrix[] modelTransforms;
            private GraphicsDevice graphicsDevice;
            private BoundingSphere boundingSphere;

        public CModel(Model Model, Vector3 Position, Vector3 Rotation,
 Vector3 Scale, GraphicsDevice graphicsDevice)
        {
            this.Model = Model;
            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            this.Material = new Material();

            buildBoundingSphere();
            generateTags();

            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
            this.graphicsDevice = graphicsDevice;            
        }

        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = Matrix.CreateScale(Scale)
                * Matrix.CreateTranslation(Position);
                BoundingSphere transformed = boundingSphere;
                transformed = transformed.Transform(worldTransform);
                return transformed;
            }
        }
        private void buildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            // Merge all the model's built in bounding spheres
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(
                    modelTransforms[mesh.ParentBone.Index]);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }
            this.boundingSphere = sphere;
        }
        public void CacheEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    ((MeshTag)part.Tag).CachedEffect = part.Effect;
        }

        public void RestoreEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (((MeshTag)part.Tag).CachedEffect != null)
                        part.Effect = ((MeshTag)part.Tag).CachedEffect;
        }
        public void SetModelEffect(Effect effect, bool CopyEffect)
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = effect;
                    // Copy the effect if necessary
                    if (CopyEffect)
                        toSet = effect.Clone();
                    MeshTag tag = ((MeshTag)part.Tag);
                    // If this ModelMeshPart has a texture, set it to the effect
                    if (tag.Texture != null)
                    {
                        setEffectParameter(toSet, "BasicTexture", tag.Texture);
                        setEffectParameter(toSet, "TextureEnabled", true);
                    }
                    else
                        setEffectParameter(toSet, "TextureEnabled", false);
                    // Set our remaining parameters to the effect
                    setEffectParameter(toSet, "DiffuseColor", tag.Color);
                    setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);
                    part.Effect = toSet;
                }
        }

        public void setEffectParameter(Effect effect, string paramName, object val)
        {
            if (effect.Parameters[paramName] == null)
                return;

            if (val is Vector3)
            { effect.Parameters[paramName].SetValue((Vector3)val); }
            else if (val is bool)
            { effect.Parameters[paramName].SetValue((bool)val); }
            else if (val is Matrix)
            { effect.Parameters[paramName].SetValue((Matrix)val); }
            else if (val is Texture2D)
            { effect.Parameters[paramName].SetValue((Texture2D)val); }

        }
        public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition, bool BlinnModel)
        {
            // Calculate the base transformation by combining
            // translation, rotation, and scaling
            Matrix baseWorld = Matrix.CreateScale(Scale)
            * Matrix.CreateFromYawPitchRoll(
            Rotation.Y, Rotation.X, Rotation.Z)
            * Matrix.CreateTranslation(Position);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] *
                baseWorld;
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Effect effect = meshPart.Effect;
                    if (effect is BasicEffect)
                    {
                        ((BasicEffect)effect).World = localWorld;
                        ((BasicEffect)effect).View = View;
                        ((BasicEffect)effect).Projection = Projection;
                        ((BasicEffect)effect).EnableDefaultLighting();
                    }
                    else
                    {
                        setEffectParameter(effect, "World", localWorld);
                        setEffectParameter(effect, "View", View);
                        setEffectParameter(effect, "Projection", Projection);
                        setEffectParameter(effect, "CameraPosition", CameraPosition);
                        setEffectParameter(effect, "BlinnModel", BlinnModel);
                    }
                    Material.SetEffectParameters(effect);
                }
                mesh.Draw();
            }
        }
        private void generateTags()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (part.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)part.Effect;
                        MeshTag tag = new MeshTag(effect.DiffuseColor, effect.Texture,
                        effect.SpecularPower);
                        part.Tag = tag;
                    }
        }
    }
}
