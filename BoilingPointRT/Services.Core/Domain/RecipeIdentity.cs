using System;

namespace BoilingPointRT.Services.Domain
{
    internal class RecipeIdentity : GuidKey
    {
        public RecipeIdentity(Guid guid) : base(guid)
        {
        }

        public override object Clone()
        {
            return new RecipeIdentity(Key);
        }
    }
}