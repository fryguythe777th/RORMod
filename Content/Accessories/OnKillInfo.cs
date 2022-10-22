using Microsoft.Xna.Framework;
using Terraria;

namespace RiskOfTerrain.Content.Accessories
{
    public struct OnKillInfo
    {
        public int type;
        public Vector2 position;
        public int width;
        public int height;
        public int lifeMax;
        public int lastHitDamage;
        public BitsByte miscInfo;
        public float value;
    }
}