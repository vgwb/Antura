---
title: Parigi - La Senna (fr_10) - Script
hide:
---

# Parigi - La Senna (fr_10) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://github.com/vgwb/Antura/discussions/634)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=754141150#gid=754141150)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_10%20Paris%20Seine/FR_10%20Paris%20Seine%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_10 | Seine (Paris)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_goods = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_people = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_house = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_alexandre = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_train = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_cars = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_people = 0&gt;&gt;</span>
<span class="yarn-line">Benvenuti a Parigi! Parigi è la capitale della Francia. A Parigi scorre un fiume famosissimo, la Senna.</span> <span class="yarn-meta">#line:042160f </span>
<span class="yarn-line">Oggi scoprirete la Senna e i suoi numerosi ponti. Sulla Senna si possono ammirare imbarcazioni di ogni tipo.</span> <span class="yarn-meta">#line:0280e8f </span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;area area_all&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo, hai completato questa missione!</span> <span class="yarn-meta">#line:0c408c3 </span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">Hai scoperto i ponti e le barche della Senna. I ponti permettono di attraversare il fiume che scorre attraverso Parigi: la Senna.</span> <span class="yarn-meta">#line:091e9fc </span>
<span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">Sono disponibili imbarcazioni per turisti. Hanno grandi finestre da cui si possono ammirare i monumenti di Parigi dalla Senna.</span> <span class="yarn-meta">#line:062cc35 </span>
<span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-line">Esistono case galleggianti. Le persone vivono su queste barche. Si chiamano chiatte.</span> <span class="yarn-meta">#line:0a29742 </span>
<span class="yarn-cmd">&lt;&lt;card seine_map_in_paris&gt;&gt;</span>
<span class="yarn-line">Importanti monumenti di Parigi sono stati costruiti intorno alla Senna.</span> <span class="yarn-meta">#line:08cc02a </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Disegna un ponte sul tuo quaderno!</span> <span class="yarn-meta">#line:0b37e92 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-protagonist"></a>

## protagonist

<div class="yarn-node" data-title="protagonist">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_photos")&gt;&gt;</span>
<span class="yarn-line">    Grazie!</span> <span class="yarn-meta">#line:0a70250</span>
<span class="yarn-line">    Hai trovato tutte le foto della mia guida!</span> <span class="yarn-meta">#line:07cac48 </span>
<span class="yarn-line">    Finalmente posso tornare a casa! Ho un'ultima domanda…</span> <span class="yarn-meta">#line:08b0aea </span>
    <span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("collect_cards")&gt;&gt;</span>
<span class="yarn-line">    Grazie!</span> <span class="yarn-meta">#shadow:0a70250 </span>
<span class="yarn-line">    Ma mi mancano 7 foto di ponti e barche sulla Senna!</span> <span class="yarn-meta">#line:006ab27 </span>
    <span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
<span class="yarn-line">    Fate una passeggiata lungo le rive della Senna e andate a vedere le foto!</span> <span class="yarn-meta">#line:084c7e4 </span>
<span class="yarn-line">    Se non riuscite a trovarli, potete usare la mappa di Parigi per localizzarli!</span> <span class="yarn-meta">#line:0bfcb05 </span>
    <span class="yarn-cmd">&lt;&lt;task_start find_photos task_find_photos_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "find_photos" or GetCurrentTask() == "collect_cards"&gt;&gt;</span>
<span class="yarn-line">    Stai andando bene!</span> <span class="yarn-meta">#line:03ea48b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Aiuto! Aiutatemi!</span> <span class="yarn-meta">#line:0aa0f3a </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_intro&gt;&gt;</span>
<span class="yarn-line">    Sono in visita a Parigi. Sono un turista. Il cane Antura mi ha rubato il libro su Parigi!</span> <span class="yarn-meta">#line:0210ef0</span>
<span class="yarn-line">    Le pagine del mio libro e le foto sono cadute vicino alla Senna!</span> <span class="yarn-meta">#line:0c8e7b6</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive antura false&gt;&gt;</span>
<span class="yarn-line">    Avvicinatevi al fiume e troverete le foto!</span> <span class="yarn-meta">#line:0a9c8e7 #task:collect_cards</span>
    <span class="yarn-cmd">&lt;&lt;area area_tutorial&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start collect_cards task_collect_cards_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-collect-cards-done"></a>

## task_collect_cards_done

<div class="yarn-node" data-title="task_collect_cards_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai trovato tutte le foto!</span> <span class="yarn-meta">#line:found_photos</span>
<span class="yarn-line">Torna indietro e parla con il turista.</span> <span class="yarn-meta">#line:go_back_tourist #task:go_back</span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-find-photos-done"></a>

