using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordRPGBot
{
    public interface ITileBehaviour
    {
        
        
        public  string Play();
        
    }

    public class Story : ITileBehaviour
    {
        
        public  string Play()
        {
            return "meg akarok halni a gecibe";
        }
    }

    // I created a sample for testing purposes
    public class BossFight : ITileBehaviour
    {
        public string BossName { get; set; }
        public int BossHP { get; set; }
        public int BossAttack { get; set; }

        // this would look more clean with having a hiearchy of bosses (so IBoss interface --> then implementing bosses)
        // store the created boss types, if you're shooting for a more complex game, but then move assets into a new folder called Models
        // for organization, and to avoid having an overcrowded project

        public BossFight(string bossName, int bossHP, int bossAttack)
        {
            this.BossName = bossName;
            this.BossHP = bossHP;
            this.BossAttack = bossAttack;
        }

        public string Play()
        {
            return $"Ha-Ha, you're a fool for wandering here. I'm {BossName}, and I will crush you with a hit of {BossAttack} HPs!";
        }
    }


}
