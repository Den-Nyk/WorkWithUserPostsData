﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WorkWithUserPostsData.Api.Controllers;

public class BaseController : ControllerBase
{
	private IMediator _mediator;
	protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
}
