using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Content.Artifacts;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain
{
    public static class Helpers
    {
        public static int ColorOnlyShaderIndex => ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex;
        public static ArmorShaderData ColorOnlyShader => GameShaders.Armor.GetSecondaryShader(ColorOnlyShaderIndex, Main.LocalPlayer);

        public static Vector2 TileDrawOffset => Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

        public static Color UseR(this Color color, int R) => new Color(R, color.G, color.B, color.A);
        public static Color UseR(this Color color, float R) => new Color((int)(R * 255), color.G, color.B, color.A);

        public static Color UseG(this Color color, int G) => new Color(color.R, G, color.B, color.A);
        public static Color UseG(this Color color, float G) => new Color(color.R, (int)(G * 255), color.B, color.A);

        public static Color UseB(this Color color, int B) => new Color(color.R, color.G, B, color.A);
        public static Color UseB(this Color color, float B) => new Color(color.R, color.G, (int)(B * 255), color.A);

        public static Color UseA(this Color color, int alpha) => new Color(color.R, color.G, color.B, alpha);
        public static Color UseA(this Color color, float alpha) => new Color(color.R, color.G, color.B, (int)(alpha * 255));

        public static UniversalAccessoryHandler GetParentHandler(this Projectile projectile)
        {
            var parent = GetParent(projectile);
            if (parent == null)
                return null;

            return GetHandler(parent);
        }

        public static UniversalAccessoryHandler GetHandler(this Entity entity)
        {
            if (entity is Player player)
                return player.ROR().Accessories;
            return null;
        }

        public static Entity GetParent(this Projectile projectile)
        {
            if (projectile.hostile)
                return null;

            if (projectile.owner >= 255 || projectile.owner < 0)
                return null;

            return Main.player[projectile.owner];
        }

        public static Vector2 ClosestDistance(this Rectangle rect, Vector2 other)
        {
            var center = rect.Center.ToVector2();
            var n = Vector2.Normalize(other - center);
            float x = Math.Min(Math.Abs(other.X - center.X), rect.Width / 2f);
            float y = Math.Min(Math.Abs(other.Y - center.Y), rect.Height / 2f);
            return center + n * new Vector2(x, y);
        }

        public static void Begin_GeneralEntities(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
        }
        public static void BeginShader_GeneralEntities(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
        }

        public static void Begin_GeneralEntities(SpriteBatch spriteBatch, Matrix matrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, matrix);
        }
        public static void BeginShader_GeneralEntities(SpriteBatch spriteBatch, Matrix matrix)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, matrix);
        }

        public static Vector2[] CircularVector(int amt, float angleAddition = 0f)
        {
            return Array.ConvertAll(Circular(amt, angleAddition), (f) => f.ToRotationVector2());
        }
        public static float[] Circular(int amt, float angleAddition = 0f)
        {
            var v = new float[amt];
            float f = MathHelper.TwoPi / amt;
            for (int i = 0; i < amt; i++)
            {
                v[i] = (f * i + angleAddition) % MathHelper.TwoPi;
            }
            return v;
        }

        public static string GetKeyName(ModKeybind keybind)
        {
            var s = keybind.GetAssignedKeys();
            if (s.Count == 0)
            {
                return Language.GetTextValue("Mods.RiskOfTerrain.UnboundKey");
            }
            return s[0];
        }

        public static void GetItemDrawData(int item, out Rectangle frame)
        {
            frame = Main.itemAnimations[item] == null ? TextureAssets.Item[item].Value.Frame() : Main.itemAnimations[item].GetFrame(TextureAssets.Item[item].Value);
        }
        public static void GetItemDrawData(this Item item, out Rectangle frame)
        {
            GetItemDrawData(item.type, out frame);
        }

        public static bool HereditarySource(IEntitySource source, out Entity entity)
        {
            entity = null;
            if (source == null)
            {
                return false;
            }
            if (source is IEntitySource_OnHit onHit)
            {
                entity = onHit.Victim;
                return true;
            }
            else if (source is EntitySource_Parent parent)
            {
                entity = parent.Entity;
                return true;
            }
            else if (source is EntitySource_Death death)
            {
                entity = death.Entity;
                return true;
            }
            return false;
        }

        public static bool IsElite(this NPC npc)
        {
            foreach (var e in RORNPC.RegisteredElites)
            {
                if (npc.GetGlobalNPC(e).Active)
                {
                    return true;
                }
            }
            return false;
        }

        public static void GetElitePrefixes(this NPC npc, out List<EliteNPCBase> prefixes)
        {
            prefixes = new List<EliteNPCBase>();
            foreach (var e in RORNPC.RegisteredElites)
            {
                var npcE = npc.GetGlobalNPC(e);
                if (npcE.Active)
                {
                    prefixes.Add(npcE);
                }
            }
        }

        public static void DrawRectangle(Rectangle rect, Color color)
        {
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, rect, color);
        }

        public static void DrawRectangle(Rectangle rect, Vector2 offset, Color color)
        {
            rect.X += (int)offset.X;
            rect.Y += (int)offset.Y;
            DrawRectangle(rect, color);
        }

        public static bool IsProbablyACritter(this NPC npc)
        {
            return NPCID.Sets.CountsAsCritter[npc.type] || (npc.lifeMax < 5 && npc.lifeMax != 1);
        }

        /// <summary>
        /// Attempts to find a projectile index using the identity and owner provided. Returns -1 otherwise.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static int FindProjectileIdentity(int owner, int identity)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].owner == owner && Main.projectile[i].identity == identity && Main.projectile[i].active)
                {
                    return i;
                }
            }
            return -1;
        }
        public static int FindProjectileIdentity_OtherwiseFindPotentialSlot(int owner, int identity)
        {
            int projectile = FindProjectileIdentity(owner, identity);
            if (projectile == -1)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (!Main.projectile[i].active)
                    {
                        projectile = i;
                        break;
                    }
                }
            }
            if (projectile == 1000)
            {
                projectile = Projectile.FindOldestProjectile();
            }
            return projectile;
        }

        public static Rectangle Frame(this Projectile projectile)
        {
            return TextureAssets.Projectile[projectile.type].Value.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
        }

        public static float UnNaN(this float value, float set)
        {
            return float.IsNaN(value) ? set : value;
        }
        public static Vector2 UnNaN(this Vector2 value, float set)
        {
            return new Vector2(UnNaN(value.X, set), UnNaN(value.Y, set));
        }

        public static float UnNaN(this float value)
        {
            return float.IsNaN(value) ? 0f : value;
        }
        public static Vector2 UnNaN(this Vector2 value)
        {
            return new Vector2(UnNaN(value.X), UnNaN(value.Y));
        }

        public static void CollideWithOthers(this Projectile projectile, float speed = 0.05f)
        {
            var rect = projectile.getRect();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && i != projectile.whoAmI && projectile.type == Main.projectile[i].type && projectile.owner == Main.projectile[i].owner
                    && projectile.Colliding(rect, Main.projectile[i].getRect()))
                {
                    projectile.velocity += Main.projectile[i].DirectionTo(projectile.Center).UnNaN(1f) * speed;
                }
            }
        }

        public static void GetDrawInfo(this Projectile projectile, out Texture2D texture, out Vector2 offset, out Rectangle frame, out Vector2 origin, out int trailLength)
        {
            texture = TextureAssets.Projectile[projectile.type].Value;
            offset = projectile.Size / 2f;
            frame = Frame(projectile);
            origin = frame.Size() / 2f;
            trailLength = ProjectileID.Sets.TrailCacheLength[projectile.type];
        }

        public static void DefaultToExplosion(this Projectile projectile, int size, DamageClass damageClass, int timeLeft = 2)
        {
            projectile.width = size;
            projectile.height = size;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.DamageType = damageClass;
            projectile.aiStyle = -1;
            projectile.timeLeft = timeLeft;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = projectile.timeLeft + 1;
            projectile.penetrate = -1;
        }

        public static ArtifactProj Artifacts(this Projectile npc)
        {
            return npc.GetGlobalProjectile<ArtifactProj>();
        }
        public static RORProjectile ROR(this Projectile projectile)
        {
            return projectile.GetGlobalProjectile<RORProjectile>();
        }

        public static ArtifactNPC Artifacts(this NPC npc)
        {
            return npc.GetGlobalNPC<ArtifactNPC>();
        }
        public static RORNPC ROR(this NPC npc)
        {
            return npc.GetGlobalNPC<RORNPC>();
        }

        public static Rectangle GetViewRectangle(this Entity entity, int width = 1920, int height = 1080)
        {
            return new Rectangle((int)entity.position.X + entity.width / 2 - width / 2, (int)entity.position.Y + entity.height / 2 - height / 2, width, height);
        }

        public static ArtifactPlayer Artifacts(this Player player)
        {
            return player.GetModPlayer<ArtifactPlayer>();
        }
        public static RORPlayer ROR(this Player player)
        {
            return player.GetModPlayer<RORPlayer>();
        }
    }
}