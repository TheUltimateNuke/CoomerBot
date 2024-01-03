const { SlashCommandBuilder } = require('discord.js');

module.exports = {
  data: new SlashCommandBuilder()
    .setName('chair_article')
    .setDescription('Dr. Coomer reads you the summary of the Chair wikipedia article from 2015'),
  async execute(interaction) {
    await interaction.reply("A chair is a piece of furniture with a raised surface used to sit on, commonly for use by one person. Chairs are most often supported by four legs and have a back; however, a chair can have three legs or could have a different shape. A chair without a back or arm rests is a stool, or when raised up, a bar stool. A chair with arms is an armchair and with folding action and reclining footrest, a recliner. A permanently fixed chair in a train or theater is a seat or, in an airplane, airline seat; when riding, it is a saddle and bicycle saddle, and for an automobile, a car seat or infant car seat. With wheels it is a wheelchair and when hung from above, a swing. A chair for more than one person is a couch, sofa, settee, or \"loveseat\"; or a bench. A separate footrest for a chair is known as an ottoman, hassock or pouffe.");
  },
};