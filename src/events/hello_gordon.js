const { Events } = require("discord.js");

module.exports = {
  name: Events.GuildMemberAdd,
  async execute(interaction) {
    if (interaction.user.id == "1192095575164846190") {
      const channel = interaction.guild.channels.cache.find(channel => channel.type === 'GUILD_TEXT')
      channel.send("Hello, Gordon!")
    }
  },
};
