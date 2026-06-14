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

namespace Location.Tracking.Application.DeviceModels.Commands.CreateDeviceModel
{
    internal class CreateDeviceModelCommandHandler : IRequestHandler<CreateDeviceModelCommand, Result>
    {
        private readonly IDeviceModelRepository _deviceModelRepository;
        private readonly IMapper _mapper;
        public CreateDeviceModelCommandHandler(IDeviceModelRepository deviceModelRepository, IMapper mapper)
        {
            _deviceModelRepository = deviceModelRepository;
            _mapper = mapper;
            
        }

        public async Task<Result> Handle(CreateDeviceModelCommand request, CancellationToken cancellationToken)
        {
            DeviceModel deviceModel = new DeviceModel();

            _mapper.Map(request, deviceModel);

            await _deviceModelRepository.AddAsync(deviceModel);
            await _deviceModelRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
