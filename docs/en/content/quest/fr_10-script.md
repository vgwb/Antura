---
title: Paris Seine (fr_10) - Script
hide:
---

# Paris Seine (fr_10) - Script
[Quest Index](./index.md) - Language: english - [french](./fr_10-script.fr.md) - [polish](./fr_10-script.pl.md) - [italian](./fr_10-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-10-paris-seine/29/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=754141150#gid=754141150)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_10%20Paris%20Seine/FR_10%20Paris%20Seine%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// fr_10 | Seine (Paris)</span>
<span class="yarn-header-dim">// </span>
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

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great, you found all bridges. <span class="yarn-meta">#line:0c408c3 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">You learned about bridges for trains, cars, and people. <span class="yarn-meta">#line:091e9fc </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>
<span class="yarn-line">You saw boats for people, for goods, and even a house boat. <span class="yarn-meta">#line:01d91fb </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">We looked at the map to see where the Seine flows. <span class="yarn-meta">#line:0ca298e </span></span>
<span class="yarn-line">The Seine ends in the English Channel. <span class="yarn-meta">#line:0c693da </span></span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">Pont Alexandre III is a famous bridge with golden statues. <span class="yarn-meta">#line:091d35c </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Paris grows around the Seine. Rivers help cities live. <span class="yarn-meta">#line:08cc02a </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Draw a bridge in your notebook! <span class="yarn-meta">#line:0b37e92 </span></span>
<span class="yarn-line">Do you remember the Pont Alexandre III? <span class="yarn-meta">#line:06c5bed </span></span>
<span class="yarn-line">Can you draw three kinds of bridges? Train, car, people. <span class="yarn-meta">#line:0d44b60 </span></span>
<span class="yarn-line">Color the river blue and add a little boat. <span class="yarn-meta">#line:04f7aaa </span></span>
<span class="yarn-line">Tell a friend one fact about the Seine. <span class="yarn-meta">#line:0062ddd </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

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

<div class="yarn-node" data-title="look_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  seine_france_map&gt;&gt;</span>
<span class="yarn-line">Now look at this map of the Seine. <span class="yarn-meta">#line:0b03341 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-seine-map"></a>
## question_seine_map

<div class="yarn-node" data-title="question_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Seine flows into a sea. Which one? <span class="yarn-meta">#line:0adc7b7 </span></span>
<span class="yarn-line">"The Mediterranean Sea": <span class="yarn-meta">#line:04cf904 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>
<span class="yarn-line">"The English Channel": <span class="yarn-meta">#line:041b8ec </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_correct&gt;&gt;</span>
<span class="yarn-line">"The Atlantic Ocean": <span class="yarn-meta">#line:0cdce92 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-correct"></a>
## question_correct

<div class="yarn-node" data-title="question_correct"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Yes! The Seine flows into the English Channel, in northern France. <span class="yarn-meta">#line:023623a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

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
<span class="yarn-line">I would love to ride a train! <span class="yarn-meta">#line:01663fa </span></span>
<span class="yarn-line">I love trains! <span class="yarn-meta">#line:0574872 </span></span>

</code></pre></div>

<a id="ys-node-npc-boat"></a>
## npc_boat

<div class="yarn-node" data-title="npc_boat"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">No, I didn't see your dog. <span class="yarn-meta">#line:0614aef </span></span>
<span class="yarn-line">A dog? There are many dogs here... <span class="yarn-meta">#line:09f3f7a </span></span>
<span class="yarn-line">I did see a dog walking around, yes. <span class="yarn-meta">#line:02ed36d </span></span>

</code></pre></div>

<a id="ys-node-boat-eiffel"></a>
## boat_eiffel

<div class="yarn-node" data-title="boat_eiffel"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_8, meta=ACTION_POST:COLLECT_8, asset=boat_eiffel</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Look how nice! <span class="yarn-meta">#line:059f66d </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-boat-river"></a>
## boat_river

<div class="yarn-node" data-title="boat_river"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_4, meta=ACTION_POST:COLLECT_4, asset=boat_river</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">There are many boats. <span class="yarn-meta">#line:076e2e6 </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>

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
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">This is the Pont Alexandre III, a historic monument. <span class="yarn-meta">#line:09c706f </span></span>

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
<span class="yarn-line">I love Paris! <span class="yarn-meta">#line:0d7db60 </span></span>
<span class="yarn-line">The Seine is so romantic! <span class="yarn-meta">#line:001b48b </span></span>
<span class="yarn-line">I want to see the Eiffel Tower! <span class="yarn-meta">#line:0be0725 </span></span>
<span class="yarn-line">I would like to sleep on a boat on the Seine! <span class="yarn-meta">#line:09039ee </span></span>

</code></pre></div>

<a id="ys-node-spawned-french-woman"></a>
## spawned_french_woman

<div class="yarn-node" data-title="spawned_french_woman"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour! <span class="yarn-meta">#line:072a345 </span></span>
<span class="yarn-line">J'adore Paris! <span class="yarn-meta">#line:0f2e0aa </span></span>
<span class="yarn-line">La Seine est magnifique! <span class="yarn-meta">#line:0f0f558 </span></span>
<span class="yarn-line">Il y a beaucoup de ponts à Paris! <span class="yarn-meta">#line:038b8f6 </span></span>

</code></pre></div>

<a id="ys-node-spawned-french-man"></a>
## spawned_french_man

<div class="yarn-node" data-title="spawned_french_man"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Salut! <span class="yarn-meta">#line:06aa0c1 </span></span>
<span class="yarn-line">J'aime bien faire du vélo le long de la Seine! <span class="yarn-meta">#line:0494564 </span></span>
<span class="yarn-line">Paris est la plus belle ville du monde! <span class="yarn-meta">#line:0d94ea8 </span></span>
<span class="yarn-line">La Seine est très longue! <span class="yarn-meta">#line:0395113 </span></span>

</code></pre></div>

<a id="ys-node-facts-bridges"></a>
## facts_bridges

<div class="yarn-node" data-title="facts_bridges"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_trains&gt;&gt;</span>
<span class="yarn-line">Bridges can carry fast trains over the water. <span class="yarn-meta">#line:035a1f6 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_cars&gt;&gt;</span>
<span class="yarn-line">Some bridges are busy with cars and buses. <span class="yarn-meta">#line:0723c02 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">Some bridges are only for people to walk and enjoy the view. <span class="yarn-meta">#line:01d93bc </span></span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">Pont Alexandre III has golden statues. <span class="yarn-meta">#line:0ba2ad8 </span></span>

</code></pre></div>

<a id="ys-node-facts-river"></a>
## facts_river

<div class="yarn-node" data-title="facts_river"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card seine_map_in_paris&gt;&gt;</span>
<span class="yarn-line">The Seine bends through Paris like a ribbon. <span class="yarn-meta">#line:09ee6d9 </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">The river starts far away and ends in the English Channel. <span class="yarn-meta">#line:074efa5 </span></span>
<span class="yarn-line">Rivers bring boats, water, and life to a city. <span class="yarn-meta">#line:0bdfb18 </span></span>

</code></pre></div>

<a id="ys-node-facts-boats"></a>
## facts_boats

<div class="yarn-node" data-title="facts_boats"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Tourist boats have wide windows to see monuments. <span class="yarn-meta">#line:0323421 </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>
<span class="yarn-line">Some boats just move along quietly. <span class="yarn-meta">#line:09a6062 </span></span>
<span class="yarn-line">Boats can move people, things, or be a home. <span class="yarn-meta">#line:0b7a149 </span></span>

</code></pre></div>

<a id="ys-node-spawned-bridge-expert"></a>
## spawned_bridge_expert

<div class="yarn-node" data-title="spawned_bridge_expert"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: bridge_expert</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A train bridge must be strong. <span class="yarn-meta">#line:0816b6b </span></span>
<span class="yarn-line">Some bridges are only for feet. <span class="yarn-meta">#line:0ba2f29 </span></span>
<span class="yarn-line">Cars cross the river every day. <span class="yarn-meta">#line:08d8ef0 </span></span>

</code></pre></div>

<a id="ys-node-spawned-river-friend"></a>
## spawned_river_friend

<div class="yarn-node" data-title="spawned_river_friend"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: river_friend</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Seine helps boats move through Paris. <span class="yarn-meta">#line:0ec822e </span></span>
<span class="yarn-line">Maps show how the river curves. <span class="yarn-meta">#line:06d2470 </span></span>
<span class="yarn-line">The river water flows to the sea. <span class="yarn-meta">#line:0d1724c </span></span>

</code></pre></div>

<a id="ys-node-spawned-boat-guide"></a>
## spawned_boat_guide

<div class="yarn-node" data-title="spawned_boat_guide"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: boat_guide</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tourist boats have big windows. <span class="yarn-meta">#line:047502b </span></span>
<span class="yarn-line">Goods boats carry many boxes. <span class="yarn-meta">#line:0ef47e5 </span></span>
<span class="yarn-line">A house boat can be a home. <span class="yarn-meta">#line:00728bb </span></span>

</code></pre></div>


