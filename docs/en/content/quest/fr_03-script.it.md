---
title: Jules Verne e i trasporti (fr_03) - Script
hide:
---

# Jules Verne e i trasporti (fr_03) - Script
[Quest Index](./index.it.md) - Language: [english](./fr_03-script.md) - [french](./fr_03-script.fr.md) - [polish](./fr_03-script.pl.md) - italian

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-03-jules-verne-and-transportation/25/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=336647638#gid=336647638)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_03%20Nantes%20Verne/FR_03%20Nantes%20Verne%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// fr_03 | Jules Verne (Nantes)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - FIND_BOOKS (collect 4 Jules Verne books)</span>
<span class="yarn-header-dim">// - COLLECT_TRAIN (collect from "Around the World in 80 Days")</span>
<span class="yarn-header-dim">// - COLLECT_ROCKET (collect from "From Earth to the Moon")</span>
<span class="yarn-header-dim">// - COLLECT_SUBMARINE (collect from "20,000 Leagues Under the Sea")</span>
<span class="yarn-header-dim">// - COLLECT_BALLOON (collect from "Five Weeks in a Balloon")</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $QUEST_ITEMS = 4&gt;&gt;</span>
<span class="yarn-line">Benvenuti al museo di Jules Verne a Nantes! <span class="yarn-meta">#line:0b5e2f3</span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ora sai qualcosa su Jules Verne <span class="yarn-meta">#line:0174104 </span></span>
<span class="yarn-line">e i suoi libri! <span class="yarn-meta">#line:0a01f9e </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Leggete uno dei suoi libri! <span class="yarn-meta">#line:06521b4 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-guide"></a>
## talk_guide

<div class="yarn-node" data-title="talk_guide"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>

<span class="yarn-cmd">&lt;&lt;if $COLLECTED_ITEMS == 0&gt;&gt;</span>
<span class="yarn-line">    Benvenuti nella casa di Jules Verne! <span class="yarn-meta">#line:08f7bc1 </span></span>
    <span class="yarn-cmd">&lt;&lt;card jules_verne_1&gt;&gt;</span>
&lt;&lt;elseif $COLLECTED_ITEMS &lt; $QUEST_ITEMS&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump task_find_books&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump won&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-find-books"></a>
## task_find_books

<div class="yarn-node" data-title="task_find_books"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset jverne_mission_overview&gt;&gt;</span>
<span class="yarn-line">Esplora la casa e trova quattro dei suoi libri! <span class="yarn-meta">#line:0aac249 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_BOOKS task_find_books_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-find-books-done"></a>
## task_find_books_done

<div class="yarn-node" data-title="task_find_books_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai trovato tutti i libri! <span class="yarn-meta">#line:0fc503c </span></span>
<span class="yarn-line">VAI a parlare con la guida! <span class="yarn-meta">#line:01b0c19 </span></span>

</code></pre></div>

<a id="ys-node-verne-painting"></a>
## verne_painting

<div class="yarn-node" data-title="verne_painting"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=jules_verne_1</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questo è Jules Verne. Era uno scrittore. <span class="yarn-meta">#line:096a3b3 </span></span>

</code></pre></div>

<a id="ys-node-verne-house"></a>
## verne_house

<div class="yarn-node" data-title="verne_house"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=jules_verne_house</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Nacque a Nantes nel 1828. <span class="yarn-meta">#line:003b311 </span></span>

</code></pre></div>

<a id="ys-node-map-nantes"></a>
## map_nantes

<div class="yarn-node" data-title="map_nantes"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=map_nantes</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questa è una mappa di Nantes. <span class="yarn-meta">#line:09bcaba </span></span>

</code></pre></div>

<a id="ys-node-open-chest"></a>
## open_chest

<div class="yarn-node" data-title="open_chest"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 4&gt;&gt;
<span class="yarn-cmd">&lt;&lt;jump won&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump task_find_books&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-won"></a>
## won

<div class="yarn-node" data-title="won"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset jules_verne_2&gt;&gt;</span>
<span class="yarn-line">Fantastico! Hai incontrato Jules Verne, <span class="yarn-meta">#line:099cdca </span></span>
<span class="yarn-line">lo scrittore di fantascienza. <span class="yarn-meta">#line:05a032e </span></span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-book-80days"></a>
## book_80days

