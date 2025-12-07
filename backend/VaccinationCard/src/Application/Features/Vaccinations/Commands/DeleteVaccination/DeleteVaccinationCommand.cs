using Application.Common.Models;
using MediatR;

namespace Application.Features.Vaccinations.Commands.DeleteVaccination;

public record DeleteVaccinationCommand(Guid VaccinationId) : IRequest<Result>;