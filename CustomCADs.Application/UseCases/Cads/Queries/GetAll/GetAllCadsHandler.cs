using CustomCADs.Application.Common.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Cads.Reads;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.GetAll;

public class GetAllCadsHandler(ICadReads reads) : IRequestHandler<GetAllCadsQuery, CadResult>
{
    public Task<CadResult> Handle(GetAllCadsQuery req, CancellationToken ct)
    {
        IQueryable<Cad> queryable = reads.GetAll(asNoTracking: true)
            .Filter(req.Creator, req.Status)
            .Search(req.Category, req.Name)
            .Sort(req.Sorting);

        IEnumerable<Cad> cads = 
        [
            .. queryable
                .Skip((req.Page - 1) * req.Limit)
                .Take(req.Limit)
        ];

        CadResult response = new()
        {
            Count = queryable.Count(),
            Cads = cads.Adapt<ICollection<CadModel>>(),
        };
        return Task.FromResult(response);
    }
}
