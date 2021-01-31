﻿using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Loadouts.UI;
using Microsoft.Xna.Framework;

namespace Loadouts
{
    public class UIModSystem : ModSystem
    {
        private GameTime _lastUpdateUiGameTime;
        internal static UserInterface BaseUserInterface;
        internal static BaseUIState BaseUIState;

        public static void Load()
        {
            if (!Main.dedServ && Main.netMode != NetmodeID.Server)
            {
                BaseUIState = new BaseUIState();
                BaseUIState.Activate();
                BaseUserInterface = new UserInterface();
                BaseUserInterface.SetState(BaseUIState);
            }
        }

        public static void Unload()
        {
            UIModSystem.BaseUserInterface = null;
            BaseUIState = null;
        }
        
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (BaseUserInterface?.CurrentState != null)
                BaseUserInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
            int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (interfaceLayer != -1)
            {
                layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer(
                    "Loadouts: Base UI",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && BaseUserInterface?.CurrentState != null)
                            BaseUserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);

                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }
    }
}