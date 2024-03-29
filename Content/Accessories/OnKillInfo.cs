﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Elites;
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
        public int lastHitProjectileType;
        public int aiStyle;
        public int whoAmI;

        public Vector2 Center => new Vector2(position.X + width / 2f, position.Y + height / 2f);
    }
}