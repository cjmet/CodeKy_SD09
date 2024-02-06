
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataLibrary;

namespace DataLibrary
{
    public interface IProductLogic : IProductRepository, IOrderRepository
    {
        public string ProductInterfaceFilename { get => "IProductLogic"; }
        public string ProductInterfaceFunctionName() => "IProductLogic";
        public string ProductDbPath { get => "IProductLogic"; }
        public string OrderInterfaceFilename { get => "IProductLogic"; }
        public string OrderInterfaceFunctionName() => "IProductLogic";
        public string OrderDbPath { get => "IProductLogic"; }
    }
}