## task_find_photos_done

<div class="yarn-node" data-title="task_find_photos_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai trovato tutte le foto!</span> <span class="yarn-meta">#shadow:found_photos</span>
<span class="yarn-line">Torna indietro e parla con il turista.</span> <span class="yarn-meta">#shadow:go_back_tourist #task:go_back</span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-question-seine-map"></a>

## question_seine_map

<div class="yarn-node" data-title="question_seine_map">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Il fiume che attraversa Parigi si chiama...</span> <span class="yarn-meta">#line:066d7bd </span>
<span class="yarn-choice">-&gt; Il Mar Mediterraneo</span> <span class="yarn-meta">#line:02ab1e0 </span>
<span class="yarn-line">    Non è la risposta corretta. La Loira attraversa Nantes, vicino al Museo Jules Verne.</span> <span class="yarn-meta">#line:099fe86 </span>
    <span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>
<span class="yarn-choice">-&gt; Il Canale della Manica</span> <span class="yarn-meta">#line:03c92db </span>
    <span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">    Sì, è la Senna. Il fiume che attraversa Parigi si chiama Senna.</span> <span class="yarn-meta">#line:0a2bcd3</span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-choice">-&gt; L'Oceano Atlantico</span> <span class="yarn-meta">#line:052fdac </span>
<span class="yarn-line">    Non è la risposta corretta. La Loira attraversa Nantes, vicino al Museo Jules Verne.</span> <span class="yarn-meta">#shadow:099fe86 </span>
    <span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>
<span class="yarn-choice">-&gt; Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-goods"></a>

## npc_boat_goods

<div class="yarn-node" data-title="npc_boat_goods">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "find_photos"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_for_goods&gt;&gt;</span>
<span class="yarn-line">    Questa barca trasporta merci.</span> <span class="yarn-meta">#line:0f13a44</span>
    &lt;&lt;if $boat_goods &lt; 10&gt;&gt;
<span class="yarn-line">    Cosa si può trovare su una nave mercantile?</span> <span class="yarn-meta">#line:04059bd </span>
<span class="yarn-choice">        -&gt; Una nave mercantile può trasportare: legno, sabbia, ma anche oggetti: automobili, mobili...</span> <span class="yarn-meta">#line:0b12cb3</span>
        <span class="yarn-cmd">&lt;&lt;set $boat_goods = 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;target chest_boat_goods&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_boat_goods&gt;&gt;</span>
<span class="yarn-line">        Sì! Ora puoi aprire quello scrigno!</span> <span class="yarn-meta">#line:yes_chest</span>
        <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-choice">        -&gt; Turisti con le macchine fotografiche</span> <span class="yarn-meta">#line:0bbd3ce </span>
<span class="yarn-line">        No. Riprova.</span> <span class="yarn-meta">#line:try_again </span>
        <span class="yarn-cmd">&lt;&lt;jump npc_boat_goods&gt;&gt;</span>
<span class="yarn-choice">        -&gt; Vagoni ferroviari</span> <span class="yarn-meta">#line:06638a3 </span>
<span class="yarn-line">        No. Riprova.</span> <span class="yarn-meta">#shadow:try_again </span>
        <span class="yarn-cmd">&lt;&lt;jump npc_boat_goods&gt;&gt;</span>
<span class="yarn-choice">        -&gt; Non lo so</span> <span class="yarn-meta">#line:dont_know #highlight </span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Torna più tardi.</span> <span class="yarn-meta">#line:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-goods"></a>

## chest_boat_goods

<div class="yarn-node" data-title="chest_boat_goods">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_goods == 1&gt;&gt;</span>
<span class="yarn-line">    Gioca a un minigioco per aprire lo scrigno!</span> <span class="yarn-meta">#line:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_goods = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_boats chest_boat_goods&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_goods == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_boat_goods&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_for_goods&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre. Trovi una foto!</span> <span class="yarn-meta">#line:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_goods = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_goods == 10&gt;&gt;</span>
<span class="yarn-line">    La cassa è vuota.</span> <span class="yarn-meta">#line:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#line:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-house"></a>

## npc_boat_house

