using System;

using BoilingPointRT.Services.Commanding.Commands;
using BoilingPointRT.Services.Commanding.Handlers;
using BoilingPointRT.Services.Domain;

using Machine.Specifications;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoilingPointRT.Services.Core.Specs.Commands.Recipes
{
    [Subject("Recipe Creation")]
    public class When_a_recipe_is_created
    {
        private Establish context = () =>
        {
            handler = new CreateRecipeCommandHandler();
        };

        private Because of = () => handler.Handle(new CreateRecipeCommand
        {
            Title = "Macaroni with cheese",
            Identity = new RecipeIdentity(Guid.NewGuid())
        });

        private It should = () =>
        {
            Assert.Fail("");
        };

        private static CreateRecipeCommandHandler handler;
    }
}