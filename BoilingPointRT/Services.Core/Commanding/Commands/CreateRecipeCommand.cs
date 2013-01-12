using BoilingPointRT.Services.Domain;

namespace BoilingPointRT.Services.Commanding.Commands
{
    internal class CreateRecipeCommand : ICommand
    {
        public string Title { get; set; }

        public RecipeIdentity Identity { get; set; }
    }
}