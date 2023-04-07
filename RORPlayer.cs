using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Content.Artifacts;
using RiskOfTerrain.Graphics;
using RiskOfTerrain.Items.Accessories.T1Common;
using RiskOfTerrain.Items.Consumable;
using RiskOfTerrain.Projectiles.Misc;
using RiskOfTerrain.UI;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain
{
    public class RORPlayer : ModPlayer
    {
        public const int ShieldRegenerationTime = 300;

        public static bool SpawnHack;
        public static byte DifficultyHack;

        public float procRate;
        public int increasedRegen;
        public int aegisLifeCheck;

        public bool accAegis;

        public Item accFocusCrystal;
        public bool focusCrystalVisible;
        public int cFocusCrystal;
        public float focusCrystalDiameter;
        public float focusCrystalDamage;

        public int accDiosBestFriend;
        public int diosCooldown;
        public bool diosDead;

        public int checkElixir;

        public float bootSpeed;

        public float glass;
        public int HPLostToGlass;

        public int barrierLife;
        public float barrierMinimumFrac;

        public float shield;
        public float maxShield;

        public bool accGlubby;
        public bool glubbyHide;
        public byte glubbyActive;

        public bool accTopazBrooch;
        public bool accShieldGenerator;
        public Item accDeathMark;
        public bool accTriTipDagger;
        public bool accIgnitionTank;
        public bool accWaxQuail;
        public bool accFuelCell;

        public bool aspCelestine;

        public int accRepulsionPlate;

        public int killStreak;
        public int killStreakClearTimer;
        public int timeSinceLastHit;
        public int idleTime;

        public int cBungus;

        public int berzerkerCounter;
        public int berzerkerTimer;

        public bool Sprinting;

        public bool hitByBlazerProj;

        /// <summary>
        /// The closest 'enemy' NPC to the player. Updated in <see cref="PostUpdate"/> -> <see cref="DangerEnemy"/>
        /// </summary>
        public int dangerEnemy;
        public int dangerEnemyOld;

        public int BarrierMinimum { get; private set; }

        /// <summary>
        /// Helper for whether or not the player is in danger
        /// </summary>
        public bool InDanger => dangerEnemy != -1;

        public UniversalAccessoryHandler Accessories { get; private set; }

        public RORPlayer()
        {
            Accessories = new UniversalAccessoryHandler();
        }

        #region Loading Stuffs

        public override void Load()
        {
            Terraria.On_Player.UpdateDead += Player_UpdateDead;
            Terraria.On_Player.CheckSpawn += Player_CheckSpawn;
            Terraria.On_Player.FindSpawn += Player_FindSpawn;
            Terraria.On_Player.DropTombstone += Player_DropTombstone;
            Terraria.On_Player.AddBuff_DetermineBuffTimeToAdd += Player_AddBuffTime;
            Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.DrawPlayers += LegacyPlayerRenderer_DrawPlayers;
        }

        private void Player_DropTombstone(On_Player.orig_DropTombstone orig, Player self, long coinsOwned, NetworkText deathText, int hitDirection)
        {
            if (self.ROR().accDiosBestFriend > 0)
            {
                SoundEngine.PlaySound(RiskOfTerrain.GetSound("extralife", 0.1f, 0f, 0.1f));
                return;
            }
            orig(self, coinsOwned, deathText, hitDirection);
        }

        public override void SetStaticDefaults()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                //AutoAssignTest("Backup Magazine Swap", "MouseRight");
            }
        }

        private void AutoAssignTest(string name, string defaultKey)
        {
            var fullName = $"{Mod.Name}: {name}";
            var keyboardInputMode = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard];
            if (!keyboardInputMode.KeyStatus.ContainsKey(fullName) || keyboardInputMode.KeyStatus[fullName] == null || keyboardInputMode.KeyStatus[fullName].Count == 0)
            {
                keyboardInputMode.KeyStatus[fullName] = new List<string> { defaultKey.ToString() };
            }
        }

        private static void LegacyPlayerRenderer_DrawPlayers(Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.orig_DrawPlayers orig, Terraria.Graphics.Renderers.LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, System.Collections.Generic.IEnumerable<Player> players)
        {
            foreach (var p in players)
            {
                var ror = p.ROR();
                if (ror.diosDead)
                    p.dead = true;
            }
            orig(self, camera, players);
            foreach (var p in players)
            {
                var ror = p.ROR();
                if (ror.diosDead)
                    p.dead = false;
            }
        }

        private static void Player_FindSpawn(Terraria.On_Player.orig_FindSpawn orig, Player self)
        {
            if (SpawnHack)
                return;
            orig(self);
        }

        private static bool Player_CheckSpawn(Terraria.On_Player.orig_CheckSpawn orig, int x, int y)
        {
            if (SpawnHack)
                return true;

            return orig(x, y);
        }

        private static void Player_UpdateDead(Terraria.On_Player.orig_UpdateDead orig, Player player)
        {
            var ror = player.ROR();
            if (ror.accDiosBestFriend > 0 && ror.diosCooldown <= 0)
            {
                player.ResetFloorFlags();
                player.ResetVisibleAccessories();

                player.slotsMinions = 0;
                player.respawnTimer = Utils.Clamp(player.respawnTimer - 1, 0, 60);
                if (player.respawnTimer <= 0 && Main.myPlayer == player.whoAmI)
                {
                    if (Main.mouseItem.type > ItemID.None)
                    {
                        Main.playerInventory = true;
                    }
                    SpawnHack = true;
                    var tileCoords = player.Center.ToTileCoordinates();
                    int spawnX = player.SpawnX;
                    int spawnY = player.SpawnY;
                    player.SpawnX = tileCoords.X;
                    player.SpawnY = tileCoords.Y + 2;
                    player.Spawn(PlayerSpawnContext.ReviveFromDeath);
                    player.SpawnX = spawnX;
                    player.SpawnY = spawnY;

                    player.statLife = player.statLifeMax2;
                    ror.diosCooldown = ror.accDiosBestFriend;
                    player.AddBuff(ModContent.BuffType<DiosCooldown>(), ror.diosCooldown - 1);
                    Projectile.NewProjectile(player.GetSource_FromThis(), new Vector2(player.position.X + player.width / 2f, player.position.Y - 12f), new Vector2(0f, 0.1f), ModContent.ProjectileType<DioRevive>(), 0, 0f, player.whoAmI);
                    ror.diosDead = false;
                    SpawnHack = false;
                    return;
                }
                ror.diosDead = true;
                player.immuneTime = 300;
                player.immuneNoBlink = false;
                player.dead = false;
                player.gravDir = 1f;
                player.grappling[0] = -1;
                player.SetTalkNPC(-1);
                player.chest = -1;
                player.sign = -1;
                player.statLife = 0;
                player.changeItem = -1;
                player.itemAnimation = 0;
                player.immuneAlpha += 2;
                if (player.immuneAlpha > 255)
                {
                    player.immuneAlpha = 255;
                }

                player.headPosition += player.headVelocity;
                player.bodyPosition += player.bodyVelocity;
                player.legPosition += player.legVelocity;
                player.headRotation += player.headVelocity.X * 0.1f;
                player.bodyRotation += player.bodyVelocity.X * 0.1f;
                player.legRotation += player.legVelocity.X * 0.1f;
                player.headVelocity.Y += 0.1f;
                player.bodyVelocity.Y += 0.1f;
                player.legVelocity.Y += 0.1f;
                player.headVelocity.X *= 0.99f;
                player.bodyVelocity.X *= 0.99f;
                player.legVelocity.X *= 0.99f;
                return;
            }

            ror.diosCooldown = 0;
            ror.accDiosBestFriend = 0;
            orig(player);
        }

        private int Player_AddBuffTime(Terraria.On_Player.orig_AddBuff_DetermineBuffTimeToAdd orig, Player self, int type, int time1)
        {
            if (self.ROR().accFuelCell && Main.debuff[type] == true)
            {
                time1 = (int)(time1 * 0.85);
            }

            if (self.ROR().accFuelCell && Main.debuff[type] == false)
            {
                time1 = (int)(time1 * 1.15);
            }

            return orig(self, type, time1);
        }

        #endregion

        public int ProcRate(int num)
        {
            return (int)(num * procRate);
        }
        public bool ProcRate()
        {
            return Main.rand.NextFloat(1f) < procRate;
        }

        public override void ModifyScreenPosition()
        {
            ROREffects.UpdateScreenPosition();
        }

        public override void CopyClientState(ModPlayer clientClone)/* tModPorter Suggestion: Replace Item.Clone usages with Item.CopyNetStateTo */
        {
            var clone = (RORPlayer)clientClone;
            clone.barrierLife = barrierLife;
            clone.shield = shield;
            clone.diosCooldown = diosCooldown;
            clone.diosDead = diosDead;
            clone.timeSinceLastHit = timeSinceLastHit;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var client = (RORPlayer)clientPlayer;
            var p = RiskOfTerrain.GetPacket(PacketType.SyncRORPlayer);
            var bb = new BitsByte(
                false,
                client.barrierLife != barrierLife,
                client.shield != shield,
                client.diosCooldown != diosCooldown || client.diosDead != diosDead,
                client.timeSinceLastHit != timeSinceLastHit);

            p.Write(Player.whoAmI);
            p.Write(bb);
            if (bb[0])
            {
            }
            if (bb[1])
            {
                p.Write(barrierLife);
            }
            if (bb[2])
            {
                p.Write(shield);
            }
            if (bb[3])
            {
                p.Write(diosCooldown);
                p.Write(diosDead);
            }
            if (bb[4])
            {
                p.Write(timeSinceLastHit);
            }
        }

        public void RecieveChanges(BinaryReader reader)
        {
            var bb = (BitsByte)reader.ReadByte();
            if (bb[0])
            {
            }
            if (bb[1])
            {
                barrierLife = reader.ReadInt32();
            }
            if (bb[2])
            {
                shield = reader.ReadSingle();
            }
            if (bb[3])
            {
                diosCooldown = reader.ReadInt32();
                diosDead = reader.ReadBoolean();
            }
            if (bb[4])
            {
                timeSinceLastHit = reader.ReadInt32();
            }
        }

        public override void PreUpdate()
        {
            if (diosDead)
            {
                Player.dead = true;
            }

            if (accWaxQuail && Sprinting && Player.justJumped)
            {
                Player.velocity.X *= 1.5f;
            }

            float num = (Player.accRunSpeed + Player.maxRunSpeed) / 1.25f;

            if (Player.velocity.X < 0f - num && Player.velocity.Y == 0f && !Player.mount.Active)
            {
                Sprinting = true;
            }
            else if (Player.velocity.X > num && Player.velocity.Y == 0f && !Player.mount.Active)
            {
                Sprinting = true;
            }
            else
            {
                Sprinting = false;
            }
        }

        public override void UpdateDead()
        {
            barrierLife = 0;
            barrierMinimumFrac = 0f;
            aegisLifeCheck = 0;
        }

        public void UpdateAegis()
        {
        }

        public void UpdateDios()
        {
            accDiosBestFriend = 0;
            if (diosCooldown > 0)
            {
                if (!Player.HasBuff<DiosCooldown>())
                    Player.AddBuff(ModContent.BuffType<DiosCooldown>(), diosCooldown);
                diosCooldown--;
            }
        }

        public void UpdateIdleTime()
        {
            if (Player.velocity.Length() < 1f)
            {
                idleTime++;
            }
            else
            {
                idleTime = 0;
            }
        }

        public void UpdateBerzerkerPauldron()
        {
            if (berzerkerCounter == 4)
            {
                berzerkerCounter = 0;
                Player.AddBuff(ModContent.BuffType<BerzerkerBuff>(), 360);
            }

            if (berzerkerTimer > -1)
            {
                berzerkerTimer--;
            }

            if (berzerkerTimer == 0)
            {
                berzerkerCounter = 0;
            }
        }

        public override void ResetEffects()
        {
            barrierMinimumFrac = 0;
            UpdateDios();

            accFocusCrystal = null;
            focusCrystalDamage = 0f;
            focusCrystalDiameter = 0f;
            focusCrystalVisible = true;
            cFocusCrystal = 0;
            accTopazBrooch = false;
            accShieldGenerator = false;
            accGlubby = false;
            accDeathMark = null;
            accTriTipDagger = false;
            accRepulsionPlate = 0;
            accIgnitionTank = false;
            accWaxQuail = false;
            accFuelCell = false;
            aspCelestine = false;

            glass = ArtifactSystem.glass ? 0.9f : 0f;
            maxShield = 0f;

            checkElixir = ItemID.None;
            bootSpeed = 0f;
            increasedRegen = 0;
            procRate = 1f;

            timeSinceLastHit++;
            UpdateIdleTime();

            if (Accessories == null)
                Accessories = new UniversalAccessoryHandler();

            Accessories.ResetEffects(Player);
            FocusCrystal.HitNPCForMakingDamageNumberPurpleHack = null;
            SpawnHack = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Accessories.ProcessTriggers(Player, this);
        }

        public override void UpdateLifeRegen()
        {
            Accessories.UpdateLifeRegeneration(Player);
            Player.lifeRegen += increasedRegen;
        }

        public override void PostUpdateRunSpeeds()
        {
            Player.accRunSpeed += bootSpeed;
        }

        public override void PostUpdateEquips()
        {
            Accessories.PostUpdateEquips(Player);
            HPLostToGlass = 0;
            int lifeMax = Player.statLifeMax;
            if (glass > 0f)
            {
                HPLostToGlass = (int)(Player.statLifeMax2 * glass);
                lifeMax = (int)(lifeMax * (1f - glass));
                Player.statLifeMax2 -= HPLostToGlass;
            }
            shield = Math.Min(shield, maxShield);
            if (maxShield > 0f && timeSinceLastHit >= ShieldRegenerationTime)
            {
                shield = maxShield;
            }

            ManageLifeSupplements(shield, lifeMax);
            BarrierMinimum = (int)(Player.statLifeMax2 * barrierMinimumFrac);
            if (Player.statLife == Player.statLifeMax2)
            {
                Player.statLife += barrierLife;
            }
            Player.statLifeMax2 += barrierLife;

            if (shield > 0f && timeSinceLastHit == ShieldRegenerationTime)
            {
                SoundEngine.PlaySound(RiskOfTerrain.GetSound("personalshield").WithVolumeScale(0.15f), Player.Center);
                Player.statLife = Math.Min(Player.statLife + (int)(Player.statLifeMax2 * shield), Player.statLifeMax2);
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                ResourceOverlays.MaxShield = shield;
                ResourceOverlays.MaxBarrier = barrierLife / (float)Player.statLifeMax2;
                ResourceOverlays.MaxGlass = glass;
            }
        }

        public void ManageLifeSupplements(float amt, int lifeMax)
        {
            int add = (int)(lifeMax * amt);
            if (Player.statLife == Player.statLifeMax2)
            {
                Player.statLife += add;
            }
            Player.statLifeMax2 += add;
        }

        public void UpdateBarrierDrainage()
        {
            if (barrierLife > BarrierMinimum)
            {
                barrierLife -= Math.Max((int)((Player.statLifeMax2 + barrierLife) * 0.05f), 1);
                if (barrierLife < BarrierMinimum)
                    barrierLife = BarrierMinimum;
            }
        }

        public override void PostUpdate()
        {
            UpdateBarrierDrainage();
            UpdateBerzerkerPauldron();
            Accessories.PostUpdate(Player);
            if (Main.myPlayer == Player.whoAmI)
            {
                if (accFocusCrystal != null && focusCrystalVisible && Player.ownedProjectileCounts[ModContent.ProjectileType<FocusCrystalProj>()] == 0)
                {
                    Projectile.NewProjectile(Player.GetSource_Accessory(accFocusCrystal), Player.Center, Vector2.Zero, ModContent.ProjectileType<FocusCrystalProj>(), 0, 0f, Player.whoAmI);
                }
            }
            DangerEnemy();
        }

        /// <summary>
        /// Finds the closest enemy to the player, and caches its index in <see cref="Main.npc"/>
        /// </summary>
        public void DangerEnemy()
        {
            dangerEnemyOld = dangerEnemy;
            dangerEnemy = -1;

            var center = Player.Center;
            var checkTangle = new Rectangle((int)Player.position.X + Player.width / 2 - 1000, (int)Player.position.Y + Player.height / 2 - 500, 2000, 1000);
            float distance = 2000f;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy && Main.npc[i].CanBeChasedBy(Player) && !Main.npc[i].IsProbablyACritter())
                {
                    if (Main.npc[i].getRect().Intersects(checkTangle))
                    {
                        float d = Main.npc[i].Distance(center);
                        if (!Main.npc[i].noTileCollide && !Collision.CanHitLine(Main.npc[i].position, Main.npc[i].width, Main.npc[i].height, Player.position, Player.width, Player.height))
                        {
                            d *= 4f;
                        }
                        if (d < distance)
                        {
                            distance = d;
                            dangerEnemy = i;
                        }
                    }
                }
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (accRepulsionPlate > 0 && Player.lifeRegen < 0)
            {
                Player.lifeRegen = Math.Min(accRepulsionPlate + Player.lifeRegen, -1);
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)/* tModPorter Override ImmuneTo, FreeDodge or ConsumableDodge instead to prevent taking damage */
        {
            if (hitByBlazerProj)
            {
                modifiers.HitDirectionOverride = 0;
                hitByBlazerProj = false;
            }
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            return Accessories.FreeDodge(Player, info);
        }

        public void CheckElixir()
        {
            if (Player.cursed || Player.CCed || Player.dead || Player.statLife == Player.statLifeMax2 || Player.potionDelay > 0)
            {
                return;
            }

            for (int i = 0; i < Main.InventoryItemSlotsCount; i++)
            {
                if (!Player.inventory[i].IsAir && Player.inventory[i].type == ModContent.ItemType<PowerElixir>())
                {
                    Utils.Swap(ref Player.inventory[0], ref Player.inventory[i]);
                    Player.QuickHeal();
                    Utils.Swap(ref Player.inventory[0], ref Player.inventory[i]);
                    break;
                }
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            Accessories.Hurt(Player, this, info);
            timeSinceLastHit = 0;
            if (barrierLife > 0f)
            {
                barrierLife -= (int)info.Damage;
                if (barrierLife < 0)
                {
                    barrierLife = 0;
                }
            }
            else if (shield > 0f)
            {
                shield = (float)Math.Max(shield - info.Damage / (float)Player.statLifeMax, 0f);
                if (shield <= 0.01f)
                {
                    shield = 0f;
                    SoundEngine.PlaySound(RiskOfTerrain.GetSound("personalshieldgone"), Player.Center);
                }
            }
            if (checkElixir != ItemID.None && Player.statLife * 2 < Player.statLifeMax2)
            {
                CheckElixir();
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            Accessories.PreKill(Player, this, damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
            if (accDiosBestFriend > 0 && diosCooldown <= 0)
            {
                DifficultyHack = Player.difficulty;
                Player.difficulty = PlayerDifficultyID.SoftCore;
            }
            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            Accessories.Kill(Player, this, damage, hitDirection, pvp, damageSource);
            if (accDiosBestFriend > 0 && diosCooldown <= 0)
            {
                diosDead = true;
                Player.dead = false;
                Player.difficulty = DifficultyHack;
            }
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            if (Player.HasBuff(ModContent.BuffType<BandolierBuff>()))
            {
                return false;
            }
            return Accessories.CanConsumeAmmo(Player, this);
            //return Player.RollLuck(100) > (int)(backupMagAmmoReduction * 100f);
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Item, consider using ModifyHitNPC instead */
        {
            Accessories.ModifyHit(Player, target, item, ref modifiers.FinalDamage, ref modifiers.Knockback, ref modifiers);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
        {
            Accessories.ModifyHit(Player, target, proj, ref modifiers.FinalDamage, ref modifiers.Knockback, ref modifiers);
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            Accessories.OnHit(Player, target, item, hit);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            Accessories.OnHit(Player, target, proj, hit);
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            Accessories.OnHitBy(Player, npc, hurtInfo);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            Accessories.OnHitBy(Player, proj, hurtInfo);
        }

        public void OnKillEffect(int type, Vector2 position, int width, int height, int lifeMax, int lastHitDamage, BitsByte miscInfo, float value)
        {
            var center = position + new Vector2(width, height) / 2f;
        }
    }
}