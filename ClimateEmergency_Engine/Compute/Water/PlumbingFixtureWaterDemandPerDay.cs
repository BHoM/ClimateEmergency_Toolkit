/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using BH.oM.Base;
using BH.oM.Reflection.Attributes;
using BH.oM.ClimateEmergency.Water;

namespace BH.Engine.ClimateEmergency
{
    public static partial class Compute
    {
        /***************************************************/
        /****   Public Methods                          ****/
        /***************************************************/

        [Description("Calculates the plumbing fixture water demand per day using the plumbing fixture flows and the building occupancy.")]
        [Input("buildingPlumbingFixtureSelection", "The BHoM Object which contains the selected building plumbing fixtures. This method requires the object's flow per fixture to be set.")]
        [Input("buildingOccupancybyGender", "The BHoM Object which contains the building's occupancy by gender. This method requires the object's occupancy percentages to be set.")]
        [Input("fixtureUsageDataSet", "The dataset or custom object which contains the residential or commercial building based daily usage of plumbing fixtures.")]
        [Output("WaterPerDay", "The amount of water in m^3 used on a daily basis by plumbing fixtures.")]
        public static double PlumbingFixtureWaterDemandPerDay(BuildingOccupancyByGender buildingOccupancybyGender, BuildingPlumbingFixtureSelection buildingPlumbingFixtureSelection, CustomObject fixtureUsageDataSet)
        {
            double numberFemales = (double)buildingOccupancybyGender.BuildingOccupancy * buildingOccupancybyGender.FemalePercentage;
            double numberMales = (double)buildingOccupancybyGender.BuildingOccupancy * buildingOccupancybyGender.MalePercentage;
            double numberGenderNeutral = (double)buildingOccupancybyGender.BuildingOccupancy * buildingOccupancybyGender.GenderNeutralPercentage;

            double toiletFlow = buildingPlumbingFixtureSelection.ToiletVolumePerUse;
            double lavatoryFlow = buildingPlumbingFixtureSelection.UrinalVolumePerUse;
            double urinalFlow = buildingPlumbingFixtureSelection.LavatoryVolumePerUse;
            double showerFlow = buildingPlumbingFixtureSelection.ShowerVolumePerUse;
            double kitchenFaucetFlow = buildingPlumbingFixtureSelection.KitchenFaucetPerUse;

            double toiletNumberOfUsesMale = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("ToiletNumberOfUsesMale"))
            {
                toiletNumberOfUsesMale = (double)fixtureUsageDataSet.CustomData["ToiletNumberOfUsesMale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ToiletNumberOfUsesMale stored in CustomData under a 'ToiletNumberOfUsesMale' key.");
                return 0;
            }

            double toiletNumberOfUsesFemale = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("ToiletNumberOfUsesFemale"))
            {
                toiletNumberOfUsesFemale = (double)fixtureUsageDataSet.CustomData["ToiletNumberOfUsesFemale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ToiletNumberOfUsesFemale stored in CustomData under a 'ToiletNumberOfUsesMale' key.");
                return 0;
            }

