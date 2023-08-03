namespace AvpVideoPlayer.Utility.Tests
{
    using AvpVideoPlayer.ViewModels;
    using System;
    using Xunit;

    public class RelayCommandTests
    {
        private readonly RelayCommand _testClass;
        private readonly Action<object?> _executeMethod;
        private readonly Func<object?, bool> _canexecuteMethod;

        public RelayCommandTests()
        {
            _executeMethod = (_) => { };
            _canexecuteMethod = (_) => true;
            _testClass = new RelayCommand(_executeMethod, _canexecuteMethod);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new RelayCommand(_executeMethod, _canexecuteMethod);
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallCanExecute()
        {
            var parameter = new object();
            Assert.True(_testClass.CanExecute(parameter));
        }

        [Fact]
        public void CanCallExecute()
        {
            try
            {
                var parameter = new object();
                _testClass.Execute(parameter);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
                throw;
            }
        }

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

        [Fact]
        public void CanExecuteChanged_StateUnderTest_ExpectedBehavior()
        {
            // Arrange\
            var test = false;
            var relayCommand = new RelayCommand((_) => { }, null);
            object? parameter = null;
            relayCommand.CanExecuteChanged += (s, e) => { test = true; };
            // Act
            relayCommand.Execute(parameter);

            // Assert
            Assert.False(test);
        }
    }
}