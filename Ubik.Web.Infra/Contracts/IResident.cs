using System.Collections.Generic;
using System.Security.Claims;

namespace Ubik.Web.Infra.Contracts
{
    public interface IResident
    {
        //IResidentAdministration Administration { get; }
        IResidentSecurity Security { get; }

        //IResidentPubSub PubSub { get; }
    }

    public interface IResidentSecurity
    {
        IEnumerable<Claim> SystemRoles { get; }

        IEnumerable<Claim> SystemRoleClaims(string role);
    }

    public interface IResidentAdministration
    {
        //AdminNavigationElements BackOfficeMenu { get; }
    }

    //public interface IResidentPubSub : IDomainCommandProcessor, IEventPublisher
    //{
    //}

    //public class ResidentPubSub : IResidentPubSub
    //{
    //    private readonly IDomainCommandProcessor _processor;
    //    private readonly IEventPublisher _publisher;

    //    public ResidentPubSub(IDomainCommandProcessor processor, IEventPublisher publisher)
    //    {
    //        _processor = processor;
    //        _publisher = publisher;
    //    }

    //    public void Process<TCommand>(TCommand command) where TCommand : IDomainCommand
    //    {
    //        _processor.Process(command);
    //    }

    //    public IEnumerable<TResult> Process<TCommand, TResult>(TCommand command) where TCommand : IDomainCommand
    //    {
    //        return _processor.Process<TCommand, TResult>(command);
    //    }

    //    public void Process<TCommand, TResult>(TCommand command, Action<TResult> resultHandler) where TCommand : IDomainCommand
    //    {
    //        _processor.Process(command, resultHandler);
    //    }

    //    public void Publish<T>(T @event) where T : IDomainEvent
    //    {
    //        _publisher.Publish(@event);
    //    }
    //}
}