            double toiletNumberOfUsesGenderNeutral = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("ToiletNumberOfUsesGenderNeutral"))
            {
                toiletNumberOfUsesGenderNeutral = (double)fixtureUsageDataSet.CustomData["ToiletNumberOfUsesGenderNeutral"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ToiletNumberOfUsesGenderNeutral stored in CustomData under a 'ToiletNumberOfUsesMale' key.");
                return 0;
            }

            double showerNumberOfUsesMale = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("ShowerNumberOfUsesMale"))
            {
                showerNumberOfUsesMale = (double)fixtureUsageDataSet.CustomData["ShowerNumberOfUsesMale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ShowerNumberOfUsesMale stored in CustomData under a 'showerNumberOfUsesMale' key.");
                return 0;
            }

            double showerNumberOfUsesFemale = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("ShowerNumberOfUsesFemale"))
            {
                showerNumberOfUsesFemale = (double)fixtureUsageDataSet.CustomData["ShowerNumberOfUsesFemale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ShowerNumberOfUsesFemale stored in CustomData under a 'showerNumberOfUsesMale' key.");
                return 0;
            }

            double showerNumberOfUsesGenderNeutral = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("ShowerNumberOfUsesGenderNeutral"))
            {
                showerNumberOfUsesGenderNeutral = (double)fixtureUsageDataSet.CustomData["ShowerNumberOfUsesGenderNeutral"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ShowerNumberOfUsesGenderNeutral stored in CustomData under a 'showerNumberOfUsesMale' key.");
                return 0;
            }

            double kitchenFaucetNumberOfUsesMale = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("KitchenFaucetNumberOfUsesMale"))
            {
                kitchenFaucetNumberOfUsesMale = (double)fixtureUsageDataSet.CustomData["KitchenFaucetNumberOfUsesMale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid KitchenFaucetNumberOfUsesMale stored in CustomData under a 'kitchenfaucetNumberOfUsesMale' key.");
                return 0;
            }

            double kitchenFaucetNumberOfUsesFemale = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("KitchenFaucetNumberOfUsesFemale"))
            {
                kitchenFaucetNumberOfUsesFemale = (double)fixtureUsageDataSet.CustomData["KitchenFaucetNumberOfUsesFemale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid KitchenFaucetNumberOfUsesFemale stored in CustomData under a 'kitchenfaucetNumberOfUsesMale' key.");
                return 0;
            }

            double kitchenFaucetNumberOfUsesGenderNeutral = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("KitchenFaucetNumberOfUsesGenderNeutral"))
            {
                kitchenFaucetNumberOfUsesGenderNeutral = (double)fixtureUsageDataSet.CustomData["KitchenFaucetNumberOfUsesGenderNeutral"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid KitchenFaucetNumberOfUsesGenderNeutral stored in CustomData under a 'kitchenfaucetNumberOfUsesMale' key.");
                return 0;
            }

            double lavatoryNumberOfUsesMale = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("LavatoryNumberOfUsesMale"))
            {
                lavatoryNumberOfUsesMale = (double)fixtureUsageDataSet.CustomData["LavatoryNumberOfUsesMale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid LavatoryNumberOfUsesMale stored in CustomData under a 'lavatoryNumberOfUsesMale' key.");
                return 0;
            }

            double lavatoryNumberOfUsesFemale = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("LavatoryNumberOfUsesFemale"))
            {
                lavatoryNumberOfUsesFemale = (double)fixtureUsageDataSet.CustomData["LavatoryNumberOfUsesFemale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid LavatoryNumberOfUsesFemale stored in CustomData under a 'lavatoryNumberOfUsesMale' key.");
                return 0;
            }

            double lavatoryNumberOfUsesGenderNeutral = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("LavatoryNumberOfUsesGenderNeutral"))
            {
                lavatoryNumberOfUsesGenderNeutral = (double)fixtureUsageDataSet.CustomData["LavatoryNumberOfUsesGenderNeutral"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid LavatoryNumberOfUsesGenderNeutral stored in CustomData under a 'lavatoryNumberOfUsesMale' key.");
                return 0;
            }

            double urinalNumberOfUsesMale = 0.0;

            if (fixtureUsageDataSet.CustomData.ContainsKey("UrinalNumberOfUsesMale"))
            {
                urinalNumberOfUsesMale = (double)fixtureUsageDataSet.CustomData["UrinalNumberOfUsesMale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid UrinalNumberOfUsesMale stored in CustomData under a 'urinalNumberOfUsesMale' key.");
                return 0;
            }

            double totaldailytoiletflow = (numberMales * toiletNumberOfUsesMale * toiletFlow) + (numberFemales * toiletNumberOfUsesFemale * toiletFlow) + (numberGenderNeutral * toiletNumberOfUsesGenderNeutral * toiletFlow);

            double totaldailylavatoryflow = (numberMales * lavatoryNumberOfUsesMale * lavatoryFlow) + (numberFemales * lavatoryNumberOfUsesFemale * lavatoryFlow) + (numberGenderNeutral * lavatoryNumberOfUsesGenderNeutral * lavatoryFlow);

            double totaldailykitchenfaucetflow = (numberMales * kitchenFaucetNumberOfUsesMale * kitchenFaucetFlow) + (numberFemales * kitchenFaucetNumberOfUsesFemale * kitchenFaucetFlow) + (numberGenderNeutral * kitchenFaucetNumberOfUsesGenderNeutral * kitchenFaucetFlow);

            double totaldailyshowerflow = (numberMales * showerNumberOfUsesMale * showerFlow) + (numberFemales * showerNumberOfUsesFemale * showerFlow) + (numberGenderNeutral * showerNumberOfUsesGenderNeutral * showerFlow);

            double totaldailyurinalflow = (numberMales * urinalNumberOfUsesMale * urinalFlow);

            double totaldailyplumbingfixturewaterdemand = totaldailytoiletflow + totaldailykitchenfaucetflow + totaldailylavatoryflow + totaldailyshowerflow + totaldailyurinalflow;

            return totaldailyplumbingfixturewaterdemand;
        }

        /***************************************************/

    }
}