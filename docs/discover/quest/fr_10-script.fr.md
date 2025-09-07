---
title: Paris Seine (fr_10) - Script
hide:
---

# Paris Seine (fr_10) - Script
[Quest Index](./index.fr.md) - Language: [english](./fr_10-script.md) - french - [polish](./fr_10-script.pl.md) - [italian](./fr_10-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-10-paris-seine/29/1)  
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
<span class="yarn-line">Bienvenue sur la Seine à Paris ! <span class="yarn-meta">#line:042160f </span></span>
<span class="yarn-line">Vous en apprendrez davantage sur la rivière, ses ponts et les bateaux qui y naviguent. <span class="yarn-meta">#line:0280e8f </span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Super, vous avez trouvé tous les ponts. <span class="yarn-meta">#line:0c408c3 </span></span>
<span class="yarn-line">Vous avez appris l’existence des ponts pour les trains, les voitures et les personnes. <span class="yarn-meta">#line:091e9fc </span></span>
<span class="yarn-line">Vous avez vu des bateaux pour les personnes, pour les marchandises et même une péniche. <span class="yarn-meta">#line:01d91fb </span></span>
<span class="yarn-line">Nous avons regardé la carte pour voir où coule la Seine. <span class="yarn-meta">#line:0ca298e </span></span>
<span class="yarn-line">La Seine se jette dans la Manche. <span class="yarn-meta">#line:0c693da </span></span>
<span class="yarn-line">Le Pont Alexandre III est un pont célèbre avec des statues dorées. <span class="yarn-meta">#line:091d35c </span></span>
<span class="yarn-line">Paris se développe autour de la Seine. Les fleuves font vivre les villes. <span class="yarn-meta">#line:08cc02a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_trains&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_cars&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dessine un pont dans ton carnet ! <span class="yarn-meta">#line:0b37e92 </span></span>
<span class="yarn-line">Vous souvenez-vous du Pont Alexandre III ? <span class="yarn-meta">#line:06c5bed </span></span>
<span class="yarn-line">Pouvez-vous dessiner trois types de ponts ? Un train, une voiture, des personnes. <span class="yarn-meta">#line:0d44b60 </span></span>
<span class="yarn-line">Colorie la rivière en bleu et ajoute un petit bateau. <span class="yarn-meta">#line:04f7aaa </span></span>
<span class="yarn-line">Racontez à un ami un fait sur la Seine. <span class="yarn-meta">#line:0062ddd </span></span>
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
<span class="yarn-line">Continuez à chercher les photos ! <span class="yarn-meta">#line:00b8877 </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-welcome"></a>
## welcome

<div class="yarn-node" data-title="welcome"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=seine_river_panoramic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">C'est la Seine ! <span class="yarn-meta">#line:0aa0f3a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">La Seine traverse Paris. <span class="yarn-meta">#line:064988b </span></span>
<span class="yarn-line">De nombreuses personnes vivent au bord de cette rivière. <span class="yarn-meta">#line:080744b </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_seine&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-found-all-photos"></a>
## found_all_photos

<div class="yarn-node" data-title="found_all_photos"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Merci! <span class="yarn-meta">#line:0a70250 </span></span>
<span class="yarn-line">Savez-vous qu’il y a 37 ponts sur la Seine ? <span class="yarn-meta">#line:0353a5c </span></span>
<span class="yarn-cmd">&lt;&lt;jump look_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-seine"></a>
## task_seine

<div class="yarn-node" data-title="task_seine"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;task_start seine&gt;&gt;</span>
<span class="yarn-line">J'ai perdu 8 photos de la Seine. <span class="yarn-meta">#line:0d77f5e </span></span>
<span class="yarn-line">Pouvez-vous tous les trouver ? <span class="yarn-meta">#line:05727a9 </span></span>
<span class="yarn-line">Alors reviens vers moi ! <span class="yarn-meta">#line:09b03cc </span></span>

</code></pre></div>

<a id="ys-node-sign-train-bridge"></a>
## sign_train_bridge

<div class="yarn-node" data-title="sign_train_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_train_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>
<span class="yarn-line">C'est un pont pour les trains ! <span class="yarn-meta">#line:0a0991d </span></span>

</code></pre></div>

<a id="ys-node-sign-car-bridge"></a>
## sign_car_bridge

<div class="yarn-node" data-title="sign_car_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_car_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>
<span class="yarn-line">C'est un pont pour les voitures ! <span class="yarn-meta">#line:0d79d7d </span></span>

</code></pre></div>

<a id="ys-node-sign-people-bridge"></a>
## sign_people_bridge

<div class="yarn-node" data-title="sign_people_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_people_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>
<span class="yarn-line">C'est un pont pour les gens ! <span class="yarn-meta">#line:0f8a96f </span></span>

</code></pre></div>

<a id="ys-node-boat-people"></a>
## boat_people

