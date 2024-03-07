const { Events } = require("discord.js");

function randomNumber(min, max) {
    return Math.random() * (max - min) + min;
}

module.exports = {
  name: Events.MessageCreate,
  async execute(interaction) {
    if (interaction.content.toLowerCase().match(/\b(c\!thirsty)\b/) != null && !interaction.author.bot) {
      await interaction.reply("Gordon, I'm thirsty!")
      interaction.delete()
    }
  },
};