<div class="yarn-node" data-title="book_80days"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset book_80days&gt;&gt;</span>
<span class="yarn-line">Questo libro è "Il giro del mondo in 80 giorni". <span class="yarn-meta">#line:03131e3</span></span>
<span class="yarn-cmd">&lt;&lt;jump train&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-train"></a>
## train

<div class="yarn-node" data-title="train"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset train&gt;&gt;</span>
<span class="yarn-line">Questo è un vecchio treno. <span class="yarn-meta">#line:0732ebc </span></span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_TRAIN&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-train"></a>
## npc_train

<div class="yarn-node" data-title="npc_train"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questo vecchio treno utilizzava il VAPORE per muoversi. <span class="yarn-meta">#line:0d10edc </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_around_the_world_80_days&gt;&gt;</span>
<span class="yarn-line">I treni hanno reso i viaggi lunghi più rapidi. <span class="yarn-meta">#line:00a9db2 </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_around_the_world_80_days&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-paint-moon"></a>
## paint_moon

<div class="yarn-node" data-title="paint_moon"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card rocket&gt;&gt;</span>
<span class="yarn-line">Questo è un razzo spaziale. <span class="yarn-meta">#line:0e5ae78 </span></span>

</code></pre></div>

<a id="ys-node-book-moon"></a>
## book_moon

<div class="yarn-node" data-title="book_moon"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset book_earthmoon&gt;&gt;</span>
<span class="yarn-line">Questo libro è "Dalla Terra alla Luna". <span class="yarn-meta">#line:06df7d0 </span></span>
<span class="yarn-cmd">&lt;&lt;jump paint_moon&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-rocket"></a>
## npc_rocket

<div class="yarn-node" data-title="npc_rocket"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Un razzo deve spingere molto forte per lasciare la Terra. <span class="yarn-meta">#line:06b6d4d </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_from_earth_to_moon&gt;&gt;</span>
<span class="yarn-line">Jules Verne immaginò presto i viaggi spaziali. <span class="yarn-meta">#line:0cd7302 </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_from_earth_to_moon&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-book-20000legues"></a>
## book_20000legues

<div class="yarn-node" data-title="book_20000legues"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset book_underthesea&gt;&gt;</span>
<span class="yarn-line">Questo libro è "20.000 leghe sotto i mari". <span class="yarn-meta">#line:03536a1 </span></span>
<span class="yarn-cmd">&lt;&lt;jump paint_20000&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-paint-20000"></a>
## paint_20000

<div class="yarn-node" data-title="paint_20000"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset submarine&gt;&gt;</span>
<span class="yarn-line">Questo è un sottomarino. <span class="yarn-meta">#line:0f298c2 </span></span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_SUBMARINE&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-submarine"></a>
## npc_submarine

<div class="yarn-node" data-title="npc_submarine"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Un sottomarino viaggia sott'acqua. <span class="yarn-meta">#line:0dcb855 </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_20000_leagues_under_the_sea&gt;&gt;</span>
<span class="yarn-line">Il Nautilus è la nave del capitano Nemo. <span class="yarn-meta">#line:0d69bb8 </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_20000_leagues_under_the_sea&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-paint-5weeks"></a>
## paint_5weeks

<div class="yarn-node" data-title="paint_5weeks"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card balloon&gt;&gt;</span>
<span class="yarn-line">Questa è una mongolfiera. <span class="yarn-meta">#line:06a7709 </span></span>

</code></pre></div>

<a id="ys-node-book-5weeks"></a>
## book_5weeks

<div class="yarn-node" data-title="book_5weeks"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card book_5weeksballoon&gt;&gt;</span>
<span class="yarn-line">Questo libro si intitola "Cinque settimane in mongolfiera". <span class="yarn-meta">#line:0934a7c </span></span>
<span class="yarn-cmd">&lt;&lt;jump paint_5weeks&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-balloon"></a>
## npc_balloon

<div class="yarn-node" data-title="npc_balloon"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Una mongolfiera si solleva con l'aria calda. <span class="yarn-meta">#line:0131b99 </span></span>
    <span class="yarn-cmd">&lt;&lt;card hot_air_balloon&gt;&gt;</span>
