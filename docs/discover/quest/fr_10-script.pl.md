---
title: Paryż Sekwana (fr_10) - Script
hide:
---

# Paryż Sekwana (fr_10) - Script
[Quest Index](./index.pl.md) - Language: [english](./fr_10-script.md) - [french](./fr_10-script.fr.md) - polish - [italian](./fr_10-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-10-paris-seine/29/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=754141150#gid=754141150)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_10%20Paris%20Seine/FR_10%20Paris%20Seine%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// Quest: fr_10 | Seine (Paris)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 8&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome to the River Seine in Paris!] <span class="yarn-meta">#line:042160f </span></span>
<span class="yarn-line">[MISSING TRANSLATION: You will learn about the river, its bridges, and the boats that sail on it.] <span class="yarn-meta">#line:0280e8f </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-the-end"></a>
## the_end

<div class="yarn-node" data-title="the_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Great, you found all bridges.] <span class="yarn-meta">#line:0c408c3 </span></span>
<span class="yarn-cmd">&lt;&lt;jump quest_proposal&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-quest-proposal"></a>
## quest_proposal

<div class="yarn-node" data-title="quest_proposal"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: Draw a bridge in your notebook!]
<span class="yarn-line">[MISSING TRANSLATION: Do you remember the Pont Alexandre III?] <span class="yarn-meta">#line:06c5bed </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-tutor"></a>
## talk_tutor

<div class="yarn-node" data-title="talk_tutor"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">tags: type=Condition</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $CURRENT_PROGRESS &gt;= $MAX_PROGRESS&gt;&gt;
<span class="yarn-cmd">&lt;&lt;jump found_all_photos&gt;&gt;</span>
&lt;&lt;elseif $CURRENT_PROGRESS &gt; 0&gt;&gt;
<span class="yarn-line">[MISSING TRANSLATION: Keep searching for the photos!] <span class="yarn-meta">#line:00b8877 </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
[MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-welcome"></a>
## welcome

<div class="yarn-node" data-title="welcome"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=seine_river_panoramic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: This is the River Seine!] <span class="yarn-meta">#line:0aa0f3a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: The River Seine flows through Paris.] <span class="yarn-meta">#line:064988b </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Many people live by this river.] <span class="yarn-meta">#line:080744b </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_seine&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-found-all-photos"></a>
## found_all_photos

<div class="yarn-node" data-title="found_all_photos"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Thank you!] <span class="yarn-meta">#line:0a70250 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Do you know that there are 37 bridges on the Seine?] <span class="yarn-meta">#line:0353a5c </span></span>
<span class="yarn-cmd">&lt;&lt;jump look_seine_map&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-task-seine"></a>
## task_seine

<div class="yarn-node" data-title="task_seine"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;task_start seine&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: I lost 8 photos of the Seine.] <span class="yarn-meta">#line:0d77f5e </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Can you find them all?] <span class="yarn-meta">#line:05727a9 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Then come back to me!] <span class="yarn-meta">#line:09b03cc </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-sign-train-bridge"></a>
## sign_train_bridge

<div class="yarn-node" data-title="sign_train_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_train_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This is a bridge for trains!] <span class="yarn-meta">#line:0a0991d </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-sign-car-bridge"></a>
## sign_car_bridge

<div class="yarn-node" data-title="sign_car_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_car_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This is a bridge for cars!] <span class="yarn-meta">#line:0d79d7d </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-sign-people-bridge"></a>
## sign_people_bridge

<div class="yarn-node" data-title="sign_people_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_people_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This is a bridge for people!] <span class="yarn-meta">#line:0f8a96f </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-boat-people"></a>
## boat_people

<div class="yarn-node" data-title="boat_people"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_5&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This boat has large windows, so tourists can see the city. It's a 'bateau-mouche' (a tourist boat).] <span class="yarn-meta">#line:0129c99 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-boat-house"></a>
## boat_house

<div class="yarn-node" data-title="boat_house"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR,</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_6&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This boat is a house! People live here!] <span class="yarn-meta">#line:0a627c2 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-boat-goods"></a>
## boat_goods

<div class="yarn-node" data-title="boat_goods"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, boat_goods</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_goods&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_7&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This boat transports goods.] <span class="yarn-meta">#line:0f13a44 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-look-seine-map"></a>
## look_seine_map

