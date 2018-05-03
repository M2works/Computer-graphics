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
    class ChaseLight : Light
    {
        public Vector3 Target { get; private set; }
        public Vector3 FollowTargetPosition { get; private set; }
        public Vector3 FollowTargetRotation { get; private set; }
        public Vector3 PositionOffset { get; set; }
        public Vector3 TargetOffset { get; set; }
        public Vector3 RelativeLightRotation { get; set; }

        float springiness = .15f;
        public float Springiness
        {
            get { return springiness; }
            set { springiness = MathHelper.Clamp(value, 0, 1); }
        }
        public ChaseLight(Vector3 PositionOffset, Vector3 TargetOffset, Vector3 RelativeCameraRotation)
        {
            this.PositionOffset = PositionOffset;
            this.TargetOffset = TargetOffset;
            this.RelativeLightRotation = RelativeCameraRotation;
            this.LightColor = new Vector3(.85f, .85f, .85f);
        }

        public void Move(Vector3 NewFollowTargetPosition,
 Vector3 NewFollowTargetRotation)
        {
            this.FollowTargetPosition = NewFollowTargetPosition;
            this.FollowTargetRotation = NewFollowTargetRotation;
        }
        public void Rotate(Vector3 RotationChange)
        {
            this.RelativeLightRotation += RotationChange;
        }
        public void Update()
        {
            // Sum the rotations of the model and the camera to ensure it
            // is rotated to the correct LightPosition relative to the model's
            // rotation
            Vector3 combinedRotation = FollowTargetRotation +
            RelativeLightRotation;
            // Calculate the rotation matrix for the camera
            Matrix rotation = Matrix.CreateFromYawPitchRoll(
            combinedRotation.Y, combinedRotation.X, combinedRotation.Z);
            // Calculate the LightPosition the camera would be without the spring
            // value, using the rotation matrix and target position
            Vector3 desiredPosition = FollowTargetPosition +
            Vector3.Transform(PositionOffset, rotation);
            // Interpolate between the current LightPosition and desired position
            Target = FollowTargetPosition + Vector3.Transform(TargetOffset,
            rotation);
            LightPosition = Vector3.Lerp(LightPosition, desiredPosition, Springiness);
            LightDirection = LightPosition - Target;
        }
    }
}