<span class="yarn-line">Si muove con il vento. <span class="yarn-meta">#line:09a8c21 </span></span>
    <span class="yarn-cmd">&lt;&lt;card hot_air_balloon&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spawned-visitor"></a>
## spawned_visitor

<div class="yarn-node" data-title="spawned_visitor"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: generic </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Adoro leggere libri! <span class="yarn-meta">#line:00f3a57 </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_around_the_world_80_days&gt;&gt;</span>
<span class="yarn-line">Sapevi che Jules Verne è considerato uno dei padri della fantascienza? <span class="yarn-meta">#line:056e79e </span></span>
    <span class="yarn-cmd">&lt;&lt;card jules_verne&gt;&gt;</span>
<span class="yarn-line">Ho sentito dire che Jules Verne ha scritto più di 60 romanzi nella sua vita! <span class="yarn-meta">#line:0caca1b </span></span>
    <span class="yarn-cmd">&lt;&lt;card jules_verne&gt;&gt;</span>
<span class="yarn-line">Ho letto che le opere di Jules Verne sono state tradotte in più di 140 lingue! <span class="yarn-meta">#line:0f5f36d </span></span>
    <span class="yarn-cmd">&lt;&lt;card jules_verne&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spawned-kid-visitor"></a>
## spawned_kid_visitor

<div class="yarn-node" data-title="spawned_kid_visitor"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi piace la storia del giro del mondo. <span class="yarn-meta">#line:0e81901 </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_around_the_world_80_days&gt;&gt;</span>
<span class="yarn-line">Il sottomarino Nautilus sembra fantastico. <span class="yarn-meta">#line:08669ce </span></span>
    <span class="yarn-cmd">&lt;&lt;card submarine_nautilus&gt;&gt;</span>
<span class="yarn-line">Un giorno vorrei fare un giro in mongolfiera. <span class="yarn-meta">#line:00be1f0 </span></span>
    <span class="yarn-cmd">&lt;&lt;card hot_air_balloon&gt;&gt;</span>
<span class="yarn-line">Il razzo diretto alla Luna sembra molto veloce. <span class="yarn-meta">#line:07ee86b </span></span>
    <span class="yarn-cmd">&lt;&lt;card space_rocket&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spawned-guide-woman"></a>
## spawned_guide_woman

<div class="yarn-node" data-title="spawned_guide_woman"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: guides </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Il giro del mondo in 80 giorni mostra molti luoghi della Terra. <span class="yarn-meta">#line:0bcc84d </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_around_the_world_80_days&gt;&gt;</span>
<span class="yarn-line">Il Nautilus è il sottomarino presente nel film 20.000 leghe sotto i mari. <span class="yarn-meta">#line:0a998ac </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_20000_leagues_under_the_sea&gt;&gt;</span>
<span class="yarn-line">Jules Verne immaginò i viaggi spaziali prima dell'avvento dei veri razzi. <span class="yarn-meta">#line:01e7e5c </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_from_earth_to_moon&gt;&gt;</span>
<span class="yarn-line">Five Weeks in a Balloon racconta di un viaggio aereo sopra l'Africa. <span class="yarn-meta">#line:09e090d </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_five_weeks_in_a_balloon&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spawned-guide-man"></a>
## spawned_guide_man

<div class="yarn-node" data-title="spawned_guide_man"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: guides </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dalla Terra alla Luna racconta di un enorme cannone spaziale. <span class="yarn-meta">#line:0f07e41 </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_from_earth_to_moon&gt;&gt;</span>
<span class="yarn-line">In 20.000 leghe sotto i mari troviamo il capitano Nemo e il Nautilus. <span class="yarn-meta">#line:09b9d24 </span></span>
    <span class="yarn-cmd">&lt;&lt;card book_20000_leagues_under_the_sea&gt;&gt;</span>
<span class="yarn-line">Una mongolfiera sale perché l'aria calda è leggera. <span class="yarn-meta">#line:0281b73 </span></span>
    <span class="yarn-cmd">&lt;&lt;card hot_air_balloon&gt;&gt;</span>
<span class="yarn-line">Molte idee contenute nei suoi libri si sono trasformate in tecnologia reale. <span class="yarn-meta">#line:06e1473 </span></span>
    <span class="yarn-cmd">&lt;&lt;card jules_verne&gt;&gt;</span>

</code></pre></div>


