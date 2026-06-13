using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DeviceModels.Query.GetDeviceModelByName
{
    public class GetDeviceModelByNameQueryHandler : IRequestHandler<GetDeviceModelByNameQuery, Result<DeviceModel>>
    {
        private readonly IDeviceModelRepository _deviceModelRepository;
        public GetDeviceModelByNameQueryHandler(IDeviceModelRepository deviceModelRepository)
        {
            _deviceModelRepository = deviceModelRepository;
        }

        public async Task<Result<DeviceModel>> Handle(GetDeviceModelByNameQuery request, CancellationToken cancellationToken)
        {
            DeviceModel? deviceModel = await _deviceModelRepository.GetDeviceModelByName(request.ModelName);

            if (deviceModel == null) return Result<DeviceModel>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            return Result<DeviceModel>.Success(deviceModel);
        }
    }
}
