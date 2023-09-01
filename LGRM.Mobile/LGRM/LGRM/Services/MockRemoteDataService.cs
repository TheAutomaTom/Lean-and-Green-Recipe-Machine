using System;
using System.Collections.Generic;
using System.Text;

namespace LGRM.XamF.Services
{
    public class MockRemoteDataService : IRemoteDataService
    {

        public DateTime GetDbUpdated()
        {
            var dt = new DateTime(2020, 12, 10);

            return dt;
        }

    }
}
