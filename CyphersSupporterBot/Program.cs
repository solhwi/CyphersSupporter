﻿namespace CyphersSupporterBot
{
    internal class Program
    {
        private static MessengerListener messengerListener = null;

        private const int MessengerServerPort = 8002;

        private static async Task Main(string[] args)
        {
            await CyphersAPI.PreLoad();
            LocalDataManager.PreLoad();

            messengerListener = new MessengerListener(MessengerServerPort);
            messengerListener.Start();

            var messengerTask = messengerListener.Listen();
            messengerTask.GetAwaiter().GetResult();

            messengerListener.Close();
        }
    }
}
