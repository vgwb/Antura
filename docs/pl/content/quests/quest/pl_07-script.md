---
title: Kopernik i układ słoneczny (pl_07) - Script
hide:
---

# Kopernik i układ słoneczny (pl_07) - Script
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
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - Fix solar system map with correct planetary order</span>
<span class="yarn-header-dim">// - Clean and learn about telescope parts</span>
<span class="yarn-header-dim">// - Visit each planet in order with individual activities</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - order planets_order (arrange 8 planets from Sun)</span>
<span class="yarn-header-dim">// - piano planet_order_song (musical mnemonic for planet order)</span>
<span class="yarn-header-dim">// - cleancanvas telescope_lens_clean (clean telescope lens)</span>
<span class="yarn-header-dim">// - memory telescope_parts (match telescope components)</span>
<span class="yarn-header-dim">// - jigsawpuzzle planet_mercury through planet_neptune (8 individual planet jigsaws)</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">image: torun_street</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $planets_found = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $planets_needed = 8&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card torun&gt;&gt;</span>
<span class="yarn-line">Witamy w TORUNIU w POLSCE.</span> <span class="yarn-meta">#line:0673aff </span>
<span class="yarn-cmd">&lt;&lt;card nicolaus_copernicus&gt;&gt;</span>
<span class="yarn-line">Tutaj urodził się KOPERNIK.</span> <span class="yarn-meta">#line:00fe710 </span>
<span class="yarn-cmd">&lt;&lt;card nicolaus_copernicus_house&gt;&gt;</span>
<span class="yarn-line">ANTURA utknął w swoim domu!</span> <span class="yarn-meta">#line:00589bc </span>
<span class="yarn-line">Poprośmy KOPERNIKA o pomoc.</span> <span class="yarn-meta">#line:031963d </span>

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
<span class="yarn-line">Świetna robota w tym zadaniu.</span> <span class="yarn-meta">#line:0a0a2ce </span>
<span class="yarn-cmd">&lt;&lt;card solar_system&gt;&gt;</span>
<span class="yarn-line">Dzisiaj poznaliśmy UKŁAD SŁONECZNY.</span> <span class="yarn-meta">#line:0cce3a4 </span>

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
<span class="yarn-line">Narysuj 8 PLANET ze SŁOŃCEM w środku.</span> <span class="yarn-meta">#line:064c54e </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-copernicus-outside"></a>

## COPERNICUS_OUTSIDE

<div class="yarn-node" data-title="COPERNICUS_OUTSIDE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// LEt'S MEET COPERNICUS</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action card nicolaus_copernicus_house&gt;&gt;</span>
<span class="yarn-line">Cześć! Jestem MIKOŁAJ KOPERNIK.</span> <span class="yarn-meta">#line:0c5050d </span>
<span class="yarn-line">Mogę pomóc ci uratować ANTURĘ.</span> <span class="yarn-meta">#line:0a41815 </span>
<span class="yarn-line">Najpierw muszę naprawić mapę Układu Słonecznego.</span> <span class="yarn-meta">#line:0f17151 </span>
<span class="yarn-line">Pomoc z planetami</span> <span class="yarn-meta">#line:07bdf91 </span>

</code>
</pre>
</div>

<a id="ys-node-planets-order"></a>

## PLANETS_ORDER

<div class="yarn-node" data-title="PLANETS_ORDER">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// ACTIVITY – ORDER THE PLANETS</span>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity order planets_order AFTER_PLANETS_ORDER&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-after-planets-order"></a>

## AFTER_PLANETS_ORDER

<div class="yarn-node" data-title="AFTER_PLANETS_ORDER">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dobrze zrobiony!</span> <span class="yarn-meta">#line:06ea147 </span>
<span class="yarn-line">Dawno temu ludzie umieścili ZIEMIĘ w centrum.</span> <span class="yarn-meta">#line:0af451b </span>
<span class="yarn-cmd">&lt;&lt;card heliocentric_model&gt;&gt;</span>
<span class="yarn-line">Odkryłem, że SŁOŃCE jest w centrum.</span> <span class="yarn-meta">#line:0392dfd </span>

</code>
</pre>
</div>

<a id="ys-node-inside-museum"></a>

## INSIDE_MUSEUM

