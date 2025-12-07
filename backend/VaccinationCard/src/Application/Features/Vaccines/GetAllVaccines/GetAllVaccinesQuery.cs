using Application.Common.Models;
using Application.Features.Vaccines.GetAllVaccines;
using MediatR;

namespace Application.Features.Vaccines.GetVaccines;

public record GetAllVaccinesQuery : IRequest<Result<IEnumerable<VaccineResponse>>>;