<div class="yarn-node" data-title="npc_boat_house">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-line">Questa barca è una casa. Ci sono persone che vivono su questa barca. Questa barca si chiama casa galleggiante.</span> <span class="yarn-meta">#line:0a627c2 </span>
&lt;&lt;if $boat_house &lt; 10&gt;&gt;
<span class="yarn-line">Perché questa imbarcazione si chiama casa galleggiante?</span> <span class="yarn-meta">#line:0165157 </span>
<span class="yarn-choice">-&gt; Si tratta di un ponte ferroviario.</span> <span class="yarn-meta">#line:055d822 </span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_house&gt;&gt;</span>
<span class="yarn-choice">-&gt; È possibile viverci.</span> <span class="yarn-meta">#line:0c86d26 </span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_boat_house&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_boat_house&gt;&gt;</span>
<span class="yarn-line">    Sì! Ora puoi aprire quello scrigno!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-choice">-&gt; Questa barca sta trasportando merci</span> <span class="yarn-meta">#line:036ba15 </span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_house&gt;&gt;</span>
<span class="yarn-choice">-&gt; Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-house"></a>

## chest_boat_house

<div class="yarn-node" data-title="chest_boat_house">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_house == 1&gt;&gt;</span>
<span class="yarn-line">    Gioca a un minigioco per aprire lo scrigno!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_boat_house chest_boat_house&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_boat_house&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 10&gt;&gt;</span>
<span class="yarn-line">    La cassa è vuota.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-people"></a>

## npc_boat_people

<div class="yarn-node" data-title="npc_boat_people">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">Questa imbarcazione ha grandi finestre. I turisti possono ammirare la città. È un "battello fluviale".</span> <span class="yarn-meta">#line:0129c99 </span>
&lt;&lt;if $boat_people &lt; 10&gt;&gt;
<span class="yarn-line">Perché una barca turistica ha finestre così grandi?</span> <span class="yarn-meta">#line:02ce3de </span>
<span class="yarn-choice">-&gt; Per trasportare le auto</span> <span class="yarn-meta">#line:02349a3</span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_people&gt;&gt;</span>
<span class="yarn-choice">-&gt; Andare sott'acqua</span> <span class="yarn-meta">#line:037efcf</span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_people&gt;&gt;</span>
<span class="yarn-choice">-&gt; Per vedere la città e i luoghi famosi</span> <span class="yarn-meta">#line:0454688 </span>
    <span class="yarn-cmd">&lt;&lt;set $boat_people = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_boat_people&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_boat_people&gt;&gt;</span>
<span class="yarn-line">    Sì! Ora puoi aprire quello scrigno!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-choice">-&gt; Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-people"></a>

## chest_boat_people

<div class="yarn-node" data-title="chest_boat_people">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_people == 1&gt;&gt;</span>
<span class="yarn-line">    Gioca a un minigioco per aprire lo scrigno!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_people = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_seine chest_boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_people == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_boat_people&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_people = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_people == 10&gt;&gt;</span>
<span class="yarn-line">    La cassa è vuota.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bridge-alexandre"></a>

## npc_bridge_alexandre

<div class="yarn-node" data-title="npc_bridge_alexandre">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">Questo è il Ponte Alessandro III. È un ponte molto famoso.</span> <span class="yarn-meta">#line:09c706f</span>
&lt;&lt;if $bridge_alexandre &lt; 10&gt;&gt;
<span class="yarn-line">Perché questo ponte è famoso?</span> <span class="yarn-meta">#line:021f382</span>
<span class="yarn-choice">-&gt; Ci sono statue fatte d'oro.</span> <span class="yarn-meta">#line:056c933</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_alexandre = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_alexandre&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_bridge_alexandre&gt;&gt;</span>
<span class="yarn-line">    Sì! Ora puoi aprire quello scrigno!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-choice">-&gt; Si tratta di un ponte ferroviario.</span> <span class="yarn-meta">#line:05c2518</span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_alexandre&gt;&gt;</span>
<span class="yarn-choice">-&gt; Si tratta di un ponte per automobili.</span> <span class="yarn-meta">#line:0d9bc29</span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_alexandre&gt;&gt;</span>
<span class="yarn-choice">-&gt; Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-alexandre"></a>

## chest_bridge_alexandre

<div class="yarn-node" data-title="chest_bridge_alexandre">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_alexandre == 1&gt;&gt;</span>
<span class="yarn-line">    Gioca a un minigioco per aprire lo scrigno!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_alexandre = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_pont chest_bridge_alexandre&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_alexandre == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_bridge_alexandre&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_alexandre = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_alexandre == 10&gt;&gt;</span>
<span class="yarn-line">    La cassa è vuota.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bridge-cars"></a>

## npc_bridge_cars

