---
title: Copernic et le système solaire (pl_07) - Script
hide:
---

# Copernic et le système solaire (pl_07) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-07-copernicus-and-the-solar-system/38/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=783699917#gid=783699917)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_07%20Solar%20System/PL_07%20Solar%20System%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_07 | Solar System (Torun)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// WANTED:</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - order planets_order              (arrange 8 planets from Sun)</span>
<span class="yarn-header-dim">// - cleancanvas collect_mercury_1    (find 1 sun-heated rock)</span>
<span class="yarn-header-dim">// - cleancanvas collect_venus_2      (find 2 hot clouds)</span>
<span class="yarn-header-dim">// - cleancanvas collect_earth_3      (find 3 water drops)</span>
<span class="yarn-header-dim">// - cleancanvas collect_mars_4       (find 4 red dust grains)</span>
<span class="yarn-header-dim">// - cleancanvas collect_jupiter_5    (find 5 storm bolts)</span>
<span class="yarn-header-dim">// - cleancanvas collect_saturn_6     (find 6 ring fragments)</span>
<span class="yarn-header-dim">// - cleancanvas collect_uranus_7     (find 7 ice crystals)</span>
<span class="yarn-header-dim">// - memory neptune_facts             (match: deep blue / farthest / Triton / strong winds)</span>
<span class="yarn-header-dim">// - quiz copernicus_basics           (3 questions: center, first planet, rings)</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">image: torun_street</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $planets_placed = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card torun&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome to TORUN in POLAND.]</span> <span class="yarn-meta">#line:2a00001</span>
<span class="yarn-cmd">&lt;&lt;card nicolaus_copernicus&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: NICOLAUS COPERNICUS was born here.]</span> <span class="yarn-meta">#line:2a00002</span>
<span class="yarn-cmd">&lt;&lt;card nicolaus_copernicus_house&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: ANTURA sneaked into his model room!]</span> <span class="yarn-meta">#line:2a00003</span>
<span class="yarn-line">[MISSING TRANSLATION: Let's ask COPERNICUS for help.]</span> <span class="yarn-meta">#line:2a00004</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Great work!]</span> <span class="yarn-meta">#line:2a00005</span>
<span class="yarn-cmd">&lt;&lt;card solar_system&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Today we learned the 8 PLANETS of the SOLAR SYSTEM.]</span> <span class="yarn-meta">#line:2a00006</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Draw the 7 PLANETS around the SUN in order.]</span> <span class="yarn-meta">#line:2a00007</span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-copernicus-outside"></a>

## COPERNICUS_OUTSIDE

<div class="yarn-node" data-title="COPERNICUS_OUTSIDE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- MEET COPERNICUS ---</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card nicolaus_copernicus_house&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Hello! I am NICOLAUS COPERNICUS.]</span> <span class="yarn-meta">#line:2a00010</span>
<span class="yarn-line">[MISSING TRANSLATION: ANTURA is trapped in my model room!]</span> <span class="yarn-meta">#line:2a00011</span>
<span class="yarn-line">[MISSING TRANSLATION: Help me fix the SOLAR SYSTEM model.]</span> <span class="yarn-meta">#line:2a00012</span>
<span class="yarn-line">[MISSING TRANSLATION: I will tell you about each PLANET.]</span> <span class="yarn-meta">#line:2a00013</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: Let's fix it!]</span> <span class="yarn-meta">#line:2a00014</span>
    <span class="yarn-cmd">&lt;&lt;jump PLANETS_ORDER_INTRO&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planets-order-intro"></a>

## PLANETS_ORDER_INTRO

<div class="yarn-node" data-title="PLANETS_ORDER_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- ORDER ACTIVITY ---</span>
<span class="yarn-header-dim">group: Order</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: First — can you put the PLANETS in order?]</span> <span class="yarn-meta">#line:2a00020</span>
<span class="yarn-line">[MISSING TRANSLATION: All 8, from the SUN outward!]</span> <span class="yarn-meta">#line:2a00021</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: I will try!]</span> <span class="yarn-meta">#line:2a00022</span>

