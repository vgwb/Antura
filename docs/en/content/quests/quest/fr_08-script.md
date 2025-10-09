---
title: Mont Blanc & Mountains (fr_08) - Script
hide:
---

# Mont Blanc & Mountains (fr_08) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-08-mont-blanc-mountains/27/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=736863861#gid=736863861)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_08%20Mont%20Blanc/FR_08%20Mont%20Blanc%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_08 | Mont Blanc</span>
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
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
Welcome to the Mont Blanc!

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You reached Mont Blanc and learned summit facts!</span> <span class="yarn-meta">#line:0b99e77 </span>
<span class="yarn-line">You placed all three flags.</span> <span class="yarn-meta">#line:057afc7 </span>
<span class="yarn-cmd">&lt;&lt;card mountain&gt;&gt;</span>
<span class="yarn-line">You got ready with warm gear.</span> <span class="yarn-meta">#line:0dda4d7 </span>
<span class="yarn-line">Brave mountain explorer!</span> <span class="yarn-meta">#line:04f90c6 </span>
<span class="yarn-cmd">&lt;&lt;card summit&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Why don't you draw Mont Blanc with the three flags.</span> <span class="yarn-meta">#line:0232ab7 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-tutor"></a>

## talk_tutor

<div class="yarn-node" data-title="talk_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags:  asset=mont_blanc</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You have to go up the Mont Blanc</span> <span class="yarn-meta">#line:0f4644b </span>
<span class="yarn-line">the highest mountain in Europe</span> <span class="yarn-meta">#line:07d23cb </span>
<span class="yarn-line">and put the 3 flags correctly</span> <span class="yarn-meta">#line:07f2699 </span>

</code>
</pre>
</div>

<a id="ys-node-item-backpack"></a>

## item_backpack

<div class="yarn-node" data-title="item_backpack">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card backpack&gt;&gt;</span>
<span class="yarn-line">This backpack carries food, water and a map.</span> <span class="yarn-meta">#line:05e80cb </span>
<span class="yarn-line">It keeps hands free while hiking.</span> <span class="yarn-meta">#line:06d7fcf </span>

</code>
</pre>
</div>

<a id="ys-node-item-coat"></a>

## item_coat

<div class="yarn-node" data-title="item_coat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card coat&gt;&gt;</span>
<span class="yarn-line">This coat keeps your body warm in wind and snow.</span> <span class="yarn-meta">#line:0e23f1f </span>
<span class="yarn-line">Always zip it up high.</span> <span class="yarn-meta">#line:06fe689 </span>

</code>
</pre>
</div>

<a id="ys-node-item-gloves"></a>

## item_gloves

<div class="yarn-node" data-title="item_gloves">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gloves&gt;&gt;</span>
<span class="yarn-line">Gloves keep fingers warm and dry.</span> <span class="yarn-meta">#line:0f2aac7 </span>
<span class="yarn-line">Cold hands make climbing hard.</span> <span class="yarn-meta">#line:0d01879 </span>

</code>
</pre>
</div>

<a id="ys-node-item-hat"></a>

## item_hat

<div class="yarn-node" data-title="item_hat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card hat&gt;&gt;</span>
<span class="yarn-line">A warm hat keeps heat from leaving your head.</span> <span class="yarn-meta">#line:000fc05 </span>
<span class="yarn-line">Wear it even in bright sun.</span> <span class="yarn-meta">#line:059d744 </span>

</code>
</pre>
</div>

<a id="ys-node-item-rope"></a>

## item_rope

<div class="yarn-node" data-title="item_rope">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card rope&gt;&gt;</span>
<span class="yarn-line">The rope helps climbers stay safe on ice and rock.</span> <span class="yarn-meta">#line:038d1bc </span>
<span class="yarn-line">Always clip it correctly.</span> <span class="yarn-meta">#line:0b3d07e </span>

</code>
</pre>
</div>

<a id="ys-node-item-scarf"></a>

## item_scarf

<div class="yarn-node" data-title="item_scarf">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card scarf&gt;&gt;</span>
<span class="yarn-line">A scarf blocks wind on your neck.</span> <span class="yarn-meta">#line:0afb2dc </span>
<span class="yarn-line">Tuck it so it will not flap.</span> <span class="yarn-meta">#line:07cb76f </span>

