---
title: Parigi Senna (fr_10) - Script
hide:
---

# Parigi Senna (fr_10) - Script
[Quest Index](./index.it.md) - Language: [english](./fr_10-script.md) - [french](./fr_10-script.fr.md) - [polish](./fr_10-script.pl.md) - italian

!!! note "Educators & Designers: help improving this quest!"
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_10%20Paris%20Seine/FR_10%20Paris%20Seine%20-%20Yarn%20Script.yarn)  
    **Improve translations**: [comment here](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1233127135#gid=1233127135)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// FR_10 PARIS_SEINE - Seine River Journey</span>
<span class="yarn-header-dim">// Location: Paris, France - Seine River and bridges</span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - seine_river (geographical feature)</span>
<span class="yarn-header-dim">// - seine_map (navigation aid)</span>
<span class="yarn-header-dim">// - seine_river_panoramic (scenic view)</span>
<span class="yarn-header-dim">// - boat_eiffel (river transportation)</span>
<span class="yarn-header-dim">// - bridge_alexander_iii (architectural landmark)</span>
<span class="yarn-header-dim">// Words used: Seine, river, boat, bridge, Paris, Notre-Dame, navigation, water, transportation, history</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 8&gt;&gt;</span>
<span class="yarn-line">Welcome to the River Seine in Paris! <span class="yarn-meta">#line:042160f </span></span>
<span class="yarn-line">You will learn about the river, its bridges, and the boats that sail on it. <span class="yarn-meta">#line:0280e8f </span></span>

</code></pre></div>

<a id="ys-node-talk-tutor"></a>
## talk_tutor

<div class="yarn-node" data-title="talk_tutor"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">tags: type=Condition</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $CURRENT_PROGRESS &gt;= $MAX_PROGRESS&gt;&gt;
<span class="yarn-cmd">&lt;&lt;jump found_all_photos&gt;&gt;</span>
&lt;&lt;elseif $CURRENT_PROGRESS &gt; 0&gt;&gt;
<span class="yarn-line">Keep searching for the photos! <span class="yarn-meta">#line:00b8877 </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-welcome"></a>
## welcome

<div class="yarn-node" data-title="welcome"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=seine_river_panoramic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the River Seine! <span class="yarn-meta">#line:0aa0f3a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">The River Seine flows through Paris. <span class="yarn-meta">#line:064988b </span></span>
<span class="yarn-line">Many people live by this river. <span class="yarn-meta">#line:080744b </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_seine&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-found-all-photos"></a>
## found_all_photos

<div class="yarn-node" data-title="found_all_photos"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Thank you! <span class="yarn-meta">#line:0a70250 </span></span>
<span class="yarn-line">Do you know that there are 37 bridges on the Seine? <span class="yarn-meta">#line:0353a5c </span></span>
<span class="yarn-cmd">&lt;&lt;jump look_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-seine"></a>
## task_seine

<div class="yarn-node" data-title="task_seine"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;task_start seine&gt;&gt;</span>
<span class="yarn-line">I lost 8 photos of the Seine. <span class="yarn-meta">#line:0d77f5e </span></span>
<span class="yarn-line">Can you find them all? <span class="yarn-meta">#line:05727a9 </span></span>
<span class="yarn-line">Then come back to me! <span class="yarn-meta">#line:09b03cc </span></span>

</code></pre></div>

<a id="ys-node-sign-train-bridge"></a>
## sign_train_bridge

<div class="yarn-node" data-title="sign_train_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_train_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>
<span class="yarn-line">This is a bridge for trains! <span class="yarn-meta">#line:0a0991d </span></span>

</code></pre></div>

<a id="ys-node-sign-car-bridge"></a>
## sign_car_bridge

<div class="yarn-node" data-title="sign_car_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_car_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>
<span class="yarn-line">This is a bridge for cars! <span class="yarn-meta">#line:0d79d7d </span></span>

</code></pre></div>

<a id="ys-node-sign-people-bridge"></a>
## sign_people_bridge

<div class="yarn-node" data-title="sign_people_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_people_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>
<span class="yarn-line">This is a bridge for people! <span class="yarn-meta">#line:0f8a96f </span></span>

</code></pre></div>

<a id="ys-node-boat-people"></a>
## boat_people

<div class="yarn-node" data-title="boat_people"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_5&gt;&gt;</span>
<span class="yarn-line">This boat has large windows, so tourists can see the city. It's a 'bateau-mouche' (a tourist boat). <span class="yarn-meta">#line:0129c99 </span></span>

</code></pre></div>

<a id="ys-node-boat-house"></a>
## boat_house

<div class="yarn-node" data-title="boat_house"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR,</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_6&gt;&gt;</span>
<span class="yarn-line">This boat is a house! People live here! <span class="yarn-meta">#line:0a627c2 </span></span>

</code></pre></div>

<a id="ys-node-boat-goods"></a>
## boat_goods

<div class="yarn-node" data-title="boat_goods"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, boat_goods</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_goods&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_7&gt;&gt;</span>
<span class="yarn-line">This boat transports goods. <span class="yarn-meta">#line:0f13a44 </span></span>

</code></pre></div>

<a id="ys-node-look-seine-map"></a>
## look_seine_map

<div class="yarn-node" data-title="look_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  seine_france_map&gt;&gt;</span>
<span class="yarn-line">Now look at this map of the Seine. <span class="yarn-meta">#line:0b03341 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-seine-map"></a>
## question_seine_map

<div class="yarn-node" data-title="question_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, type=Choice</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Seine flows into a sea. Which one? <span class="yarn-meta">#line:0adc7b7 </span></span>
<span class="yarn-line">-&gt; "The Mediterranean Sea": <span class="yarn-meta">#line:04cf904 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>
<span class="yarn-line">-&gt; "The English Channel": <span class="yarn-meta">#line:041b8ec </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_correct&gt;&gt;</span>
<span class="yarn-line">-&gt; "The Atlantic Ocean": <span class="yarn-meta">#line:0cdce92 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-correct"></a>
## question_correct

<div class="yarn-node" data-title="question_correct"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Yes! The Seine flows into the English Channel, in northern France. <span class="yarn-meta">#line:023623a </span></span>
<span class="yarn-cmd">&lt;&lt;card  seine_map&gt;&gt;</span>
<span class="yarn-line">On this map, name the monuments you recognize. <span class="yarn-meta">#line:0406145 </span></span>
<span class="yarn-line">Draw a bridge in your notebook! Do you remember the Pont Alexandre III? <span class="yarn-meta">#line:06c5bed </span></span>
<span class="yarn-line">Great, you finished this level. <span class="yarn-meta">#line:0c408c3 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-wrong"></a>
## question_wrong

<div class="yarn-node" data-title="question_wrong"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hmm... not really. Look at the map again. <span class="yarn-meta">#line:01c9d19 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-train"></a>
## npc_train

<div class="yarn-node" data-title="npc_train"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">=&gt; I would love to ride a train! <span class="yarn-meta">#line:01663fa </span></span>
<span class="yarn-line">=&gt; I love trains! <span class="yarn-meta">#line:0574872 </span></span>

</code></pre></div>

<a id="ys-node-npc-boat"></a>
## npc_boat

<div class="yarn-node" data-title="npc_boat"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">=&gt; No, I didn't see your dog. <span class="yarn-meta">#line:0614aef </span></span>
<span class="yarn-line">=&gt; A dog? There are many dogs here... <span class="yarn-meta">#line:09f3f7a </span></span>
<span class="yarn-line">=&gt; I did see a dog walking around, yes. <span class="yarn-meta">#line:02ed36d </span></span>

</code></pre></div>

<a id="ys-node-boat-eiffel"></a>
## boat_eiffel

<div class="yarn-node" data-title="boat_eiffel"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_8, meta=ACTION_POST:COLLECT_8, asset=boat_eiffel</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Look how nice! <span class="yarn-meta">#line:059f66d </span></span>

</code></pre></div>

<a id="ys-node-boat-river"></a>
## boat_river

<div class="yarn-node" data-title="boat_river"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_4, meta=ACTION_POST:COLLECT_4, asset=boat_river</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">There are many boats. <span class="yarn-meta">#line:076e2e6 </span></span>

</code></pre></div>

<a id="ys-node-talk-ile-de-france"></a>
## talk_ile_de_france

<div class="yarn-node" data-title="talk_ile_de_france"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ile_de_france&gt;&gt;</span>
<span class="yarn-line">Look: there is an island, Île de la Cité. <span class="yarn-meta">#line:03e5667 </span></span>
<span class="yarn-line">It is the historic center of Paris. <span class="yarn-meta">#line:04b2cf2 </span></span>

</code></pre></div>

<a id="ys-node-talk-pont-alexandre"></a>
## talk_pont_alexandre

<div class="yarn-node" data-title="talk_pont_alexandre"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card alexander_iii_bridge&gt;&gt;</span>
<span class="yarn-line">This is the Pont Alexandre III, a historic monument. <span class="yarn-meta">#line:09c706f </span></span>

</code></pre></div>