<div class="yarn-node" data-title="INSIDE_MUSEUM">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: museum_hall</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card solar_system&gt;&gt;</span>
<span class="yarn-line">Oto trójwymiarowy układ słoneczny.</span> <span class="yarn-meta">#line:0069f70 </span>
<span class="yarn-line">Kręci się w holu.</span> <span class="yarn-meta">#line:04de41d </span>
<span class="yarn-line">Znajdź PLANETY i dodaj je tutaj.</span> <span class="yarn-meta">#line:01e7dda </span>
<span class="yarn-line">Wyczyść TELESKOP.</span> <span class="yarn-meta">#line:0c22e2c </span>


</code>
</pre>
</div>

<a id="ys-node-telescope-dirty"></a>

## TELESCOPE_DIRTY

<div class="yarn-node" data-title="TELESCOPE_DIRTY">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// ACTIVITY – CLEAN THE TELESCOPE LENS</span>
<span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">image: telescope_lens_dirty</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas telescope_lens_clean tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-telescope-lens-clean-done"></a>

## telescope_lens_clean_done

<div class="yarn-node" data-title="telescope_lens_clean_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetna robota przy czyszczeniu soczewki!</span> <span class="yarn-meta">#line:06ba2c4 </span>
<span class="yarn-line">Teraz możemy lepiej zobaczyć planety.</span> <span class="yarn-meta">#line:0dd17cf </span>

</code>
</pre>
</div>

<a id="ys-node-telescope-explain"></a>

## TELESCOPE_EXPLAIN

<div class="yarn-node" data-title="TELESCOPE_EXPLAIN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card telescope&gt;&gt;</span>
<span class="yarn-line">TELESKOP pomaga nam widzieć daleko.</span> <span class="yarn-meta">#line:0314ed5 </span>
<span class="yarn-cmd">&lt;&lt;card lens&gt;&gt;</span>
<span class="yarn-line">Posiada SOCZEWKĘ i OKULAR.</span> <span class="yarn-meta">#line:062693e </span>
<span class="yarn-cmd">&lt;&lt;card planetarium&gt;&gt;</span>
<span class="yarn-line">PLANETARIUM pozwala zobaczyć niebo wewnątrz pomieszczeń.</span> <span class="yarn-meta">#line:0cab30b </span>

</code>
</pre>
</div>

<a id="ys-node-telescope-parts"></a>

## TELESCOPE_PARTS

<div class="yarn-node" data-title="TELESCOPE_PARTS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity memory telescope_parts tutorial&gt;&gt;</span>
<span class="yarn-line">Pamięć fortepianowa</span> <span class="yarn-meta">#line:0db50a7 </span>
    <span class="yarn-cmd">&lt;&lt;jump PIANO_INTRO&gt;&gt;</span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PIANO – PLANET ORDER SONG</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code>
</pre>
</div>

<a id="ys-node-piano-intro"></a>

## PIANO_INTRO

<div class="yarn-node" data-title="PIANO_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: planet_piano_card</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Muzyka wspomaga pamięć.</span> <span class="yarn-meta">#line:08d3111 </span>
<span class="yarn-line">Zagraj melodię PLANET.</span> <span class="yarn-meta">#line:0471676 </span>
<span class="yarn-line">Grać</span> <span class="yarn-meta">#line:0f08a91 </span>


</code>
</pre>
</div>

<a id="ys-node-piano-play"></a>

## PIANO_PLAY

<div class="yarn-node" data-title="PIANO_PLAY">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Museum</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity piano planet_order_song tutorial&gt;&gt;</span>
<span class="yarn-line">Znajdź PLANETY</span> <span class="yarn-meta">#line:013de52 </span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// DISCOVER THE PLANETS</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code>
</pre>
</div>

<a id="ys-node-find-planets"></a>

## FIND_PLANETS

<div class="yarn-node" data-title="FIND_PLANETS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: solar_model_center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card astronomy&gt;&gt;</span>
<span class="yarn-line">Odwiedźmy każdą PLANETĘ.</span> <span class="yarn-meta">#line:0b015a8 </span>
<span class="yarn-line">Pojawiają się w modelu po ich znalezieniu.</span> <span class="yarn-meta">#line:00d6fdf </span>
<span class="yarn-line">Zacznij od MERCURY</span> <span class="yarn-meta">#line:04ba61d </span>

