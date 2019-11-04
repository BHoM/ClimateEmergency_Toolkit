/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
        [Input("BuildingPlumbingFixtureSelection", "The BHoM Object which contains the selected building plumbing fixtures. This method requires the object's flow per fixture to be set.")]
        [Input("BuildingOccupancybyGender", "The BHoM Object which contains the building's occupancy by gender. This method requires the object's occupancy percentages to be set.")]
        [Input("FixtureUsageDataSet", "The dataset or custom object which contains the residential or commercial building based daily usage of plumbing fixtures.")]
        public static double PlumbingFixtureWaterDemandPerDay(BuildingOccupancyByGender BuildingOccupancybyGender, BuildingPlumbingFixtureSelection BuildingPlumbingFixtureSelection, CustomObject FixtureUsageDataSet)
        {

            double numberfemales = (double)BuildingOccupancybyGender.BuildingOccupancy * BuildingOccupancybyGender.FemalePercentage;
            double numbermales = (double)BuildingOccupancybyGender.BuildingOccupancy * BuildingOccupancybyGender.MalePercentage;
            double numbergenderneutral = (double)BuildingOccupancybyGender.BuildingOccupancy * BuildingOccupancybyGender.GenderNeutralPercentage;

            double toiletflow = BuildingPlumbingFixtureSelection.ToiletFlow;
            double lavatoryflow = BuildingPlumbingFixtureSelection.LavatoryFlow;
            double urinalflow = BuildingPlumbingFixtureSelection.UrinalFlow;
            double showerflow = BuildingPlumbingFixtureSelection.ShowerFlow;
            double kitchenfaucetflow = BuildingPlumbingFixtureSelection.KitchenFaucetFlow;

            double toiletnumberofusesmale = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("ToiletNumberOfUsesMale"))
            {
                toiletnumberofusesmale = (double)FixtureUsageDataSet.CustomData["ToiletNumberOfUsesMale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ToiletNumberOfUsesMale stored in CustomData under a 'ToiletNumberOfUsesMale' key.");
                return 0;
            }

            double toiletnumberofusesfemale = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("ToiletNumberOfUsesFemale"))
            {
                toiletnumberofusesfemale = (double)FixtureUsageDataSet.CustomData["ToiletNumberOfUsesFemale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ToiletNumberOfUsesFemale stored in CustomData under a 'ToiletNumberOfUsesMale' key.");
                return 0;
            }

            double toiletnumberofusesgenderneutral = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("ToiletNumberOfUsesGenderNeutral"))
            {
                toiletnumberofusesgenderneutral = (double)FixtureUsageDataSet.CustomData["ToiletNumberOfUsesGenderNeutral"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ToiletNumberOfUsesGenderNeutral stored in CustomData under a 'ToiletNumberOfUsesMale' key.");
                return 0;
            }

            double showernumberofusesmale = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("ShowerNumberOfUsesMale"))
            {
                showernumberofusesmale = (double)FixtureUsageDataSet.CustomData["ShowerNumberOfUsesMale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ShowerNumberOfUsesMale stored in CustomData under a 'showerNumberOfUsesMale' key.");
                return 0;
            }

            double showernumberofusesfemale = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("ShowerNumberOfUsesFemale"))
            {
                showernumberofusesfemale = (double)FixtureUsageDataSet.CustomData["ShowerNumberOfUsesFemale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ShowerNumberOfUsesFemale stored in CustomData under a 'showerNumberOfUsesMale' key.");
                return 0;
            }

            double showernumberofusesgenderneutral = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("ShowerNumberOfUsesGenderNeutral"))
            {
                showernumberofusesgenderneutral = (double)FixtureUsageDataSet.CustomData["ShowerNumberOfUsesGenderNeutral"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid ShowerNumberOfUsesGenderNeutral stored in CustomData under a 'showerNumberOfUsesMale' key.");
                return 0;
            }

            double kitchenfaucetnumberofusesmale = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("KitchenFaucetNumberOfUsesMale"))
            {
                kitchenfaucetnumberofusesmale = (double)FixtureUsageDataSet.CustomData["KitchenFaucetNumberOfUsesMale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid KitchenFaucetNumberOfUsesMale stored in CustomData under a 'kitchenfaucetNumberOfUsesMale' key.");
                return 0;
            }

            double kitchenfaucetnumberofusesfemale = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("KitchenFaucetNumberOfUsesFemale"))
            {
                kitchenfaucetnumberofusesfemale = (double)FixtureUsageDataSet.CustomData["KitchenFaucetNumberOfUsesFemale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid KitchenFaucetNumberOfUsesFemale stored in CustomData under a 'kitchenfaucetNumberOfUsesMale' key.");
                return 0;
            }

            double kitchenfaucetnumberofusesgenderneutral = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("KitchenFaucetNumberOfUsesGenderNeutral"))
            {
                kitchenfaucetnumberofusesgenderneutral = (double)FixtureUsageDataSet.CustomData["KitchenFaucetNumberOfUsesGenderNeutral"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid KitchenFaucetNumberOfUsesGenderNeutral stored in CustomData under a 'kitchenfaucetNumberOfUsesMale' key.");
                return 0;
            }

            double lavatorynumberofusesmale = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("LavatoryNumberOfUsesMale"))
            {
                lavatorynumberofusesmale = (double)FixtureUsageDataSet.CustomData["LavatoryNumberOfUsesMale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid LavatoryNumberOfUsesMale stored in CustomData under a 'lavatoryNumberOfUsesMale' key.");
                return 0;
            }

            double lavatorynumberofusesfemale = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("LavatoryNumberOfUsesFemale"))
            {
                lavatorynumberofusesfemale = (double)FixtureUsageDataSet.CustomData["LavatoryNumberOfUsesFemale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid LavatoryNumberOfUsesFemale stored in CustomData under a 'lavatoryNumberOfUsesMale' key.");
                return 0;
            }

            double lavatorynumberofusesgenderneutral = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("LavatoryNumberOfUsesGenderNeutral"))
            {
                lavatorynumberofusesgenderneutral = (double)FixtureUsageDataSet.CustomData["LavatoryNumberOfUsesGenderNeutral"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid LavatoryNumberOfUsesGenderNeutral stored in CustomData under a 'lavatoryNumberOfUsesMale' key.");
                return 0;
            }

            double urinalnumberofusesmale = 0.0;

            if (FixtureUsageDataSet.CustomData.ContainsKey("UrinalNumberOfUsesMale"))
            {
                urinalnumberofusesmale = (double)FixtureUsageDataSet.CustomData["UrinalNumberOfUsesMale"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid UrinalNumberOfUsesMale stored in CustomData under a 'urinalNumberOfUsesMale' key.");
                return 0;
            }

            double totaldailytoiletflow = (numbermales * toiletnumberofusesmale * toiletflow) + (numberfemales * toiletnumberofusesfemale * toiletflow) + (numbergenderneutral * toiletnumberofusesgenderneutral * toiletflow);

            double totaldailylavatoryflow = (numbermales * lavatorynumberofusesmale * lavatoryflow) + (numberfemales * lavatorynumberofusesfemale * lavatoryflow) + (numbergenderneutral * lavatorynumberofusesgenderneutral * lavatoryflow);

            double totaldailykitchenfaucetflow = (numbermales * kitchenfaucetnumberofusesmale * kitchenfaucetflow) + (numberfemales * kitchenfaucetnumberofusesfemale * kitchenfaucetflow) + (numbergenderneutral * kitchenfaucetnumberofusesgenderneutral * kitchenfaucetflow);

            double totaldailyshowerflow = (numbermales * showernumberofusesmale * showerflow) + (numberfemales * showernumberofusesfemale * showerflow) + (numbergenderneutral * showernumberofusesgenderneutral * showerflow);

            double totaldailyurinalflow = (numbermales * urinalnumberofusesmale * urinalflow);

            double totaldailyplumbingfixturewaterdemand = totaldailytoiletflow + totaldailykitchenfaucetflow + totaldailylavatoryflow + totaldailyshowerflow + totaldailyurinalflow;

            return totaldailyplumbingfixturewaterdemand;
        }

        /***************************************************/

    }
}