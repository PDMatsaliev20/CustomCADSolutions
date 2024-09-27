﻿using static CustomCADs.Domain.DataConstants.RoleConstants;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Categories
{
    public class CategoriesGroup : Group
    {
        public CategoriesGroup()
        {
            Configure("API/Categories", ep =>
            {
                ep.Options(opt => opt.WithTags("Categories"));
                ep.Roles(Admin);
            });   
        }
    }
}
