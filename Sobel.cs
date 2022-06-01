using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace biomatria_proj
{
    public class Sobel
    {
            protected Game Game;

            protected string EffectFileName;

            protected Effect Effect;
            protected SpriteBatch batch;

            public Sobel(Game game, string effect = "filter")
        {
                EffectFileName = effect;
                Game = game;
            }

            public virtual void Load()
            {
                Effect = Game.Content.Load<Effect>(EffectFileName);
                batch = new SpriteBatch(Game.GraphicsDevice);
            }

            public virtual void Update()
            {

            }

            public virtual void Draw(Texture2D input)
            {
                batch.Begin(SpriteSortMode.Deferred, null, null, null, null, Effect);
                batch.Draw(input, new Vector2(), Color.White);
                batch.End();

            /*Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.BlendState = BlendState.Opaque;

            Game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;*/
        }
    }
}

