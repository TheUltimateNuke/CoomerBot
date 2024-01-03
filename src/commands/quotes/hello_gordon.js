const { SlashCommandBuilder } = require('discord.js');

module.exports = {
  data: new SlashCommandBuilder()
    .setName('hello_gordon')
    .setDescription('Hello, Gordon!'),
  async execute(interaction) {
    await interaction.reply('Hello, Gordon!');
  },
};