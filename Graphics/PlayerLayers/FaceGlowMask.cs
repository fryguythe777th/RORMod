using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Graphics.PlayerLayers
{
    public class FaceGlowMask : PlayerDrawLayer
    {
        public static Dictionary<int, string> GlowMask { get; private set; }

        public override void Load()
        {
            GlowMask = new Dictionary<int, string>();
        }

        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.FaceAcc);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.face > 0 && !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawInfo.drawPlayer.face] && GlowMask.TryGetValue(drawInfo.drawPlayer.face, out string texturePath))
            {
                var texture = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad).Value;
                drawInfo.DrawDataCache.Add(
                    new DrawData(texture,
                    new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.bodyFrame.Width / 2 + drawInfo.drawPlayer.width / 2),
                    (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.headPosition + drawInfo.headVect,
                    drawInfo.drawPlayer.bodyFrame, drawInfo.drawPlayer.GetImmuneAlphaPure(Color.White, drawInfo.shadow) * drawInfo.drawPlayer.stealth, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0)
                    { shader = drawInfo.cFace, });
            }
        }
    }
}
