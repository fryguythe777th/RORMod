﻿using Microsoft.Xna.Framework;

namespace RiskOfTerrain.Graphics.Primitives
{
    public interface IPrimRenderer
    {
        void Draw(Vector2[] arr, float uvAdd = 0f, float uvMultiplier = 1f);
    }
}