const { Events } = require('discord.js');
const wiki = require("wikijs").default;

const regexMatch = /(what is an? |what'?s an? |what'?s |what is |what are )\S+/

module.exports = {
  name: Events.MessageCreate,
  async execute(interaction) {
    if (interaction.content.toLowerCase().match(regexMatch) != null && !interaction.author.bot)
    {
        let pageString = interaction.content.toLowerCase().match(regexMatch)[1];
        console.log("pageString: \"" + pageString + "\"");
        let finalString = interaction.content.toLowerCase().replace(pageString, "");
        console.log("searching Wikipedia for \"" + finalString + "\"");
        
        wiki().page(finalString)
        .then(async(page) => {
          let summaryText = await page.summary();
          if (summaryText.length > 2000) 
          {
            for (let i = 0; i < summaryText.length; i++) {
              if ((i / 2000) % 1 == 0) {
                let piece = summaryText.slice(i, (i + 2000))
                await interaction.reply(piece)
              }
            }
          }
          else{
            await interaction.reply(summaryText)
          }
        })
        .catch((error) => {
          interaction.reply("There's nothing there.\n\n||This message is being displayed because an error occurred: " + error + "||")
          console.error(error);
        });
    }
  }
}