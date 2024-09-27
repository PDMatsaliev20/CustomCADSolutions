﻿using CustomCADs.API.Endpoints.Categories.GetCategoryById;
using CustomCADs.API.Models.Others;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Categories;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Categories.CreateCategory
{
    using static StatusCodes;

    public class CreateCategoryEndpoint(ICategoryService service) : Endpoint<CreateCategoryRequest, CategoryDTO>
    {
        public override void Configure()
        {
            Post("");
            Group<CategoriesGroup>();
            Description(s => s.WithSummary("Creates a new Category with the provided name."));
            Options(opt =>
            {
                opt.Produces<GetCategoryByIdRequest>(Status200OK, "application/json");
                opt.ProducesProblem(Status500InternalServerError);
            });
        }

        public override async Task HandleAsync(CreateCategoryRequest req, CancellationToken ct)
        {
            CategoryModel model = new()
            {
                Name = req.Name,
            };
            int id = await service.CreateAsync(model).ConfigureAwait(false);

            CategoryDTO category = new()
            {
                Id = id,
                Name = req.Name,
            };
            await SendAsync(category, Status200OK).ConfigureAwait(false);
        }
    }
}
