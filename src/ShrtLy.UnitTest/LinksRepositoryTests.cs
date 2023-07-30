using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ShrtLy.DAL.Entities;
using ShrtLy.DAL.Repositories.Interfaces;
using ShrtLy.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrtLy.UnitTest
{
    public sealed class LinksRepositoryTests
    {
        private ShrtLyContext _dbContext;
        private ILinksRepository _linksRepository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ShrtLyContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _dbContext = new ShrtLyContext(options);
            _linksRepository = new LinksRepository(_dbContext);
        }

        [Test]
        public async Task CreateLinkAsync_Should_AddNewLinkEntityToDatabase()
        {
            var entity = new LinkEntity { Url = "https://example.com/page1" };

            var createdLinkId = await _linksRepository.CreateLinkAsync(entity);

            Assert.IsTrue(createdLinkId > 0); 
            var savedEntity = await _dbContext.Links.FindAsync(createdLinkId);
            Assert.IsNotNull(savedEntity);
            Assert.AreEqual(entity.Url, savedEntity.Url);
        }

        [Test]
        public async Task GetAllLinksAsync_Should_ReturnAllLinksInDatabase()
        {
            var entities = new List<LinkEntity>
            {
                new LinkEntity { Url = "https://example.com/page1" },
                new LinkEntity { Url = "https://example.com/page2" },
                new LinkEntity { Url = "https://example.com/page3" }
            };

            _dbContext.Links.AddRange(entities);
            await _dbContext.SaveChangesAsync();

            var result = await _linksRepository.GetAllLinksAsync();

            Assert.AreEqual(entities.Count, result.Count());
            foreach (var entity in entities)
            {
                Assert.IsTrue(result.Any(r => r.Id == entity.Id && r.Url == entity.Url));
            }
        }

        [Test]
        public async Task GetLinkAsync_Should_ReturnLinkEntityIfExists()
        {
            var url = "https://example.com/page1";
            var entity = new LinkEntity { Url = url };

            _dbContext.Links.Add(entity);
            await _dbContext.SaveChangesAsync();

            var result = await _linksRepository.GetLinkAsync(url);

            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Id, result.Id);
            Assert.AreEqual(entity.Url, result.Url);
        }

        [Test]
        public async Task GetLinkAsync_Should_ReturnNullIfLinkDoesNotExist()
        {
            var url = "https://example.com/nonexistent";

            var result = await _linksRepository.GetLinkAsync(url);

            Assert.IsNull(result);
        }
    }
}