<span class="yarn-comment">// MERCURY</span>

</code>
</pre>
</div>

<a id="ys-node-planet-mercury"></a>

## PLANET_MERCURY

<div class="yarn-node" data-title="PLANET_MERCURY">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: planet_mercury</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card mercury&gt;&gt;</span>
<span class="yarn-line">MERKURY znajduje się najbliżej SŁOŃCA.</span> <span class="yarn-meta">#line:0235ac2 </span>
<span class="yarn-line">Rozchodzi się bardzo szybko.</span> <span class="yarn-meta">#line:03c1c86 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_mercury tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Mercury to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>

<span class="yarn-comment">// VENUS</span>

</code>
</pre>
</div>

<a id="ys-node-planet-venus"></a>

## PLANET_VENUS

<div class="yarn-node" data-title="PLANET_VENUS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: planet_venus</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card venus&gt;&gt;</span>
<span class="yarn-line">WENUS jest bardzo gorąca.</span> <span class="yarn-meta">#line:0fbc89d </span>
<span class="yarn-line">Gęste chmury pokrywają to miejsce.</span> <span class="yarn-meta">#line:0244228 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_venus tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Venus to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>

<span class="yarn-comment">// EARTH</span>

</code>
</pre>
</div>

<a id="ys-node-planet-earth"></a>

## PLANET_EARTH

<div class="yarn-node" data-title="PLANET_EARTH">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: planet_earth</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card earth&gt;&gt;</span>
<span class="yarn-line">ZIEMIA jest naszym domem.</span> <span class="yarn-meta">#line:01002c7 </span>
<span class="yarn-line">Ma ląd, powietrze i morze.</span> <span class="yarn-meta">#line:032ae50 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_earth tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Earth to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>


<span class="yarn-comment">// MARS</span>

</code>
</pre>
</div>

<a id="ys-node-planet-mars"></a>

## PLANET_MARS

<div class="yarn-node" data-title="PLANET_MARS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: planet_mars</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card mars&gt;&gt;</span>
<span class="yarn-line">MARS jest czerwony i zakurzony.</span> <span class="yarn-meta">#line:007f580 </span>
<span class="yarn-line">Są tam duże wulkany.</span> <span class="yarn-meta">#line:0bc9c83 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_mars tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Mars to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-planet-jupiter"></a>

## PLANET_JUPITER

<div class="yarn-node" data-title="PLANET_JUPITER">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// JUPITER</span>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: planet_jupiter</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card jupiter&gt;&gt;</span>
<span class="yarn-line">JOWISZ jest największą PLANETĄ.</span> <span class="yarn-meta">#line:06e72b3 </span>
<span class="yarn-line">Posiada Wielką Czerwoną Plamę.</span> <span class="yarn-meta">#line:094d7dd </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_jupiter tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Jupiter to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>
<span class="yarn-line">Następny jest SATURN</span> <span class="yarn-meta">#line:07229a6 </span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_SATURN&gt;&gt;</span>

<span class="yarn-comment">// SATURN</span>

</code>
</pre>
</div>

<a id="ys-node-planet-saturn"></a>

## PLANET_SATURN

<div class="yarn-node" data-title="PLANET_SATURN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: planet_saturn</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card saturn&gt;&gt;</span>
<span class="yarn-line">SATURN ma jasne PIERŚCIENIE.</span> <span class="yarn-meta">#line:0e6de7d </span>
<span class="yarn-line">Wiele KSIĘŻYCÓW krąży wokół niego.</span> <span class="yarn-meta">#line:00a70f4 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_saturn tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Saturn to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>
<span class="yarn-line">Następny jest URAN</span> <span class="yarn-meta">#line:09ba1f7 </span>
    <span class="yarn-cmd">&lt;&lt;jump PLANET_URANUS&gt;&gt;</span>

<span class="yarn-comment">// URANUS</span>

</code>
</pre>
</div>

<a id="ys-node-planet-uranus"></a>

## PLANET_URANUS

