using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise.GameLogic
{
    /// <summary>
    /// Structure to store the name and point of the collectibles
    /// </summary>
   public struct FruitType
    {
        public string name;
        public int point;
        /// <summary>
        /// Upon calling FruitType it return the given collectibles name and point
        /// </summary>
        /// <param name="name"></param>
        /// <param name="point"></param>
        public FruitType(string name,int point)
        {
            this.name = name;
            this.point = point;
        }

    }


}