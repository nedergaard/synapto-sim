using AutoFixture.Xunit2;
using AutoFixture;

namespace SynaptoSimTests.Customizations
{
    public class AutoDomainAttribute : AutoDataAttribute
    {
        public AutoDomainAttribute()
            : base(() => 
                new Fixture()
                    .Customize(new AutoDomainCustomization()))
        {
        }
    }
}