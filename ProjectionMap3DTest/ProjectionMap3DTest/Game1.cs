using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjectionMap3DTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice device;

        Effect effect;

        SpriteBatch spriteBatch;
        VertexPositionColor[] vertices;

        Matrix viewMatrix;
        Matrix projectionMatrix;
        Camera camera;

        int[] indices;
        VertexBuffer myVertexBuffer;
        IndexBuffer myIndexBuffer;

        SpriteFont defaultSpriteFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            device = graphics.GraphicsDevice;
            effect = Content.Load<Effect>("effects");
            defaultSpriteFont = Content.Load<SpriteFont>("DefaultSpriteFont");

            spriteBatch = new SpriteBatch(device);

            SetUpVertices();
            SetUpIndices();
            SetUpCamera();
            CopyToBuffers();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            camera.ProcessKeyboard(Keyboard.GetState(), gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            device.Clear(Color.Black);

            RasterizerState rs = new RasterizerState();
            rs.FillMode = FillMode.WireFrame;
            device.RasterizerState = rs;

            effect.CurrentTechnique = effect.Techniques["ColoredNoShading"];
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                pass.Apply();

            viewMatrix = Matrix.CreateLookAt(camera.Position, camera.LookAt, camera.UpDirection);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(camera.FieldOfView, device.Viewport.AspectRatio, 1.0f, 1000.0f);
            Matrix worldMatrix = Matrix.Identity;

            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            device.Indices = myIndexBuffer;
            device.SetVertexBuffer(myVertexBuffer);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, indices.Length / 3);

            spriteBatch.Begin();
            {
                string cameraPositionString = String.Format("Camera position: {0:0}, {1:0}, {2:0}", camera.Position.X, camera.Position.Y, camera.Position.Z);
                spriteBatch.DrawString(defaultSpriteFont, cameraPositionString, new Vector2(10, 10), Color.White);

                string cameraLookDirectionString = String.Format("Camera lookDirection: {0:0.000}, {1:0.000}, {2:0.000}", camera.LookDirection.X, camera.LookDirection.Y, camera.LookDirection.Z);
                spriteBatch.DrawString(defaultSpriteFont, cameraLookDirectionString, new Vector2(10, 25), Color.White);

                string cameraFieldOfViewString = String.Format("Camera fieldOfView: {0:0.0} degrees", MathHelper.ToDegrees(camera.FieldOfView));
                spriteBatch.DrawString(defaultSpriteFont, cameraFieldOfViewString, new Vector2(10, 40), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetUpVertices()
        {
            vertices = new VertexPositionColor[8];

            vertices[0].Position = new Vector3(0f, 0f, 0f);
            vertices[1].Position = new Vector3(9.5f, 0f, 0f);
            vertices[2].Position = new Vector3(9.5f, 12.3f, 0f);
            vertices[3].Position = new Vector3(0f, 12.3f, 0f);
            vertices[4].Position = new Vector3(0f, 0f, 4.8f);
            vertices[5].Position = new Vector3(9.5f, 0f, 4.8f);
            vertices[6].Position = new Vector3(9.5f, 12.3f, 4.8f);
            vertices[7].Position = new Vector3(0f, 12.3f, 4.8f);

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Color = Color.White;
        }

        private void SetUpIndices()
        {
            indices = new int[36];

            indices[0] = 0;
            indices[1] = 2;
            indices[2] = 3;
            indices[3] = 0;
            indices[4] = 1;
            indices[5] = 2;
            indices[6] = 1;
            indices[7] = 5;
            indices[8] = 2;
            indices[9] = 5;
            indices[10] = 6;
            indices[11] = 2;
            indices[12] = 7;
            indices[13] = 3;
            indices[14] = 6;
            indices[15] = 6;
            indices[16] = 3;
            indices[17] = 2;
            indices[18] = 0;
            indices[19] = 4;
            indices[20] = 5;
            indices[21] = 0;
            indices[22] = 5;
            indices[23] = 1;
            indices[24] = 4;
            indices[25] = 3;
            indices[26] = 7;
            indices[27] = 4;
            indices[28] = 0;
            indices[29] = 3;
            indices[30] = 4;
            indices[31] = 7;
            indices[32] = 6;
            indices[33] = 4;
            indices[34] = 6;
            indices[35] = 5;
        }

        private void SetUpCamera()
        {
            camera = new Camera();
            camera.Position = new Vector3(0, -65, 70);
            camera.LookDirection = new Vector3(0, 65, -70);
            camera.LookDirection.Normalize();
            camera.FieldOfView = 1.86f; // in radians
        }

        private void CopyToBuffers()
        {
            myVertexBuffer = new VertexBuffer(device, VertexPositionColor.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            myVertexBuffer.SetData(vertices);

            myIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            myIndexBuffer.SetData(indices);
        }
    }
}
