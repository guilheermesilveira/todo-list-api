using FluentValidation.Results;

namespace TodoList.Application.Notifications;

public class Notificator : INotificator
{
    private readonly List<string> _notifications = new();
    private bool _notFoundResource;

    public void Handle(string message)
    {
        if (_notFoundResource)
            throw new InvalidOperationException("Cannot call Handle when there are NotFoundResource!");

        _notifications.Add(message);
    }

    public void Handle(List<ValidationFailure> failures)
    {
        failures.ForEach(failure => Handle(failure.ErrorMessage));
    }

    public void HandleNotFoundResource()
    {
        if (HasNotification)
            throw new InvalidOperationException("Cannot call HandleNotFoundResource when there are notifications!");

        _notFoundResource = true;
    }

    public IEnumerable<string> GetNotifications()
    {
        return _notifications;
    }

    public bool HasNotification => _notifications.Any();
    public bool IsNotFoundResource => _notFoundResource;
}