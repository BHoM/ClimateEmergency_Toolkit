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

        public class LatLonConversions
        {
            const double a = 6377563.396;
            const double b = 6356256.91;
            const double e2 = (a - b) / a;
            const double n0 = -100000;
            const double e0 = 400000;
            const double f0 = 0.999601272;
            const double phi0 = 0.855211333;
            const double lambda0 = -0.034906585;
            const double n = (a - b) / (a + b);

            static double lat, lng;

            private LatLonConversions() { }

            private static double Deg2Rad(double x)
            {
                return x * (Math.PI / 180);
            }

            private static double Rad2Deg(double x)
            {
                return x * (180 / Math.PI);
            }

            private static double SinSquared(double x)
            {
                return Math.Sin(x) * Math.Sin(x);
            }

            private static double TanSquared(double x)
            {
                return Math.Tan(x) * Math.Tan(x);
            }

            private static double Sec(double x)
            {
                return 1.0 / Math.Cos(x);
            }

            private static void OSGB36ToWGS84()
            {
                var airy1830 = new RefEll(6377563.396, 6356256.909);
                var a = airy1830.maj;
                var b = airy1830.min;
                var eSquared = airy1830.ecc;
                var phi = Deg2Rad(lat);
                var lambda = Deg2Rad(lng);
                var v = a / (Math.Sqrt(1 - eSquared * SinSquared(phi)));
                var H = 0; // height
                var x = (v + H) * Math.Cos(phi) * Math.Cos(lambda);
                var y = (v + H) * Math.Cos(phi) * Math.Sin(lambda);
                var z = ((1 - eSquared) * v + H) * Math.Sin(phi);

                var tx = 446.448;
                var ty = -124.157;
                var tz = 542.060;
                var s = -0.0000204894;
                var rx = Deg2Rad(0.00004172222);
                var ry = Deg2Rad(0.00006861111);
                var rz = Deg2Rad(0.00023391666);

                var xB = tx + (x * (1 + s)) + (-rx * y) + (ry * z);
                var yB = ty + (rz * x) + (y * (1 + s)) + (-rx * z);
                var zB = tz + (-ry * x) + (rx * y) + (z * (1 + s));

                var wgs84 = new RefEll(6378137.000, 6356752.3141);
                a = wgs84.maj;
                b = wgs84.min;
                eSquared = wgs84.ecc;

                var lambdaB = Rad2Deg(Math.Atan(yB / xB));
                var p = Math.Sqrt((xB * xB) + (yB * yB));
                var phiN = Math.Atan(zB / (p * (1 - eSquared)));
                for (var i = 1; i < 10; i++)
                {
                    v = a / (Math.Sqrt(1 - eSquared * SinSquared(phiN)));
                    double phiN1 = Math.Atan((zB + (eSquared * v * Math.Sin(phiN))) / p);
                    phiN = phiN1;
                }

                var phiB = Rad2Deg(phiN);

                lat = phiB;
                lng = lambdaB;
            }

            public static LatLon ConvertOSToLatLon(double easting, double northing)
            {
                RefEll airy1830 = new RefEll(6377563.396, 6356256.909);
                double OSGB_F0 = 0.9996012717;
                double N0 = -100000.0;
                double E0 = 400000.0;
                double phi0 = Deg2Rad(49.0);
                double lambda0 = Deg2Rad(-2.0);
                double a = airy1830.maj;
                double b = airy1830.min;
                double eSquared = airy1830.ecc;
                double phi = 0.0;
                double lambda = 0.0;
                double E = easting;
                double N = northing;
                double n = (a - b) / (a + b);
                double M = 0.0;
                double phiPrime = ((N - N0) / (a * OSGB_F0)) + phi0;
                do
                {
                    M =
                      (b * OSGB_F0)
                        * (((1 + n + ((5.0 / 4.0) * n * n) + ((5.0 / 4.0) * n * n * n))
                          * (phiPrime - phi0))
                          - (((3 * n) + (3 * n * n) + ((21.0 / 8.0) * n * n * n))
                            * Math.Sin(phiPrime - phi0)
                            * Math.Cos(phiPrime + phi0))
                          + ((((15.0 / 8.0) * n * n) + ((15.0 / 8.0) * n * n * n))
                            * Math.Sin(2.0 * (phiPrime - phi0))
                            * Math.Cos(2.0 * (phiPrime + phi0)))
                          - (((35.0 / 24.0) * n * n * n)
                            * Math.Sin(3.0 * (phiPrime - phi0))
                            * Math.Cos(3.0 * (phiPrime + phi0))));
                    phiPrime += (N - N0 - M) / (a * OSGB_F0);
                } while ((N - N0 - M) >= 0.001);
                var v = a * OSGB_F0 * Math.Pow(1.0 - eSquared * SinSquared(phiPrime), -0.5);
                var rho =
                  a
                    * OSGB_F0
                    * (1.0 - eSquared)
                    * Math.Pow(1.0 - eSquared * SinSquared(phiPrime), -1.5);
                var etaSquared = (v / rho) - 1.0;
                var VII = Math.Tan(phiPrime) / (2 * rho * v);
                var VIII =
                  (Math.Tan(phiPrime) / (24.0 * rho * Math.Pow(v, 3.0)))
                    * (5.0
                      + (3.0 * TanSquared(phiPrime))
                      + etaSquared
                      - (9.0 * TanSquared(phiPrime) * etaSquared));
                var IX =
                  (Math.Tan(phiPrime) / (720.0 * rho * Math.Pow(v, 5.0)))
                    * (61.0
                      + (90.0 * TanSquared(phiPrime))
                      + (45.0 * TanSquared(phiPrime) * TanSquared(phiPrime)));
                var X = Sec(phiPrime) / v;
                var XI =
                  (Sec(phiPrime) / (6.0 * v * v * v))
                    * ((v / rho) + (2 * TanSquared(phiPrime)));
                var XII =
                  (Sec(phiPrime) / (120.0 * Math.Pow(v, 5.0)))
                    * (5.0
                      + (28.0 * TanSquared(phiPrime))
                      + (24.0 * TanSquared(phiPrime) * TanSquared(phiPrime)));
                var XIIA =
                  (Sec(phiPrime) / (5040.0 * Math.Pow(v, 7.0)))
                    * (61.0
                      + (662.0 * TanSquared(phiPrime))
                      + (1320.0 * TanSquared(phiPrime) * TanSquared(phiPrime))
                      + (720.0
                        * TanSquared(phiPrime)
                        * TanSquared(phiPrime)
                        * TanSquared(phiPrime)));
                phi =
                  phiPrime
                    - (VII * Math.Pow(E - E0, 2.0))
                    + (VIII * Math.Pow(E - E0, 4.0))
                    - (IX * Math.Pow(E - E0, 6.0));
                lambda =
                  lambda0
                    + (X * (E - E0))
                    - (XI * Math.Pow(E - E0, 3.0))
                    + (XII * Math.Pow(E - E0, 5.0))
                    - (XIIA * Math.Pow(E - E0, 7.0));


                lat = Rad2Deg(phi);
                lng = Rad2Deg(lambda);
                // convert to WGS84
                OSGB36ToWGS84();

                return new LatLon(lat, lng);
            }
        }

        public class RefEll
        {
            public double maj, min, ecc;
            public RefEll(double major, double minor)
            {
                maj = major;
                min = minor;
                ecc = ((major * major) - (minor * minor)) / (major * major);
            }
        }

        public class LatLon
        {
            public double Latitude;
            public double Longitude;

            public LatLon()
            {
                Latitude = 0;
                Longitude = 0;
            }

            public LatLon(double lat, double lon)
            {
                Latitude = lat;
                Longitude = lon;
            }
        }

        /***************************************************/

    }
}
