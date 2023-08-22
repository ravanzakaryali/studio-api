using Space.Application.Abstraction.Common;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Space.Infrastructure.Services;

public class TelegramService : ITelegramService
{
    readonly TelegramBotClient _telegramBotClient;
    readonly long _chatId;

    public TelegramService(IConfiguration configuration)
    {
        _telegramBotClient = new TelegramBotClient(configuration["Telegram:BotToken"]);
        _chatId = long.Parse(configuration["Telegram:ChatId"]);
    }
    public void SendMessage(string message)
    {
        _telegramBotClient.SendTextMessageAsync(_chatId, message);
    }
}
