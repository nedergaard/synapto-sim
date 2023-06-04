using Assets.Scripts.Creature.Brain;
using FluentAssertions;

namespace SynaptoSimTests.Creature;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("123456", 2, 3)]
    [InlineData("", 2, 0)]
    [InlineData("123456", 6, 1)]
    public void EnumerableCharChunks_returns_correct_number_of_strings(string dut, int chunkSize, int expected)
    {
        // Arrange

        // Act
        var actual =
            dut
                .Chunks(chunkSize)
                .Count();

        // Assert
        actual.Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("123456", 3, "123", "456")]
    [InlineData("XYZ", 1, "X", "Y", "Z")]
    [InlineData("X Z", 1, "X", " ", "Z")]
    [InlineData(" XZ ", 2, " X", "Z ")]
    [InlineData("it does not add up => remainder", 22, "it does not add up => ", "remainder")]
    public void EnumerableCharChunks_returns_correct_strings(string dut, int chunkSize, params string[] expected)
    {
        // Arrange

        // Act
        var actual =
            dut
                .Chunks(chunkSize)
                .ToArray();

        // Assert
        actual.Should()
            .BeEquivalentTo(expected);
    }
}