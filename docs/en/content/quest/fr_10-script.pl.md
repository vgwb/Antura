---
title: Paryż Sekwana (fr_10) - Script
hide:
---

# Paryż Sekwana (fr_10) - Script
[Quest Index](./index.pl.md) - Language: [english](./fr_10-script.md) - [french](./fr_10-script.fr.md) - polish - [italian](./fr_10-script.it.md)

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
<span class="yarn-line">Witamy nad Sekwaną w Paryżu! <span class="yarn-meta">#line:042160f </span></span>
<span class="yarn-line">Dowiesz się o rzece, jej mostach i łodziach, które po niej pływają. <span class="yarn-meta">#line:0280e8f </span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetnie, znalazłeś wszystkie mosty. <span class="yarn-meta">#line:0c408c3 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">Dowiedziałeś się o mostach dla pociągów, samochodów i ludzi. <span class="yarn-meta">#line:091e9fc </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>
<span class="yarn-line">Widziałeś łodzie do przewozu ludzi, łodzie towarowe, a nawet dom na wodzie. <span class="yarn-meta">#line:01d91fb </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">Spojrzeliśmy na mapę, żeby zobaczyć którędy płynie Sekwana. <span class="yarn-meta">#line:0ca298e </span></span>
<span class="yarn-line">Sekwana kończy się w kanale La Manche. <span class="yarn-meta">#line:0c693da </span></span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">Most Aleksandra III to słynny most ze złotymi posągami. <span class="yarn-meta">#line:091d35c </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Paryż rozrasta się wokół Sekwany. Rzeki pomagają miastom żyć. <span class="yarn-meta">#line:08cc02a </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Narysuj most w swoim zeszycie! <span class="yarn-meta">#line:0b37e92 </span></span>
<span class="yarn-line">Czy pamiętasz Most Aleksandra III? <span class="yarn-meta">#line:06c5bed </span></span>
<span class="yarn-line">Czy potrafisz narysować trzy rodzaje mostów? Pociąg, samochód, ludzi. <span class="yarn-meta">#line:0d44b60 </span></span>
<span class="yarn-line">Pokoloruj rzekę na niebiesko i dodaj małą łódkę. <span class="yarn-meta">#line:04f7aaa </span></span>
<span class="yarn-line">Opowiedz znajomemu jedną ciekawostkę o Sekwanie. <span class="yarn-meta">#line:0062ddd </span></span>
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
<span class="yarn-line">Kontynuuj wyszukiwanie zdjęć! <span class="yarn-meta">#line:00b8877 </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-welcome"></a>
## welcome

<div class="yarn-node" data-title="welcome"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=seine_river_panoramic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To jest rzeka Sekwana! <span class="yarn-meta">#line:0aa0f3a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">Rzeka Sekwana przepływa przez Paryż. <span class="yarn-meta">#line:064988b </span></span>
<span class="yarn-line">Wiele osób mieszka nad tą rzeką. <span class="yarn-meta">#line:080744b </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_seine&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-found-all-photos"></a>
## found_all_photos

<div class="yarn-node" data-title="found_all_photos"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dziękuję! <span class="yarn-meta">#line:0a70250 </span></span>
<span class="yarn-line">Czy wiesz, że na Sekwanie jest 37 mostów? <span class="yarn-meta">#line:0353a5c </span></span>
<span class="yarn-cmd">&lt;&lt;jump look_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-seine"></a>
## task_seine

<div class="yarn-node" data-title="task_seine"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;task_start seine&gt;&gt;</span>
<span class="yarn-line">Straciłem 8 zdjęć Sekwany. <span class="yarn-meta">#line:0d77f5e </span></span>
<span class="yarn-line">Czy potrafisz je wszystkie znaleźć? <span class="yarn-meta">#line:05727a9 </span></span>
<span class="yarn-line">To wróć do mnie! <span class="yarn-meta">#line:09b03cc </span></span>

</code></pre></div>

<a id="ys-node-sign-train-bridge"></a>
## sign_train_bridge

<div class="yarn-node" data-title="sign_train_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_train_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>
<span class="yarn-line">To jest most dla pociągów! <span class="yarn-meta">#line:0a0991d </span></span>

</code></pre></div>

<a id="ys-node-sign-car-bridge"></a>
## sign_car_bridge

<div class="yarn-node" data-title="sign_car_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_car_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>
<span class="yarn-line">To jest most dla samochodów! <span class="yarn-meta">#line:0d79d7d </span></span>

</code></pre></div>

<a id="ys-node-sign-people-bridge"></a>
## sign_people_bridge

<div class="yarn-node" data-title="sign_people_bridge"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sign_people_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>
<span class="yarn-line">To jest most dla ludzi! <span class="yarn-meta">#line:0f8a96f </span></span>

</code></pre></div>

<a id="ys-node-boat-people"></a>
## boat_people

