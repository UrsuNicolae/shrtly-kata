using NUnit.Framework;
using ShrtLy.BLL.Services;
using System.Collections.Generic;

namespace ShrtLy.UnitTest
{

    public class ShorteningServiceTests
    {
        private ShorteningService _shorteningService;

        [SetUp]
        public void SetUp()
        {
            _shorteningService = new ShorteningService();
        }

        [Test]
        public void ShortLink_Should_GenerateUniqueShortUrl()
        {
            var url1 = "https://example.com/page1";
            var url2 = "https://example.com/page2";

            var shortLink1 = _shorteningService.ShortLink(url1);
            var shortLink2 = _shorteningService.ShortLink(url2);

            Assert.AreNotEqual(shortLink1.ShortUrl, shortLink2.ShortUrl);
        }

        [Test]
        public void ShortLink_Should_NotGenerateDuplicateShortUrls()
        {
            var url = "https://example.com/page1";
            var shortLinks = new HashSet<string>();

            for (int i = 0; i < 1000; i++)
            {
                var shortLink = _shorteningService.ShortLink(url);
                shortLinks.Add(shortLink.ShortUrl);
            }

            Assert.AreEqual(1000, shortLinks.Count);
        }
    }
}
