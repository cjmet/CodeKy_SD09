using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeKY_SD01.Products
{
    public class DryCatFood : CatFood
    {
        override public double Weight { get; set; }
    }
}
