﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Graphics;
using System;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace RiskOfTerrain.Graphics.RenderTargets
{
    public abstract class RequestableRenderTarget : ARenderTargetContentByRequest, ILoadable
    {
        protected RenderTarget2D helperTarget;

        protected virtual void PrepareRenderTargetsForDrawing(GraphicsDevice device, SpriteBatch spriteBatch)
        {
        }

        protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            PrepareRenderTargetsForDrawing(device, spriteBatch);

            var targets = device.GetRenderTargets();
            device.SetRenderTarget(helperTarget);
            device.Clear(Color.Transparent);
            DrawOntoTarget(device, spriteBatch);
            device.SetRenderTargets(targets);
        }

        protected virtual void DrawOntoTarget(GraphicsDevice device, SpriteBatch spriteBatch)
        {
        }

        protected void DrawHelperToTarget(RenderTarget2D target, GraphicsDevice device, SpriteBatch spriteBatch, Action action)
        {
            device.SetRenderTarget(target);

            action();

            device.SetRenderTarget(helperTarget);
        }

        public virtual void Load(Mod mod)
        {
            if (ROREffects.Renderers == null)
                ROREffects.Renderers = new List<RequestableRenderTarget>();
            ROREffects.Renderers.Add(this);
        }
        public virtual void Unload()
        {

        }

        public void CheckSelfRequest()
        {
            if (SelfRequest())
            {
                Request();
            }
        }

        protected virtual bool SelfRequest()
        {
            return false;
        }
    }
}