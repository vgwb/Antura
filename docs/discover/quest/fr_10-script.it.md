---
title: Parigi Senna (fr_10) - Script
hide:
---

# Parigi Senna (fr_10) - Script
[Quest Index](./index.it.md) - Language: [english](./fr_10-script.md) - [french](./fr_10-script.fr.md) - [polish](./fr_10-script.pl.md) - italian

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
<span class="yarn-line">Benvenuti sulla Senna a Parigi! <span class="yarn-meta">#line:042160f </span></span>
<span class="yarn-line">Imparerai a conoscere il fiume, i suoi ponti e le imbarcazioni che lo navigano. <span class="yarn-meta">#line:0280e8f </span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo, hai trovato tutti i ponti. <span class="yarn-meta">#line:0c408c3 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">Hai imparato a conoscere i ponti per treni, automobili e persone. <span class="yarn-meta">#line:091e9fc </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>
<span class="yarn-line">Si vedevano barche per il trasporto di persone, merci e persino una casa galleggiante. <span class="yarn-meta">#line:01d91fb </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">Abbiamo guardato la mappa per vedere dove scorre la Senna. <span class="yarn-meta">#line:0ca298e </span></span>
<span class="yarn-line">La Senna sfocia nella Manica. <span class="yarn-meta">#line:0c693da </span></span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">Il Ponte Alessandro III è un famoso ponte con statue dorate. <span class="yarn-meta">#line:091d35c </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Parigi si sviluppa attorno alla Senna. I fiumi aiutano le città a vivere. <span class="yarn-meta">#line:08cc02a </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Disegna un ponte sul tuo quaderno! <span class="yarn-meta">#line:0b37e92 </span></span>
<span class="yarn-line">Vi ricordate il Ponte Alessandro III? <span class="yarn-meta">#line:06c5bed </span></span>
<span class="yarn-line">Sai disegnare tre tipi di ponti? Un treno, un'auto, delle persone. <span class="yarn-meta">#line:0d44b60 </span></span>
<span class="yarn-line">Colora il fiume di blu e aggiungi una piccola barca. <span class="yarn-meta">#line:04f7aaa </span></span>
<span class="yarn-line">Racconta a un amico un fatto sulla Senna. <span class="yarn-meta">#line:0062ddd </span></span>
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
<span class="yarn-line">Continua a cercare le foto! <span class="yarn-meta">#line:00b8877 </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-welcome"></a>
## welcome

<div class="yarn-node" data-title="welcome"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=seine_river_panoramic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questa è la Senna! <span class="yarn-meta">#line:0aa0f3a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">La Senna attraversa Parigi. <span class="yarn-meta">#line:064988b </span></span>
<span class="yarn-line">Molte persone vivono lungo questo fiume. <span class="yarn-meta">#line:080744b </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_seine&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-found-all-photos"></a>
## found_all_photos

<div class="yarn-node" data-title="found_all_photos"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Grazie! <span class="yarn-meta">#line:0a70250 </span></span>
<span class="yarn-line">Sapevi che ci sono 37 ponti sulla Senna? <span class="yarn-meta">#line:0353a5c </span></span>
<span class="yarn-cmd">&lt;&lt;jump look_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-seine"></a>
## task_seine

<div class="yarn-node" data-title="task_seine"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;task_start seine&gt;&gt;</span>
<span class="yarn-line">Ho perso 8 foto della Senna. <span class="yarn-meta">#line:0d77f5e </span></span>
<span class="yarn-line">Riesci a trovarli tutti? <span class="yarn-meta">#line:05727a9 </span></span>
<span class="yarn-line">Allora torna da me! <span class="yarn-meta">#line:09b03cc </span></span>

</code></pre></div>

<a id="ys-node-sign-train-bridge"></a>
## sign_train_bridge

<div class="yarn-node" data-title="sign_train_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_train_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>
<span class="yarn-line">Questo è un ponte per treni! <span class="yarn-meta">#line:0a0991d </span></span>

</code></pre></div>

<a id="ys-node-sign-car-bridge"></a>
## sign_car_bridge

<div class="yarn-node" data-title="sign_car_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_car_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>
<span class="yarn-line">Questo è un ponte per le auto! <span class="yarn-meta">#line:0d79d7d </span></span>

</code></pre></div>

<a id="ys-node-sign-people-bridge"></a>
## sign_people_bridge

<div class="yarn-node" data-title="sign_people_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_people_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>
<span class="yarn-line">Questo è un ponte per le persone! <span class="yarn-meta">#line:0f8a96f </span></span>

</code></pre></div>

<a id="ys-node-boat-people"></a>
## boat_people

<div class="yarn-node" data-title="boat_people"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_5&gt;&gt;</span>
<span class="yarn-line">Questa barca ha grandi finestre, così i turisti possono ammirare la città. È un "bateau-mouche" (una barca turistica). <span class="yarn-meta">#line:0129c99 </span></span>

