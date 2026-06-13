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

namespace Location.Tracking.Application.Devices.Commands.UpdateDevice
{
    public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, Result>
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceModelRepository _deviceModelRepository;
        private readonly IMapper _mapper;
        public UpdateDeviceCommandHandler(IDeviceRepository deviceRepository, IMapper mapper, IDeviceModelRepository deviceModelRepository)
        {
            _deviceRepository = deviceRepository;
            _deviceModelRepository = deviceModelRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {
            var deviceModel = await _deviceModelRepository.GetDeviceModelByName(request.DeviceConfiguration.DeviceModelName);

            if (deviceModel == null) return Result.Failure(Errors.DeviceModelErrors.DeviceModelNotFound); 

            var device = await _deviceRepository.GetByIdAsync(request.DeviceId);

            if (device == null) return Result.Failure(Errors.DeviceErrors.DeviceNotFound);

            device = _mapper.Map(request.DeviceConfiguration, device);
            //device.DeviceModelId = deviceModel.Data.Id;

            _deviceRepository.Update(device);
            await _deviceRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
