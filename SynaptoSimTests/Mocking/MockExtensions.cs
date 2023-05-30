using Moq;

namespace SynaptoSimTests.Mocking
{
    public static class MockExtensions
    {
        public static Mock<T> AsMock<T>(this T mockInstance) 
            where T : class => Mock.Get(mockInstance);
    }
}
