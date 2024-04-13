Hi,

This is an experimental shader for typing and, in future calligraphy. In order to type on a material, you need to convert the text into a texture. The script <b>CharacterMap</b> does this:
- You need to put your material in its <b>Material</b> field.
- Add the texture <b>colormap</b> to the <b>Color Map</b> field. This is the standard color maps. You can control the color of individual characters by adding number 0 to F (hex) to the <b>Color</b> field.
- You can also set the color of specific characters with the <b>Specia</b> field.
- Changes will be applied during the Play mode when the <b>Text</b> field is changed.

For the material:
- You need to add the <b>text</b> texture as its <b>Text</b> property. Do not change this texture.
- You also need to assign the <b>Character Map</b> property (see the included textures). You can also add the normal and emission.
- The included map is 9x11. Please make sure that this is set properly in the <b>Map Size</b> property.
- The paper size can also be set that represents how the UV of 0..1 is turned into a grid. UVs beyond this will be clamped.
- You can set the direction of the text here as well.
- Currently, the shader can only work with fixed and same length characters (I'm using Courier font).

Calligraphy:
- By calligraphy I mean the time-based writing of each letter. The time is calculated by the red channel of the character map (0..1 corresponds to 0..100% of the time allocated to each character, with the <b>Appear After</b> property). Currently, only the "i" letter has this feature for testing.

To-Do roadmap:
- Managing characters with different lengths.
- Capability to delay parts of the character until a certain event (e.g. space). In some cases and scripts, the dots or accent are placed after the whole word is written.
- Finding a way to design character calligraphy more intuitive.
- Checking the functionality in HDRP and mobile.
