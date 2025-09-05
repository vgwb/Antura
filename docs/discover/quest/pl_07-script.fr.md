---
title: Copernic et le système solaire (pl_07) - Script
hide:
---

# Copernic et le système solaire (pl_07) - Script
[Quest Index](./index.fr.md) - Language: [english](./pl_07-script.md) - french - [polish](./pl_07-script.pl.md) - [italian](./pl_07-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-07-copernicus-and-the-solar-system/38/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=783699917#gid=783699917)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_07%20Solar%20System/PL_07%20Solar%20System%20-%20Yarn%20Script.yarn)  

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">// PL_07 - Nicolaus Copernicus &amp; Solar System</span>
<span class="yarn-header-dim">// Location: Toruń, Poland - Copernicus House, Museum, and Planetarium</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - NicolausCopernicus (scientific figure)</span>
<span class="yarn-header-dim">// - planet_mercury (planetary education)</span>
<span class="yarn-header-dim">// - planet_venus (planetary education)</span>
<span class="yarn-header-dim">// - planet_earth (planetary education)</span>
<span class="yarn-header-dim">// - planet_mars (planetary education)</span>
<span class="yarn-header-dim">// - planet_jupiter (planetary education)</span>
<span class="yarn-header-dim">// - planet_saturn (planetary education)</span>
<span class="yarn-header-dim">// - planet_uranus (planetary education)</span>
<span class="yarn-header-dim">// - planet_neptune (planetary education)</span>
<span class="yarn-header-dim">// - telescope (scientific instrument)</span>
<span class="yarn-header-dim">// - planetarium (educational facility)</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - Fix solar system map with correct planetary order</span>
<span class="yarn-header-dim">// - Clean and learn about telescope parts</span>
<span class="yarn-header-dim">// - Visit each planet in order with individual activities</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - order planets_order (arrange 8 planets from Sun)</span>
<span class="yarn-header-dim">// - piano planet_order_song (musical mnemonic for planet order)</span>
<span class="yarn-header-dim">// - cleancanvas telescope_lens_clean (clean telescope lens)</span>
<span class="yarn-header-dim">// - memory telescope_parts (match telescope components)</span>
<span class="yarn-header-dim">// - jigsawpuzzle planet_mercury through planet_neptune (8 individual planet jigsaws)</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Words used: POLAND, TORUŃ, COPERNICUS, SUN, PLANET, SOLAR SYSTEM, TELESCOPE, LENS, PLANETARIUM, MERCURY, VENUS, EARTH, MARS, JUPITER, SATURN, URANUS, NEPTUNE, MOONS, CENTER, ASTRONOMY, SCIENTIST</span>
<span class="yarn-header-dim">// </span>
</code></pre></div>

<a id="ys-node-init"></a>
## Init

<div class="yarn-node" data-title="Init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: torun_street</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $planets_found = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $planets_needed = 8&gt;&gt;</span>
<span class="yarn-line">Welcome to TORUŃ in POLAND. <span class="yarn-meta">#line:0673aff </span></span>
<span class="yarn-line">COPERNICUS was born here. <span class="yarn-meta">#line:00fe710 </span></span>
<span class="yarn-line">ANTURA is stuck in his house! <span class="yarn-meta">#line:00589bc </span></span>
<span class="yarn-line">Let's ask COPERNICUS for help. <span class="yarn-meta">#line:031963d </span></span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// MEET COPERNICUS</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-copernicus-outside"></a>
## COPERNICUS_OUTSIDE

<div class="yarn-node" data-title="COPERNICUS_OUTSIDE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Copernicus</span>
<span class="yarn-header-dim">image: copernicus_house_front</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hello! I am NICOLAUS COPERNICUS. <span class="yarn-meta">#line:0c5050d </span></span>
<span class="yarn-line">I can help you rescue ANTURA. <span class="yarn-meta">#line:0a41815 </span></span>
<span class="yarn-line">First, fix my SOLAR SYSTEM map. <span class="yarn-meta">#line:0f17151 </span></span>
<span class="yarn-line">-&gt; Help with planets <span class="yarn-meta">#line:07bdf91 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump PLANETS_ORDER&gt;&gt;</span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// ACTIVITY – ORDER THE PLANETS</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-planets-order"></a>
## PLANETS_ORDER

<div class="yarn-node" data-title="PLANETS_ORDER"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity order planets_order tutorial&gt;&gt;</span>
<span class="yarn-line">-&gt; Done <span class="yarn-meta">#line:06c0d36 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump AFTER_PLANETS_ORDER&gt;&gt;</span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// AFTER ORDERING – HELIOCENTRIC IDEA</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-after-planets-order"></a>
## AFTER_PLANETS_ORDER

<div class="yarn-node" data-title="AFTER_PLANETS_ORDER"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Copernicus</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done! <span class="yarn-meta">#line:06ea147 </span></span>
<span class="yarn-line">Long ago, people put EARTH in the center. <span class="yarn-meta">#line:0af451b </span></span>
<span class="yarn-line">I found the SUN is in the center. <span class="yarn-meta">#line:0392dfd </span></span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// MUSEUM – CENTER MODEL</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-inside-museum"></a>
## INSIDE_MUSEUM

<div class="yarn-node" data-title="INSIDE_MUSEUM"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: museum_hall</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Here is a 3D SOLAR SYSTEM. <span class="yarn-meta">#line:0069f70 </span></span>
<span class="yarn-line">It spins in the hall. <span class="yarn-meta">#line:04de41d </span></span>
<span class="yarn-line">Find PLANETS to add them here. <span class="yarn-meta">#line:01e7dda </span></span>
<span class="yarn-line">Clean the TELESCOPE <span class="yarn-meta">#line:0c22e2c </span></span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// ACTIVITY – CLEAN THE TELESCOPE LENS</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-telescope-dirty"></a>
## TELESCOPE_DIRTY

<div class="yarn-node" data-title="TELESCOPE_DIRTY"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: telescope_lens_dirty</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas telescope_lens_clean tutorial&gt;&gt;</span>
<span class="yarn-line">-&gt; Learn the parts <span class="yarn-meta">#line:0e91a5e </span></span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// TELESCOPE – PARTS &amp; PLANETARIUM</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-telescope-explain"></a>
## TELESCOPE_EXPLAIN

<div class="yarn-node" data-title="TELESCOPE_EXPLAIN"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">tags: actor=Copernicus</span>
<span class="yarn-header-dim">image: telescope_display</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A TELESCOPE helps us see far. <span class="yarn-meta">#line:0314ed5 </span></span>
<span class="yarn-line">It has a LENS and an EYEPIECE. <span class="yarn-meta">#line:062693e </span></span>
<span class="yarn-line">A PLANETARIUM shows the sky indoors. <span class="yarn-meta">#line:0cab30b </span></span>
<span class="yarn-line">-&gt; Match the parts <span class="yarn-meta">#line:015eb34 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump TELESCOPE_PARTS&gt;&gt;</span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// ACTIVITY – MATCH TELESCOPE PARTS</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-telescope-parts"></a>
## TELESCOPE_PARTS

<div class="yarn-node" data-title="TELESCOPE_PARTS"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity memory telescope_parts tutorial&gt;&gt;</span>
<span class="yarn-line">-&gt; Piano memory <span class="yarn-meta">#line:0db50a7 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump PIANO_INTRO&gt;&gt;</span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PIANO – PLANET ORDER SONG</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-piano-intro"></a>
## PIANO_INTRO

<div class="yarn-node" data-title="PIANO_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">tags: actor=Copernicus</span>
<span class="yarn-header-dim">image: planet_piano_card</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Music helps memory. <span class="yarn-meta">#line:08d3111 </span></span>
<span class="yarn-line">Play the PLANET order tune. <span class="yarn-meta">#line:0471676 </span></span>
<span class="yarn-line">-&gt; Play <span class="yarn-meta">#line:0f08a91 </span></span>


</code></pre></div>

<a id="ys-node-piano-play"></a>
## PIANO_PLAY

<div class="yarn-node" data-title="PIANO_PLAY"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity piano planet_order_song tutorial&gt;&gt;</span>
<span class="yarn-line">-&gt; Find PLANETS <span class="yarn-meta">#line:013de52 </span></span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// DISCOVER THE PLANETS</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-find-planets"></a>
## FIND_PLANETS

<div class="yarn-node" data-title="FIND_PLANETS"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: solar_model_center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's visit each PLANET. <span class="yarn-meta">#line:0b015a8 </span></span>
<span class="yarn-line">They appear in the model when found. <span class="yarn-meta">#line:00d6fdf </span></span>
<span class="yarn-line">-&gt; Start with MERCURY <span class="yarn-meta">#line:04ba61d </span></span>

<span class="yarn-comment">// MERCURY</span>

</code></pre></div>

<a id="ys-node-planet-mercury"></a>
## PLANET_MERCURY

<div class="yarn-node" data-title="PLANET_MERCURY"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: planet_mercury</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">MERCURY is closest to the SUN. <span class="yarn-meta">#line:0235ac2 </span></span>
<span class="yarn-line">It goes around very fast. <span class="yarn-meta">#line:03c1c86 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_mercury tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Mercury to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>

<span class="yarn-comment">// VENUS</span>

</code></pre></div>

<a id="ys-node-planet-venus"></a>
## PLANET_VENUS

<div class="yarn-node" data-title="PLANET_VENUS"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: planet_venus</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">VENUS is very hot. <span class="yarn-meta">#line:0fbc89d </span></span>
<span class="yarn-line">Thick clouds cover it. <span class="yarn-meta">#line:0244228 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_venus tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Venus to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>

<span class="yarn-comment">// EARTH</span>

</code></pre></div>

<a id="ys-node-planet-earth"></a>
## PLANET_EARTH

<div class="yarn-node" data-title="PLANET_EARTH"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: planet_earth</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">EARTH is our home. <span class="yarn-meta">#line:01002c7 </span></span>
<span class="yarn-line">It has land, air, and seas. <span class="yarn-meta">#line:032ae50 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_earth tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Earth to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>


<span class="yarn-comment">// MARS</span>

</code></pre></div>

<a id="ys-node-planet-mars"></a>
## PLANET_MARS

<div class="yarn-node" data-title="PLANET_MARS"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: planet_mars</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">MARS is red and dusty. <span class="yarn-meta">#line:007f580 </span></span>
<span class="yarn-line">It has big volcanoes. <span class="yarn-meta">#line:0bc9c83 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_mars tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Mars to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>
<span class="yarn-line">-&gt; Next: JUPITER <span class="yarn-meta">#line:08af681 </span></span>


<span class="yarn-comment">// JUPITER</span>

</code></pre></div>

<a id="ys-node-planet-jupiter"></a>
## PLANET_JUPITER

<div class="yarn-node" data-title="PLANET_JUPITER"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: planet_jupiter</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">JUPITER is the biggest PLANET. <span class="yarn-meta">#line:06e72b3 </span></span>
<span class="yarn-line">It has a Great Red Spot. <span class="yarn-meta">#line:094d7dd </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_jupiter tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Jupiter to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>
<span class="yarn-line">-&gt; Next: SATURN <span class="yarn-meta">#line:07229a6 </span></span>


<span class="yarn-comment">// SATURN</span>

</code></pre></div>

<a id="ys-node-planet-saturn"></a>
## PLANET_SATURN

<div class="yarn-node" data-title="PLANET_SATURN"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: planet_saturn</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">SATURN has bright RINGS. <span class="yarn-meta">#line:0e6de7d </span></span>
<span class="yarn-line">Many MOONS orbit it. <span class="yarn-meta">#line:00a70f4 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_saturn tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Saturn to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>
<span class="yarn-line">-&gt; Next: URANUS <span class="yarn-meta">#line:09ba1f7 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_URANUS&gt;&gt;</span>

<span class="yarn-comment">// URANUS</span>

</code></pre></div>

<a id="ys-node-planet-uranus"></a>
## PLANET_URANUS

<div class="yarn-node" data-title="PLANET_URANUS"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: planet_uranus</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">URANUS spins on its side. <span class="yarn-meta">#line:04df973 </span></span>
<span class="yarn-line">It looks blue‑green. <span class="yarn-meta">#line:01e724d </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_uranus tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Uranus to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>


<span class="yarn-comment">// NEPTUNE</span>

</code></pre></div>

<a id="ys-node-planet-neptune"></a>
## PLANET_NEPTUNE

<div class="yarn-node" data-title="PLANET_NEPTUNE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: planet_neptune</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">NEPTUNE is far and windy. <span class="yarn-meta">#line:0191651 </span></span>
<span class="yarn-line">It is deep blue. <span class="yarn-meta">#line:0584147 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_neptune tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Neptune to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;jump ALL_PLANETS_FOUND&gt;&gt;</span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// ALL PLANETS FOUND – MOVE TO RESCUE</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-all-planets-found"></a>
## ALL_PLANETS_FOUND

<div class="yarn-node" data-title="ALL_PLANETS_FOUND"><pre class="yarn-code"><code><span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags: actor=Copernicus</span>
<span class="yarn-header-dim">image: solar_model_finale</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You found all 8 PLANETS. <span class="yarn-meta">#line:0d91787 </span></span>
<span class="yarn-line">The model is complete. <span class="yarn-meta">#line:0d9ce49 </span></span>
<span class="yarn-line">-&gt; Rescue ANTURA <span class="yarn-meta">#line:0896149 </span></span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// RESCUE ANTURA – SCENE</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-rescue-antura-guide"></a>
## RESCUE_ANTURA_GUIDE

<div class="yarn-node" data-title="RESCUE_ANTURA_GUIDE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Finale</span>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">image: antura_trail</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">ANTURA is free! <span class="yarn-meta">#line:0294715 </span></span>


</code></pre></div>

<a id="ys-node-rescue-antura-cop"></a>
## RESCUE_ANTURA_COP

<div class="yarn-node" data-title="RESCUE_ANTURA_COP"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=Copernicus</span>
<span class="yarn-header-dim">image: antura_rescued</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Thank you for your help. <span class="yarn-meta">#line:0d7e5be </span></span>
<span class="yarn-line">"Remember: the SUN is in the center."" <span class="yarn-meta">#line:0b0e4bf </span></span>
<span class="yarn-line">-&gt; Final Quiz <span class="yarn-meta">#line:0a535da </span></span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ&gt;&gt;</span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// FINAL QUIZ – ALWAYS LAST</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-final-quiz"></a>
## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: quiz_solar_system</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Answer the questions. <span class="yarn-meta">#line:08bea58 </span></span>
<span class="yarn-line">Tap the best choice. <span class="yarn-meta">#line:082b5ff </span></span>
<span class="yarn-cmd">&lt;&lt;activity quiz copernicus_basics tutorial&gt;&gt;</span>
<span class="yarn-comment">// Suggested questions in preset:</span>
<span class="yarn-comment">// 1) Who is in the center of the SOLAR SYSTEM? a) EARTH  b) SUN ✅  c) MOON</span>
<span class="yarn-comment">// 2) How many PLANETS are there?             a) 7  b) 8 ✅  c) 9</span>
<span class="yarn-comment">// 3) COPERNICUS was from which country?       a) POLAND ✅  b) ITALY  c) FRANCE</span>

<span class="yarn-cmd">&lt;&lt;jump QUEST_COMPLETE&gt;&gt;</span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// END</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-quest-complete"></a>
## QUEST_COMPLETE

<div class="yarn-node" data-title="QUEST_COMPLETE"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: quest_complete</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You helped COPERNICUS. <span class="yarn-meta">#line:0be7346 </span></span>
<span class="yarn-line">The SOLAR SYSTEM is complete! <span class="yarn-meta">#line:0410bab </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>

</code></pre></div>


