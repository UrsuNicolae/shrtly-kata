using AutoMapper;
using Moq;
using NUnit.Framework;
using ShrtLy.Api.Controllers;
using ShrtLy.Api.ViewModels;
using ShrtLy.BLL.Dtos;
using ShrtLy.BLL.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShrtLy.UnitTest
{
    public class ControllerTests
    {
        public LinksController controller;
        public Mock<IShorteningService> serviceMock;
        public Mock<IMapper> mapperMock;

        public static List<LinkViewModel> viewModels = new List<LinkViewModel>
            {
                new LinkViewModel
                {
                    Id = 1,
                    ShortUrl = "short-url-1",
                    Url = "url-1"
                },
                new LinkViewModel
                {
                    Id = 2,
                    ShortUrl = "short-url-2",
                    Url = "url-2"
                }
            };

        public static List<LinkDto> linkDtos = new List<LinkDto>
            {
                new LinkDto
                {
                    Id = 1,
                    ShortUrl = "short-url-1",
                    Url = "url-1"
                },
                new LinkDto
                {
                    Id = 2,
                    ShortUrl = "short-url-2",
                    Url = "url-2"
                }
            };

        [SetUp]
        public void Setup()
        {
            serviceMock = new Mock<IShorteningService>();
            mapperMock = new Mock<IMapper>();
            controller = new LinksController(serviceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task GetShortLink_ProcessLinkHasBeenCalled()
        {
            await controller.GetShortLinkAsync("http://google.com");

            serviceMock.Verify(x => x.ProcessLinkAsync("http://google.com"), Times.Once);
        }

        [Test]
        public async Task GetShortLink_ProcessLinksHasBeenCalled()
        {
            serviceMock.Setup(x => x.GetShortLinksAsync()).ReturnsAsync(new List<LinkDto>());

            await controller.GetShortLinksAsync();

            serviceMock.Verify(x => x.GetShortLinksAsync(), Times.Once);
        }

        [Test]
        public async Task GetShortLinks_AllLinksAreCorrect()
        {
            serviceMock.Setup(x => x.GetShortLinksAsync()).ReturnsAsync(new List<LinkDto>());

            await controller.GetShortLinksAsync();

            for (int i = 0; i < linkDtos.Count; i++)
            {
                Assert.AreEqual(viewModels[i].Id, linkDtos[i].Id);
                Assert.AreEqual(viewModels[i].ShortUrl, linkDtos[i].ShortUrl);
                Assert.AreEqual(viewModels[i].Url, linkDtos[i].Url);
            }
        }
    }
}