<div class="yarn-node" data-title="look_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  seine_france_map&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Now look at this map of the Seine.] <span class="yarn-meta">#line:0b03341 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-question-seine-map"></a>
## question_seine_map

<div class="yarn-node" data-title="question_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: The Seine flows into a sea. Which one?] <span class="yarn-meta">#line:0adc7b7 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; "The Mediterranean Sea":] <span class="yarn-meta">#line:04cf904 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; "The English Channel":] <span class="yarn-meta">#line:041b8ec </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_correct&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; "The Atlantic Ocean":] <span class="yarn-meta">#line:0cdce92 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-question-correct"></a>
## question_correct

<div class="yarn-node" data-title="question_correct"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Yes! The Seine flows into the English Channel, in northern France.] <span class="yarn-meta">#line:023623a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump the_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-question-wrong"></a>
## question_wrong

<div class="yarn-node" data-title="question_wrong"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Hmm... not really. Look at the map again.] <span class="yarn-meta">#line:01c9d19 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-npc-train"></a>
## npc_train

<div class="yarn-node" data-title="npc_train"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; I would love to ride a train!] <span class="yarn-meta">#line:01663fa </span></span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; I love trains!] <span class="yarn-meta">#line:0574872 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-npc-boat"></a>
## npc_boat

<div class="yarn-node" data-title="npc_boat"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; No, I didn't see your dog.] <span class="yarn-meta">#line:0614aef </span></span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; A dog? There are many dogs here...] <span class="yarn-meta">#line:09f3f7a </span></span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; I did see a dog walking around, yes.] <span class="yarn-meta">#line:02ed36d </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-boat-eiffel"></a>
## boat_eiffel

<div class="yarn-node" data-title="boat_eiffel"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_8, meta=ACTION_POST:COLLECT_8, asset=boat_eiffel</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Look how nice!] <span class="yarn-meta">#line:059f66d </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-boat-river"></a>
## boat_river

<div class="yarn-node" data-title="boat_river"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_4, meta=ACTION_POST:COLLECT_4, asset=boat_river</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: There are many boats.] <span class="yarn-meta">#line:076e2e6 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-ile-de-france"></a>
## talk_ile_de_france

<div class="yarn-node" data-title="talk_ile_de_france"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ile_de_france&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Look: there is an island, Île de la Cité.] <span class="yarn-meta">#line:03e5667 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: It is the historic center of Paris.] <span class="yarn-meta">#line:04b2cf2 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-pont-alexandre"></a>
## talk_pont_alexandre

<div class="yarn-node" data-title="talk_pont_alexandre"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card alexander_iii_bridge&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This is the Pont Alexandre III, a historic monument.] <span class="yarn-meta">#line:09c706f </span></span>
[MISSING TRANSLATION: ]
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
[MISSING TRANSLATION: =&gt; I love Paris!]
[MISSING TRANSLATION: =&gt; The Seine is so romantic!]
[MISSING TRANSLATION: =&gt; I want to see the Eiffel Tower!]
[MISSING TRANSLATION: =&gt; I would like to sleep on a boat on the Seine!]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-spawned-french-woman"></a>
## spawned_french_woman

<div class="yarn-node" data-title="spawned_french_woman"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: =&gt; Bonjour! #do_not_translate]
[MISSING TRANSLATION: =&gt; J'adore Paris! #do_not_translate]
[MISSING TRANSLATION: =&gt; La Seine est magnifique! #do_not_translate]
[MISSING TRANSLATION: =&gt; Il y a beaucoup de ponts à Paris! #do_not_translate]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-spawned-french-man"></a>
## spawned_french_man

<div class="yarn-node" data-title="spawned_french_man"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: =&gt; Salut! #do_not_translate]
[MISSING TRANSLATION: =&gt; J'aime bien faire du vélo le long de la Seine! #do_not_translate]
[MISSING TRANSLATION: =&gt; Paris est la plus belle ville du monde! #do_not_translate]
[MISSING TRANSLATION: =&gt; La Seine est très longue! #do_not_translate]
[MISSING TRANSLATION: ]
</code></pre></div>


