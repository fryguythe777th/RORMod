using Microsoft.Xna.Framework;
using RORMod.Buffs;
using RORMod.Buffs.Debuff;
using RORMod.Content.Artifacts;
using RORMod.Graphics;
using RORMod.Items.Consumable;
using RORMod.NPCs;
using RORMod.Projectiles.Misc;
using RORMod.UI;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RORMod
{
    public class RORPlayer : ModPlayer
    {
        public const int ShieldRegenerationTime = 300;

        public static ModKeybind AmmoSwapKey { get; private set; }

        public static bool SpawnHack;
        public static byte DifficultyHack;

        public float procRate;
        public int increasedRegen;

        public Item accBustlingFungus;
        public int cBungus;
        public float bungusRadius;
        public float bungusHealingPercent;

        public Item accGasoline;
        public bool accStunGrenade;

        public bool accBackupMagazine;
        public bool backupMagVisible;
        public float backupMagAmmoReduction;

        public bool accStickyBomb;

        public int accDiosBestFriend;
        public int diosCooldown;
        public bool diosDead;

        public bool checkRustedKey;
        public int checkElixir;

        public Item accMonsterTooth;

        public Item accWarbanner;
        public byte warbannerProgress_Enemies;

        public float bootSpeed;

        public float glass;
        public int HPLostToGlass;

        public float barrier;

        public float shield;
        public float maxShield;

        public bool accGlubby;
        public bool glubbyHide;
        public byte glubbyActive;

        public bool gLegPlayedSound;
        public bool gLegSounds;

        public bool accTopazBrooch;
        public bool accShieldGenerator;
        public bool accDeathMark;
        public Item accShatterspleen;
        public bool accMedkit;
        public bool accTougherTimes;
        public bool accTriTipDagger;
        public bool accPennies;
        public Item accFireworks;
        public bool accCrowbar;

        public bool accOpal;
        public int opalShieldTimer;
        public bool opalShieldActive;

        public int flatDebuffDamageReduction;

        public int timeSinceLastHit;
        public int idleTime;

        public float bossDamageMultiplier;

        /// <summary>
        /// The closest 'enemy' NPC to the player. Updated in <see cref="PostUpdate"/> -> <see cref="DangerEnemy"/>
        /// </summary>
        public int dangerEnemy;
        public int dangerEnemyOld;

        /// <summary>
        /// Helper for whether or not the player is in danger
        /// </summary>
        public bool InDanger => dangerEnemy != -1;

        public override void Load()
        {
            AmmoSwapKey = RegisterKeybind("Backup Magazine Swap", "MouseRight");
            On.Terraria.Player.UpdateDead += Player_UpdateDead;
            On.Terraria.Player.CheckSpawn += Player_CheckSpawn;
            On.Terraria.Player.FindSpawn += Player_FindSpawn;
            On.Terraria.Player.DropTombstone += Player_DropTombstone;
            On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.DrawPlayers += LegacyPlayerRenderer_DrawPlayers;
        }

        private ModKeybind RegisterKeybind(string name, string defaultKey)
        {
            var key = KeybindLoader.RegisterKeybind(Mod, name, defaultKey);
            return key;
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

        private static void LegacyPlayerRenderer_DrawPlayers(On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.orig_DrawPlayers orig, Terraria.Graphics.Renderers.LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, System.Collections.Generic.IEnumerable<Player> players)
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

        private static void Player_DropTombstone(On.Terraria.Player.orig_DropTombstone orig, Player self, int coinsOwned, NetworkText deathText, int hitDirection)
        {
            if (self.ROR().accDiosBestFriend > 0)
                return;
            orig(self, coinsOwned, deathText, hitDirection);
        }

        private static void Player_FindSpawn(On.Terraria.Player.orig_FindSpawn orig, Player self)
        {
            if (SpawnHack)
                return;
            orig(self);
        }

        private static bool Player_CheckSpawn(On.Terraria.Player.orig_CheckSpawn orig, int x, int y)
        {
            if (SpawnHack)
                return true;

            return orig(x, y);
        }

        private static void Player_UpdateDead(On.Terraria.Player.orig_UpdateDead orig, Player player)
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

        public bool ProcRate()
        {
            return Main.rand.NextFloat(1f) < procRate;
        }

        public override void ModifyScreenPosition()
        {
            ROREffects.UpdateScreenPosition();
        }

        public override void clientClone(ModPlayer clientClone)
        {
            var clone = (RORPlayer)clientClone;
            clone.warbannerProgress_Enemies = warbannerProgress_Enemies;
            clone.barrier = barrier;
            clone.shield = shield;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var client = (RORPlayer)clientPlayer;
            var p = RORMod.GetPacket(PacketType.SyncRORPlayer);
            var bb = new BitsByte(
                client.warbannerProgress_Enemies != warbannerProgress_Enemies,
                client.barrier != barrier,
                client.shield != shield);

            p.Write(Player.whoAmI);
            p.Write(bb);
            if (bb[0])
            {
                p.Write(warbannerProgress_Enemies);
            }
            if (bb[1])
            {
                p.Write(barrier);
            }
            if (bb[2])
            {
                p.Write(shield);
            }
        }

        public void RecieveChanges(BinaryReader reader)
        {
            var bb = (BitsByte)reader.ReadByte();
            if (bb[0])
            {
                warbannerProgress_Enemies = reader.ReadByte();
            }
            if (bb[1])
            {
                barrier = reader.ReadSingle();
            }
            if (bb[2])
            {
                shield = reader.ReadSingle();
            }
        }

        public override void PreUpdate()
        {
            if (diosDead)
            {
                Player.dead = true;
            }
        }

        public override void ResetEffects()
        {
            cBungus = 0;
            accBustlingFungus = null;
            bungusHealingPercent = 0f;
            bungusRadius = 0f;
            accGasoline = null;
            accStunGrenade = false;
            accStickyBomb = false;
            accTopazBrooch = false;
            accShieldGenerator = false;
            accGlubby = false;
            accDeathMark = false;
            accShatterspleen = null;
            accMedkit = false;
            accTougherTimes = false;
            accTriTipDagger = false;
            accOpal = false;
            accPennies = false;
            accFireworks = null;
            accCrowbar = false;

            accDiosBestFriend = 0;

            accWarbanner = null;
            accMonsterTooth = null;

            backupMagAmmoReduction = 0f;
            accBackupMagazine = false;
            backupMagVisible = false;
            procRate = 1f;

            if (diosCooldown > 0)
            {
                if (!Player.HasBuff<DiosCooldown>())
                    Player.AddBuff(ModContent.BuffType<DiosCooldown>(), diosCooldown);
                diosCooldown--;
            }
            checkRustedKey = false;
            checkElixir = ItemID.None;
            timeSinceLastHit++;
            maxShield = 0f;
            if (barrier > 0f)
            {
                barrier -= 0.05f / Player.statLifeMax2 + barrier * 0.001f;
                if (barrier < 0f)
                    barrier = 0f;
            }
            glass = ArtifactSystem.glass ? 0.9f : 0f;
            bootSpeed = 0f;
            gLegSounds = false;
            flatDebuffDamageReduction = 0;
            bossDamageMultiplier = 1f;

            if (Player.velocity.Length() < 1f)
            {
                idleTime++;
            }
            else
            {
                idleTime = 0;
            }

            SpawnHack = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (accBackupMagazine && !Player.mouseInterface && !Player.lastMouseInterface && AmmoSwapKey.JustPressed && ModContent.GetInstance<BackupMagazineInterface>().Rotation == 0f)
            {
                ProcessAmmoSwap();
            }
        }

        public void ProcessAmmoSwap()
        {
            var heldItem = Player.HeldItem;

            if (heldItem.useAmmo == 0)
            {
                return;
            }

            int count = 0;
            Item siftDownItem = null;
            for (int i = Main.InventoryAmmoSlotsStart; i < Main.InventoryAmmoSlotsStart + Main.InventoryAmmoSlotsCount; i++)
            {
                if (Player.inventory[i].IsAir || Player.inventory[i].ammo != heldItem.useAmmo)
                    continue;

                if (siftDownItem == null)
                {
                    siftDownItem = Player.inventory[i];
                    continue;
                }
                count++;
                Utils.Swap(ref Player.inventory[i], ref siftDownItem);
            }

            if (count == 0)
                return;

            for (int i = Main.InventoryAmmoSlotsStart; i < Main.InventoryAmmoSlotsStart + Main.InventoryAmmoSlotsCount; i++)
            {
                if (Player.inventory[i].IsAir || Player.inventory[i].ammo != heldItem.useAmmo)
                    continue;
                count++;
                Utils.Swap(ref Player.inventory[i], ref siftDownItem);
                break;
            }

            SoundEngine.PlaySound(RORMod.GetSound("backupmagazine"), Player.Center);

            ModContent.GetInstance<BackupMagazineInterface>().Opacity = 1f;
            ModContent.GetInstance<BackupMagazineInterface>().TimeActive = 0;
        }

        public override void UpdateLifeRegen()
        {
            UpdateCautiousSlug();
            Player.lifeRegen += increasedRegen;
        }

        public void UpdateCautiousSlug()
        {
            if (accGlubby)
            {
                if (glubbyActive > 120)
                {
                    if (InDanger)
                    {
                        glubbyActive = 0;
                        if (!glubbyHide)
                            SoundEngine.PlaySound(RORMod.GetSound("glubbyhide").WithVolumeScale(0.4f));
                    }
                    Player.lifeRegen += 25;
                    return;
                }

                if (!InDanger || Player.Distance(Main.npc[dangerEnemy].Center) > 800f)
                {
                    glubbyActive++;
                    if (glubbyActive == 120)
                    {
                        if (!glubbyHide)
                            SoundEngine.PlaySound(RORMod.GetSound("glubby").WithVolumeScale(0.4f));
                    }
                }
            }
        }

        public override void PostUpdateRunSpeeds()
        {
            if (Player.accRunSpeed > 0f)
            {
                Player.accRunSpeed += bootSpeed;
            }
        }

        public override void PostUpdateEquips()
        {
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
            ManageLifeSupplements(barrier, lifeMax);

            if (shield > 0f && timeSinceLastHit == ShieldRegenerationTime)
            {
                if (accShieldGenerator)
                    SoundEngine.PlaySound(RORMod.GetSound("personalshield").WithVolumeScale(0.15f), Player.Center);
                Player.statLife = Math.Min(Player.statLife + (int)(Player.statLifeMax2 * shield), Player.statLifeMax2);
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                ResourceOverlaySystem.MaxShield = shield;
                ResourceOverlaySystem.MaxBarrier = barrier;
                ResourceOverlaySystem.MaxGlass = glass;
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

        public void SpawnRustedLockbox()
        {
            for (int i = 0; i < 100; i++)
            {
                var spawnLocation = Player.Center + new Vector2(Main.rand.NextFloat(1000f, 1500f) * (Main.rand.NextBool() ? -1f : 1f), Main.rand.NextFloat(-1000f, 500f));
                if (!Collision.SolidCollision(spawnLocation - new Vector2(16f, 0f), 32, 32))
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), spawnLocation, Vector2.UnitY, ModContent.ProjectileType<SmallChest>(), 0, 0f, Player.whoAmI);
                    break;
                }
            }
        }

        public void SpawnSmallChest()
        {
            for (int i = 0; i < 100; i++)
            {
                var spawnLocation = Player.Center + new Vector2(Main.rand.NextFloat(1000f, 1500f) * (Main.rand.NextBool() ? -1f : 1f), Main.rand.NextFloat(-1000f, 500f));
                if (!Collision.SolidCollision(spawnLocation - new Vector2(16f, 0f), 32, 32))
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), spawnLocation, Vector2.UnitY, ModContent.ProjectileType<SmallChest>(), 0, 0f, Player.whoAmI);
                    break;
                }
            }
        }

        public override void PostUpdate()
        {
            if (Main.myPlayer == Player.whoAmI)
            {
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<SmallChest>()] < 3 && Player.RollLuck(15000) == 0)
                {
                    SpawnSmallChest();
                }
                if (checkRustedKey && Player.ownedProjectileCounts[ModContent.ProjectileType<RustyLockbox>()] < 1 && Player.RollLuck(5000) == 0)
                {
                    SpawnRustedLockbox();
                }
                if (accBustlingFungus != null && idleTime > 60 && Player.ownedProjectileCounts[ModContent.ProjectileType<BustlingFungusProj>()] == 0)
                {
                    Projectile.NewProjectile(Player.GetSource_Accessory(accBustlingFungus), Player.Center, Vector2.Zero, ModContent.ProjectileType<BustlingFungusProj>(), 0, 0f, Player.whoAmI);
                }
                if (warbannerProgress_Enemies > 15)
                {
                    SpawnWarbanner();
                }
            }
            if (gLegSounds)
            {
                UpdateGoatFootsteps();
            }
            DangerEnemy();
            if (opalShieldTimer != -1)
            {
                opalShieldTimer++;
                if (opalShieldTimer == 300)
                {
                    opalShieldTimer = -1;
                    opalShieldActive = true;
                }
            }
        }

        public void SpawnWarbanner()
        {
            warbannerProgress_Enemies = 0;
            Projectile.NewProjectile(Player.GetSource_Accessory(accWarbanner), Player.Center - new Vector2(0f, 30f), Vector2.UnitY, ModContent.ProjectileType<WarbannerProj>(),
                0, 0f, Player.whoAmI);
        }

        public void UpdateGoatFootsteps()
        {
            int legFrame = Player.legFrame.Y / 56;
            if (legFrame == 5 || legFrame == 10 || legFrame == 17)
            {
                if (!gLegPlayedSound)
                    SoundEngine.PlaySound(RORMod.GetSounds("hoofstep_", 7, 0.33f, 0f, 0.1f));
                gLegPlayedSound = true;
                return;
            }
            gLegPlayedSound = false;
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

        public void TougherTimesDodge()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                SoundEngine.PlaySound(RORMod.GetSound("toughertimes").WithVolumeScale(0.2f), Player.Center);
                int c = CombatText.NewText(new Rectangle((int)Player.position.X + Player.width / 2 - 1, (int)Player.position.Y, 2, 2), new Color(255, 255, 255, 255), 0, false, true);
                if (c != -1 && c != Main.maxCombatText)
                {
                    Main.combatText[c].text = Language.GetTextValue("Mods.RORMod.Blocked");
                    Main.combatText[c].rotation = 0f;
                    Main.combatText[c].scale *= 0.8f;
                    Main.combatText[c].alphaDir = 0;
                    Main.combatText[c].alpha = 0.99f;
                    Main.combatText[c].position.X = Player.position.X + Player.width / 2f - FontAssets.CombatText[0].Value.MeasureString(Main.combatText[c].text).X / 2f;
                }
            }
            Player.SetImmuneTimeForAllTypes(60);
        }

        public override void UpdateBadLifeRegen()
        {
            if (flatDebuffDamageReduction > 0 && Player.lifeRegen < 0)
            {
                Player.lifeRegen = Math.Min(flatDebuffDamageReduction + Player.lifeRegen, -1);
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (Player.whoAmI == Main.myPlayer && accTougherTimes && Player.RollLuck(10) == 0)
            {
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    var p = RORMod.GetPacket(PacketType.TougherTimesDodge);
                    p.Write(Player.whoAmI);
                }
                TougherTimesDodge();
                return false;
            }
            return true;
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

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            timeSinceLastHit = 0;
            if (accMedkit)
            {
                Player.AddBuff(ModContent.BuffType<MedkitBuff>(), 120);
            }
            if (barrier > 0f)
            {
                barrier = (float)Math.Max(barrier - damage / (float)Player.statLifeMax, 0f);
                if (barrier <= 0.01f)
                {
                    barrier = 0f;
                }
            }
            else if (shield > 0f)
            {
                shield = (float)Math.Max(shield - damage / (float)Player.statLifeMax, 0f);
                if (shield <= 0.01f)
                {
                    shield = 0f;
                    if (accShieldGenerator)
                    {
                        SoundEngine.PlaySound(RORMod.GetSound("personalshieldgone"), Player.Center);
                    }
                }
            }
            if (accOpal)
            {
                if (opalShieldActive)
                    opalShieldActive = false;
                if (opalShieldTimer == -1)
                    opalShieldTimer = 0;
            }
            if (checkElixir != ItemID.None && Player.statLife * 2 < Player.statLifeMax2)
            {
                CheckElixir();
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (accDiosBestFriend > 0 && diosCooldown <= 0)
            {
                DifficultyHack = Player.difficulty;
                Player.difficulty = PlayerDifficultyID.SoftCore;
            }
            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (accDiosBestFriend > 0 && diosCooldown <= 0)
            {
                diosDead = true;
                Player.dead = false;
                Player.difficulty = DifficultyHack;
            }
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            return Player.RollLuck(100) > (int)(backupMagAmmoReduction * 100f);
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            ModifyHitEffects(target, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            ModifyHitEffects(target, ref damage, ref knockback, ref crit);
        }

        public void ModifyHitEffects(NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if ((target.boss || RORNPC.CountsAsBoss.Contains(target.type)) && bossDamageMultiplier != 1)
            {
                damage = (int)(damage * bossDamageMultiplier);
            }
            if (accCrowbar && target.life * 10 < target.lifeMax * 9)
            {
                damage = (int)(damage * 1.25);
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            OnHitEffects(target, damage, knockback, crit);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            OnHitEffects(target, damage, knockback, crit);
        }

        public void OnHitEffects(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.immortal)
            {
                if (accStunGrenade && ProcRate() && Player.RollLuck(10) == 0)
                {
                    Projectile.NewProjectile(Player.GetSource_OnHurt(target), target.Center, Vector2.Zero,
                        ModContent.ProjectileType<StunGrenadeProj>(), 0, 0f, Player.whoAmI, target.whoAmI);
                }

                if (accStickyBomb && ProcRate() && Player.RollLuck(10) == 0)
                {
                    Projectile.NewProjectile(Player.GetSource_OnHurt(target), target.Center + Main.rand.NextVector2Unit() * 100f, Vector2.Zero,
                        ModContent.ProjectileType<StickyExplosivesProj>(), (int)(damage * 1.25f), 0f, Player.whoAmI, target.whoAmI);
                }
            }
            if (accDeathMark)
            {
                int buffCount = 0;
                for (int i = 0; i < NPC.maxBuffs; i++)
                {
                    if (target.buffType[i] != 0 && Main.debuff[target.buffType[i]] && target.buffType[i] != ModContent.BuffType<DeathMarkDebuff>())
                    {
                        buffCount++;
                    }
                    if (buffCount >= 2)
                    {
                        target.AddBuff(ModContent.BuffType<DeathMarkDebuff>(), 420);
                        break;
                    }
                }
            }
            if (accShatterspleen != null && crit)
            {
                target.GetGlobalNPC<RORNPC>().bleedShatterspleen = true;
                BleedingDebuff.AddStack(target, 300, 1);
                target.netUpdate = true;
            }
            if (accTriTipDagger && ProcRate() && Player.RollLuck(10) == 0)
            {
                BleedingDebuff.AddStack(target, 180, 1);
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            UseRollOfPennies(damage);
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            UseRollOfPennies(damage);
        }

        public void UseRollOfPennies(int damage)
        {
            if (accPennies)
            {
                int[] coins = Utils.CoinsSplit(50 * (int)damage);

                if (coins[0] > 0)
                    Item.NewItem(Player.GetSource_FromThis(), Player.Center, ItemID.CopperCoin, coins[0]);

                if (coins[1] > 0)
                    Item.NewItem(Player.GetSource_FromThis(), Player.Center, ItemID.SilverCoin, coins[1]);

                if (coins[2] > 0)
                    Item.NewItem(Player.GetSource_FromThis(), Player.Center, ItemID.GoldCoin, coins[2]);

                if (coins[3] > 0)
                    Item.NewItem(Player.GetSource_FromThis(), Player.Center, ItemID.PlatinumCoin, coins[3]);
            }
        }

        public void OnKillEffect(int type, Vector2 position, int width, int height, int lifeMax, int lastHitDamage, BitsByte miscInfo)
        {
            var center = position + new Vector2(width, height) / 2f;
            if (accGasoline != null)
            {
                Projectile.NewProjectile(Player.GetSource_Accessory(accGasoline), center, new Vector2(0f, -2f), ModContent.ProjectileType<GasolineProj>(), (int)(lastHitDamage * 1f), 3f, Player.whoAmI);
            }
            if (accMonsterTooth != null)
            {
                Projectile.NewProjectile(Player.GetSource_Accessory(accMonsterTooth), center, new Vector2(0f, -2f), ModContent.ProjectileType<HealingOrb>(), 0, 0, Player.whoAmI);
            }
            if (accTopazBrooch)
            {
                barrier = Math.Min(barrier + 15f / Player.statLifeMax2, 1f);
                Player.statLife += 15;
            }
            if (accWarbanner != null)
            {
                warbannerProgress_Enemies++;
            }
            if (accFireworks != null)
            {
                Projectile.NewProjectile(Player.GetSource_Accessory(accFireworks), Player.Center, Vector2.Zero, ModContent.ProjectileType<FireworksProj>(), Math.Clamp((int)(lifeMax * 0.05), 20, 80), 0, Player.whoAmI, 40);
            }
            if (miscInfo[0])
            {
                Projectile.NewProjectile(Player.GetSource_Accessory(accShatterspleen), center, Vector2.Normalize(center - Player.Center) * 0.1f,
                    ModContent.ProjectileType<ShatterspleenExplosion>(), lifeMax / 4, 6f, Player.whoAmI);
            }
        }
    }
}