using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.BLL.DFetchDataBLL
{
    public class ElectricBLL
    {
        private readonly DataContext context;

        public ElectricBLL(DataContext _context)
        {
            this.context = _context;
        }


        public bool WriteData(IEnumerable<ApdFctElectric> list,string year)
        {
            try
            {
                return 1 == 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