<div class="yarn-node" data-title="boat_people"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_5&gt;&gt;</span>
<span class="yarn-line">Ta łódź ma duże okna, więc turyści mogą podziwiać miasto. To „bateau-mouche” (łódź turystyczna). <span class="yarn-meta">#line:0129c99 </span></span>

</code></pre></div>

<a id="ys-node-boat-house"></a>
## boat_house

<div class="yarn-node" data-title="boat_house"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR,</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_6&gt;&gt;</span>
<span class="yarn-line">Ta łódź to dom! Ludzie tu mieszkają! <span class="yarn-meta">#line:0a627c2 </span></span>

</code></pre></div>

<a id="ys-node-boat-goods"></a>
## boat_goods

<div class="yarn-node" data-title="boat_goods"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, boat_goods</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_goods&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_7&gt;&gt;</span>
<span class="yarn-line">Ta łódź przewozi towary. <span class="yarn-meta">#line:0f13a44 </span></span>

</code></pre></div>

<a id="ys-node-look-seine-map"></a>
## look_seine_map

<div class="yarn-node" data-title="look_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  seine_france_map&gt;&gt;</span>
<span class="yarn-line">Teraz spójrz na tę mapę Sekwany. <span class="yarn-meta">#line:0b03341 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-seine-map"></a>
## question_seine_map

<div class="yarn-node" data-title="question_seine_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sekwana wpada do morza. Do którego? <span class="yarn-meta">#line:0adc7b7 </span></span>
<span class="yarn-line">„Morze Śródziemne”: <span class="yarn-meta">#line:04cf904 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>
<span class="yarn-line">„Kanał La Manche” <span class="yarn-meta">#line:041b8ec </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_correct&gt;&gt;</span>
<span class="yarn-line">„Ocean Atlantycki”: <span class="yarn-meta">#line:0cdce92 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump question_wrong&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-correct"></a>
## question_correct

<div class="yarn-node" data-title="question_correct"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tak! Sekwana wpada do kanału La Manche w północnej Francji. <span class="yarn-meta">#line:023623a </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-question-wrong"></a>
## question_wrong

<div class="yarn-node" data-title="question_wrong"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hmm... nie do końca. Spójrz jeszcze raz na mapę. <span class="yarn-meta">#line:01c9d19 </span></span>
<span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-train"></a>
## npc_train

<div class="yarn-node" data-title="npc_train"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Chciałbym pojeździć pociągiem! <span class="yarn-meta">#line:01663fa </span></span>
<span class="yarn-line">Uwielbiam pociągi! <span class="yarn-meta">#line:0574872 </span></span>

</code></pre></div>

<a id="ys-node-npc-boat"></a>
## npc_boat

<div class="yarn-node" data-title="npc_boat"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Nie, nie widziałem twojego psa. <span class="yarn-meta">#line:0614aef </span></span>
<span class="yarn-line">Pies? Jest tu wiele psów... <span class="yarn-meta">#line:09f3f7a </span></span>
<span class="yarn-line">Tak, widziałem spacerującego psa. <span class="yarn-meta">#line:02ed36d </span></span>

</code></pre></div>

<a id="ys-node-boat-eiffel"></a>
## boat_eiffel

<div class="yarn-node" data-title="boat_eiffel"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_8, meta=ACTION_POST:COLLECT_8, asset=boat_eiffel</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Spójrz, jakie ładne! <span class="yarn-meta">#line:059f66d </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-boat-river"></a>
## boat_river

<div class="yarn-node" data-title="boat_river"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR, meta_ACTION_POST=COLLECT_4, meta=ACTION_POST:COLLECT_4, asset=boat_river</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jest wiele łodzi. <span class="yarn-meta">#line:076e2e6 </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-ile-de-france"></a>
## talk_ile_de_france

<div class="yarn-node" data-title="talk_ile_de_france"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ile_de_france&gt;&gt;</span>
<span class="yarn-line">Spójrz: jest wyspa, Île de la Cité. <span class="yarn-meta">#line:03e5667 </span></span>
<span class="yarn-line">Jest to historyczne centrum Paryża. <span class="yarn-meta">#line:04b2cf2 </span></span>

</code></pre></div>

<a id="ys-node-talk-pont-alexandre"></a>
## talk_pont_alexandre

<div class="yarn-node" data-title="talk_pont_alexandre"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">To jest Most Aleksandra III, zabytek historyczny. <span class="yarn-meta">#line:09c706f </span></span>

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
<span class="yarn-line">Uwielbiam Paryż! <span class="yarn-meta">#line:0d7db60 </span></span>
<span class="yarn-line">Sekwana jest taka romantyczna! <span class="yarn-meta">#line:001b48b </span></span>
<span class="yarn-line">Chcę zobaczyć Wieżę Eiffla! <span class="yarn-meta">#line:0be0725 </span></span>
<span class="yarn-line">Chciałbym spać na statku na Sekwanie! <span class="yarn-meta">#line:09039ee </span></span>

</code></pre></div>

<a id="ys-node-spawned-french-woman"></a>
## spawned_french_woman