</code></pre></div>

<a id="ys-node-boat-house"></a>
## boat_house

<div class="yarn-node" data-title="boat_house"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR,</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_6&gt;&gt;</span>
<span class="yarn-line">Questa barca è una casa! Qui vive della gente! <span class="yarn-meta">#line:0a627c2 </span></span>

</code></pre></div>

<a id="ys-node-boat-goods"></a>
## boat_goods

<div class="yarn-node" data-title="boat_goods"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, boat_goods</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_goods&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_7&gt;&gt;</span>
<span class="yarn-line">Questa barca trasporta merci. <span class="yarn-meta">#line:0f13a44 </span></span>

</code></pre></div>

<a id="ys-node-look-seine-map"></a>
## look_seine_map

<div class="yarn-node" data-title="look_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  seine_france_map&gt;&gt;</span>
<span class="yarn-line">Ora guarda questa mappa della Senna. <span class="yarn-meta">#line:0b03341 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-seine-map"></a>
## question_seine_map

<div class="yarn-node" data-title="question_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La Senna sfocia in un mare. Quale? <span class="yarn-meta">#line:0adc7b7 </span></span>
<span class="yarn-line">"Il Mar Mediterraneo": <span class="yarn-meta">#line:04cf904 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>
<span class="yarn-line">"La Manica": <span class="yarn-meta">#line:041b8ec </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_correct&gt;&gt;</span>
<span class="yarn-line">"L'Oceano Atlantico": <span class="yarn-meta">#line:0cdce92 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-correct"></a>
## question_correct

<div class="yarn-node" data-title="question_correct"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sì! La Senna sfocia nella Manica, nel nord della Francia. <span class="yarn-meta">#line:023623a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-wrong"></a>
## question_wrong

<div class="yarn-node" data-title="question_wrong"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mmm... non proprio. Guarda di nuovo la mappa. <span class="yarn-meta">#line:01c9d19 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-train"></a>
## npc_train

<div class="yarn-node" data-title="npc_train"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi piacerebbe molto viaggiare in treno! <span class="yarn-meta">#line:01663fa </span></span>
<span class="yarn-line">Adoro i treni! <span class="yarn-meta">#line:0574872 </span></span>

</code></pre></div>

<a id="ys-node-npc-boat"></a>
## npc_boat

<div class="yarn-node" data-title="npc_boat"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">No, non ho visto il tuo cane. <span class="yarn-meta">#line:0614aef </span></span>
<span class="yarn-line">Un cane? Ci sono molti cani qui... <span class="yarn-meta">#line:09f3f7a </span></span>
<span class="yarn-line">Sì, ho visto un cane che passeggiava in giro. <span class="yarn-meta">#line:02ed36d </span></span>

</code></pre></div>

<a id="ys-node-boat-eiffel"></a>
## boat_eiffel

<div class="yarn-node" data-title="boat_eiffel"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_8, meta=ACTION_POST:COLLECT_8, asset=boat_eiffel</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Guarda che bello! <span class="yarn-meta">#line:059f66d </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-boat-river"></a>
## boat_river

<div class="yarn-node" data-title="boat_river"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_4, meta=ACTION_POST:COLLECT_4, asset=boat_river</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ci sono molte barche. <span class="yarn-meta">#line:076e2e6 </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-ile-de-france"></a>
## talk_ile_de_france

<div class="yarn-node" data-title="talk_ile_de_france"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ile_de_france&gt;&gt;</span>
<span class="yarn-line">Guarda: c'è un'isola, l'Île de la Cité. <span class="yarn-meta">#line:03e5667 </span></span>
<span class="yarn-line">È il centro storico di Parigi. <span class="yarn-meta">#line:04b2cf2 </span></span>

</code></pre></div>

<a id="ys-node-talk-pont-alexandre"></a>
## talk_pont_alexandre

<div class="yarn-node" data-title="talk_pont_alexandre"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">Questo è il Ponte Alessandro III, un monumento storico. <span class="yarn-meta">#line:09c706f </span></span>

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
<span class="yarn-line">Adoro Parigi! <span class="yarn-meta">#line:0d7db60 </span></span>
<span class="yarn-line">La Senna è così romantica! <span class="yarn-meta">#line:001b48b </span></span>
<span class="yarn-line">Voglio vedere la Torre Eiffel! <span class="yarn-meta">#line:0be0725 </span></span>
<span class="yarn-line">Vorrei dormire su una barca sulla Senna! <span class="yarn-meta">#line:09039ee </span></span>

</code></pre></div>

<a id="ys-node-spawned-french-woman"></a>
## spawned_french_woman