</code>
</pre>
</div>

<a id="ys-node-planets-order"></a>

## PLANETS_ORDER

<div class="yarn-node" data-title="PLANETS_ORDER">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Order</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity order planets_order AFTER_PLANETS_ORDER&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-after-planets-order"></a>

## AFTER_PLANETS_ORDER

<div class="yarn-node" data-title="AFTER_PLANETS_ORDER">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Order</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Well done!]</span> <span class="yarn-meta">#line:2a00030</span>
<span class="yarn-cmd">&lt;&lt;card heliocentric_model&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: The SUN is at the center.]</span> <span class="yarn-meta">#line:2a00031</span>
<span class="yarn-line">[MISSING TRANSLATION: Long ago, people put EARTH in the center.]</span> <span class="yarn-meta">#line:2a00032</span>
<span class="yarn-line">[MISSING TRANSLATION: I proved the SUN should be there!]</span> <span class="yarn-meta">#line:2a00033</span>
<span class="yarn-line">[MISSING TRANSLATION: Now let's visit each PLANET and collect its treasures.]</span> <span class="yarn-meta">#line:2a00034</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: Start with MERCURY]</span> <span class="yarn-meta">#line:2a00035</span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_MERCURY_INTRO&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-mercury-intro"></a>

## PLANET_MERCURY_INTRO

<div class="yarn-node" data-title="PLANET_MERCURY_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- MERCURY  (1st — collect 1 sun-heated rock) ---</span>
<span class="yarn-header-dim">group: Mercury</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: planet_mercury</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card mercury&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: MERCURY is the closest PLANET to the SUN.]</span> <span class="yarn-meta">#line:2a00040</span>
<span class="yarn-line">[MISSING TRANSLATION: It has no air and is covered in CRATERS.]</span> <span class="yarn-meta">#line:2a00041</span>
<span class="yarn-line">[MISSING TRANSLATION: The SUN bakes its rocks red-hot!]</span> <span class="yarn-meta">#line:2a00042</span>
<span class="yarn-line">[MISSING TRANSLATION: Find the 1 SUN-HEATED ROCK to add it to the model.]</span> <span class="yarn-meta">#line:2a00043</span>

</code>
</pre>
</div>

<a id="ys-node-planet-mercury-collect"></a>

## PLANET_MERCURY_COLLECT

<div class="yarn-node" data-title="PLANET_MERCURY_COLLECT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Mercury</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas collect_mercury_1 PLANET_MERCURY_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-mercury-done"></a>

## PLANET_MERCURY_DONE

<div class="yarn-node" data-title="PLANET_MERCURY_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Mercury</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Great! MERCURY is now in the model.]</span> <span class="yarn-meta">#line:2a00050</span>
<span class="yarn-cmd">&lt;&lt;set $planets_placed += 1&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: Next: VENUS]</span> <span class="yarn-meta">#line:2a00051</span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_VENUS_INTRO&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-venus-intro"></a>

## PLANET_VENUS_INTRO

<div class="yarn-node" data-title="PLANET_VENUS_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- VENUS  (2nd — collect 2 hot clouds) ---</span>
<span class="yarn-header-dim">group: Venus</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: planet_venus</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card venus&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: VENUS is the second PLANET from the SUN.]</span> <span class="yarn-meta">#line:2a00060</span>
<span class="yarn-line">[MISSING TRANSLATION: It is wrapped in thick, hot CLOUDS.]</span> <span class="yarn-meta">#line:2a00061</span>
<span class="yarn-line">[MISSING TRANSLATION: It is the hottest PLANET of all!]</span> <span class="yarn-meta">#line:2a00062</span>
<span class="yarn-line">[MISSING TRANSLATION: Find 2 HOT CLOUDS to add it to the model.]</span> <span class="yarn-meta">#line:2a00063</span>

</code>
</pre>
</div>

<a id="ys-node-planet-venus-collect"></a>

## PLANET_VENUS_COLLECT

<div class="yarn-node" data-title="PLANET_VENUS_COLLECT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Venus</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas collect_venus_2 PLANET_VENUS_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-venus-done"></a>