<div class="yarn-node" data-title="npc_bridge_cars">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_cars&gt;&gt;</span>
<span class="yarn-line">È un ponte per auto!</span> <span class="yarn-meta">#line:0d79d7d </span>
&lt;&lt;if $bridge_cars &lt; 10&gt;&gt;
<span class="yarn-line">Le macchine vanno veloci. Come posso attraversare in sicurezza?</span> <span class="yarn-meta">#line:01f30c4</span>
<span class="yarn-choice">-&gt; Puoi attraversare la strada ovunque.</span> <span class="yarn-meta">#line:076ee7f </span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_cars&gt;&gt;</span>
<span class="yarn-choice">-&gt; È obbligatorio camminare sul marciapiede e attraversare sulle strisce pedonali.</span> <span class="yarn-meta">#line:0c8cdae </span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_cars = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_cars&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_bridge_cars&gt;&gt;</span>
<span class="yarn-line">    Sì! Ora puoi aprire quello scrigno!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-choice">-&gt; Devi camminare sulla strada</span> <span class="yarn-meta">#line:0cf371b</span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_cars&gt;&gt;</span>
<span class="yarn-choice">-&gt; Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-cars"></a>

## chest_bridge_cars

<div class="yarn-node" data-title="chest_bridge_cars">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_cars == 1&gt;&gt;</span>
<span class="yarn-line">    Gioca a un minigioco per aprire lo scrigno!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_cars = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_bridge_cars chest_bridge_cars&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_cars == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_bridge_cars&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card place_bridge_cars&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_cars = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_cars == 10&gt;&gt;</span>
<span class="yarn-line">    La cassa è vuota.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bridge-train"></a>

## npc_bridge_train

<div class="yarn-node" data-title="npc_bridge_train">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_trains&gt;&gt;</span>
<span class="yarn-line">È un ponte ferroviario!</span> <span class="yarn-meta">#line:0a0991d </span>
&lt;&lt;if $bridge_train &lt; 10&gt;&gt;
<span class="yarn-line">Un ponte ferroviario è un ponte...</span> <span class="yarn-meta">#line:09731f9 </span>
<span class="yarn-choice">-&gt; per auto</span> <span class="yarn-meta">#line:08b4fad </span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_train&gt;&gt;</span>
<span class="yarn-choice">-&gt; Solo per camminare</span> <span class="yarn-meta">#line:03ff912</span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_train&gt;&gt;</span>
<span class="yarn-choice">-&gt; Molto forte</span> <span class="yarn-meta">#line:018a0eb </span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_train&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_bridge_train&gt;&gt;</span>
<span class="yarn-line">    Sì! Ora puoi aprire quello scrigno!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-choice">-&gt; Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-train"></a>

## chest_bridge_train

<div class="yarn-node" data-title="chest_bridge_train">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_train == 1&gt;&gt;</span>
<span class="yarn-line">    Gioca a un minigioco per aprire lo scrigno!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_bridges chest_bridge_train&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_bridge_train&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card place_bridge_trains&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 10&gt;&gt;</span>
<span class="yarn-line">    La cassa è vuota.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-train"></a>

## npc_train

<div class="yarn-node" data-title="npc_train">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi piacerebbe tantissimo viaggiare in treno!</span> <span class="yarn-meta">#line:01663fa </span>
<span class="yarn-line">Adoro i treni!</span> <span class="yarn-meta">#line:0574872 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-bridge-people"></a>

## npc_bridge_people

<div class="yarn-node" data-title="npc_bridge_people">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">Questo è un ponte per le persone!</span> <span class="yarn-meta">#line:0f8a96f </span>
&lt;&lt;if $bridge_people &lt; 10&gt;&gt;
<span class="yarn-line">Un ponte su cui possono camminare solo le persone si chiama...</span> <span class="yarn-meta">#line:0484ee9 </span>
<span class="yarn-choice">-&gt; ponte ferroviario</span> <span class="yarn-meta">#line:0f74c53 </span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_people&gt;&gt;</span>
<span class="yarn-choice">-&gt; Passerella</span> <span class="yarn-meta">#line:0e48593 </span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_people = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_people&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_bridge_people&gt;&gt;</span>
<span class="yarn-line">    Sì! Ora puoi aprire quello scrigno!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-choice">-&gt; un ponte per auto</span> <span class="yarn-meta">#line:07f732f </span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_people&gt;&gt;</span>
<span class="yarn-choice">-&gt; Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-people"></a>

## chest_bridge_people

<div class="yarn-node" data-title="chest_bridge_people">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_people == 1&gt;&gt;</span>
<span class="yarn-line">    Gioca a un minigioco per aprire lo scrigno!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_people = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity match_bridges chest_bridge_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_people == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_bridge_people&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">    Il forziere si apre. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_people = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_people == 10&gt;&gt;</span>
<span class="yarn-line">    La cassa è vuota.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Lo scrigno è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-seine-map"></a>

