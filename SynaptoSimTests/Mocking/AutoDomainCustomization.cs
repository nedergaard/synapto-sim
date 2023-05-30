using AutoFixture.AutoMoq;
using AutoFixture;

namespace SynaptoSimTests.Customizations
{
    public class AutoDomainCustomization : CompositeCustomization
    {
        public AutoDomainCustomization()
            : base(
                new AutoMoqCustomization { ConfigureMembers = true })
        {
        }
    }
}