## PLANET_VENUS_DONE

<div class="yarn-node" data-title="PLANET_VENUS_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Venus</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Well done! VENUS joins the model.]</span> <span class="yarn-meta">#line:2a00070</span>
<span class="yarn-cmd">&lt;&lt;set $planets_placed += 1&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: Next: EARTH]</span> <span class="yarn-meta">#line:2a00071</span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_EARTH_INTRO&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-earth-intro"></a>

## PLANET_EARTH_INTRO

<div class="yarn-node" data-title="PLANET_EARTH_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- EARTH  (3rd — collect 3 water drops) ---</span>
<span class="yarn-header-dim">group: Earth</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: planet_earth</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card earth&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: EARTH is the third PLANET from the SUN.]</span> <span class="yarn-meta">#line:2a00080</span>
<span class="yarn-line">[MISSING TRANSLATION: It is our home — full of WATER and LIFE.]</span> <span class="yarn-meta">#line:2a00081</span>
<span class="yarn-line">[MISSING TRANSLATION: No other PLANET has OCEANS like ours!]</span> <span class="yarn-meta">#line:2a00082</span>
<span class="yarn-line">[MISSING TRANSLATION: Find 3 WATER DROPS to add it to the model.]</span> <span class="yarn-meta">#line:2a00083</span>

</code>
</pre>
</div>

<a id="ys-node-planet-earth-collect"></a>

## PLANET_EARTH_COLLECT

<div class="yarn-node" data-title="PLANET_EARTH_COLLECT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Earth</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas collect_earth_3 PLANET_EARTH_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-earth-done"></a>

## PLANET_EARTH_DONE

<div class="yarn-node" data-title="PLANET_EARTH_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Earth</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Wonderful! EARTH joins the model.]</span> <span class="yarn-meta">#line:2a00090</span>
<span class="yarn-cmd">&lt;&lt;set $planets_placed += 1&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: Next: MARS]</span> <span class="yarn-meta">#line:2a00091</span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_MARS_INTRO&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-mars-intro"></a>

## PLANET_MARS_INTRO

<div class="yarn-node" data-title="PLANET_MARS_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- MARS  (4th — collect 4 red dust grains) ---</span>
<span class="yarn-header-dim">group: Mars</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: planet_mars</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card mars&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: MARS is the fourth PLANET from the SUN.]</span> <span class="yarn-meta">#line:2a00100</span>
<span class="yarn-line">[MISSING TRANSLATION: Its surface is covered in red DUST and rocks.]</span> <span class="yarn-meta">#line:2a00101</span>
<span class="yarn-line">[MISSING TRANSLATION: It has the tallest VOLCANO in the SOLAR SYSTEM!]</span> <span class="yarn-meta">#line:2a00102</span>
<span class="yarn-line">[MISSING TRANSLATION: Find 4 RED DUST GRAINS to add it to the model.]</span> <span class="yarn-meta">#line:2a00103</span>

</code>
</pre>
</div>

<a id="ys-node-planet-mars-collect"></a>

## PLANET_MARS_COLLECT

<div class="yarn-node" data-title="PLANET_MARS_COLLECT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Mars</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas collect_mars_4 PLANET_MARS_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-mars-done"></a>

## PLANET_MARS_DONE

<div class="yarn-node" data-title="PLANET_MARS_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Mars</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Excellent! MARS joins the model.]</span> <span class="yarn-meta">#line:2a00110</span>
<span class="yarn-cmd">&lt;&lt;set $planets_placed += 1&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: Next: JUPITER]</span> <span class="yarn-meta">#line:2a00111</span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_JUPITER_INTRO&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-jupiter-intro"></a>

## PLANET_JUPITER_INTRO

