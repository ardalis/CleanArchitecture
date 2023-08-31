using NimblePros.SampleToDo.Core.Interfaces;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate.Events;
using NimblePros.SampleToDo.Core.ProjectAggregate.Handlers;
using Xunit;
using NSubstitute;

namespace NimblePros.SampleToDo.UnitTests.Core.Handlers;

public class ItemCompletedEmailNotificationHandlerHandle
{
  private ItemCompletedEmailNotificationHandler _handler;
  private IEmailSender _emailSender = Substitute.For<IEmailSender>();

  public ItemCompletedEmailNotificationHandlerHandle()
  {
    _handler = new ItemCompletedEmailNotificationHandler(_emailSender);
  }

  [Fact]
  public async Task ThrowsExceptionGivenNullEventArgument()
  {
#nullable disable
    Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));
#nullable enable
  }

  [Fact]
  public async Task SendsEmailGivenEventInstance()
  {
    await _handler.Handle(new ToDoItemCompletedEvent(new ToDoItem()), CancellationToken.None);

    await _emailSender.Received().SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
  }
}
