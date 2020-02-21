﻿/*
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
using BH.oM.ClimateEmergency;

namespace BH.Engine.ClimateEmergency
{
    public static partial class Compute
    {
        /***************************************************/
        /****   Public Methods                          ****/
        /***************************************************/

        [Description("Calculates select metrics from Environmental Product Declarations of a BHoM Object based on defined volume and Environmental Product Declaration dataset information.")]
        [Input("obj", "The BHoM Object to evaluate. The object must have a material density and sufficient location data to define a volume, or a custom data field `Volume`.")]
        [Input("epdData", "BHoM Data object containing Environmental Product Declaration metrics.")]
        [Input("epdField", "Text string corresponding to one of the available fields in the epdData set.")]
        [Output("quantity", "The effect of the EPD field specified for this object")]
        public static double EvaluateEPD(BHoMObject obj = null, CustomObject epdData = null, EPDField epdField = EPDField.GlobalWarmingPotential)
        {
            return IMass(obj) * CompileEPD(epdData, epdField);
        }

        public static double CompileEPD(CustomObject epdData, EPDField epdField)
        {
            double epdValue = 0;

            if (epdData.CustomData.ContainsKey(System.Convert.ToString(epdField)))
            {
                try
                {
                    epdValue = System.Convert.ToDouble(epdData.CustomData[System.Convert.ToString(epdField)]);
                }
                catch
                {
                    BH.Engine.Reflection.Compute.RecordError("The value for {epdField} cannot be measured. Please check your data source to ensure the metric is being evaluated before computing.");
                    return 0;
                }
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError($"The EPDDataset must have a value for {epdField}");
                return 0;
            }
            return epdValue;
        }

        /***************************************************/

        [Description("Calculates the acidification potential of a BHoM Object based on explicitly defined volume and Environmental Product Declaration dataset.")]
        [Input("volume", "Volume as a double. This method does not extract the Volume of an object and should be provided manually. The property may be extracted from a BHoM EPD Dataset.")]
        [Input("density", "Density as a double. This method does not extract the Density of an object and should be provided manually. The property may be extracted from a BHoM EPD Dataset.")]
        [Input("epdData", "BHoM Data object containing values for typical metrics within Environmental Product Declarations.")]
        [Input("epdField", "BHoM EPDField Enum to select Environmental Product Declaration metric for evaluation.")]
        [Output("quantity", "The effect of the EPD field specified for this object")]
        public static double EvaluateEPD(double volume = 0, double density = 0, CustomObject epdData = null, EPDField epdField = EPDField.GlobalWarmingPotential)
        {
            double calcDensity = double.NaN;

            if (density != 0)
            {
                calcDensity = density;
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordNote("Density value is either zero or is being derived from the epdData. Please verify the correct value within the dataset before continuing by using the explode component.");

                if (epdData.CustomData.ContainsKey("Density"))
                {
                    try
                    {
                        calcDensity = System.Convert.ToDouble(epdData.CustomData["Density"]);
                    }
                    catch (Exception e)
                    {
                        BH.Engine.Reflection.Compute.RecordError("An error occurred in converting the custom data key Density to a valid number (double) - error was: " + e.ToString());
                    }
                }
                if ((calcDensity == 0 || double.IsNaN(calcDensity)) && (density == 0 || double.IsNaN(density)))
                {
                    BH.Engine.Reflection.Compute.RecordError($"Results cannot be calculated. Please check your input values for Density, Volume, and {epdField}");
                    return double.NaN;
                }
            }
            return (volume * calcDensity) * CompileEPD(epdData, epdField);
        }
    }
}
