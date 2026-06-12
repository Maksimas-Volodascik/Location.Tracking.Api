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

namespace Location.Tracking.Application.DeviceModels.Commands.DeleteDeviceModel
{
    public class DeleteDeviceModelCommandHandler : IRequestHandler<DeleteDeviceModelCommand, Result>
    {
        private readonly IDeviceModelRepository _deviceModelRepository;
        public DeleteDeviceModelCommandHandler(IDeviceModelRepository deviceModelRepository)
        {
            _deviceModelRepository = deviceModelRepository;
        }
        public async Task<Result> Handle(DeleteDeviceModelCommand request, CancellationToken cancellationToken)
        {
            DeviceModel? deviceModel = await _deviceModelRepository.GetByIdAsync(request.deviceModelId);

            if (deviceModel == null) return Result.Failure(Errors.DeviceModelErrors.DeviceModelNotFound);

            _deviceModelRepository.Delete(deviceModel);
            await _deviceModelRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
