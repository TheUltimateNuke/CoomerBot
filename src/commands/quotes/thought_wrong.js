const { SlashCommandBuilder } = require('discord.js');

module.exports = {
  data: new SlashCommandBuilder()
    .setName('thought_wrong')
    .setDescription('You thought wrong, my good bitch!'),
  async execute(interaction) {
    await interaction.reply('You thought wrong, my good bitch!');
  },
};