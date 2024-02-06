
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
    }
}
