
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataLibrary;

namespace CodeKY_SD01.Logic
{
    public interface IProductLogic : IProductRepository, IOrderRepository
    {
    }
}
