const { Events } = require("discord.js");

function randomNumber(min, max) {
    return Math.random() * (max - min) + min;
}

module.exports = {
  name: Events.MessageCreate,
  async execute(interaction) {
    if (interaction.content.toLowerCase().match(/\b(c\!playcoins)\b/) != null && !interaction.author.bot) {
      const rand = Math.floor(randomNumber(0, 39))
      if (rand == 0) {
        await interaction.reply("You have " + rand + " PlayCoins... (THERES NOTHING HERE yet, wait for HL2VRAI)") //TODO: change for HL2VRAI
      }
      else {
        await interaction.reply("You have " + rand + " PlayCoins!")
      }
    }
  },
};
