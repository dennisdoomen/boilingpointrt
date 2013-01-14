using System;

using BoilingPointRT.Services.Commanding.Commands;
using BoilingPointRT.Services.Commanding.Handlers;
using BoilingPointRT.Services.Core.Specs.Support;
using BoilingPointRT.Services.DataAccess;
using BoilingPointRT.Services.Domain;

using FluentAssertions;

using Machine.Specifications;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;

namespace BoilingPointRT.Services.Core.Specs.Commands.Recipes
{
    [Subject("Recipe Creation")]
    public class When_a_recipe_is_created
    {
        private Establish context = () =>
        {
            dataMapper = new InMemoryDataMapper();
            var uow = new DomainUnitOfWork(dataMapper);

            handler = new CreateRecipeCommandHandler(() => uow);
        };

        private Because of = () => handler.Handle(new CreateRecipeCommand
        {
            Title = "Macaroni with cheese",
            Identity = new RecipeIdentity(Guid.NewGuid())
        });

        private It should = () =>
        {
            var recipes = dataMapper.GetCommittedEntities<Recipe>();
            recipes.Should().NotBeEmpty();

            var recipe = recipes.Single();
            
        };

        private static CreateRecipeCommandHandler handler;
        private static InMemoryDataMapper dataMapper;
    }
}