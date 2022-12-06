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
        public bool friendly;
        public bool spawnedFromStatue;

        public Vector2 Center => new Vector2(position.X + width / 2f, position.Y + height / 2f);
    }
}