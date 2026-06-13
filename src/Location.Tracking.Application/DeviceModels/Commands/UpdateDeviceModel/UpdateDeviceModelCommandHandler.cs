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

namespace Location.Tracking.Application.DeviceModels.Commands.UpdateDeviceModel
{
    public class UpdateDeviceModelCommandHandler : IRequestHandler<UpdateDeviceModelCommand, Result>
    {
        private readonly IDeviceModelRepository _deviceModelRepository;
        private readonly IMapper _mapper;
        public UpdateDeviceModelCommandHandler(IDeviceModelRepository deviceModelRepository, IMapper mapper)
        {
            _deviceModelRepository = deviceModelRepository;
            _mapper = mapper;

        }

        public async Task<Result> Handle(UpdateDeviceModelCommand request, CancellationToken cancellationToken)
        {
            DeviceModel? deviceModel = await _deviceModelRepository.GetByIdAsync(request.DeviceModelId);

            if (deviceModel == null) return Result.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            _mapper.Map(request.DeviceModelParams, deviceModel);

            _deviceModelRepository.Update(deviceModel);
            await _deviceModelRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
