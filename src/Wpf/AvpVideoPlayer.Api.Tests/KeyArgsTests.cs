//namespace AvpVideoPlayer.Api.Tests
//{
//    using System;
//    using Xunit;
//    using System.Windows.Input;
//    using System.Windows;

//    public class KeyArgsTests
//    {
//        private KeyArgs _testClass;

//        public KeyArgsTests()
//        {
//            _testClass = new KeyArgs();
//        }

//        [Fact]
//        public void CanCallConvert()
//        {
//            var r = new KeyEventArgs(new KeyboardDevice(new InputManager()), PresentationSource.FromDependencyObject(new DependencyObject()), 1137704605, Key.ImeProcessed);
//            var result = KeyArgs.Convert(r);
//            throw new NotImplementedException("Create or modify test");
//        }

//        [Fact]
//        public void CannotCallConvertWithNullR()
//        {
//            Assert.Throws<ArgumentNullException>(() => KeyArgs.Convert(default(KeyEventArgs)));
//        }

//        [Fact]
//        public void ConvertPerformsMapping()
//        {
//            var r = new KeyEventArgs(new KeyboardDevice(new InputManager()), PresentationSource.FromDependencyObject(new DependencyObject()), 459813342, Key.H);
//            var result = KeyArgs.Convert(r);
//            Assert.Equal(r.IsDown, result.IsDown);
//            Assert.Equal(r.IsRepeat, result.IsRepeat);
//            Assert.Equal(r.IsToggled, result.IsToggled);
//            Assert.Equal(r.IsUp, result.IsUp);
//            Assert.Equal(r.Key, result.Key);
//            Assert.Equal(r.SystemKey, result.SystemKey);
//        }

//        [Fact]
//        public void CanSetAndGetHandled()
//        {
//            var testValue = true;
//            _testClass.Handled = testValue;
//            Assert.Equal(testValue, _testClass.Handled);
//        }

//        [Fact]
//        public void CanSetAndGetKey()
//        {
//            var testValue = Key.NoName;
//            _testClass.Key = testValue;
//            Assert.Equal(testValue, _testClass.Key);
//        }

//        [Fact]
//        public void CanSetAndGetRoutedEvent()
//        {
//            var testValue = new RoutedEvent();
//            _testClass.RoutedEvent = testValue;
//            Assert.Equal(testValue, _testClass.RoutedEvent);
//        }

//        [Fact]
//        public void CanSetAndGetIsRepeat()
//        {
//            var testValue = true;
//            _testClass.IsRepeat = testValue;
//            Assert.Equal(testValue, _testClass.IsRepeat);
//        }

//        [Fact]
//        public void CanSetAndGetTimestamp()
//        {
//            var testValue = 1137216670;
//            _testClass.Timestamp = testValue;
//            Assert.Equal(testValue, _testClass.Timestamp);
//        }

//        [Fact]
//        public void CanSetAndGetSystemKey()
//        {
//            var testValue = Key.F17;
//            _testClass.SystemKey = testValue;
//            Assert.Equal(testValue, _testClass.SystemKey);
//        }

//        [Fact]
//        public void CanSetAndGetSource()
//        {
//            var testValue = new object();
//            _testClass.Source = testValue;
//            Assert.Equal(testValue, _testClass.Source);
//        }

//        [Fact]
//        public void CanSetAndGetIsDown()
//        {
//            var testValue = false;
//            _testClass.IsDown = testValue;
//            Assert.Equal(testValue, _testClass.IsDown);
//        }

//        [Fact]
//        public void CanSetAndGetIsUp()
//        {
//            var testValue = true;
//            _testClass.IsUp = testValue;
//            Assert.Equal(testValue, _testClass.IsUp);
//        }

//        [Fact]
//        public void CanSetAndGetIsToggled()
//        {
//            var testValue = true;
//            _testClass.IsToggled = testValue;
//            Assert.Equal(testValue, _testClass.IsToggled);
//        }
//    }
//}