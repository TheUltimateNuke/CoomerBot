const { Events } = require('discord.js');

module.exports = {
  name: Events.MessageCreate,
  async execute(interaction) {
    if (interaction.content.toLowerCase().match(/i thought /) != null && !interaction.author.bot) 
    {
      await interaction.reply("You thought wrong, my good bitch!");
    }
  }
}