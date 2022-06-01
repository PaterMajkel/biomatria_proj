using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace biomatria_proj
{
        public class BoundingRenderer
        {
            private static BasicEffect effect;
            private static GraphicsDevice graphics;

            private static VertexPositionColor[] krabiceVerts = new VertexPositionColor[8];
            private static readonly int[] krabiceIndices = new int[]
            {
            0, 1,
            1, 2,
            2, 3,
            3, 0,
            0, 4,
            1, 5,
            2, 6,
            3, 7,
            4, 5,
            5, 6,
            6, 7,
            7, 4,
            };

            // na buffery ještě v našich návodech nedošlo, považujte je za lepší než je to pole
            // hlavne v rychlosti vykreslování
            private static VertexBuffer kouleBuffer;
            private static int pocetCasti;

            public static void Initialize(GraphicsDevice device)
            {
                effect = new BasicEffect(device);
                effect.LightingEnabled = false;
                effect.VertexColorEnabled = true;

                graphics = device;

                pocetCasti = 15;// zde si nastavte jakoukoliv konstantu, ale 15 uz dava dobre vysledky, doporucuji liche cislo

                VertexPositionColor[] verts = new VertexPositionColor[(pocetCasti + 1) * 3];

                int index = 0;

                float step = MathHelper.TwoPi / (float)pocetCasti;

                //create the loop on the XY plane first
                for (float a = 0f; a <= MathHelper.TwoPi; a += step)
                {
                    verts[index++] = new VertexPositionColor(
                        new Vector3((float)Math.Cos(a), (float)Math.Sin(a), 0f),
                        Color.White);
                }

                //next on the XZ plane
                for (float a = 0f; a <= MathHelper.TwoPi; a += step)
                {
                    verts[index++] = new VertexPositionColor(
                        new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a)),
                        Color.White);
                }

                //finally on the YZ plane
                for (float a = 0f; a <= MathHelper.TwoPi; a += step)
                {
                    verts[index++] = new VertexPositionColor(
                        new Vector3(0f, (float)Math.Cos(a), (float)Math.Sin(a)),
                        Color.White);
                }

                kouleBuffer = new VertexBuffer(device, typeof(VertexPositionColor), verts.Length, BufferUsage.None);
                kouleBuffer.SetData(verts);
            }

            public static void Render(BoundingBox box, Matrix view, Matrix projection, Color color)
            {
                Vector3[] corners = box.GetCorners();
                for (int i = 0; i < 8; i++)
                {
                    krabiceVerts[i].Position = corners[i];
                    krabiceVerts[i].Color = color;
                }

                effect.View = view;
                effect.Projection = projection;
                effect.World = Matrix.Identity;

                effect.CurrentTechnique.Passes[0].Apply();
                //graphics.DepthStencilState = DepthStencilState.None;
                graphics.DrawUserIndexedPrimitives(PrimitiveType.LineList, krabiceVerts, 0, 8, krabiceIndices, 0, krabiceIndices.Length / 2);
                //graphics.DepthStencilState = DepthStencilState.Default;
            }

            public static void Render(BoundingSphere sphere, Matrix view, Matrix projection, Color Color)
            {
                Render(sphere, view, projection, Color, Color, Color);
            }

            public static void Render(BoundingSphere sphere, Matrix view, Matrix projection, Color xyColor, Color xzColor, Color yzColor)
            {
                graphics.SetVertexBuffer(kouleBuffer);// budeme pouzivat buffer

                // nastavime kouli spravny rozmer a posuneme ji do stredu
                effect.World =
                    Matrix.CreateScale(sphere.Radius) *
                    Matrix.CreateTranslation(sphere.Center);
                effect.View = view;
                effect.Projection = projection;

                // 1. prstenec zacina od nuly po pocet casti
                effect.DiffuseColor = xyColor.ToVector3();
                effect.CurrentTechnique.Passes[0].Apply();
                graphics.DrawPrimitives(PrimitiveType.LineStrip, 0, pocetCasti);

                // 2. prstenec zacina na pocet casti+1 a je jich pocet casti
                effect.DiffuseColor = xzColor.ToVector3();
                effect.CurrentTechnique.Passes[0].Apply();
                graphics.DrawPrimitives(PrimitiveType.LineStrip, pocetCasti + 1, pocetCasti);

                // 3. prstenec zacinak na 2*(pocetcasti+1) a je jich opet pocet casti
                effect.DiffuseColor = yzColor.ToVector3();
                effect.CurrentTechnique.Passes[0].Apply();
                graphics.DrawPrimitives(PrimitiveType.LineStrip, (pocetCasti + 1) * 2, pocetCasti);

                effect.DiffuseColor = Color.White.ToVector3();
            }
    }

}
