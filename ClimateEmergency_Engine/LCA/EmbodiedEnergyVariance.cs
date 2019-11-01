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

namespace BH.Engine.ClimateEmergency
{
    public static partial class Compute
    {
        /***************************************************/
        /****   Public Methods                          ****/
        /***************************************************/

        [Description("Calculates the percentage change of kgCO2 between a proposed, user-created building and a baseline building based on a defined building type.")]
        [Input("Project kgCO2", "Combined kgCO2 per building.")]
        [Input("Total Project Area", "Total area of the building, including all floor surface area in m2.")]
        [Input("Program Benchmark kgCO2/m2", "Benchmark kgCO2/m2 per building type based on program benchmark dataset")]
        [Input("Structural Benchmark kgCO2/m2", "Benchmark kgCO2/m2 per building structure based on structural benchmark dataset")]
        public static double EmbodiedEnergyVariance(double projectEmbodiedCarbon, double projectArea, CustomObject embodiedCarbonBenchmarkTypeDataset, CustomObject embodiedCarbonBenchmarkStructureDataset, double typeWeighting, double structureWeighting)
        {
            double typeBenchmark, structureBenchmark, weightedAverage;

            if (embodiedCarbonBenchmarkTypeDataset.CustomData.ContainsKey("Average"))
            {
                typeBenchmark = (double)embodiedCarbonBenchmarkTypeDataset.CustomData["Average"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The Benchmark Dataset must have a valid value for Average kg CO2 per building type stored in CustomData under an 'Average' key.");
                return 0;
            }

            if (embodiedCarbonBenchmarkStructureDataset.CustomData.ContainsKey("Average"))
            {
                structureBenchmark = (double)embodiedCarbonBenchmarkStructureDataset.CustomData["Average"];
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The Benchmark Dataset must have a valid value for Average kg CO2 per building structure type stored in CustomData under an 'Average' key.");
                return 0;
            }

            weightedAverage = (((typeBenchmark * typeWeighting) + (structureBenchmark * structureWeighting)) / (typeWeighting + structureWeighting));

            return (1 - ((projectEmbodiedCarbon / projectArea) / weightedAverage)) * 100;
        }

        /***************************************************/

    }
}
