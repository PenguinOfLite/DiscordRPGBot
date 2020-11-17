using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordRPGBot
{
    static class TileBehaviourHelper
    {
        /// <summary>
        /// Fills a new queue with some behaviour data
        /// </summary>
        /// <returns>A new <see cref="Queue{ITileBehaviour}"/></returns>
        public static Queue<ITileBehaviour> GetTileBehaviours()
        {
            var behavs = new Queue<ITileBehaviour>();
            behavs.Enqueue(new BossFight("Geralt of Rivia", 30, 4));
            behavs.Enqueue(new Story());
            behavs.Enqueue(new BossFight("Sonic", 5, 3));
            behavs.Enqueue(new Story());

            return behavs;
        }
    }
}