## npc_seine_map

<div class="yarn-node" data-title="npc_seine_map">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card seine_map_in_paris&gt;&gt;</span>
<span class="yarn-line">Osservate questa mappa della Senna.</span> <span class="yarn-meta">#line:08b1177 </span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">La Senna sfocia nel Canale della Manica (un mare) in direzione nord.</span> <span class="yarn-meta">#line:0281b33 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat"></a>

## npc_boat

<div class="yarn-node" data-title="npc_boat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">No, non ho visto il tuo cane.</span> <span class="yarn-meta">#line:0614aef </span>
<span class="yarn-line">Un cane? Qui ci sono molti cani...</span> <span class="yarn-meta">#line:09f3f7a </span>
<span class="yarn-line">Sì, ho visto un cane che gironzolava.</span> <span class="yarn-meta">#line:02ed36d </span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-eiffel"></a>

## npc_boat_eiffel

<div class="yarn-node" data-title="npc_boat_eiffel">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Guarda quanto è bello!</span> <span class="yarn-meta">#line:059f66d </span>

</code>
</pre>
</div>

<a id="ys-node-npc-ile-de-france"></a>

## npc_ile_de_france

<div class="yarn-node" data-title="npc_ile_de_france">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ile_de_france&gt;&gt;</span>
<span class="yarn-line">C'è un'isola chiamata Île de la Cité.</span> <span class="yarn-meta">#line:03e5667 </span>
<span class="yarn-line">Si trova nel centro storico di Parigi.</span> <span class="yarn-meta">#line:04b2cf2 </span>

</code>
</pre>
</div>

<a id="ys-node-facts-river"></a>

## facts_river

<div class="yarn-node" data-title="facts_river">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La Senna sfocia nel Canale della Manica.</span> <span class="yarn-meta">#line:074efa5 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-french-woman"></a>

## spawned_french_woman

<div class="yarn-node" data-title="spawned_french_woman">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#line:072a345 </span>
<span class="yarn-line">La Seine est magnifique!</span> <span class="yarn-meta">#line:0f0f558 </span>
<span class="yarn-line">A Parigi ci sono molti ponti!</span> <span class="yarn-meta">#line:038b8f6 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-french-man"></a>

## spawned_french_man

<div class="yarn-node" data-title="spawned_french_man">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">CIAO!</span> <span class="yarn-meta">#line:06aa0c1 </span>
<span class="yarn-line">Mi piace molto andare in bicicletta lungo la Senna!</span> <span class="yarn-meta">#line:0494564 </span>
<span class="yarn-line">Parigi è la città più bella del mondo!</span> <span class="yarn-meta">#line:0d94ea8 </span>
<span class="yarn-line">La Senna è lunghissima!</span> <span class="yarn-meta">#line:0395113 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-bridge-expert"></a>

## spawned_bridge_expert

<div class="yarn-node" data-title="spawned_bridge_expert">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: bridge_expert</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Un ponte ferroviario deve essere robusto.</span> <span class="yarn-meta">#line:0816b6b </span>
<span class="yarn-line">Alcuni ponti sono destinati esclusivamente ai pedoni.</span> <span class="yarn-meta">#line:0ba2f29 </span>
<span class="yarn-line">Ogni giorno le auto attraversano il fiume.</span> <span class="yarn-meta">#line:08d8ef0 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-river-friend"></a>

## spawned_river_friend

<div class="yarn-node" data-title="spawned_river_friend">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: river_friend</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La Senna permette alle imbarcazioni di circolare a Parigi.</span> <span class="yarn-meta">#line:0ec822e </span>
<span class="yarn-line">Le mappe mostrano le anse del fiume.</span> <span class="yarn-meta">#line:06d2470 </span>
<span class="yarn-line">Il fiume sfocia nel mare.</span> <span class="yarn-meta">#line:0d1724c </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-boat-guide"></a>

## spawned_boat_guide

<div class="yarn-node" data-title="spawned_boat_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: boat_guide</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Le barche turistiche hanno grandi finestre.</span> <span class="yarn-meta">#line:047502b </span>
<span class="yarn-line">Le navi da carico trasportano molte casse.</span> <span class="yarn-meta">#line:0ef47e5 </span>
<span class="yarn-line">Una casa galleggiante può essere una vera casa.</span> <span class="yarn-meta">#line:00728bb </span>

</code>
</pre>
</div>


