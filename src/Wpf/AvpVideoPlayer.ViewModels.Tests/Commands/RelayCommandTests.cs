using AvpVideoPlayer.ViewModels;
using System;
using Xunit;

namespace AvpVideoPlayer.ViewModels.Tests.Commands
{
    public class RelayCommandTests
    {
        [Fact]
        public void CanExecute_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var relayCommand = new RelayCommand((_) => { }, null);
            object? parameter = null;

            // Act
            var result = relayCommand.CanExecute(
                parameter);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_StateUnderTest_ExpectedBehavior()
        {
            // Arrange\
            var test = false;
            var relayCommand = new RelayCommand((_) => { test = true; }, null);
            object? parameter = null;

            // Act
            relayCommand.Execute(
                parameter);

            // Assert
            Assert.True(test);
        }
    }
}
