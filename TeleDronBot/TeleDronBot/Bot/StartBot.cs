﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.Commons;
using TeleDronBot.DTO;
using TeleDronBot.Interfaces;
using TeleDronBot.Interfaces.Bot;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.Bot
{
    class StartBot
    {
        ApplicationContext context = new ApplicationContext();
        TelegramBotClient client;
        public void BotRun()
        {
            context.Database.EnsureCreated();
            MainProvider provider = new MainProvider();

            client = new TelegramBotClient(Constant.Token);
            CommandProvider commandProvider = new CommandProvider(client, provider);
            client.StartReceiving();

            List<string> commandList = CommandList.GetCommands();
            var scope = new ServiceCollection()
                .AddScoped<IMessageHandler, MessageHandler>(x => new MessageHandler(client, provider, commandProvider))
                .AddScoped<ICallbackHandler, CallBackHandler>(i => new CallBackHandler(client, provider)).BuildServiceProvider();
            
            client.OnCallbackQuery += async (object sender, CallbackQueryEventArgs args) =>
            {
                var callbackHandler = scope.GetService<ICallbackHandler>();
                await callbackHandler.BaseCallBackHandler(args);
            };

            client.OnMessage += async (object sender, MessageEventArgs args) =>
            {
                var handler = scope.GetService<IMessageHandler>();
                await handler.BaseHandlerMessage(args, args.Message.Text);
            };
        }
    }
}
