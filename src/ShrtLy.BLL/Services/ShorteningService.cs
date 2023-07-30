using ShrtLy.BLL.Services.Interfaces;
using ShrtLy.DAL.Entities;
using System.Threading;
using System;

namespace ShrtLy.BLL.Services
{
    public sealed class ShorteningService : IShorteningService
    {
        public LinkEntity ShortLink(string url)
        {
            Thread.Sleep(1);//make everything unique while looping
            long ticks = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;//EPOCH
            char[] baseChars = new char[] { '0','1','2','3','4','5','6','7','8','9',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x'};

            int i = 32;
            char[] buffer = new char[i];
            int targetBase = baseChars.Length;

            do
            {
                buffer[--i] = baseChars[ticks % targetBase];
                ticks = ticks / targetBase;
            }
            while (ticks > 0);

            char[] result = new char[32 - i];
            Array.Copy(buffer, i, result, 0, 32 - i);

            var shortUrl = new string(result);

            return  new LinkEntity
            {
                ShortUrl = shortUrl,
                Url = url
            }; 
        }
    }
}
