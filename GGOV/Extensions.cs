﻿using GTA;
using GTA.Native;
using System;
using System.Drawing;
using System.Linq;

namespace GGO
{
    /// <summary>
    /// The style of the weapon that the player currently has.
    /// </summary>
    public enum WeaponStyle
    {
        Banned = -1,
        Main = 0,
        Sidearm = 1,
        Melee = 2,
        Double = 3
    }

    /// <summary>
    /// Extensions used to get more features from existing classes.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Types of weapons that are not going to be shown on the HUD.
        /// In order: Gas Can (Type), Throwables, Fists (Type), None/Phone (Type).
        /// </summary>
        public static uint[] BannedWeapons = new uint[] { 1595662460u, 1548507267u, 2685387236u, 0 };
        /// <summary>
        /// Types of weapons that can be considered sidearm either by the size or firing mechanism.
        /// In order: Pistol (Type), SMG.
        /// </summary>
        public static uint[] SecondaryWeapons = new uint[] { 416676503u, 3337201093u };
        /// <summary>
        /// Types of weapons that can be considered melee.
        /// In order: Melee (Type).
        /// </summary>
        public static uint[] MeleeWeapons = new uint[] { 3566412244u };

        /// <summary>
        /// Checks if the position is being clicked by the user.
        /// </summary>
        /// <param name="Position">The starting position.</param>
        /// <param name="Area">The size of the area being clicked.</param>
        /// <returns>True if the area is being clicked, false otherwise.</returns>
        public static bool IsClicked(this Point Position, Size Area)
        {
            int MouseX = (int)Math.Round(Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorX) * UI.WIDTH);
            int MouseY = (int)Math.Round(Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorY) * UI.HEIGHT);

            return (MouseX >= Position.X && MouseX <= Position.X + Area.Width) &&
                      (MouseY > Position.Y && MouseY < Position.Y + Area.Height);
        }

        /// <summary>
        /// If the entity is part of the mission.
        /// </summary>
        /// <returns>True if the entity is part of the mission, False otherwise.</returns>
        public static bool IsMissionEntity(this Entity GameEntity)
        {
            return Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, GameEntity);
        }

        /// <summary>
        /// Checks if the specified relationship ID is for a friendly ped.
        /// </summary>
        /// <param name="Relationship">The relationship ID.</param>
        /// <returns>True if the ped is friendly, False otherwise.</returns>
        public static bool IsFriendly(this Ped GamePed)
        {
            return (int)Game.Player.Character.GetRelationshipWithPed(GamePed) <= 2 && (int)GamePed.GetRelationshipWithPed(Game.Player.Character) <= 2;
        }

        /// <summary>
        /// Checks for the weapon type.
        /// </summary>
        /// <param name="ID">The Weapon Type ID.</param>
        /// <returns>The style of weapon.</returns>
        public static WeaponStyle GetStyle(this WeaponCollection Collection)
        {
            // Return the first match, in order
            // From dangerous to normal
            if (BannedWeapons.Contains((uint)Collection.Current.Group))
                return WeaponStyle.Banned;
            else if (MeleeWeapons.Contains((uint)Collection.Current.Group))
                return WeaponStyle.Melee;
            else if (SecondaryWeapons.Contains((uint)Collection.Current.Group))
                return WeaponStyle.Sidearm;
            else
                return WeaponStyle.Main;
        }

        /// <summary>
        /// Gets the image for the weapon caliber and cartridge.
        /// </summary>
        public static string GetAmmoImage(this Weapon PlayerWeapon)
        {
            switch (PlayerWeapon.Hash)
            {
                case WeaponHash.MarksmanPistol:
                    return "Ammo22Long";
                case WeaponHash.APPistol:
                    return "Ammo22Scamp";
                case WeaponHash.VintagePistol:
                    return "Ammo32ACP";
                case WeaponHash.DoubleActionRevolver:
                    return "Ammo38LongColt";
                case WeaponHash.Revolver:
                    return "Ammo44Magnum";
                case WeaponHash.HeavyPistol:
                case WeaponHash.SNSPistol:
                case WeaponHash.MicroSMG: // Model and Texture says ".45 ACB"
                case WeaponHash.Gusenberg:
                    return "Ammo45ACP";
                case WeaponHash.Pistol50:
                    return "Ammo50AE";
                case WeaponHash.HeavySniper:
                case WeaponHash.HeavySniperMk2:
                    return "Ammo50BMG";
                case WeaponHash.CarbineRifle:
                case WeaponHash.CarbineRifleMk2:
                case WeaponHash.SpecialCarbine:
                case WeaponHash.SpecialCarbineMk2:
                case WeaponHash.AdvancedRifle:
                    return "Ammo556NATO";
                case WeaponHash.SniperRifle:
                case WeaponHash.Minigun:
                case WeaponHash.CombatMG:
                case WeaponHash.CombatMGMk2:
                    return "Ammo762NATO";
                case WeaponHash.BullpupRifle:
                case WeaponHash.BullpupRifleMk2:
                case WeaponHash.CompactRifle:
                case WeaponHash.AssaultRifle:
                case WeaponHash.AssaultrifleMk2:
                case WeaponHash.MarksmanRifle:
                case WeaponHash.MarksmanRifleMk2:
                case WeaponHash.MG: // PKM is actually 7.62x54mmR instead of 7.62x39mm
                    return "Ammo762Soviet";
                case WeaponHash.CombatPistol:
                case WeaponHash.Pistol: // "9mm semiautomatic" in Complications
                case WeaponHash.PistolMk2:
                case WeaponHash.SMG: // MP5N
                case WeaponHash.SMGMk2: // MP5K
                case WeaponHash.MachinePistol:
                case WeaponHash.MiniSMG:
                case WeaponHash.CombatPDW:
                    return "Ammo9mm";
                case WeaponHash.SweeperShotgun:
                case WeaponHash.AssaultShotgun:
                case WeaponHash.BullpupShotgun:
                case WeaponHash.SawnOffShotgun:
                case WeaponHash.PumpShotgun:
                case WeaponHash.PumpShotgunMk2:
                case WeaponHash.HeavyShotgun:
                case WeaponHash.DoubleBarrelShotgun:
                    return "Ammo12Gauge";
                case WeaponHash.CompactGrenadeLauncher:
                case WeaponHash.GrenadeLauncher:
                    return "Ammo40mm";
                case WeaponHash.AssaultSMG:
                    return "AmmoFN57";
                case WeaponHash.RPG:
                    return "AmmoPG7VM";
                default:
                    return "AmmoUnknown";
            }
        }
    }
}
