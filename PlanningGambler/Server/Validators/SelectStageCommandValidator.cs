﻿using FluentValidation;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Validators;

public class SelectStageCommandValidator : AbstractValidator<SelectStageCommand>
{
    private readonly IRoomStorage _roomStorage;

    public SelectStageCommandValidator(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;

        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoomId).NotEmpty();
        RuleFor(x => x.StageId).NotEmpty();

        RuleFor(x => x)
            .MustAsync(CheckStageSelect);
    }

    public async Task<bool> CheckStageSelect(SelectStageCommand command, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(command.RoomId);

        return 
            room != null
            && room.Members.Exists(x => x.Id == command.UserId)
            && room.Stages.Exists(x => x.Id == command.StageId)
            && command.MemberType == Shared.Data.MemberType.Administrator;
    }
}