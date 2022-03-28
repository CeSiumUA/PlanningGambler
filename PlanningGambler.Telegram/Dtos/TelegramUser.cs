using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningGambler.Telegram.Dtos
{
    public record TelegramUser(
        long ChatId,
        string UserName,
        Guid RoomId);
}
