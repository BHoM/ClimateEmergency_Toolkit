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
using BH.oM.Structure.Elements;
using BH.oM.Physical.Elements;
using BH.Engine.Structure;
using BH.Engine.Physical;
using BH.oM.Geometry;

namespace BH.Engine.ClimateEmergency
{
    public static partial class Compute
    {
        /***************************************************/
        /****   Public Methods                          ****/
        /***************************************************/

        [Description("Returns mass of an IBHoMObject")]
        [Input("obj", "Any IBHoMObject for which to calculate the mass. Must contain location data sufficient to define volume and a material density.")]
        [Output("mass", "Volume of the object in m^3.")]
        public static double IMass(IBHoMObject obj)
        {
            return Mass(obj as dynamic);
        }

        /***************************************************/

        private static double Mass(object obj)
        {
            BH.Engine.Reflection.Compute.RecordWarning("Could not compute mass for the object.");
            return 0;
        }

        /***************************************************/

        private static double Mass(Bar obj)
        {
            if (obj.SectionProperty != null)
                return Engine.Structure.Query.Mass(obj);
            else
                BH.Engine.Reflection.Compute.RecordWarning("Bar has no SectionProperty. Could not compute mass.");
                return 0;
        }

        /***************************************************/

        private static double Mass(Panel obj)
        {
            if (obj.Property != null)
                return Engine.Structure.Query.Mass(obj);
            else
                BH.Engine.Reflection.Compute.RecordWarning("Panel has no SurfaceProperty. Could not compute mass.");
            return 0;

        }

        /***************************************************/

        private static double Mass(FramingElement obj)
        {
            return obj.AnalyticalBars().Sum(x => Mass(x as Bar));
        }

        /***************************************************/

    }
}