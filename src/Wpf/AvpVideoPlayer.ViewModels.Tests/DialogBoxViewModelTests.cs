using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels;
using Moq;
using System;
using System.Windows;
using Xunit;

namespace AvpVideoPlayer.ViewModels.Tests
{
    public class DialogBoxViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<IViewRegistrationService> mockViewRegistrationService;

        public DialogBoxViewModelTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockViewRegistrationService = mockRepository.Create<IViewRegistrationService>();
        }

        private DialogBoxViewModel CreateViewModel()
        {
            return new DialogBoxViewModel(
                mockViewRegistrationService.Object, "text", "title", true, IDialogService.DialogTypes.Information);
        }
    }
}
