using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectionMap3DTest
{
    class Camera
    {
        private const float SPEED = 10f;
        private const float OMEGA = 0.1f;

        #region Properties

        public Vector3 Position { get; set; }
        public Vector3 UpDirection { get; set; }
        public Vector3 LookDirection { get; set; }

        public Vector3 LookAt
        {
            get
            {
                return Position + LookDirection;
            }
        }

        #endregion Properties

        public Camera()
        {
            Position = Vector3.Zero;
            UpDirection = new Vector3(0, 1, 0);
            LookDirection = new Vector3(0, 0, -1);
        }

        public void ProcessKeyboard(KeyboardState keyboardState, GameTime gameTime)
        {
            // Position
            {
                Vector3 positionDirection = Vector3.Zero;
                float speedModifier = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) ? 5f : 1f;

                positionDirection.X += keyboardState.IsKeyDown(Keys.A) ? -1 : 0;
                positionDirection.X += keyboardState.IsKeyDown(Keys.D) ? 1 : 0;

                positionDirection.Y += keyboardState.IsKeyDown(Keys.F) ? -1 : 0;
                positionDirection.Y += keyboardState.IsKeyDown(Keys.R) ? 1 : 0;

                positionDirection.Z += keyboardState.IsKeyDown(Keys.W) ? -1 : 0;
                positionDirection.Z += keyboardState.IsKeyDown(Keys.S) ? 1 : 0;

                Position += positionDirection * SPEED * speedModifier * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // LookDirection
            {
                Vector2 rotation = Vector2.Zero;
                float speedModifier = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) ? 5f : 1f;

                rotation.X += keyboardState.IsKeyDown(Keys.L) ? -1 : 0;
                rotation.X += keyboardState.IsKeyDown(Keys.J) ? 1 : 0;

                rotation.Y += keyboardState.IsKeyDown(Keys.K) ? -1 : 0;
                rotation.Y += keyboardState.IsKeyDown(Keys.I) ? 1 : 0;

                float rotY = OMEGA * rotation.X * speedModifier * (float)gameTime.ElapsedGameTime.TotalSeconds;
                float rotX = OMEGA * rotation.Y * speedModifier * (float)gameTime.ElapsedGameTime.TotalSeconds;
                LookDirection = Vector3.Transform(LookDirection, Matrix.CreateRotationY(rotY) * Matrix.CreateRotationX(rotX)); ;
            }
        }
    }
}
