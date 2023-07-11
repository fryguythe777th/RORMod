using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Graphics.RenderTargets;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Graphics
{
    public class ROREffects : ModSystem
    {
        public class ScreenShake
        {
            public float Intensity;
            public float MultiplyPerTick;

            private Vector2 _shake;

            public void Update()
            {
                if (Intensity > 0f)
                {
                    _shake = (Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(Intensity * 0.8f, Intensity)).Floor();
                    Intensity *= MultiplyPerTick;
                }
                else
                {
                    Clear();
                }
            }

            public Vector2 GetScreenOffset()
            {
                return _shake;
            }

            public void Set(float intensity, float multiplier = 0.9f)
            {
                Intensity = intensity;
                MultiplyPerTick = multiplier;
            }

            public void Clear()
            {
                Intensity = 0f;
                MultiplyPerTick = 0.9f;
                _shake = new Vector2();
            }
        }

        public static ScreenShake Shake { get; private set; }

        public static List<RequestableRenderTarget> Renderers { get; internal set; }

        public override void Load()
        {
            if (Main.dedServ)
            {
                return;
            }
            Shake = new ScreenShake();
            if (Renderers == null)
                Renderers = new List<RequestableRenderTarget>();
            LoadHooks();
        }
        private void LoadHooks()
        {
            Terraria.On_Main.DoDraw_UpdateCameraPosition += Main_DoDraw_UpdateCameraPosition;
            Terraria.On_Main.DrawDust += Hook_OnDrawDust;
        }

        public override void Unload()
        {
            Shake = null;
            Renderers?.Clear();
            Renderers = null;
        }

        public override void OnWorldLoad()
        {
            if (Main.dedServ)
            {
                return;
            }
        }

        public override void PreUpdatePlayers()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Shake.Update();
            }
        }

        internal static void UpdateScreenPosition()
        {
            Main.screenPosition += Shake.GetScreenOffset() * ClientConfig.Instance.ScreenShake;
        }

        private static void Main_DoDraw_UpdateCameraPosition(Terraria.On_Main.orig_DoDraw_UpdateCameraPosition orig)
        {
            orig();
            foreach (var r in Renderers)
            {
                r.CheckSelfRequest();
                r.PrepareRenderTarget(Main.instance.GraphicsDevice, Main.spriteBatch);
            }
        }

        private static void Hook_OnDrawDust(Terraria.On_Main.orig_DrawDust orig, Main self)
        {
            orig(self);
            try
            {
            }
            catch
            {

            }
        }
    }
}