<div class="yarn-node" data-title="spawned_french_woman"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buongiorno! <span class="yarn-meta">#line:072a345 </span></span>
<span class="yarn-line">Adoro Parigi! <span class="yarn-meta">#line:0f2e0aa </span></span>
<span class="yarn-line">La Seine est magnifique! <span class="yarn-meta">#line:0f0f558 </span></span>
<span class="yarn-line">Il y a beaucoup de ponts à Paris! <span class="yarn-meta">#line:038b8f6 </span></span>

</code></pre></div>

<a id="ys-node-spawned-french-man"></a>
## spawned_french_man

<div class="yarn-node" data-title="spawned_french_man"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Saluti! <span class="yarn-meta">#line:06aa0c1 </span></span>
<span class="yarn-line">J'aime bien faire du vélo le long de la Seine! <span class="yarn-meta">#line:0494564 </span></span>
<span class="yarn-line">Parigi è la plus belle ville del mondo! <span class="yarn-meta">#line:0d94ea8 </span></span>
<span class="yarn-line">La Seine est très longue! <span class="yarn-meta">#line:0395113 </span></span>

</code></pre></div>

<a id="ys-node-facts-bridges"></a>
## facts_bridges

<div class="yarn-node" data-title="facts_bridges"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_trains&gt;&gt;</span>
<span class="yarn-line">I ponti possono trasportare treni veloci sull'acqua. <span class="yarn-meta">#line:035a1f6 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_cars&gt;&gt;</span>
<span class="yarn-line">Alcuni ponti sono trafficati da auto e autobus. <span class="yarn-meta">#line:0723c02 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">Alcuni ponti sono riservati solo alle persone che vogliono passeggiare e godersi il panorama. <span class="yarn-meta">#line:01d93bc </span></span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">Il ponte Alessandro III è decorato con statue dorate. <span class="yarn-meta">#line:0ba2ad8 </span></span>

</code></pre></div>

<a id="ys-node-facts-river"></a>
## facts_river

<div class="yarn-node" data-title="facts_river"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card seine_map_in_paris&gt;&gt;</span>
<span class="yarn-line">La Senna attraversa Parigi come un nastro. <span class="yarn-meta">#line:09ee6d9 </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">Il fiume nasce molto lontano e sfocia nella Manica. <span class="yarn-meta">#line:074efa5 </span></span>
<span class="yarn-line">I fiumi portano barche, acqua e vita alle città. <span class="yarn-meta">#line:0bdfb18 </span></span>

</code></pre></div>

<a id="ys-node-facts-boats"></a>
## facts_boats

<div class="yarn-node" data-title="facts_boats"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Le imbarcazioni turistiche sono dotate di ampie finestre per ammirare i monumenti. <span class="yarn-meta">#line:0323421 </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>
<span class="yarn-line">Alcune barche si muovono silenziosamente. <span class="yarn-meta">#line:09a6062 </span></span>
<span class="yarn-line">Le barche possono trasportare persone, cose o essere una casa. <span class="yarn-meta">#line:0b7a149 </span></span>

</code></pre></div>

<a id="ys-node-spawned-bridge-expert"></a>
## spawned_bridge_expert

<div class="yarn-node" data-title="spawned_bridge_expert"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: bridge_expert</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Un ponte ferroviario deve essere robusto. <span class="yarn-meta">#line:0816b6b </span></span>
<span class="yarn-line">Alcuni ponti sono percorribili solo a piedi. <span class="yarn-meta">#line:0ba2f29 </span></span>
<span class="yarn-line">Ogni giorno le auto attraversano il fiume. <span class="yarn-meta">#line:08d8ef0 </span></span>

</code></pre></div>

<a id="ys-node-spawned-river-friend"></a>
## spawned_river_friend

<div class="yarn-node" data-title="spawned_river_friend"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: river_friend</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La Senna facilita la circolazione delle imbarcazioni attraverso Parigi. <span class="yarn-meta">#line:0ec822e </span></span>
<span class="yarn-line">Le mappe mostrano la curva del fiume. <span class="yarn-meta">#line:06d2470 </span></span>
<span class="yarn-line">L'acqua del fiume scorre verso il mare. <span class="yarn-meta">#line:0d1724c </span></span>

</code></pre></div>

<a id="ys-node-spawned-boat-guide"></a>
## spawned_boat_guide

<div class="yarn-node" data-title="spawned_boat_guide"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: boat_guide</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Le imbarcazioni turistiche hanno grandi finestre. <span class="yarn-meta">#line:047502b </span></span>
<span class="yarn-line">Le navi merci trasportano molte scatole. <span class="yarn-meta">#line:0ef47e5 </span></span>
<span class="yarn-line">Una casa galleggiante può essere una casa. <span class="yarn-meta">#line:00728bb </span></span>

</code></pre></div>