<div class="yarn-node" data-title="spawned_french_woman"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dzień dobry! <span class="yarn-meta">#line:072a345 </span></span>
<span class="yarn-line">Uwielbiam Paryż! <span class="yarn-meta">#line:0f2e0aa </span></span>
<span class="yarn-line">La Seine est magnifique! <span class="yarn-meta">#line:0f0f558 </span></span>
<span class="yarn-line">Il y a beaucoup de ponts à Paris! <span class="yarn-meta">#line:038b8f6 </span></span>

</code></pre></div>

<a id="ys-node-spawned-french-man"></a>
## spawned_french_man

<div class="yarn-node" data-title="spawned_french_man"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć! <span class="yarn-meta">#line:06aa0c1 </span></span>
<span class="yarn-line">J'aime bien faire du vélo le long de la Seine! <span class="yarn-meta">#line:0494564 </span></span>
<span class="yarn-line">Paryż jest piękną pięknością świata! <span class="yarn-meta">#line:0d94ea8 </span></span>
<span class="yarn-line">La Seine est très longue! <span class="yarn-meta">#line:0395113 </span></span>

</code></pre></div>

<a id="ys-node-facts-bridges"></a>
## facts_bridges

<div class="yarn-node" data-title="facts_bridges"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_trains&gt;&gt;</span>
<span class="yarn-line">Mosty mogą przewozić szybkie pociągi nad wodą. <span class="yarn-meta">#line:035a1f6 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_cars&gt;&gt;</span>
<span class="yarn-line">Na niektórych mostach jest duży ruch samochodowy i autobusowy. <span class="yarn-meta">#line:0723c02 </span></span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">Niektóre mosty służą wyłącznie do spacerów i podziwiania widoków. <span class="yarn-meta">#line:01d93bc </span></span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">Na moście Aleksandra III znajdują się złote posągi. <span class="yarn-meta">#line:0ba2ad8 </span></span>

</code></pre></div>

<a id="ys-node-facts-river"></a>
## facts_river

<div class="yarn-node" data-title="facts_river"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card seine_map_in_paris&gt;&gt;</span>
<span class="yarn-line">Sekwana wije się przez Paryż niczym wstęga. <span class="yarn-meta">#line:09ee6d9 </span></span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">Rzeka bierze swój początek daleko stąd i kończy się w kanale La Manche. <span class="yarn-meta">#line:074efa5 </span></span>
<span class="yarn-line">Rzeki przynoszą łodzie, wodę i życie do miasta. <span class="yarn-meta">#line:0bdfb18 </span></span>

</code></pre></div>

<a id="ys-node-facts-boats"></a>
## facts_boats

<div class="yarn-node" data-title="facts_boats"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Statki turystyczne mają szerokie okna, przez które można oglądać zabytki. <span class="yarn-meta">#line:0323421 </span></span>
<span class="yarn-cmd">&lt;&lt;card boat_river&gt;&gt;</span>
<span class="yarn-line">Niektóre łodzie po prostu poruszają się cicho. <span class="yarn-meta">#line:09a6062 </span></span>
<span class="yarn-line">Łodzie mogą przewozić ludzi i rzeczy, a także pełnić funkcję domów. <span class="yarn-meta">#line:0b7a149 </span></span>

</code></pre></div>

<a id="ys-node-spawned-bridge-expert"></a>
## spawned_bridge_expert

<div class="yarn-node" data-title="spawned_bridge_expert"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: bridge_expert</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Most kolejowy musi być solidny. <span class="yarn-meta">#line:0816b6b </span></span>
<span class="yarn-line">Niektóre mosty są przeznaczone tylko dla stóp. <span class="yarn-meta">#line:0ba2f29 </span></span>
<span class="yarn-line">Samochody przejeżdżają przez rzekę codziennie. <span class="yarn-meta">#line:08d8ef0 </span></span>

</code></pre></div>

<a id="ys-node-spawned-river-friend"></a>
## spawned_river_friend

<div class="yarn-node" data-title="spawned_river_friend"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: river_friend</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sekwana ułatwia statkom przemieszczanie się przez Paryż. <span class="yarn-meta">#line:0ec822e </span></span>
<span class="yarn-line">Mapy pokazują jak rzeka się zakręca. <span class="yarn-meta">#line:06d2470 </span></span>
<span class="yarn-line">Woda z rzeki wpływa do morza. <span class="yarn-meta">#line:0d1724c </span></span>

</code></pre></div>

<a id="ys-node-spawned-boat-guide"></a>
## spawned_boat_guide

<div class="yarn-node" data-title="spawned_boat_guide"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: boat_guide</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Statki turystyczne mają duże okna. <span class="yarn-meta">#line:047502b </span></span>
<span class="yarn-line">Statki towarowe przewożą wiele pudeł. <span class="yarn-meta">#line:0ef47e5 </span></span>
<span class="yarn-line">Dom na wodzie może być domem. <span class="yarn-meta">#line:00728bb </span></span>

</code></pre></div>


