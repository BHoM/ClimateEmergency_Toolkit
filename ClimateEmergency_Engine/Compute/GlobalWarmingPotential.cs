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
using BH.oM.Structure.MaterialFragments;

namespace BH.Engine.ClimateEmergency
{
    public static partial class Compute
    {
        /***************************************************/
        /****   Public Methods                          ****/
        /***************************************************/

        [Description("Calculates the global warming potential of a BHoM Object based on explicitly defined volume and Environmental Product Declaration dataset.")]
        [Input("obj", "The BHoM Object to calculate the embodied kg CO2 - Global Warming Potential. This method requires the object's volume to be stored in CustomData under a 'Volume' key.")]
        [Input("EPDData", "Currently a custom object with a valid value for global warming potential stored in CustomData under a 'GlobalWarmingPotential' key.")]
        public static double GlobalWarmingPotential(BHoMObject obj, CustomObject EPDData)
        {
            double volume, density, globalWarmingPotential;

            if (obj.CustomData.ContainsKey("Volume"))
            {
                volume = (double)obj.CustomData["Volume"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid volume stored in CustomData under a 'Volume' key.");
                return 0;
            }

            if (EPDData.CustomData.ContainsKey("Density"))
            {
                density = (double)EPDData.CustomData["Density"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The EPDDataset must have a valid value for density under a 'Density' key.");
                return 0;
            }

            if (EPDData.CustomData.ContainsKey("GlobalWarmingPotential"))
            {
                globalWarmingPotential = (double)EPDData.CustomData["GlobalWarmingPotential"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The EPDDataset must have a valid value for global warming potential stored in CustomData under a 'GlobalWarmingPotential' key.");
                return 0;
            }

            return volume * density * globalWarmingPotential;
        }

        /***************************************************/

    }
}
