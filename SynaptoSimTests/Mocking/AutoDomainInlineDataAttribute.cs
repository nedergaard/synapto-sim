using AutoFixture.Xunit2;
using SynaptoSimTests.Customizations;

namespace SynaptoSimTests.Mocking
{
    public class AutoDomainInlineDataAttribute : InlineAutoDataAttribute
    {
        public AutoDomainInlineDataAttribute(params object[] values)
            : base(new AutoDomainAttribute(), values)
        {
        }
    }
}
