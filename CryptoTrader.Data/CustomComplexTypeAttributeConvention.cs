using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data
{
    public class CustomComplexTypeAttributeConvention : ComplexTypeAttributeConvention
    {
        public CustomComplexTypeAttributeConvention(ProviderConventionSetBuilderDependencies dependencies) : base(dependencies)
        {
        }

        public override void ProcessComplexPropertyAdded(IConventionComplexPropertyBuilder propertyBuilder, IConventionContext<IConventionComplexPropertyBuilder> context)
        {
            base.ProcessComplexPropertyAdded(propertyBuilder, context);
        }
        protected override void ProcessComplexTypeAdded(IConventionComplexTypeBuilder complexTypeBuilder, ComplexTypeAttribute attribute, IConventionContext context)
        {
            base.ProcessComplexTypeAdded(complexTypeBuilder, attribute, context);
        }

        protected override void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, ComplexTypeAttribute attribute, IConventionContext<IConventionEntityTypeBuilder> context)
        {
            base.ProcessEntityTypeAdded(entityTypeBuilder, attribute, context);
        }

        public override void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
        {
            base.ProcessEntityTypeAdded(entityTypeBuilder, context);
        }
    }
}
