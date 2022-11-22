using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using System.Reflection;

namespace Grains
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private readonly ILogger _logger;
        private readonly IPersistentState<HelloGrainState> _state;

        public HelloGrain(ILogger<HelloGrain> logger,
            [PersistentState("hellostate", "SimpleTableStorage1")] IPersistentState<HelloGrainState> state
            )
        {
            _logger = logger;
            _state = state;
        }

        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            var grainId = this.GetGrainId();

            if (String.IsNullOrWhiteSpace(this._state?.State?.LastMessage))
            {
                _logger.LogInformation($"LOADING {grainId} - New");
            }
            else
            {
                _logger.LogInformation($"LOADING {grainId} - msg={this._state?.State?.LastMessage}");
            }
            return base.OnActivateAsync(cancellationToken);
        }

        async Task<string> IHello.SayHello(string greeting)
        {
            this._state.State.LastMessage = greeting;
            await this._state.WriteStateAsync();
            _logger.LogInformation("SayHello message received: greeting = '{Greeting}'", greeting);
            return $"\n Client said: '{greeting}', so HelloGrain says: Hello!";
        }
    }


    [GenerateSerializer]
    public class HelloGrainState
    {
        [Id(0)]
        public string LastMessage { get; set; }
    }
}
