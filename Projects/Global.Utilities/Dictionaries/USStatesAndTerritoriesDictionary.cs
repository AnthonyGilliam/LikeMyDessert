using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Global.Utilities.Enums;
using Global.Utilities.ExtensionMethods;

namespace Global.Utilities.Dictionaries
{
    public static class USStatesAndTerritoriesDictionary
    {
        private static IDictionary<USStatesAndTerritories, string> _dictionary;

        static USStatesAndTerritoriesDictionary()
        {
            _dictionary = new Dictionary<USStatesAndTerritories, string>();
            _dictionary.Add(USStatesAndTerritories.Alabama, "AL");
            _dictionary.Add(USStatesAndTerritories.Alaska, "AK");
            _dictionary.Add(USStatesAndTerritories.AmericanSamoa, "AS");
            _dictionary.Add(USStatesAndTerritories.Arizona, "AZ");
            _dictionary.Add(USStatesAndTerritories.Arkansas, "AR");
            _dictionary.Add(USStatesAndTerritories.California, "CA");
            _dictionary.Add(USStatesAndTerritories.Colorado, "CO");
            _dictionary.Add(USStatesAndTerritories.Connecticut, "CT");
            _dictionary.Add(USStatesAndTerritories.Delaware, "DE");
            _dictionary.Add(USStatesAndTerritories.DistrictOfColumbia, "DC");
            _dictionary.Add(USStatesAndTerritories.FederatedStatesOfMicronesia, "FM");
            _dictionary.Add(USStatesAndTerritories.Florida, "FL");
            _dictionary.Add(USStatesAndTerritories.Georgia, "GA");
            _dictionary.Add(USStatesAndTerritories.Guam, "GU");
            _dictionary.Add(USStatesAndTerritories.Hawaii, "HI");
            _dictionary.Add(USStatesAndTerritories.Idaho, "ID");
            _dictionary.Add(USStatesAndTerritories.Illinois, "IL");
            _dictionary.Add(USStatesAndTerritories.Indiana, "IN");
            _dictionary.Add(USStatesAndTerritories.Iowa, "IA");
            _dictionary.Add(USStatesAndTerritories.Kansas, "KS");
            _dictionary.Add(USStatesAndTerritories.Kentucky, "KY");
            _dictionary.Add(USStatesAndTerritories.Louisiana, "LA");
            _dictionary.Add(USStatesAndTerritories.Maine, "ME");
            _dictionary.Add(USStatesAndTerritories.MarshallIslands, "MH");
            _dictionary.Add(USStatesAndTerritories.Maryland, "MD");
            _dictionary.Add(USStatesAndTerritories.Massachusetts, "MA");
            _dictionary.Add(USStatesAndTerritories.Michigan, "MI");
            _dictionary.Add(USStatesAndTerritories.Minnesota, "MN");
            _dictionary.Add(USStatesAndTerritories.Mississippi, "MS");
            _dictionary.Add(USStatesAndTerritories.Missouri, "MO");
            _dictionary.Add(USStatesAndTerritories.Montana, "MT");
            _dictionary.Add(USStatesAndTerritories.Nebraska, "NE");
            _dictionary.Add(USStatesAndTerritories.Nevada, "NV");
            _dictionary.Add(USStatesAndTerritories.NewHampshire, "NH");
            _dictionary.Add(USStatesAndTerritories.NewJersey, "NJ");
            _dictionary.Add(USStatesAndTerritories.NewMexico, "NM");
            _dictionary.Add(USStatesAndTerritories.NewYork, "NY");
            _dictionary.Add(USStatesAndTerritories.NorthCarolina, "NC");
            _dictionary.Add(USStatesAndTerritories.NorthDakota, "ND");
            _dictionary.Add(USStatesAndTerritories.NorthernMarianaIslands, "MP");
            _dictionary.Add(USStatesAndTerritories.Ohio, "OH");
            _dictionary.Add(USStatesAndTerritories.Oklahoma, "OK");
            _dictionary.Add(USStatesAndTerritories.Oregon, "OR");
            _dictionary.Add(USStatesAndTerritories.Palau, "PW");
            _dictionary.Add(USStatesAndTerritories.Pennsylvania, "PA");
            _dictionary.Add(USStatesAndTerritories.PuertoRico, "PR");
            _dictionary.Add(USStatesAndTerritories.RhodeIsland, "RI");
            _dictionary.Add(USStatesAndTerritories.SouthCarolina, "SC");
            _dictionary.Add(USStatesAndTerritories.SouthDakota, "SD");
            _dictionary.Add(USStatesAndTerritories.Tennessee, "TN");
            _dictionary.Add(USStatesAndTerritories.Texas, "TX");
            _dictionary.Add(USStatesAndTerritories.Utah, "UT");
            _dictionary.Add(USStatesAndTerritories.Vermont, "VT");
            _dictionary.Add(USStatesAndTerritories.VirginIslands, "VI");
            _dictionary.Add(USStatesAndTerritories.Virginia, "VA");
            _dictionary.Add(USStatesAndTerritories.Washington, "WA");
            _dictionary.Add(USStatesAndTerritories.WestVirginia, "WV");
            _dictionary.Add(USStatesAndTerritories.Wisconsin, "WI");
            _dictionary.Add(USStatesAndTerritories.Wyoming, "WY");
        }

        public static string Lookup(USStatesAndTerritories state)
        {
            return _dictionary[state];
        }

        public static IDictionary<string, int> GetKeyIntegerPairs()
        {
            IDictionary<string, int> keyIntPairs = new Dictionary<string, int>();

            foreach (USStatesAndTerritories state in Enum.GetValues(typeof(USStatesAndTerritories)))
            {
                keyIntPairs.Add(state.ToString().SeperateWords(), (int)state);
            }

            return keyIntPairs;
        }
    }
}
