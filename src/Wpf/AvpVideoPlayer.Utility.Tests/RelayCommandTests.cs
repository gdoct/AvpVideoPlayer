namespace AvpVideoPlayer.Utility.Tests
{
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
            var parameter = new object();
            _testClass.Execute(parameter);
        }
    }
}