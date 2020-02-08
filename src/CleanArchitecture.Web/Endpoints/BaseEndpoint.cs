using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Endpoints
{
    public abstract class BaseEndpoint<TRequest, TResponse> : ControllerBase
    {
        public abstract ActionResult<TResponse> Handle(TRequest request);
    }

    public abstract class BaseEndpoint<TResponse> : ControllerBase
    {
        public abstract ActionResult<TResponse> Handle();
    }
}
