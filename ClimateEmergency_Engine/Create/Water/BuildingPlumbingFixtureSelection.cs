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
using System.Text;
using System.Threading.Tasks;
using BH.oM.ClimateEmergency.Water;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.ClimateEmergency
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Returns the building plumbing fixture selection")]
        [Input("toiletFlow", "Default 0.0")]
        [Input("urinalFlow", "Default 0.0")]
        [Input("lavatoryFlow", "Default 0.0")]
        [Input("showerFlow", "Default 0.0")]
        [Input("kitchenFaucetFlow", "Default 0.0")]
        [Output("building plumbing fixture selection object")]
        public static BuildingPlumbingFixtureSelection BuildingPlumbingFixtureSelection(double toiletVolumePerUse = 0.0, double urinalVolumePerUse = 0.0, double lavatoryVolumePerUse = 0.0, double showerVolumePerUse = 0.0, double kitchenFaucetPerUse = 0.0)
        {
            return new BuildingPlumbingFixtureSelection
            {
                ToiletVolumePerUse = toiletVolumePerUse,
                UrinalVolumePerUse = urinalVolumePerUse,
                LavatoryVolumePerUse = lavatoryVolumePerUse,
                ShowerVolumePerUse = showerVolumePerUse,
                KitchenFaucetPerUse = kitchenFaucetPerUse
            };
        }
    }
}
