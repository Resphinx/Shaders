Hi,

This is an experimental shader for drawing a pie chart. Currently, it uses textures for transferring the data. There are two variants, lit and unlit in URP.

For inputing data, you need the <b>PieChart</b> script:
- Paste the data in the <b>Values</b> field. The values should be separated by comma, semicolon or space. There can be maximum <b>16</b> items in each row.
- The number of items is decided by the row with most values in it. For rows with fewer values, the missing ones are assume as 0.
- The values can be absolute or percentages. Set the value types in the designate field.
- The number of rows indicate the number of states for the data (for example different years). Each row is considered a "step". In the play mode, you can change the current step and progress to update it in the material.
- Remember to add the material to this script.

For the material you can set the size and angle of the chart:
- The <b>Size</b>, between 0 and 1, corresponds with the UV (though the chart is centred on UV 0.5,0.5).
- The <b>Hole</b>, between 0 and 1, sets the inner percentage of the chart that would be empty, creating a torus.
- The <b>Start</b> and <b>End</b> set the starting and ending angles of the chart (corresponding to 0 to 360 degrees).
- <b>RoundEdge</b> defines the round edges of the end of the chart. For the lit shader, it is between -1 and 1 that also defines the normal direction. For the unlit shader, it is between 0 and 1.

- You need to set the <b>Colors</b> and <b>Data</b> properties of the material (the textures are included, do not change their resolution). The first contains the color theme of the chart and the latter the data.
- Other properties do not have a function yet.

To-do roadmap:
- Adding the background.
- Changing the input type to float arrays.
- Fixing the transparency.