<div class="yarn-node" data-title="PLANET_JUPITER_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- JUPITER  (5th — collect 5 storm bolts) ---</span>
<span class="yarn-header-dim">group: Jupiter</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: planet_jupiter</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card jupiter&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: JUPITER is the fifth PLANET and the biggest!]</span> <span class="yarn-meta">#line:2a00120</span>
<span class="yarn-line">[MISSING TRANSLATION: It has a giant STORM called the Great Red Spot.]</span> <span class="yarn-meta">#line:2a00121</span>
<span class="yarn-line">[MISSING TRANSLATION: It has at least 95 MOONS!]</span> <span class="yarn-meta">#line:2a00122</span>
<span class="yarn-line">[MISSING TRANSLATION: Find 5 STORM BOLTS to add it to the model.]</span> <span class="yarn-meta">#line:2a00123</span>

</code>
</pre>
</div>

<a id="ys-node-planet-jupiter-collect"></a>

## PLANET_JUPITER_COLLECT

<div class="yarn-node" data-title="PLANET_JUPITER_COLLECT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Jupiter</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas collect_jupiter_5 PLANET_JUPITER_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-jupiter-done"></a>

## PLANET_JUPITER_DONE

<div class="yarn-node" data-title="PLANET_JUPITER_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Jupiter</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Amazing! JUPITER joins the model.]</span> <span class="yarn-meta">#line:2a00130</span>
<span class="yarn-cmd">&lt;&lt;set $planets_placed += 1&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: Next: SATURN]</span> <span class="yarn-meta">#line:2a00131</span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_SATURN_INTRO&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-saturn-intro"></a>

## PLANET_SATURN_INTRO

<div class="yarn-node" data-title="PLANET_SATURN_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- SATURN  (6th — collect 6 ring fragments) ---</span>
<span class="yarn-header-dim">group: Saturn</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: planet_saturn</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card saturn&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: SATURN is the sixth PLANET.]</span> <span class="yarn-meta">#line:2a00140</span>
<span class="yarn-line">[MISSING TRANSLATION: It has beautiful RINGS made of ice and rock.]</span> <span class="yarn-meta">#line:2a00141</span>
<span class="yarn-line">[MISSING TRANSLATION: Its rings stretch thousands of kilometers wide!]</span> <span class="yarn-meta">#line:2a00142</span>
<span class="yarn-line">[MISSING TRANSLATION: Find 6 RING FRAGMENTS to add it to the model.]</span> <span class="yarn-meta">#line:2a00143</span>

</code>
</pre>
</div>

<a id="ys-node-planet-saturn-collect"></a>

## PLANET_SATURN_COLLECT

<div class="yarn-node" data-title="PLANET_SATURN_COLLECT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Saturn</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas collect_saturn_6 PLANET_SATURN_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-saturn-done"></a>

## PLANET_SATURN_DONE

<div class="yarn-node" data-title="PLANET_SATURN_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Saturn</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Beautiful! SATURN joins the model.]</span> <span class="yarn-meta">#line:2a00150</span>
<span class="yarn-cmd">&lt;&lt;set $planets_placed += 1&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: Next: URANUS]</span> <span class="yarn-meta">#line:2a00151</span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_URANUS_INTRO&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-uranus-intro"></a>

## PLANET_URANUS_INTRO

<div class="yarn-node" data-title="PLANET_URANUS_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- URANUS  (7th — collect 7 ice crystals) ---</span>
<span class="yarn-header-dim">group: Uranus</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: planet_uranus</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card uranus&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: URANUS is the seventh PLANET.]</span> <span class="yarn-meta">#line:2a00160</span>
<span class="yarn-line">[MISSING TRANSLATION: It is an icy blue-green GIANT.]</span> <span class="yarn-meta">#line:2a00161</span>
<span class="yarn-line">[MISSING TRANSLATION: It spins on its side — like a rolling ball!]</span> <span class="yarn-meta">#line:2a00162</span>
<span class="yarn-line">[MISSING TRANSLATION: Find 7 ICE CRYSTALS to add it to the model.]</span> <span class="yarn-meta">#line:2a00163</span>

</code>
</pre>
</div>

<a id="ys-node-planet-uranus-collect"></a>

## PLANET_URANUS_COLLECT

<div class="yarn-node" data-title="PLANET_URANUS_COLLECT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Uranus</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas collect_uranus_7 PLANET_URANUS_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-uranus-done"></a>