<div class="yarn-node" data-title="boat_people"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_5&gt;&gt;</span>
<span class="yarn-line">Ce bateau est doté de grandes fenêtres permettant aux touristes de voir la ville. C'est un bateau-mouche. <span class="yarn-meta">#line:0129c99 </span></span>

</code></pre></div>

<a id="ys-node-boat-house"></a>
## boat_house

<div class="yarn-node" data-title="boat_house"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR,</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_6&gt;&gt;</span>
<span class="yarn-line">Ce bateau est une maison ! Des gens vivent ici ! <span class="yarn-meta">#line:0a627c2 </span></span>

</code></pre></div>

<a id="ys-node-boat-goods"></a>
## boat_goods

<div class="yarn-node" data-title="boat_goods"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, boat_goods</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_goods&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_7&gt;&gt;</span>
<span class="yarn-line">Ce bateau transporte des marchandises. <span class="yarn-meta">#line:0f13a44 </span></span>

</code></pre></div>

<a id="ys-node-look-seine-map"></a>
## look_seine_map

<div class="yarn-node" data-title="look_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  seine_france_map&gt;&gt;</span>
<span class="yarn-line">Regardez maintenant cette carte de la Seine. <span class="yarn-meta">#line:0b03341 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-seine-map"></a>
## question_seine_map

<div class="yarn-node" data-title="question_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La Seine se jette dans une mer. Laquelle ? <span class="yarn-meta">#line:0adc7b7 </span></span>
<span class="yarn-line">« La mer Méditerranée » : <span class="yarn-meta">#line:04cf904 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>
<span class="yarn-line">« La Manche » : <span class="yarn-meta">#line:041b8ec </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_correct&gt;&gt;</span>
<span class="yarn-line">« L'océan Atlantique » : <span class="yarn-meta">#line:0cdce92 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-correct"></a>
## question_correct

<div class="yarn-node" data-title="question_correct"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oui ! La Seine se jette dans la Manche, dans le nord de la France. <span class="yarn-meta">#line:023623a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-wrong"></a>
## question_wrong

<div class="yarn-node" data-title="question_wrong"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hmm… pas vraiment. Regarde la carte. <span class="yarn-meta">#line:01c9d19 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-train"></a>
## npc_train

<div class="yarn-node" data-title="npc_train"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">J'adorerais prendre le train ! <span class="yarn-meta">#line:01663fa </span></span>
<span class="yarn-line">J'adore les trains ! <span class="yarn-meta">#line:0574872 </span></span>

</code></pre></div>

<a id="ys-node-npc-boat"></a>
## npc_boat

<div class="yarn-node" data-title="npc_boat"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Non, je n'ai pas vu ton chien. <span class="yarn-meta">#line:0614aef </span></span>
<span class="yarn-line">Un chien ? Il y en a beaucoup ici... <span class="yarn-meta">#line:09f3f7a </span></span>
<span class="yarn-line">J'ai vu un chien se promener, oui. <span class="yarn-meta">#line:02ed36d </span></span>

</code></pre></div>

<a id="ys-node-boat-eiffel"></a>
## boat_eiffel

<div class="yarn-node" data-title="boat_eiffel"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_8, meta=ACTION_POST:COLLECT_8, asset=boat_eiffel</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Regarde comme c'est joli ! <span class="yarn-meta">#line:059f66d </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-boat-river"></a>
## boat_river

<div class="yarn-node" data-title="boat_river"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_4, meta=ACTION_POST:COLLECT_4, asset=boat_river</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Il y a beaucoup de bateaux. <span class="yarn-meta">#line:076e2e6 </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-ile-de-france"></a>
## talk_ile_de_france

<div class="yarn-node" data-title="talk_ile_de_france"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ile_de_france&gt;&gt;</span>
<span class="yarn-line">Regardez : il y a une île, l'Île de la Cité. <span class="yarn-meta">#line:03e5667 </span></span>
<span class="yarn-line">C'est le centre historique de Paris. <span class="yarn-meta">#line:04b2cf2 </span></span>

</code></pre></div>

<a id="ys-node-talk-pont-alexandre"></a>
## talk_pont_alexandre

<div class="yarn-node" data-title="talk_pont_alexandre"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">Il s'agit du Pont Alexandre III, monument historique. <span class="yarn-meta">#line:09c706f </span></span>

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
<span class="yarn-line">J'adore Paris ! <span class="yarn-meta">#line:0d7db60 </span></span>
<span class="yarn-line">La Seine est tellement romantique ! <span class="yarn-meta">#line:001b48b </span></span>
<span class="yarn-line">Je veux voir la Tour Eiffel ! <span class="yarn-meta">#line:0be0725 </span></span>
<span class="yarn-line">J'aimerais dormir sur un bateau sur la Seine ! <span class="yarn-meta">#line:09039ee </span></span>

</code></pre></div>

