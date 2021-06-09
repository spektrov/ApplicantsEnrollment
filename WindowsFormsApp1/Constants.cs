using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Class saves all constant values
    /// that are used for changing parameters.
    /// </summary>
    internal static class Constants
    {
        private static int _budget;
        private static int _contract;
        private static int _privilege;
        private static decimal _coefficient1;
        private static decimal _coefficient2;
        private static decimal _coefficient3;
        private static decimal _coefficient4;
        private static decimal _ruralCoefficient = 1;

        /// <summary>
        /// Amount of students on budget.
        /// </summary>
        public static int Budget
        {
            get => _budget;
            set => _budget = value >= 0 ? value : _budget;
        }

        /// <summary>
        /// Amount of students on contract.
        /// </summary>
        public static int Contract
        {
            get => _contract;
            set => _contract = value >= 0 ? value : _contract;
        }

        /// <summary>
        /// Amount of students with privilege.
        /// </summary>
        public static int Privilege
        {
            get => _privilege;
            set => _privilege = value >= 0 ? value : _privilege;
        }

        /// <summary>
        /// Value of the first subject coefficient.
        /// </summary>
        public static decimal Coefficient1
        {
            get => _coefficient1;
            set => _coefficient1 = value >= 0 && value <= 1 ? value : _coefficient1;
        }

        /// <summary>
        /// Value of the second subject coefficient.
        /// </summary>
        public static decimal Coefficient2
        {
            get => _coefficient2;
            set => _coefficient2 = value >= 0 && value <= 1 ? value : _coefficient2;
        }


        /// <summary>
        /// Value of the third subject coefficient.
        /// </summary>
        public static decimal Coefficient3
        {
            get => _coefficient3;
            set => _coefficient3 = value >= 0 && value <= 1 ? value : _coefficient3;
        }


        /// <summary>
        /// Value of the certificate coefficient.
        /// </summary>
        public static decimal Coefficient4
        {
            get => _coefficient4;
            set => _coefficient4 = value >= 0 && value <= 1 ? value : _coefficient4;
        }


        /// <summary>
        /// Value of the coefficient for applicants from countryside.
        /// </summary>
        public static decimal RuralCoefficient
        {
            get => _ruralCoefficient;
            set => _ruralCoefficient = value >= 1 ? value : _ruralCoefficient;
        }

        /// <summary>
        /// Save changed parameters into XML file.
        /// </summary>
        /// <returns></returns>
        public static bool SaveData()
        {
            var list = new List<decimal>()
            {
                _budget, _contract, _privilege,
                _coefficient1, _coefficient2, _coefficient3, _coefficient4, _ruralCoefficient
            };

            var xmlFormatter = new XmlSerializer(typeof(List<decimal>));

            try
            {
                using (var file = new FileStream("logicData.xml", FileMode.Create))
                {
                    xmlFormatter.Serialize(file, list);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Read parameters from XML file.
        /// </summary>
        public static void ReadData()
        {
            var xmlFormatter = new XmlSerializer(typeof(List<decimal>));

            try
            {
                using (var file = new FileStream("logicData.xml", FileMode.OpenOrCreate))
                {
                    if (xmlFormatter.Deserialize(file) is List<decimal> logicData)
                    {
                        _budget = (int) logicData[0];
                        _contract = (int) logicData[1];
                        _privilege = (int) logicData[2];
                        _coefficient1 = logicData[3];
                        _coefficient2 = logicData[4];
                        _coefficient3 = logicData[5];
                        _coefficient4 = logicData[6];
                        _ruralCoefficient = logicData[7];
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Checks if sum of all coefficients is equal 1. 
        /// </summary>
        /// <returns>true if valid</returns>
        public static bool ValidateCoefficient(decimal c1, decimal c2, decimal c3, decimal c4)
        {
            return c1 + c2 + c3 + c4 == 1;
        }

        /// <summary>
        /// Check if students with privilege is less than on budget.
        /// </summary>
        /// <returns>true if valid</returns>
        public static bool ValidatePrivilege(int budget, int privilege)
        {
            return budget > privilege;
        }
    }
}