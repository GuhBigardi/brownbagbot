using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using System;
using System.Configuration;

namespace ActiveLearningBot.Dialogs
{
    [Serializable]
    public class BaseLuisDialog<T> : LuisDialog<T>
    {
        public BaseLuisDialog() : base(GetNewService())
        {

        }

        private static ILuisService[] GetNewService()
        {
            var modelId = ConfigurationManager.AppSettings.Get("LuisModelID");
            var subscriptionKey = ConfigurationManager.AppSettings.Get("LuisSubscriptionKey");
            var luisModel = new LuisModelAttribute(modelId, subscriptionKey);
            return new ILuisService[] { new LuisService(luisModel) };
        }
    }

}