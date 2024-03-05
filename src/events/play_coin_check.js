const { Events } = require("discord.js");

function randomNumber(min, max) {
    return Math.random() * (max - min) + min;
}

module.exports = {
  name: Events.MessageCreate,
  async execute(interaction) {
    if (interaction.content.toLowerCase().match(/\b(c\!playcoins)\b/) != null && !interaction.author.bot) {
      const rand = Math.floor(randomNumber(1, 39))
      await interaction.reply("You have " + rand + " PlayCoins! Don't hit zero, Gordon!")
    }
  },
};
