---
title: Mont Blanc i góry (fr_08) - Script
hide:
---

# Mont Blanc i góry (fr_08) - Script
[Quest Index](./index.pl.md) - Language: [english](./fr_08-script.md) - [french](./fr_08-script.fr.md) - polish - [italian](./fr_08-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-08-mont-blanc-mountains/27/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=736863861#gid=736863861)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_08%20Mont%20Blanc/FR_08%20Mont%20Blanc%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// fr_08 | Mont Blanc</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - mont_blanc (geographical landmark)</span>
<span class="yarn-header-dim">// - flag_france (national symbol)</span>
<span class="yarn-header-dim">// - flag_italy (national symbol) </span>
<span class="yarn-header-dim">// - flag_swiss (national symbol)</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - Place 3 flags correctly on Mont Blanc summit</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - Basic flag placement</span>
<span class="yarn-header-dim">// - Order activity for mountain equipment preparation</span>
<span class="yarn-header-dim">// - Memory game for Alpine animal identification</span>
<span class="yarn-header-dim">// - Quiz on mountain safety and geography</span>
<span class="yarn-header-dim">// Words used:Europe, flags, France, Italy, Switzerland, Alps, altitude, summit, glacier, avalanche, marmot, crampons, rope, weather, safety</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dotarłeś do Mont Blanc i poznałeś fakty dotyczące szczytu! <span class="yarn-meta">#line:0b99e77 </span></span>
<span class="yarn-line">Umieściłeś wszystkie trzy flagi. <span class="yarn-meta">#line:057afc7 </span></span>
<span class="yarn-cmd">&lt;&lt;card mountain&gt;&gt;</span>
<span class="yarn-line">Ubrałeś się ciepło. <span class="yarn-meta">#line:0dda4d7 </span></span>
<span class="yarn-line">Odważny odkrywco gór! <span class="yarn-meta">#line:04f90c6 </span></span>
<span class="yarn-cmd">&lt;&lt;card summit&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dlaczego nie narysujesz Mont Blanc z trzema flagami? <span class="yarn-meta">#line:0232ab7 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-tutor"></a>
## talk_tutor

<div class="yarn-node" data-title="talk_tutor"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:  asset=mont_blanc</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Musisz wjechać na Mont Blanc <span class="yarn-meta">#line:0f4644b </span></span>
<span class="yarn-line">najwyższa góra w Europie <span class="yarn-meta">#line:07d23cb </span></span>
<span class="yarn-line">i poprawnie umieścić 3 flagi <span class="yarn-meta">#line:07f2699 </span></span>

</code></pre></div>

<a id="ys-node-item-backpack"></a>
## item_backpack

<div class="yarn-node" data-title="item_backpack"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR asset=backpack</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card backpack&gt;&gt;</span>
<span class="yarn-line">W tym plecaku zmieści się jedzenie, woda i mapa. <span class="yarn-meta">#line:05e80cb </span></span>
<span class="yarn-line">Pozwala zachować wolne ręce podczas wędrówek. <span class="yarn-meta">#line:06d7fcf </span></span>

</code></pre></div>

<a id="ys-node-item-coat"></a>
## item_coat

<div class="yarn-node" data-title="item_coat"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR asset=coat</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card coat&gt;&gt;</span>
<span class="yarn-line">Ten płaszcz chroni ciało przed wiatrem i śniegiem. <span class="yarn-meta">#line:0e23f1f </span></span>
<span class="yarn-line">Zawsze zapinaj zamek wysoko. <span class="yarn-meta">#line:06fe689 </span></span>

</code></pre></div>

<a id="ys-node-item-gloves"></a>
## item_gloves

<div class="yarn-node" data-title="item_gloves"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR asset=gloves</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gloves&gt;&gt;</span>
<span class="yarn-line">Rękawiczki utrzymują palce w cieple i suchości. <span class="yarn-meta">#line:0f2aac7 </span></span>
<span class="yarn-line">Zimne ręce utrudniają wspinaczkę. <span class="yarn-meta">#line:0d01879 </span></span>

</code></pre></div>

<a id="ys-node-item-hat"></a>
## item_hat

<div class="yarn-node" data-title="item_hat"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR asset=hat</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card hat&gt;&gt;</span>
<span class="yarn-line">Ciepła czapka chroni głowę przed utratą ciepła. <span class="yarn-meta">#line:000fc05 </span></span>
<span class="yarn-line">Można ją nosić nawet w jasnym słońcu. <span class="yarn-meta">#line:059d744 </span></span>

</code></pre></div>

<a id="ys-node-item-rope"></a>
## item_rope

<div class="yarn-node" data-title="item_rope"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR asset=rope</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card rope&gt;&gt;</span>
<span class="yarn-line">Lina pomaga wspinaczom zachować bezpieczeństwo na lodzie i skałach. <span class="yarn-meta">#line:038d1bc </span></span>
<span class="yarn-line">Zawsze przycinaj je prawidłowo. <span class="yarn-meta">#line:0b3d07e </span></span>

</code></pre></div>

<a id="ys-node-item-scarf"></a>
## item_scarf

<div class="yarn-node" data-title="item_scarf"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR asset=scarf</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card scarf&gt;&gt;</span>
<span class="yarn-line">Szalik chroni przed wiatrem na szyi. <span class="yarn-meta">#line:0afb2dc </span></span>
<span class="yarn-line">Złóż je tak, aby nie łopotało. <span class="yarn-meta">#line:07cb76f </span></span>

</code></pre></div>

<a id="ys-node-item-sunglasses"></a>
## item_sunglasses

<div class="yarn-node" data-title="item_sunglasses"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR asset=sunglasses</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sunglasses&gt;&gt;</span>
<span class="yarn-line">Śnieg i lód odbijają jasne słońce. <span class="yarn-meta">#line:02a2ea9 </span></span>
<span class="yarn-line">Okulary chronią oczy. <span class="yarn-meta">#line:088fb7f </span></span>

</code></pre></div>

<a id="ys-node-flag-france"></a>
## flag_france

<div class="yarn-node" data-title="flag_france"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:  asset=flag_france</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znajdź flagę francuską. <span class="yarn-meta">#line:0d23529 </span></span>

</code></pre></div>

<a id="ys-node-flag-italy"></a>
## flag_italy

<div class="yarn-node" data-title="flag_italy"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:  asset=flag_italy</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znajdź włoską flagę. <span class="yarn-meta">#line:050fe70 </span></span>

</code></pre></div>

<a id="ys-node-flag-swiss"></a>
## flag_swiss

<div class="yarn-node" data-title="flag_swiss"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:  asset=flag_swiss</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znajdź flagę Szwajcarii. <span class="yarn-meta">#line:03db010 </span></span>

</code></pre></div>

<a id="ys-node-spawned-tourist"></a>
## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Uwielbiam góry! <span class="yarn-meta">#line:011dc7e </span></span>
<span class="yarn-line">Alpy są piękne! <span class="yarn-meta">#line:0e6a8d1 </span></span>
<span class="yarn-line">Mont Blanc jest najwyższą górą Europy! <span class="yarn-meta">#line:003ecb2 </span></span>
<span class="yarn-line">Chcę kiedyś wspiąć się na Mont Blanc! <span class="yarn-meta">#line:01c1599 </span></span>
<span class="yarn-line">Mam nadzieję zobaczyć świstaka! <span class="yarn-meta">#line:04bbada </span></span>
<span class="yarn-line">Widok ze szczytu musi być niesamowity! <span class="yarn-meta">#line:031feca </span></span>
<span class="yarn-line">Nie zapomnij zabrać ciepłych ubrań! <span class="yarn-meta">#line:0590129 </span></span>
<span class="yarn-line">Góry mogą być niebezpieczne, bądź ostrożny! <span class="yarn-meta">#line:097ecf0 </span></span>

</code></pre></div>

<a id="ys-node-spawned-hiker"></a>
## spawned_hiker

<div class="yarn-node" data-title="spawned_hiker"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">spawn_group: hikers</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Do pieszych wędrówek tutaj potrzebne są dobre buty. <span class="yarn-meta">#line:0dbac73 </span></span>
<span class="yarn-cmd">&lt;&lt;card hiking&gt;&gt;</span>
<span class="yarn-line">Wiatr może się szybko zmienić. <span class="yarn-meta">#line:0557ef5 </span></span>
<span class="yarn-cmd">&lt;&lt;card wind&gt;&gt;</span>
<span class="yarn-line">Gdy jest ślisko, podążam za liną. <span class="yarn-meta">#line:0bc67f7 </span></span>
<span class="yarn-cmd">&lt;&lt;card rope&gt;&gt;</span>
<span class="yarn-line">Ten lodowiec wygląda jak zamarznięta rzeka. <span class="yarn-meta">#line:01e5f66 </span></span>
<span class="yarn-cmd">&lt;&lt;card glacier&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spawned-alps-climber"></a>
## spawned_alps_climber

<div class="yarn-node" data-title="spawned_alps_climber"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">spawn_group: alps_climber</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Alpy są moim placem zabaw. <span class="yarn-meta">#line:0809dce </span></span>
<span class="yarn-cmd">&lt;&lt;card alps&gt;&gt;</span>
<span class="yarn-line">Już niedługo szczyt! Czuję słońce. <span class="yarn-meta">#line:0face28 </span></span>
<span class="yarn-cmd">&lt;&lt;card summit&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card sun&gt;&gt;</span>
<span class="yarn-line">Raki pomagają mi na twardym lodzie. <span class="yarn-meta">#line:081fbe4 </span></span>
<span class="yarn-cmd">&lt;&lt;card crampons&gt;&gt;</span>
<span class="yarn-line">Wspinaczka z przewodnikiem jest bezpieczniejsza. <span class="yarn-meta">#line:0371d31 </span></span>
<span class="yarn-cmd">&lt;&lt;card climbing&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card mountain_guide&gt;&gt;</span>

</code></pre></div>


