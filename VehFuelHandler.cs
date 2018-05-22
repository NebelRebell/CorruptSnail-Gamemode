﻿using CitizenFX.Core;
using CitizenFX.Core.UI;
using CorruptSnail.Util;
using System;
using System.Threading.Tasks;

namespace CorruptSnail
{
    class VehFuelHandler : BaseScript
    {
        public const string VEH_FUEL_DECOR = "_CORRUPTSNAIL_FUEL";

        public VehFuelHandler()
        {
            EntityDecoration.RegisterProperty(VEH_FUEL_DECOR, DecorationType.Float);

            Tick += OnTick;
        }

        private async Task OnTick()
        {
            Ped playerPed; Vehicle veh;
            if ((playerPed = LocalPlayer.Character) != null && (veh = playerPed.CurrentVehicle) != null)
            {
                if (!EntityDecoration.HasDecor(veh, VEH_FUEL_DECOR))
                {
                    veh.FuelLevel = new Random().Next(0, 10000);
                    EntityDecoration.Set(veh, VEH_FUEL_DECOR, veh.FuelLevel);
                }
                else
                {
                    if (veh.GetPedOnSeat(VehicleSeat.Driver) == playerPed)
                    {
                        float newFuelLevel = EntityDecoration.Get<float>(veh, VEH_FUEL_DECOR) - veh.Speed * 0.01f;
                        if (newFuelLevel < 0f)
                            newFuelLevel = 0f;
                        EntityDecoration.Set(veh, VEH_FUEL_DECOR, newFuelLevel);
                    }

                    veh.FuelLevel = EntityDecoration.Get<float>(veh, VEH_FUEL_DECOR);

                    if (veh.FuelLevel == 0f)
                        Screen.DisplayHelpTextThisFrame("No Fuel left");
                    else if (veh.FuelLevel < 100f)
                        Screen.DisplayHelpTextThisFrame("Low Fuel Level");
                }
            }

            await Task.FromResult(0);
        }
    }
}