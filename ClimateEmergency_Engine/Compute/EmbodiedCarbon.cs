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

        [Description("Calculates the embodied carbon of a BHoM Object based on explicitly defined volume, material and embodied carbon dataset.")]
        [Input("obj", "The BHoM Object to calculate the embodied energy of. This method requires the object's volume to be stored in CustomData under a 'Volume' key.")]
        [Input("material", "This is currently hardcoded as a BH.oM.Structure.MaterialFragments type - compatible with the current BHoM Materials Dataset.")]
        [Input("embodiedCarbonData", "Currently a custom object with a valid value for embodied carbon stored in CustomData under an 'EmbodiedCarbon' key.")]
        public static double EmbodiedCarbon(BHoMObject obj, IMaterialFragment material, CustomObject embodiedCarbonData)
        {
            double volume, density, embodiedCarbon;

            if (obj.CustomData.ContainsKey("Volume"))
            {
                volume = (double)obj.CustomData["Volume"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The BHoMObject must have a valid volume stored in CustomData under a 'Volume' key.");
                return 0;
            }


            density = material.Density;


            if (embodiedCarbonData.CustomData.ContainsKey("EmbodiedCarbon"))
            {
                embodiedCarbon = (double)embodiedCarbonData.CustomData["EmbodiedCarbon"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The embodiedCarbonDataset must have a valid value for embodied carbon stored in CustomData under an 'EmbodiedCarbon' key.");
                return 0;
            }

            return volume * density * embodiedCarbon;
        }

        /***************************************************/

    }
}
