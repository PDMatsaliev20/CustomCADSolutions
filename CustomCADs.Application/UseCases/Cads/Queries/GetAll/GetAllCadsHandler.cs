using CustomCADs.Application.Common.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.GetAll;

public class GetAllCadsHandler(ICadQueries queries) : IRequestHandler<GetAllCadsQuery, CadResult>
{
    public Task<CadResult> Handle(GetAllCadsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Cad> queryable = queries.GetAll(asNoTracking: true)
            .Filter(request.Creator, request.Status)
            .Search(request.Category, request.Name)
            .Sort(request.Sorting);

        IEnumerable<Cad> cads = 
        [
            .. queryable
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
        ];

        CadResult response = new()
        {
            Count = queryable.Count(),
            Cads = cads.Adapt<ICollection<CadModel>>(),
        };
        return Task.FromResult(response);
    }
}
