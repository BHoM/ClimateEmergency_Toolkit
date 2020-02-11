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
using BH.oM.Structure.MaterialFragments;

namespace BH.Engine.ClimateEmergency
{
    public static partial class Compute
    {
        /***************************************************/
        /****   Public Methods                          ****/
        /***************************************************/

        [Description("Calculates the acidification potential of a BHoM Object based on explicitly defined volume and Environmental Product Declaration dataset.")]
        [Input("obj", "The BHoM Object to evaluate. The object must have a material density and sufficient location data to define a volume, or a custom data field `Volume`.")]
        [Input("epdData", "BHoM Data object containing an EPD")]
        [Input("epdField", "Text string corresponding to one of the available fields in the epdData set.")]
        [Output("quantity", "The effect of the EPD field specified for this object")]
        public static double EvaluateEPD(BHoMObject obj, CustomObject epdData, string epdField)
        {
            double mass = IMass(obj);
            double epdValue;

            if (epdData.CustomData.ContainsKey(epdField))
            {
                epdValue = System.Convert.ToDouble(epdData.CustomData[epdField]);
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError($"The EPDDataset must have a value for {epdField}");
                return 0;
            }

            return mass * epdValue;
        }

        /***************************************************/

    }
}
