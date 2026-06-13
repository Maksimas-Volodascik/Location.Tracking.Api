using AutoMapper;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DeviceModels.Query.GetAllDeviceModels
{
    public class GetAllDeviceModelsQueryHandler : IRequestHandler<GetAllDeviceModelsQuery, Result<IEnumerable<DeviceModel>>>
    {
        private readonly IDeviceModelRepository _deviceModelRepository;
        public GetAllDeviceModelsQueryHandler(IDeviceModelRepository deviceModelRepository)
        {
            _deviceModelRepository = deviceModelRepository;
        }

        public async Task<Result<IEnumerable<DeviceModel>>> Handle(GetAllDeviceModelsQuery request, CancellationToken cancellationToken)
        {
            var deviceModelList = await _deviceModelRepository.GetAllDeviceModelsAsync();

            return Result<IEnumerable<DeviceModel>>.Success(deviceModelList);
        }
    }
}