## PLANET_URANUS_DONE

<div class="yarn-node" data-title="PLANET_URANUS_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Uranus</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Incredible! URANUS joins the model.]</span> <span class="yarn-meta">#line:2a00170</span>
<span class="yarn-cmd">&lt;&lt;set $planets_placed += 1&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: Now for the last PLANET!]</span> <span class="yarn-meta">#line:2a00171</span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_NEPTUNE_INTRO&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-neptune-intro"></a>

## PLANET_NEPTUNE_INTRO

<div class="yarn-node" data-title="PLANET_NEPTUNE_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- NEPTUNE  (8th — memory game, introduced by boatswain) ---</span>
<span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: planet_neptune</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card neptune&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Ah — one more! I am an old SAILOR.]</span> <span class="yarn-meta">#line:2a00300</span>
<span class="yarn-line">[MISSING TRANSLATION: NEPTUNE is the farthest PLANET.]</span> <span class="yarn-meta">#line:2a00301</span>
<span class="yarn-line">[MISSING TRANSLATION: It is deep blue and has the strongest WINDS in the SOLAR SYSTEM.]</span> <span class="yarn-meta">#line:2a00302</span>
<span class="yarn-line">[MISSING TRANSLATION: Its biggest MOON is called TRITON.]</span> <span class="yarn-meta">#line:2a00303</span>
<span class="yarn-line">[MISSING TRANSLATION: Can you match the NEPTUNE facts?]</span> <span class="yarn-meta">#line:2a00304</span>

</code>
</pre>
</div>

<a id="ys-node-planet-neptune-memory"></a>

## PLANET_NEPTUNE_MEMORY

<div class="yarn-node" data-title="PLANET_NEPTUNE_MEMORY">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity memory neptune_facts PLANET_NEPTUNE_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-neptune-done"></a>

## PLANET_NEPTUNE_DONE

<div class="yarn-node" data-title="PLANET_NEPTUNE_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Well done! NEPTUNE joins the model.]</span> <span class="yarn-meta">#line:2a00310</span>
<span class="yarn-cmd">&lt;&lt;set $planets_placed += 1&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: Now the model is complete!]</span> <span class="yarn-meta">#line:2a00311</span>
    <span class="yarn-cmd">&lt;&lt;jump ALL_PLANETS_PLACED&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-all-planets-placed"></a>

## ALL_PLANETS_PLACED

<div class="yarn-node" data-title="ALL_PLANETS_PLACED">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- MODEL COMPLETE ---</span>
<span class="yarn-header-dim">group: Finale</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: solar_model_finale</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: You placed all 8 PLANETS in the model!]</span> <span class="yarn-meta">#line:2a00180</span>
<span class="yarn-line">[MISSING TRANSLATION: Now the SOLAR SYSTEM is complete.]</span> <span class="yarn-meta">#line:2a00181</span>
<span class="yarn-cmd">&lt;&lt;card heliocentric_model&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: The SUN is in the center — just as I discovered!]</span> <span class="yarn-meta">#line:2a00182</span>
<span class="yarn-line">[MISSING TRANSLATION: Now let's rescue ANTURA!]</span> <span class="yarn-meta">#line:2a00183</span>

</code>
</pre>
</div>

<a id="ys-node-rescue-antura"></a>

## RESCUE_ANTURA

<div class="yarn-node" data-title="RESCUE_ANTURA">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- RESCUE ANTURA ---</span>
<span class="yarn-header-dim">group: Finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: antura_rescued</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: ANTURA is free!]</span> <span class="yarn-meta">#line:2a00190</span>
<span class="yarn-line">[MISSING TRANSLATION: He was hiding behind NEPTUNE's model!]</span> <span class="yarn-meta">#line:2a00191</span>

</code>
</pre>
</div>

<a id="ys-node-copernicus-farewell"></a>

## COPERNICUS_FAREWELL

<div class="yarn-node" data-title="COPERNICUS_FAREWELL">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Finale</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Thank you for your help!]</span> <span class="yarn-meta">#line:2a00200</span>
<span class="yarn-line">[MISSING TRANSLATION: Remember: the SUN is at the center.]</span> <span class="yarn-meta">#line:2a00201</span>
<span class="yarn-line">[MISSING TRANSLATION: EARTH is the third PLANET.]</span> <span class="yarn-meta">#line:2a00202</span>
<span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz"></a>

## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- FINAL QUIZ ---</span>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Let's test what you learned!]</span> <span class="yarn-meta">#line:2a00210</span>
<span class="yarn-cmd">&lt;&lt;jump QUIZ_Q1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quiz-q1"></a>

## QUIZ_Q1

<div class="yarn-node" data-title="QUIZ_Q1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card solar_system&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: What is at the center of the SOLAR SYSTEM?]</span> <span class="yarn-meta">#line:2a00220</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: The SUN]</span> <span class="yarn-meta">#line:2a00221</span>
    <span class="yarn-cmd">&lt;&lt;jump QUIZ_Q2&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: The EARTH]</span> <span class="yarn-meta">#line:2a00222</span>
<span class="yarn-line">    [MISSING TRANSLATION:     No — the SUN is in the center!]</span> <span class="yarn-meta">#line:2a00223</span>
    <span class="yarn-cmd">&lt;&lt;jump QUIZ_Q1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quiz-q2"></a>

## QUIZ_Q2

<div class="yarn-node" data-title="QUIZ_Q2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card mercury&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Which PLANET is closest to the SUN?]</span> <span class="yarn-meta">#line:2a00230</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: MERCURY]</span> <span class="yarn-meta">#line:2a00231</span>
    <span class="yarn-cmd">&lt;&lt;jump QUIZ_Q3&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: VENUS]</span> <span class="yarn-meta">#line:2a00232</span>
<span class="yarn-line">    [MISSING TRANSLATION:     VENUS is second — MERCURY is first!]</span> <span class="yarn-meta">#line:2a00233</span>
    <span class="yarn-cmd">&lt;&lt;jump QUIZ_Q2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quiz-q3"></a>

## QUIZ_Q3

<div class="yarn-node" data-title="QUIZ_Q3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card saturn&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Which PLANET has beautiful RINGS?]</span> <span class="yarn-meta">#line:2a00240</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: SATURN]</span> <span class="yarn-meta">#line:2a00241</span>
    <span class="yarn-cmd">&lt;&lt;jump QUEST_COMPLETE&gt;&gt;</span>
<span class="yarn-choice">-&gt; [MISSING TRANSLATION: JUPITER]</span> <span class="yarn-meta">#line:2a00242</span>
<span class="yarn-line">    [MISSING TRANSLATION:     JUPITER has storms — SATURN has RINGS!]</span> <span class="yarn-meta">#line:2a00243</span>
    <span class="yarn-cmd">&lt;&lt;jump QUIZ_Q3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-complete"></a>

## QUEST_COMPLETE

<div class="yarn-node" data-title="QUEST_COMPLETE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: You know the SOLAR SYSTEM!]</span> <span class="yarn-meta">#line:2a00250</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-child"></a>

## spawned_child

<div class="yarn-node" data-title="spawned_child">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// --- SPAWNED NPCs ---</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; SATURN has rings around it.]</span> <span class="yarn-meta">#line:2a00260</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; NEPTUNE is the farthest PLANET.]</span> <span class="yarn-meta">#line:2a00262</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-student"></a>

## spawned_student

<div class="yarn-node" data-title="spawned_student">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; MERCURY is closest to the SUN.]</span> <span class="yarn-meta">#line:2a00270</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; EARTH is the third PLANET.]</span> <span class="yarn-meta">#line:2a00271</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-local"></a>

## spawned_local

<div class="yarn-node" data-title="spawned_local">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; NEPTUNE has the strongest WINDS.]</span> <span class="yarn-meta">#line:2a00280</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; MARS is red and dusty.]</span> <span class="yarn-meta">#line:2a00281</span>

</code>
</pre>
</div>


