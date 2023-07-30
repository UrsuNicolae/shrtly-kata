using BenchmarkDotNet.Attributes;
using ShrtLy.BLL.Services;
using ShrtLy.BLL.Services.Interfaces;
using System.Threading.Tasks;

namespace ShrtLy.BLL.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ShorteningServiceBenchmark
    {
        private const string url = "https://github.com/alexei-corduneanu/shrtly-kata";
        private readonly IShorteningService _shorteningService = new ShorteningService();

        [Benchmark]
        public  void ShortLink()
        {
            _shorteningService.ShortLink(url);
        }
    }
}