<a id="ys-node-spawned-french-woman"></a>
## spawned_french_woman

<div class="yarn-node" data-title="spawned_french_woman"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour! <span class="yarn-meta">#line:072a345 </span></span>
<span class="yarn-line">J'adore Paris ! <span class="yarn-meta">#line:0f2e0aa </span></span>
<span class="yarn-line">La Seine est magnifique ! <span class="yarn-meta">#line:0f0f558 </span></span>
<span class="yarn-line">Il y a beaucoup de ponts à Paris ! <span class="yarn-meta">#line:038b8f6 </span></span>

</code></pre></div>

<a id="ys-node-spawned-french-man"></a>
## spawned_french_man

<div class="yarn-node" data-title="spawned_french_man"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Salut! <span class="yarn-meta">#line:06aa0c1 </span></span>
<span class="yarn-line">J'aime bien faire du vélo le long de la Seine ! <span class="yarn-meta">#line:0494564 </span></span>
<span class="yarn-line">Paris est la plus belle ville du monde ! <span class="yarn-meta">#line:0d94ea8 </span></span>
<span class="yarn-line">La Seine est très longue ! <span class="yarn-meta">#line:0395113 </span></span>

</code></pre></div>

<a id="ys-node-facts-bridges"></a>
## facts_bridges

<div class="yarn-node" data-title="facts_bridges"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_trains&gt;&gt;</span>
<span class="yarn-line">Les ponts peuvent permettre aux trains rapides de traverser l’eau. <span class="yarn-meta">#line:035a1f6 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_cars&gt;&gt;</span>
<span class="yarn-line">Certains ponts sont très fréquentés par les voitures et les bus. <span class="yarn-meta">#line:0723c02 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">Certains ponts sont uniquement destinés aux piétons qui souhaitent profiter de la vue. <span class="yarn-meta">#line:01d93bc </span></span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">Le pont Alexandre III possède des statues dorées. <span class="yarn-meta">#line:0ba2ad8 </span></span>

</code></pre></div>

<a id="ys-node-facts-river"></a>
## facts_river

<div class="yarn-node" data-title="facts_river"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card seine_map_in_paris&gt;&gt;</span>
<span class="yarn-line">La Seine serpente à travers Paris comme un ruban. <span class="yarn-meta">#line:09ee6d9 </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">Le fleuve commence loin et se termine dans la Manche. <span class="yarn-meta">#line:074efa5 </span></span>
<span class="yarn-line">Les rivières apportent des bateaux, de l’eau et de la vie à une ville. <span class="yarn-meta">#line:0bdfb18 </span></span>

</code></pre></div>

<a id="ys-node-facts-boats"></a>
## facts_boats

<div class="yarn-node" data-title="facts_boats"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Les bateaux touristiques disposent de larges fenêtres permettant de voir les monuments. <span class="yarn-meta">#line:0323421 </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>
<span class="yarn-line">Certains bateaux naviguent tranquillement. <span class="yarn-meta">#line:09a6062 </span></span>
<span class="yarn-line">Les bateaux peuvent déplacer des personnes, des choses ou être une maison. <span class="yarn-meta">#line:0b7a149 </span></span>

</code></pre></div>

<a id="ys-node-spawned-bridge-expert"></a>
## spawned_bridge_expert

<div class="yarn-node" data-title="spawned_bridge_expert"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: bridge_expert</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Un pont ferroviaire doit être solide. <span class="yarn-meta">#line:0816b6b </span></span>
<span class="yarn-line">Certains ponts sont réservés aux pieds. <span class="yarn-meta">#line:0ba2f29 </span></span>
<span class="yarn-line">Des voitures traversent la rivière tous les jours. <span class="yarn-meta">#line:08d8ef0 </span></span>

</code></pre></div>

<a id="ys-node-spawned-river-friend"></a>
## spawned_river_friend

<div class="yarn-node" data-title="spawned_river_friend"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: river_friend</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La Seine permet aux bateaux de circuler dans Paris. <span class="yarn-meta">#line:0ec822e </span></span>
<span class="yarn-line">Les cartes montrent comment la rivière se courbe. <span class="yarn-meta">#line:06d2470 </span></span>
<span class="yarn-line">L'eau de la rivière coule vers la mer. <span class="yarn-meta">#line:0d1724c </span></span>

</code></pre></div>

<a id="ys-node-spawned-boat-guide"></a>
## spawned_boat_guide

<div class="yarn-node" data-title="spawned_boat_guide"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: boat_guide</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Les bateaux touristiques ont de grandes fenêtres. <span class="yarn-meta">#line:047502b </span></span>
<span class="yarn-line">Les bateaux de marchandises transportent de nombreuses caisses. <span class="yarn-meta">#line:0ef47e5 </span></span>
<span class="yarn-line">Une péniche peut être une maison. <span class="yarn-meta">#line:00728bb </span></span>

</code></pre></div>


