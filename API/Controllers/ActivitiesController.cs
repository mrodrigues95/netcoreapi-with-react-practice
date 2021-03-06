﻿using Application.Activities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers {

    public class ActivitiesController : BaseController {
        // GET a list of activities.
        [HttpGet]
        public async Task<ActionResult<List.ActivitiesEnvelope>> List(int? limit, int? offset) {
            return await Mediator.Send(new List.Query(limit, offset));
        }

        // GET a single activity.
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ActivityDto>> Details(Guid id) {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        // POST an activity.
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command) {
            return await Mediator.Send(command);
        }

        // PUT an activity.
        [HttpPut("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command) {
            command.Id = id;
            return await Mediator.Send(command);
        }

        // DELETE an activity.
        [HttpDelete("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult<Unit>> Delete(Guid id) {
            return await Mediator.Send(new Delete.Command { Id = id });
        }

        // POST an attendance.
        [HttpPost("{id}/attend")]
        public async Task<ActionResult<Unit>> Attend(Guid id) {
            return await Mediator.Send(new Attend.Command { Id = id });
        }

        // DELETE an attendance.
        [HttpDelete("{id}/attend")]
        public async Task<ActionResult<Unit>> Unattend(Guid id) {
            return await Mediator.Send(new Unattend.Command { Id = id });
        }
    }
}
