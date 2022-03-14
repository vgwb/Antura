# Shaders

## General

The following are simple high performance shaders created for environment art (such as trees, houses),
and Antura's customization pieces.

* **Diffuse.shader** - Opaque, Lambert lighting, Colored, single texture
	* Sample: Letters Emoticons
* **DiffuseJustColor.shader** - Opaque, Lambert lighting, Colored, no texture
	* Sample: quite every environment object (e.g. mountains, trees)
* **Specular.shader** - Opaque, BlinnPhong lighting, Colored, single texture
	* Sample: Living Letters
* **SpecularDual.shader** - As Specular, but renders both sides
	* Sample: Reward Glass Punk
* **SpecularJustColor.shader** - Opaque, BlinnPhong lighting, Colored, no texture
	* Sample: Clouds in Tobogan
* **Transparent.shader** - Very simple unlit transparent
* **TransparentBothSides.shader** - As Transparent, but renders both sides
	* Sample: Reward Candy Stick, Reward Glass Punk
 
## Antura
* **AnturaDog.shader** - The 1st material used by Antura's model. It allows us to change the hue of Antura's fabric without using different textures; it also provide a simplified lighting model compared to the Standard shader, in order to support mobile platforms.
* **AnturaDecals.shader** - The 2nd material used by Antura's model. It allows decals (e.g. the smile patch on Antura's back) and body to change color without the need of multiple textures and independently of Antura's fabric.


## Image Effects
* **MobileBlur.shader** - used by ReadingGame to blur the text behind the lens; though this is known to be a low performance effect on mobile, it is not applied for each frame. On the contrary, the image effect is rendered once each time the text changes.

* **VignettingSimpleShader.shader** - used in each game scene, it simulates the vignetting effect by just rendering a fullscreen quad in which pixels that are closer to the center of the screen are rendered with higher transparency than the others. It does not read from the framebuffer, so its render weight is roughly the same of an unlit semi-transparent quad without any texture. It also provide a way to create fade in/fade out effects.

## Minigames

### ReadingGame/Alphabet Song
* **BlurredPaste.shader** - used to blit the blurred text on the screen
* **MagnifyingGlass.shader** - used to implement the magnifying glass effect
* **TreeSlicesSprite.shader** - used by the back white sprite to adapt its size to the length of the text
* **GlassShines.shader** - used by the *sunshine* effect, which is visible when the magnifying glass is positioned on the correct part of the song.

### Tobogan
* **MeterDot.shader** - This shader is used to render the red dashed line which measures the tower's height.
* **EnlargingTube.shader** - This shader is used to add an opening/closing animation to the pipes.
* **TransparentColor.shader** - Used by pipes' placeholders. It renders an unlit semi-transparent mesh.

### Egg/Mixed Letters
* **MobileParticleAdd_Alpha8.shader** - Used by the particles system "Egg_vfx_particle_win" (the shiny particles behind the living letter)

### Hide and Sick
* **TransparentMasker.shader** - This shader is used by the floor material (light stage).

### Sick Letters
* **Antura/Glass.shader** - Used for the glass

## TextMesh Pro

The following list includes the custom shaders accompanying the TextMesh Pro plug-in:

* **TMP_Bitmap.shader**
* **TMP_Bitmap-Mobile.shader**
* **TMP_SDF.shader**
* **TMP_SDF Overlay.shader**
* **TMP_SDF-Mobile.shader**
* **TMP_SDF-Mobile Masking.shader**
* **TMP_SDF-Mobile Overlay.shader**
* **TMP_SDF-Surface.shader**
* **TMP_SDF-Surface-Mobile.shader**
* **TMP_SDF_SL.shader**
* **TMP_Sprite.shader**

## Misc
* **Confetti.shader** - Used in Ending Scene by colored confetti
* **Glitter.shader** - Used in Ending Scene by shining glitters

