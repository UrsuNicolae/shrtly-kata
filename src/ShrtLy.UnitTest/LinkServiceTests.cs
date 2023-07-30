using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShrtLy.BLL.Dtos;
using ShrtLy.BLL.Services;
using ShrtLy.BLL.Services.Interfaces;
using ShrtLy.DAL;
using ShrtLy.DAL.Entities;
using ShrtLy.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShrtLy.UnitTest
{
    public sealed class LinkServiceTests
    {
        private Mock<ILinksRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IShorteningService> _shorteningServiceMock;
        private LinkService _linkService;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<ILinksRepository>();
            _mapperMock = new Mock<IMapper>();
            _shorteningServiceMock = new Mock<IShorteningService>();
            _linkService = new LinkService(_repositoryMock.Object, _mapperMock.Object, _shorteningServiceMock.Object);
        }

        [Test]
        public async Task ProcessLinkAsync_WithNewUrl_Should_ReturnShortUrl()
        {
            var inputUrl = "https://example.com/page1";
            var shortLink = new LinkEntity { Url = inputUrl, ShortUrl = "https://short.ly/abc123" };
            var shortLinkDto = new LinkDto { Url = inputUrl, ShortUrl = "https://short.ly/abc123" };

            _repositoryMock.Setup(repo => repo.GetLinkAsync(inputUrl)).ReturnsAsync((LinkEntity)null);
            _shorteningServiceMock.Setup(service => service.ShortLink(inputUrl)).Returns(shortLink);
            _repositoryMock.Setup(repo => repo.CreateLinkAsync(shortLink)).ReturnsAsync(1);

            var result = await _linkService.ProcessLinkAsync(inputUrl);

            Assert.AreEqual(shortLink.ShortUrl, result);
            _repositoryMock.Verify(repo => repo.CreateLinkAsync(shortLink), Times.Once);
        }

        [Test]
        public async Task ProcessLinkAsync_WithExistingUrl_Should_ReturnExistingShortUrl()
        {
            var inputUrl = "https://example.com/page1";
            var existingLink = new LinkEntity { Url = inputUrl, ShortUrl = "https://short.ly/existing" };

            _repositoryMock.Setup(repo => repo.GetLinkAsync(inputUrl)).ReturnsAsync(existingLink);

            var result = await _linkService.ProcessLinkAsync(inputUrl);

            Assert.AreEqual(existingLink.ShortUrl, result);
            _repositoryMock.Verify(repo => repo.CreateLinkAsync(It.IsAny<LinkEntity>()), Times.Never);
        }

        [Test]
        public async Task GetAllLinksAsync_Should_ReturnMappedLinkDtos()
        {
            var mockEntities = new List<LinkEntity>
            {
                new LinkEntity { Id = 1, Url = "https://example.com/page1" },
                new LinkEntity { Id = 2, Url = "https://example.com/page2" }
            };

            var expectedDtos = new List<LinkDto>
            {
                new LinkDto { Id = 1, Url = "https://example.com/page1" },
                new LinkDto { Id = 2, Url = "https://example.com/page2" }
            };

            _repositoryMock.Setup(repo => repo.GetAllLinksAsync()).ReturnsAsync(mockEntities);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<LinkDto>>(It.IsAny<IEnumerable<LinkEntity>>())).Returns(expectedDtos);

            var result = await _linkService.GetShortLinksAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDtos.Count(), result.Count());
        }

        [Test]
        public async Task GetLinkAsync_Should_ReturnMappedLinkDto()
        {
            var inputUrl = "https://example.com/page1";
            var linkEntity = new LinkEntity { Id = 1, Url = inputUrl };
            var expectedDto = new LinkDto { Id = 1, Url = inputUrl };

            _repositoryMock.Setup(repo => repo.GetLinkAsync(inputUrl)).ReturnsAsync(linkEntity);
            _mapperMock.Setup(mapper => mapper.Map<LinkDto>(linkEntity)).Returns(expectedDto);

            var result = await _linkService.GetLinkAsync(inputUrl);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto.Id, result.Id);
            Assert.AreEqual(expectedDto.Url, result.Url);
        }

        [Test]
        public async Task GetByShortNameAsync_Should_ReturnMappedLinkDto()
        {
            var inputUrl = "https://example.com/page1";
            var linkEntity = new LinkEntity { Id = 1, Url = inputUrl };
            var expectedDto = new LinkDto { Id = 1, Url = inputUrl };

            _repositoryMock.Setup(repo => repo.GetByShortNameAsync(inputUrl)).ReturnsAsync(linkEntity);
            _mapperMock.Setup(mapper => mapper.Map<LinkDto>(linkEntity)).Returns(expectedDto);

            var result = await _linkService.GetByShortNameAsync(inputUrl);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto.Id, result.Id);
            Assert.AreEqual(expectedDto.Url, result.Url);
        }
    }
}
