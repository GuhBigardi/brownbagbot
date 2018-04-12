using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ActiveLearningBot.Services
{
    [Serializable]
    public class AnnouncerService
    {
        private string botImageUrl;
        private string botImageUrlHappy;
        private string myNameIs;

        public AnnouncerService()
        {
            myNameIs = "Hey, eu sou Bot Paul!";
            botImageUrl = ConfigurationManager.AppSettings["BotImageUrl"];
            botImageUrlHappy = ConfigurationManager.AppSettings["BotImageUrlCongrats"];

        }

        public HeroCard GenerateIntroduction() => new HeroCard()
        {
            Title = myNameIs,
            Subtitle = "Já apontou suas horas hoje? 🕵",
            Text = "Ta esperando o que? Vamos apontar as horas, vamos la já que você não aponta de maneira nenhuma fui criado para te ajudar a apontar essas benditas horas!!!!",
            Images = new List<CardImage>()
            {
                new CardImage(botImageUrl, "eu sou Bot Paul")
            }
        };

        public HeroCard SendMessageCongratulations() => new HeroCard()
        {
            Title = "Aeee PARABÉNS",
            Text = "Horas apontadas, agora estou FELIZ hehe",
            Images = new List<CardImage>()
            {
                new CardImage(botImageUrlHappy, "eu sou Bot Paul")
            },
        };

    }
}