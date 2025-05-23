﻿using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.DeviceRepositories
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        Task<Device> GetDeviceById(string id);

        Task<Device> GetDeviceByUri(string uri);
    }
}
