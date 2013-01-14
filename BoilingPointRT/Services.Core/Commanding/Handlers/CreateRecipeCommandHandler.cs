using System;

using BoilingPointRT.Services.Commanding.Commands;
using BoilingPointRT.Services.DataAccess;

namespace BoilingPointRT.Services.Commanding.Handlers
{
    internal class CreateRecipeCommandHandler : IHandleCommand<CreateRecipeCommand>
    {
        private readonly Func<DomainUnitOfWork> uowFactory;

        public CreateRecipeCommandHandler(Func<DomainUnitOfWork> uowFactory)
        {
            this.uowFactory = uowFactory;
        }

        public void Handle(CreateRecipeCommand command)
        {
        }
    }
}