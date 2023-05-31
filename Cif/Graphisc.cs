using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Cif
{
    public static class GraphiscHelper
    {

        public static GraphicsDevice graphisc_device;
        public static BasicEffect basicEffect;
        private static Texture2D texture;
 

        public static void Ini(GraphicsDevice _graphisc_device, BasicEffect _basicEffect)
        {
            graphisc_device = _graphisc_device;
            basicEffect = _basicEffect;

            texture = new Texture2D(graphisc_device, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[1] { new Color(1f, 1f, 1f) });
        }

        public static void Line(Vector3 vector1, Vector3 vector2, Color color)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[2];
            vertices[0].Position = new Vector3(vector1.X, vector1.Y, vector1.Z);
            vertices[0].Color = color;
            vertices[1].Position = new Vector3(vector2.X, vector2.Y, vector2.Z);
            vertices[1].Color = color;

            basicEffect.Projection = Camera.Projection2;
            basicEffect.View = Camera.View;
            basicEffect.World = Matrix.Identity;

            //Draw the line
            basicEffect.CurrentTechnique.Passes[0].Apply();
            graphisc_device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 1);

        }
        public static void Line(Vector2 vector1, Vector2 vector2, Color color)
        {
            basicEffect.Projection = Camera.Projection2;
            basicEffect.View = Matrix.Identity;
            basicEffect.World = Matrix.Identity;
            basicEffect.TextureEnabled = false;
            basicEffect.VertexColorEnabled = true;

            VertexPositionColor[] vertices = new VertexPositionColor[2];
            vertices[0].Position = new Vector3(vector1.X, vector1.Y, 0);
            vertices[0].Color = color;
            vertices[1].Position = new Vector3(vector2.X, vector2.Y, 0);
            vertices[1].Color = color;

            //Draw the line
            basicEffect.CurrentTechnique.Passes[0].Apply();
            graphisc_device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 1);

        }

        public static void MultiLine(List<Vector2> Position, Color color)
        {
            basicEffect.Projection = Camera.Projection2;
            basicEffect.View = Matrix.Identity;
            basicEffect.World = Matrix.Identity;


            if (Position.Count < 2) return;
            VertexPositionColor[] vertices = new VertexPositionColor[Position.Count + 1];
            for (int i = 0; i < Position.Count; i++)
            {
                vertices[i].Position = new Vector3(Position[i].X, Position[i].Y, 0);
                vertices[i].Color = color;
            }
            vertices[Position.Count] = vertices[0];


            //Draw multi line
            basicEffect.CurrentTechnique.Passes[0].Apply();
            graphisc_device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, Position.Count);
        }


        public static void Circle(Vector2 Position, float Radius, Color color, int NrumberOfVertices = 360, int vertice_number = 360)
        {
            if (vertice_number > NrumberOfVertices) vertice_number = NrumberOfVertices;
            VertexPositionColor[] vertices = new VertexPositionColor[NrumberOfVertices + 1];
            for (int i = 0; i < vertice_number; i++)
            {
                float angle = (float)(1.0f * i / NrumberOfVertices * Math.PI * 2);
                vertices[i].Position = new Vector3(Position.X + (float)Math.Cos(angle) * Radius, Position.Y + (float)Math.Sin(angle) * Radius, 0);
                vertices[i].Color = color;
            }
            vertices[NrumberOfVertices] = vertices[0];

            //Draw circle
            basicEffect.CurrentTechnique.Passes[0].Apply();
            graphisc_device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, NrumberOfVertices);
        }

        public static void FiledCircle(Vector2 Position, float Radius, Color color1, Color color2, int NrumberOfVertices = 360, int vertice_number = 360, bool breaks = false)
        {
            if (vertice_number > NrumberOfVertices) vertice_number = NrumberOfVertices;
            float angle1, angle2;
            VertexPositionColor[] vertices = new VertexPositionColor[NrumberOfVertices + 1];

            float temp = (float)(1.0 / NrumberOfVertices * Math.PI * 2);
            int space = 1;
            if (breaks) space = 2;
            for (int i = 0; i < vertice_number; i += space)
            {
                angle1 = temp * i;
                angle2 = temp * (i + 1);

                vertices[0].Position = new Vector3(Position.X, Position.Y, 0);
                vertices[0].Color = color1;
                vertices[1].Position = new Vector3(Position.X + (float)Math.Cos(angle1) * Radius, Position.Y + (float)Math.Sin(angle1) * Radius, 0);
                vertices[1].Color = color2;
                vertices[2].Position = new Vector3(Position.X + (float)Math.Cos(angle2) * Radius, Position.Y + (float)Math.Sin(angle2) * Radius, 0);
                vertices[2].Color = color2;

                basicEffect.CurrentTechnique.Passes[0].Apply();
                //short[] triangleListIndices = new short[3] { 0, 1, 2 };
                graphisc_device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 1);

            }
        }
        public static void FiledRectangleTexture(Vector2 Position, float DimensionX, float DimensionY, float z, float alpha, Texture2D texture_intro)
        {
            VertexPositionTexture[] vertices = new VertexPositionTexture[6];


            vertices[0].Position = new Vector3(Position.X, Position.Y, z);
            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].Position = new Vector3(Position.X + DimensionX, Position.Y, z);
            vertices[1].TextureCoordinate = new Vector2(1, 0);
            vertices[2].Position = new Vector3(Position.X, Position.Y + DimensionY, z);
            vertices[2].TextureCoordinate = new Vector2(0, 1);

            vertices[3].Position = new Vector3(Position.X + DimensionX, Position.Y, 0);
            vertices[3].TextureCoordinate = new Vector2(1, 0);
            vertices[4].Position = new Vector3(Position.X + DimensionX, Position.Y + DimensionY, 0);
            vertices[4].TextureCoordinate = new Vector2(1, 1);
            vertices[5].Position = new Vector3(Position.X, Position.Y + DimensionY, 0);
            vertices[5].TextureCoordinate = new Vector2(0, 1);

            basicEffect.Projection = Camera.Projection2;
            basicEffect.View = Matrix.Identity;
            basicEffect.World = Matrix.Identity;

            basicEffect.PreferPerPixelLighting = true;
            basicEffect.Texture = texture_intro;
            basicEffect.TextureEnabled = true;
            basicEffect.LightingEnabled = true;
            basicEffect.VertexColorEnabled = false;

            basicEffect.Alpha = (float)Math.Sin(alpha * 3.14f);
            basicEffect.DirectionalLight0.DiffuseColor = basicEffect.AmbientLightColor = new Vector3(.9f, .9f, 0.9f);

            basicEffect.CurrentTechnique.Passes[0].Apply();
            graphisc_device.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);

        }

        public static void FiledRectangle(Vector2 Position, float DimensionX, float DimensionY, float z, Color color1, Color color2)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[6];


            vertices[0].Position = new Vector3(Position.X, Position.Y, z);
            vertices[0].Color = color1;
            vertices[1].Position = new Vector3(Position.X + DimensionX, Position.Y, z);
            vertices[1].Color = color1;
            vertices[2].Position = new Vector3(Position.X, Position.Y + DimensionY, z);
            vertices[2].Color = color2;

            vertices[3].Position = new Vector3(Position.X + DimensionX, Position.Y, 0);
            vertices[3].Color = color1;
            vertices[4].Position = new Vector3(Position.X + DimensionX, Position.Y + DimensionY, 0);
            vertices[4].Color = color2;
            vertices[5].Position = new Vector3(Position.X, Position.Y + DimensionY, 0);
            vertices[5].Color = color2;

            basicEffect.Projection = Camera.Projection2;
            basicEffect.View = Matrix.Identity;
            basicEffect.World = Matrix.Identity;

            basicEffect.TextureEnabled = false;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;
            basicEffect.Alpha = 1;
            basicEffect.CurrentTechnique.Passes[0].Apply();
            graphisc_device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 2);
        }

        public static void FiledTriangle(Vector2 Position1, float radious, float angle_sin, float angle_cos, float angle, Color color1, Color color2)
        {

            VertexPositionColor[] vertices = new VertexPositionColor[3];

            vertices[0].Position = new Vector3(Position1.X, Position1.Y, 0);
            vertices[0].Color = color1;
            vertices[1].Position = new Vector3(Position1.X + angle_sin * radious, Position1.Y + angle_sin * radious, 0);
            vertices[1].Color = color1;
            vertices[2].Position = new Vector3(Position1.X + (float)(Math.Sin(angle) * radious + 0.1), Position1.Y + (float)(Math.Cos(angle + 0.1) * radious), 0);

            vertices[2].Color = color2;
            basicEffect.CurrentTechnique.Passes[0].Apply();
            graphisc_device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 1);
        }

        // VertexBuffer vertexBuffer;
        public static void FiledTriangle(VertexPositionColor[] vertices, Matrix world, Matrix view, Matrix projection, Vector3 VLight)
        {

            //Vert buffer
            //vertexBuffer = new VertexBuffer(graphisc_device, typeof(
            //               VertexPositionColor), 3, BufferUsage.
            //               WriteOnly);
            //vertexBuffer.SetData<VertexPositionColor>(vertices);


            basicEffect.Projection = projection;
            basicEffect.View = view;
            basicEffect.World = world;
            basicEffect.VertexColorEnabled = true;
            basicEffect.PreferPerPixelLighting = true;
            basicEffect.LightingEnabled = true;
            basicEffect.DirectionalLight0.DiffuseColor = basicEffect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.9f);
            basicEffect.DirectionalLight0.SpecularColor = basicEffect.SpecularColor = new Vector3(0.9f, 0.9f, 0.9f);
            basicEffect.SpecularPower = 10;
            basicEffect.Alpha = 0.05f;
            basicEffect.DirectionalLight0.Direction = VLight;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphisc_device.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                basicEffect.CurrentTechnique.Passes[0].Apply();
                // graphisc_device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length-2 );
                graphisc_device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length - 2);

            }
        }

        public static void FiledTriangle(VertexPositionNormalTexture[] vertices, Vector3 color, Matrix world, Vector3 VLight)
        {
            //Vert buffer
            //vertexBuffer = new VertexBuffer(graphisc_device, typeof(
            //               VertexPositionColor), 3, BufferUsage.
            //               WriteOnly);
            //vertexBuffer.SetData<VertexPositionColor>(vertices);
            if (vertices == null)
                return;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphisc_device.RasterizerState = rasterizerState;

            basicEffect.Texture = texture;
            basicEffect.TextureEnabled = true;
            basicEffect.LightingEnabled = true;
            basicEffect.VertexColorEnabled = false;

            basicEffect.DirectionalLight0.DiffuseColor = basicEffect.AmbientLightColor = color;
            basicEffect.DirectionalLight0.SpecularColor = basicEffect.SpecularColor = new Vector3(1f, 1f, 0.9f);
            basicEffect.SpecularPower = 20;
            basicEffect.Alpha = 0.2f;
            basicEffect.DirectionalLight0.Direction = VLight;

            basicEffect.Projection = Camera.Projection;
            basicEffect.View = Camera.View;
            basicEffect.World = world;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                basicEffect.CurrentTechnique.Passes[0].Apply();
                graphisc_device.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);//
            }

            //rasterizerState.CullMode = CullMode.CullClockwiseFace;
            //graphisc_device.RasterizerState = rasterizerState;
        }

    }
}
