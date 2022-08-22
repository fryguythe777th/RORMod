using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;

namespace ROR2Artifacts.Items.Accessories
{
    public class Shatterspleen : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+5% critical strike chance\n" +
                "All critical strikes will inflict Bleeding to enemies\n" +
                "Any enemy that dies with this Bleeding will erupt into a bloody explosion");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ArtifactPlayer artifactPlayer = player.GetModPlayer<ArtifactPlayer>();
            artifactPlayer.Shatterspleen = true;
            player.GetCritChance<GenericDamageClass>() += 5;
        }
    }

    public class ShatterBleedingDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Description.SetDefault("This enemy will constantly take damage and erupt upon death");
            Main.buffNoSave[this.Type] = true;
            Main.buffNoTimeDisplay[this.Type] = true;
        }

        public override string Texture => "ROR2Artifacts/Items/Accessories/DeathMarkDebuff";

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.NextBool(6))
                npc.StrikeNPC(1, 0, 1, false);
        }
    }

    public class BloodExplosion : ModProjectile
    {
        public override string Texture => "ROR2Artifacts/Items/Accessories/DeathMarkDebuff";

        public override void SetDefaults()
        {
            //Projectile.hide = true;
            Projectile.timeLeft = 5;
            Projectile.damage = 1;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 27;
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode);
            if (!Main.dedServ)
            {
                for (int i = 1; i < 17; i++)
                {
                    Vector2 vect = new Vector2(0, 6);
                    vect = vect.RotatedBy(MathHelper.ToRadians(i * 45) + (Main.rand.Next(0, 5) * Main.rand.NextBool().ToDirectionInt()));
                    Dust.NewDust(Projectile.position, 20, 20, DustID.Blood, vect.X, vect.Y, 0, Scale:3f);
                }
            }
            int explosion = Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileID.Grenade, 100, 17, Main.myPlayer);
            Main.projectile[explosion].timeLeft = 2;
            Main.projectile[explosion].hide = true;
            Main.projectile[explosion].damage = 100;
        }
    }
}