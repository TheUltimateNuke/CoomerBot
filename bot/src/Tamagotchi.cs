using Discord.Interactions;
using Discord.Rest;

namespace CoomerBot;

public class Tamagotchi : InteractionModuleBase
{
    public class Need
    {
        public string name = "Untitled Need";
        public float maxValue = 100;
        public float decayRate = 0.03f;
        public float curValue;

        public Need() { curValue = maxValue; }
    }

    private const string commandFailureMessage = "...Hello, Gordon!";

    public static TimeSpan? StartActiveHours { get; private set; }
    public static TimeSpan? EndActiveHours { get; private set; }

    public bool TamagotchiActive 
    {
        get
        {
            return TamagotchiActive; 
        } 
        private set
        {
            SetTamagotchiMode(value);
        }
    }

    public Need[] Needs => needs.ToArray();
    private readonly List<Need> needs = new() 
    {
        new Need()
        {
            name = "Hunger"
        },
        new Need()
        {
            name = "Thirst",
            decayRate = 0.06f
        }
    };

    private PeriodicTimer? tickTimer;

    [SlashCommand("tamagotchimode", "Controls for CoomerBot's \"Tamagotchi Mode\".")]
    public async Task TamagotchiMode(string input) 
    {   
        switch (input.ToLower()) 
        {
            case "toggle":
                TamagotchiActive = !TamagotchiActive;
                break;
            default:
                await RespondAsync($"{commandFailureMessage}\n\n||This message is being displayed because the passed command went unrecognized.||");
                break;
        }
    }

    [SlashCommand("activehours", "Sets a time range where CoomerBot is \"awake\" and his needs are depleting.")]
    public async Task SetActiveHours(string input) 
    {
        var args = input.Split(' ');
        if (args.Length != 2) 
        {
            await RespondAsync($"{commandFailureMessage}\n\n||This message is being displayed because the incorrect amount of arguments for this command was passed.||");
            return;
        }

        if (!TimeSpan.TryParse(args[0], out var startTime)) 
        {
            await RespondAsync($"{commandFailureMessage}\n\n||This message is being displayed because CoomerBot could not parse \"{args[0]}\" as a time of day. Please input a correctly formatted 24-hour clock time.||");
            return;
        }

        if (!TimeSpan.TryParse(args[1], out var endTime)) 
        {
            await RespondAsync($"{commandFailureMessage}\n\n||This message is being displayed because CoomerBot could not parse \"{args[1]}\" as a time of day. Please input a correctly formatted 24-hour clock time.||");
            return;
        }

        StartActiveHours = startTime;
        EndActiveHours = endTime;
    }

    private async Task StartTamagotchiTick() 
    {
        tickTimer ??= new(TimeSpan.FromSeconds(10));
        while (await tickTimer.WaitForNextTickAsync()) MainTickLoop();
    }

    private void MainTickLoop()
    {
        if (!TamagotchiActive) return;

        if (StartActiveHours is not null || EndActiveHours is not null)
        {
            var now = DateTime.Now.TimeOfDay;

            if ((now > StartActiveHours) && (now < EndActiveHours))
            {
                foreach (Need need in Needs)
                {
                    need.curValue -= need.decayRate;
                }
            }
        }   
    }

    public void SetTamagotchiMode(bool enabled) 
    {
        TamagotchiActive = enabled;
        if (enabled) _ = StartTamagotchiTick(); else tickTimer?.Dispose();

        return;
    }
}