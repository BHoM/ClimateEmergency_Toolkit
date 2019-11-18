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

        [Description("Calculates the depletion of abiotic resources fossil fuels of a BHoM Object based on explicitly defined volume and Environmental Product Declaration dataset.")]
        [Input("volume", "Provide material volume in m^3. ")]
        [Input("density", "Provide material density in kg/m^3. This value may be available within an EPD Dataset.")]
        [Input("EPDData", "Currently a custom object with a valid value for depletion of abiotic resources of fossil fuels stored in CustomData under a 'DepletionOfAbioticResourcesFossilFuels' key.")]
        public static double DepletionofAbioticResourcesFossilFuels(double volume, double density, CustomObject EPDData)
        {
            double depletionOfAbioticResourcesFossilFuels;

            if (EPDData.CustomData.ContainsKey("DepletionOfAbioticResourcesFossilFuels"))
            {
                depletionOfAbioticResourcesFossilFuels = (double)EPDData.CustomData["DepletionOfAbioticResourcesFossilFuels"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The EPDDataset must have a valid value for acidification potential stored in CustomData under a 'DepletionOfAbioticResourcesFossilFuels' key.");
                return 0;
            }

            return volume * density * depletionOfAbioticResourcesFossilFuels;
        }

        /***************************************************/

    }
}