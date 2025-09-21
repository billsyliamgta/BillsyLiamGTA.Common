/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using System;
using System.Linq;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Ped
{
    public class EnemyHandler : Script
    {
        #region Properties

        public static List<Enemy> enemies = new List<Enemy> { };

        public static bool IsAlerted => enemies.Any(p => p.IsAlerted);

        #endregion

        #region Constructor

        public EnemyHandler()
        {
            Tick += OnTick;
        }

        #endregion

        #region Functions

        public static Enemy CreateEnemyPed(Model model, Vector3 position, float heading = 0f, bool provokedMode = false)
        {
            model.Request(5000);
            GTA.Ped ped = World.CreatePed(model, position, heading);
            model.MarkAsNoLongerNeeded();
            ped.RelationshipGroup = "GOON";
            ped.RelationshipGroup.SetRelationshipBetweenGroups(Game.Player.Character.RelationshipGroup, Relationship.Dislike);
            Game.Player.Character.RelationshipGroup.SetRelationshipBetweenGroups(ped.RelationshipGroup, Relationship.Dislike);
            ped.BlockPermanentEvents = true;
            ped.SeeingRange = 7f;
            ped.HearingRange = 7f;
            ped.VisualFieldPeripheralRange = 7f;
            ped.VisualFieldCenterAngle = 50f;
            ped.VisualFieldMinAngle = -90f;
            ped.VisualFieldMaxAngle = 90f;
            Blip blip = ped.AddBlip();
            blip.Sprite = BlipSprite.Enemy;
            blip.Scale = 0.7f;
            blip.Color = provokedMode ? BlipColor.White : BlipColor.Red;
            blip.IsShortRange = true;
            Function.Call(Hash.SET_BLIP_SHOW_CONE, blip, true, 11);
            Enemy returnValue = new Enemy(ped);
            returnValue.ProvokedMode = provokedMode;
            return returnValue;
        }

        public static bool Contains(Enemy enemy)
        {
            if (enemies != null)
            {
                if (enemies.Contains(enemy))
                {
                    return true;
                }
            }

            return false;
        }

        public static void Add(Enemy enemy)
        {
            if (!Contains(enemy)) enemies.Add(enemy);
        }

        public void Remove(Enemy enemy)
        {
            if (Contains(enemy)) enemies.Remove(enemy);
        }

        public static void Dispose()
        {
            if (enemies?.Count > 0)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy?.Dispose();
                }
                enemies.Clear();
            }
        }

        public static void SetAlertedStat(bool toggle)
        {
            if (enemies?.Count > 0)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.IsAlerted = toggle;
                }
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (enemies != null)
            {
                if (enemies.Count > 0)
                {
                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Update();
                    }
                }
            }
        }

        #endregion
    }
}