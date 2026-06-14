using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DeviceModels.Query.GetDeviceModelById
{
    public class GetDeviceModelByIdQueryHandler : IRequestHandler<GetDeviceModelByIdQuery, Result<DeviceModel>>
    {
        private readonly IDeviceModelRepository _deviceModelRepository;
        public GetDeviceModelByIdQueryHandler(IDeviceModelRepository deviceModelRepository)
        {
            _deviceModelRepository = deviceModelRepository;
        }

        public async Task<Result<DeviceModel>> Handle(GetDeviceModelByIdQuery request, CancellationToken cancellationToken)
        {
            DeviceModel? deviceModel = await _deviceModelRepository.GetByIdAsync(request.DeviceModelId);

            if (deviceModel == null) return Result<DeviceModel>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            return Result<DeviceModel>.Success(deviceModel);
        }
    }
}
