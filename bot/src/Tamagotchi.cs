using Discord.Interactions;

namespace CoomerBot;

public class Tamagotchi : InteractionModuleBase
{
    public struct TamagotchiNeeds 
    {
        public float hunger = 100f;
        public float thirst = 100f;

        public TamagotchiNeeds() { }
    }

    private const string commandFailureMessage = "...Hello, Gordon!";

    public bool TamagotchiActive {get; private set;}

    public readonly TamagotchiNeeds needs = new();

    private PeriodicTimer? tickTimer;

    [SlashCommand("tamagotchimode", "Controls for CoomerBot's \"Tamagotchi Mode\".")]
    public async Task TamagotchiMode(string input) 
    {   
        switch (input.ToLower()) 
        {
            case "toggle":
                await SetTamagotchiMode(!TamagotchiActive);
                break;
            default:
                await RespondAsync($"{commandFailureMessage}\n\n||This message is being displayed because the passed command went unrecognized.||");
                break;
        }
    }

    private async Task StartTamagotchiTick() 
    {
        tickTimer ??= new(TimeSpan.FromSeconds(10));

        while (await tickTimer.WaitForNextTickAsync()) MainTickLoop();
    }

    private void MainTickLoop()
    {
        
    }

    public async Task SetTamagotchiMode(bool enabled) 
    {
        TamagotchiActive = enabled;
        if (enabled) await StartTamagotchiTick(); else tickTimer?.Dispose();

        return;
    }
}