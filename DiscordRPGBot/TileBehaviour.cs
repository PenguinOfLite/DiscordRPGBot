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
  


}