<div class="yarn-node" data-title="PLANET_URANUS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: planet_uranus</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card uranus&gt;&gt;</span>
<span class="yarn-line">URAN obraca się na boku.</span> <span class="yarn-meta">#line:04df973 </span>
<span class="yarn-line">Wygląda na niebiesko-zielone.</span> <span class="yarn-meta">#line:01e724d </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_uranus tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Uranus to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>


<span class="yarn-comment">// NEPTUNE</span>

</code>
</pre>
</div>

<a id="ys-node-planet-neptune"></a>

## PLANET_NEPTUNE

<div class="yarn-node" data-title="PLANET_NEPTUNE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: planet_neptune</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card neptune&gt;&gt;</span>
<span class="yarn-line">NEPTUN jest daleko i wietrznie.</span> <span class="yarn-meta">#line:0191651 </span>
<span class="yarn-line">Jest głęboko niebieski.</span> <span class="yarn-meta">#line:0584147 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle planet_neptune tutorial&gt;&gt;</span>
<span class="yarn-comment">// Add Neptune to the model (handled by activity)</span>
<span class="yarn-cmd">&lt;&lt;set $planets_found += 1&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;jump ALL_PLANETS_FOUND&gt;&gt;</span>

<span class="yarn-comment">// ALL PLANETS FOUND</span>

</code>
</pre>
</div>

<a id="ys-node-all-planets-found"></a>

## ALL_PLANETS_FOUND

<div class="yarn-node" data-title="ALL_PLANETS_FOUND">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SolarSystem</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: solar_model_finale</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znaleziono wszystkie 8 PLANET.</span> <span class="yarn-meta">#line:0d91787 </span>
<span class="yarn-line">Model jest ukończony.</span> <span class="yarn-meta">#line:0d9ce49 </span>
<span class="yarn-line">Teraz ratuj ANTURĘ!</span> <span class="yarn-meta">#line:0896149 </span>

</code>
</pre>
</div>

<a id="ys-node-rescue-antura-guide"></a>

## RESCUE_ANTURA_GUIDE

<div class="yarn-node" data-title="RESCUE_ANTURA_GUIDE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">image: antura_trail</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">ANTURA jest darmowa!</span> <span class="yarn-meta">#line:0294715 </span>


</code>
</pre>
</div>

<a id="ys-node-rescue-antura-cop"></a>

## RESCUE_ANTURA_COP

<div class="yarn-node" data-title="RESCUE_ANTURA_COP">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: antura_rescued</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dziękuję za pomoc.</span> <span class="yarn-meta">#line:0d7e5be </span>
<span class="yarn-line">Pamiętaj, że SŁOŃCE jest w centrum.</span> <span class="yarn-meta">#line:0b0e4bf </span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz"></a>

## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">image: quiz_solar_system</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Odpowiedz na pytania.</span> <span class="yarn-meta">#line:08bea58 </span>
<span class="yarn-cmd">&lt;&lt;activity quiz copernicus_basics tutorial&gt;&gt;</span>
<span class="yarn-comment">// Suggested questions in preset:</span>
<span class="yarn-comment">// 1) Who is in the center of the SOLAR SYSTEM? a) EARTH  b) SUN ✅  c) MOON</span>
<span class="yarn-comment">// 2) How many PLANETS are there?             a) 7  b) 8 ✅  c) 9</span>
<span class="yarn-comment">// 3) COPERNICUS was from which country?       a) POLAND ✅  b) ITALY  c) FRANCE</span>

<span class="yarn-cmd">&lt;&lt;jump QUEST_COMPLETE&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-quest-complete"></a>

## QUEST_COMPLETE

<div class="yarn-node" data-title="QUEST_COMPLETE">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dobrze zrobiony!</span> <span class="yarn-meta">#line:03529aa </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-child"></a>

## spawned_child

<div class="yarn-node" data-title="spawned_child">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Widziałem niebieskiego NEPTUN.</span> <span class="yarn-meta">#line:0db085c </span>
<span class="yarn-line">Pierścienie na Saturnie świecą.</span> <span class="yarn-meta">#line:0a7ab40 </span>

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
<span class="yarn-line">Planety krążą wokół SŁOŃCA.</span> <span class="yarn-meta">#line:0275852 </span>
<span class="yarn-line">KSIĘŻYC jest naszym satelitą.</span> <span class="yarn-meta">#line:07430b2 </span>

</code>
</pre>
</div>