</code>
</pre>
</div>

<a id="ys-node-item-sunglasses"></a>

## item_sunglasses

<div class="yarn-node" data-title="item_sunglasses">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sunglasses&gt;&gt;</span>
<span class="yarn-line">Snow and ice reflect bright sun.</span> <span class="yarn-meta">#line:02a2ea9 </span>
<span class="yarn-line">Glasses protect your eyes.</span> <span class="yarn-meta">#line:088fb7f </span>

</code>
</pre>
</div>

<a id="ys-node-flag-france"></a>

## flag_france

<div class="yarn-node" data-title="flag_france">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_france&gt;&gt;</span>
<span class="yarn-line">Find the French flag.</span> <span class="yarn-meta">#line:0d23529 </span>

</code>
</pre>
</div>

<a id="ys-node-flag-italy"></a>

## flag_italy

<div class="yarn-node" data-title="flag_italy">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>

<span class="yarn-line">Find the Italian flag.</span> <span class="yarn-meta">#line:050fe70 </span>

</code>
</pre>
</div>

<a id="ys-node-flag-swiss"></a>

## flag_swiss

<div class="yarn-node" data-title="flag_swiss">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_swiss&gt;&gt;</span>
<span class="yarn-line">Find the Swiss flag.</span> <span class="yarn-meta">#line:03db010 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I love the mountains!</span> <span class="yarn-meta">#line:011dc7e </span>
<span class="yarn-line">The Alps are beautiful!</span> <span class="yarn-meta">#line:0e6a8d1 </span>
<span class="yarn-line">Mont Blanc is the highest mountain in Europe!</span> <span class="yarn-meta">#line:003ecb2 </span>
<span class="yarn-line">I want to climb Mont Blanc one day!</span> <span class="yarn-meta">#line:01c1599 </span>
<span class="yarn-line">I hope to see a marmot!</span> <span class="yarn-meta">#line:04bbada </span>
<span class="yarn-line">The view from the summit must be amazing!</span> <span class="yarn-meta">#line:031feca </span>
<span class="yarn-line">Don't forget to bring warm clothes!</span> <span class="yarn-meta">#line:0590129 </span>
<span class="yarn-line">Mountains can be dangerous, be careful!</span> <span class="yarn-meta">#line:097ecf0 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-hiker"></a>

## spawned_hiker

<div class="yarn-node" data-title="spawned_hiker">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: hikers</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hiking here needs good boots.</span> <span class="yarn-meta">#line:0dbac73 </span>
<span class="yarn-cmd">&lt;&lt;card hiking&gt;&gt;</span>
<span class="yarn-line">The wind can change fast.</span> <span class="yarn-meta">#line:0557ef5 </span>
<span class="yarn-cmd">&lt;&lt;card wind&gt;&gt;</span>
<span class="yarn-line">I follow the rope line when it is icy.</span> <span class="yarn-meta">#line:0bc67f7 </span>
<span class="yarn-cmd">&lt;&lt;card rope&gt;&gt;</span>
<span class="yarn-line">That glacier looks like a frozen river.</span> <span class="yarn-meta">#line:01e5f66 </span>
<span class="yarn-cmd">&lt;&lt;card glacier&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-alps-climber"></a>

## spawned_alps_climber

<div class="yarn-node" data-title="spawned_alps_climber">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: alps_climber</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Alps are my playground.</span> <span class="yarn-meta">#line:0809dce </span>
<span class="yarn-cmd">&lt;&lt;card alps&gt;&gt;</span>
<span class="yarn-line">Summit soon! I can feel the sun.</span> <span class="yarn-meta">#line:0face28 </span>
<span class="yarn-cmd">&lt;&lt;card summit&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card sun&gt;&gt;</span>
<span class="yarn-line">Crampons help me on hard ice.</span> <span class="yarn-meta">#line:081fbe4 </span>
<span class="yarn-cmd">&lt;&lt;card crampons&gt;&gt;</span>
<span class="yarn-line">Climbing with a guide is safer.</span> <span class="yarn-meta">#line:0371d31 </span>
<span class="yarn-cmd">&lt;&lt;card climbing&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card mountain_guide&gt;&gt;</span>

</code>
</pre>
</div>


