---
title: Jules Verne et les transports (fr_03) - Script
hide:
---

# Jules Verne et les transports (fr_03) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-03-jules-verne-and-transportation/25/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=336647638#gid=336647638)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_03%20Nantes%20Verne/FR_03%20Nantes%20Verne%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_03 | Jules Verne (Nantes)</span>
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
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $QUEST_ITEMS = 4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $museum_clean = false&gt;&gt;</span>
<span class="yarn-line">Siamo a NANTES, al MUSEO Jules Verne!</span> <span class="yarn-meta">#line:0b5e2f3</span>
<span class="yarn-line">Scopriamo questo SCRITTORE e i suoi fantastici VIAGGI.</span> <span class="yarn-meta">#line:006e7fc</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_door&gt;&gt;</span>
<span class="yarn-comment">// DEBUG</span>
<span class="yarn-comment">//&lt;&lt;set $museum_clean= true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;SetActive museum_door false&gt;&gt;</span>

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
<span class="yarn-line">La missione è completata! Congratulazioni!</span> <span class="yarn-meta">#line:08f0d37 </span>
<span class="yarn-line">Ora sai tutto su Jules Verne e i suoi LIBRI!</span> <span class="yarn-meta">#line:0174104</span>
<span class="yarn-line">Quando venite a Nantes, venite a visitare il MUSEO!</span> <span class="yarn-meta">#line:0f72ec7 </span>
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
<span class="yarn-line">Come attività in classe, puoi DISEGNARE un SOTTOMARINO o un RAZZO!</span> <span class="yarn-meta">#line:087acef </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-door"></a>

## npc_door

<div class="yarn-node" data-title="npc_door">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("clean_sidewalk")&gt;&gt;</span>
<span class="yarn-line">    Grazie per aver ripulito!</span> <span class="yarn-meta">#line:0cec96a </span>
<span class="yarn-line">    Il Museo è ora aperto! Parla con la GUIDA DEL MUSEO!</span> <span class="yarn-meta">#line:0d2e2f0 </span>
    <span class="yarn-cmd">&lt;&lt;SetActive museum_door false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target museum_guide&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Che DISASTRO fuori dal MUSEO!</span> <span class="yarn-meta">#line:0a3e2f4</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_sidewalk&gt;&gt;</span>
<span class="yarn-line">    Quel CANE blu stava correndo e ha combinato un pasticcio!</span> <span class="yarn-meta">#line:0210ef0 </span>
<span class="yarn-line">    Non posso aprire il MUSEO con questo casino qui.</span> <span class="yarn-meta">#line:0d58543 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">    Aiutami a pulire il marciapiede!</span> <span class="yarn-meta">#line:01a9d0f </span>
    <span class="yarn-cmd">&lt;&lt;SetActive antura false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive trash_down false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive trash_up true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_full&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start clean_sidewalk task_clean_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-clean-done"></a>

## task_clean_done

<div class="yarn-node" data-title="task_clean_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ben fatto! Torna dal portiere</span> <span class="yarn-meta">#line:0f8dd6c </span>
<span class="yarn-cmd">&lt;&lt;target npc_door&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-museum-guide"></a>

## museum_guide

<div class="yarn-node" data-title="museum_guide">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;area area_full&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if $museum_clean == false&gt;&gt;</span>
<span class="yarn-line">    Abbiamo un PROBLEMA!</span> <span class="yarn-meta">#line:0642734 </span>
<span class="yarn-line">    Un buffo CANE blu è entrato nel MUSEO e ha combinato un bel pasticcio!</span> <span class="yarn-meta">#line:0525efc</span>
<span class="yarn-line">    Puoi aiutarci a pulire il MUSEO?</span> <span class="yarn-meta">#line:09eafa1</span>
    <span class="yarn-cmd">&lt;&lt;task_start clean_museum task_clean_museum_done&gt;&gt;</span>
&lt;&lt;elseif $COLLECTED_ITEMS &gt;= $QUEST_ITEMS&gt;&gt;
<span class="yarn-line">    Grazie! Ora il MUSEO è in ordine</span> <span class="yarn-meta">#line:0c81640 </span>
<span class="yarn-line">    Jules Verne sarebbe orgoglioso di te!</span> <span class="yarn-meta">#line:0bc6c7b </span>
<span class="yarn-line">    Per favore abbina i LIBRI ai loro VEICOLI!</span> <span class="yarn-meta">#line:0245e0e </span>
    <span class="yarn-cmd">&lt;&lt;activity match_verne_vehicles activity_match_done&gt;&gt;</span>
&lt;&lt;elseif $COLLECTED_ITEMS &gt;0&gt;&gt;
<span class="yarn-line">    Stai andando bene!</span> <span class="yarn-meta">#line:0c52f3c </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Abbiamo un altro PROBLEMA!</span> <span class="yarn-meta">#line:0a7dfb3 </span>
<span class="yarn-line">    I nostri veicoli speciali sono scomparsi.</span> <span class="yarn-meta">#line:0bb46da </span>
<span class="yarn-line">    Per favore, parla con il guardiano della prima stanza.</span> <span class="yarn-meta">#line:0386706 </span>
    <span class="yarn-cmd">&lt;&lt;target tutor_20000&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-done"></a>

## activity_match_done

<div class="yarn-node" data-title="activity_match_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo lavoro! Hai abbinato tutti i LIBRI ai rispettivi VEICOLI.</span> <span class="yarn-meta">#line:0e86b01 </span>
<span class="yarn-line">Ora tutto è pronto per i visitatori!</span> <span class="yarn-meta">#line:00eefe0 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-clean-museum-done"></a>

## task_clean_museum_done

<div class="yarn-node" data-title="task_clean_museum_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Perfetto. Ora il Museo è pulito! Torna alla GUIDA DEL MUSEO.</span> <span class="yarn-meta">#line:0b5eb24 </span>
<span class="yarn-cmd">&lt;&lt;set $museum_clean = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target museum_guide&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-20000"></a>

## tutor_20000

<div class="yarn-node" data-title="tutor_20000">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// --------------------------------------------------------------------------- </span>
<span class="yarn-header-dim">// 20000 - SUBMARINE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_submarine")&gt;&gt;</span>
<span class="yarn-line">    Hai trovato il SOTTOMARINO!</span> <span class="yarn-meta">#line:0666ee2 </span>
<span class="yarn-line">    Ma è ROTTO a pezzi!</span> <span class="yarn-meta">#line:097c3fc </span>
<span class="yarn-line">    Puoi per favore sistemarlo?</span> <span class="yarn-meta">#line:02e7812 </span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_verne_submarine activity_submarine_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card submarine_nautilus&gt;&gt;</span>
<span class="yarn-line">    Aiuto! Il Nautilus è scomparso!</span> <span class="yarn-meta">#line:00a6154 </span>
<span class="yarn-line">    Trova il SOTTOMARINO</span> <span class="yarn-meta">#line:0bb264b</span>
<span class="yarn-line">    Se non riesci a trovarlo, USA LA MAPPA</span> <span class="yarn-meta">#line:05bc5e9 </span>
    <span class="yarn-cmd">&lt;&lt;SetActive submarine_lost true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start find_submarine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-submarine-done"></a>

## activity_submarine_done

<div class="yarn-node" data-title="activity_submarine_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_20000&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive submarine true&gt;&gt;</span>
<span class="yarn-line">L'hai riparato! Grazie!</span> <span class="yarn-meta">#line:0ba85d3 </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Ora passa al guardiano successivo.</span> <span class="yarn-meta">#line:09438f6 </span>
<span class="yarn-cmd">&lt;&lt;target tutor_5weeks&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-submarine-lost"></a>

## submarine_lost

<div class="yarn-node" data-title="submarine_lost">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card submarine_nautilus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = $COLLECTED_ITEMS + 1&gt;&gt;</span>
<span class="yarn-line">Ottimo! Ora riportalo al suo guardiano</span> <span class="yarn-meta">#line:0757b30 </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_20000&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-submarine"></a>

## submarine

<div class="yarn-node" data-title="submarine">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card submarine_nautilus&gt;&gt;</span>
<span class="yarn-line">Un SOTTOMARINO si muove sotto il MARE.</span> <span class="yarn-meta">#line:06aef1a </span>

</code>
</pre>
</div>

<a id="ys-node-book-20000"></a>

## book_20000

<div class="yarn-node" data-title="book_20000">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card book_20000_leagues_under_the_sea&gt;&gt;</span>
<span class="yarn-line">Questo LIBRO è "20.000 leghe sotto i mari".</span> <span class="yarn-meta">#line:03536a1 </span>
<span class="yarn-line">Un SOTTOMARINO viaggia SOTT'ACQUA.</span> <span class="yarn-meta">#line:0dcb855 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-5weeks"></a>

## tutor_5weeks

<div class="yarn-node" data-title="tutor_5weeks">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// --------------------------------------------------------------------------- </span>
<span class="yarn-header-dim">// 5 weeks - BALLOON</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_balloon")&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la MONGOLFIERA!</span> <span class="yarn-meta">#line:0286510</span>
    [MISSING TRANSLATION:     But it's BROKEN in pieces! #shadow:097c3fc]
    [MISSING TRANSLATION:     Can you please fix it? #shadow:02e7812]
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_verne_balloon activity_balloon_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("find_submarine")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card hot_air_balloon&gt;&gt;</span>
<span class="yarn-line">    Aiuto! Manca qualcosa!</span> <span class="yarn-meta">#line:0e928ab </span>
<span class="yarn-line">    Trova la MONGOLFIERA</span> <span class="yarn-meta">#line:0a1520b</span>
    [MISSING TRANSLATION:     If you can't find it, USE THE MAP #shadow:05bc5e9]
    <span class="yarn-cmd">&lt;&lt;SetActive balloon_lost true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start find_balloon&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Torna da me più tardi.</span> <span class="yarn-meta">#line:03af9a8 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-balloon-done"></a>

## activity_balloon_done

<div class="yarn-node" data-title="activity_balloon_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_5weeks&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive balloon true&gt;&gt;</span>
[MISSING TRANSLATION: You repaired it! Thank you! #shadow:0ba85d3]
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
[MISSING TRANSLATION: Now go to next guardian. #shadow:09438f6]
<span class="yarn-cmd">&lt;&lt;target tutor_earthmoon&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-balloon-lost"></a>

## balloon_lost

<div class="yarn-node" data-title="balloon_lost">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card hot_air_balloon&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = $COLLECTED_ITEMS + 1&gt;&gt;</span>
[MISSING TRANSLATION: Great! Now bring it back to its guardian #shadow:0757b30]
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_5weeks&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-balloon"></a>

## balloon

<div class="yarn-node" data-title="balloon">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card hot_air_balloon&gt;&gt;</span>
<span class="yarn-line">Un PALLONCINO sale con ARIA calda.</span> <span class="yarn-meta">#line:0131b99 </span>
<span class="yarn-line">Si muove con il VENTO.</span> <span class="yarn-meta">#line:09a8c21 </span>

</code>
</pre>
</div>

<a id="ys-node-book-5weeks"></a>

## book_5weeks

<div class="yarn-node" data-title="book_5weeks">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card book_5weeksballoon&gt;&gt;</span>
<span class="yarn-line">Questo LIBRO si intitola "Cinque settimane in mongolfiera".</span> <span class="yarn-meta">#line:0c3d777 </span>
<span class="yarn-line">Un PALLONCINO sale perché l'ARIA calda è leggera.</span> <span class="yarn-meta">#line:020d924 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-earthmoon"></a>

## tutor_earthmoon

<div class="yarn-node" data-title="tutor_earthmoon">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// --------------------------------------------------------------------------- </span>
<span class="yarn-header-dim">// EARTH MOON - ROCKET</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_rocket")&gt;&gt;</span>
<span class="yarn-line">    Hai trovato il RAZZO!</span> <span class="yarn-meta">#line:08fed68 </span>
    [MISSING TRANSLATION:     But it's BROKEN in pieces! #shadow:097c3fc]
    [MISSING TRANSLATION:     Can you please fix it? #shadow:02e7812]
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_verne_rocket activity_rocket_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("find_balloon")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card space_rocket&gt;&gt;</span>
<span class="yarn-line">    Aiuto! La nostra macchina più preziosa è scomparsa!</span> <span class="yarn-meta">#line:07c4369 </span>
<span class="yarn-line">    Trova il RAZZO SPAZIALE</span> <span class="yarn-meta">#line:03efbc0 </span>
    [MISSING TRANSLATION:     If you can't find it, USE THE MAP #shadow:05bc5e9]
    <span class="yarn-cmd">&lt;&lt;SetActive rocket_lost true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start find_rocket&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    [MISSING TRANSLATION:     Come back to me later. #shadow:03af9a8]
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-rocket-done"></a>

## activity_rocket_done

<div class="yarn-node" data-title="activity_rocket_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_earthmoon&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive rocket true&gt;&gt;</span>
[MISSING TRANSLATION: You repaired it! Thank you! #shadow:0ba85d3]
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
[MISSING TRANSLATION: Now go to next guardian. #shadow:09438f6]
<span class="yarn-cmd">&lt;&lt;target tutor_80days&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-rocket-lost"></a>

## rocket_lost

<div class="yarn-node" data-title="rocket_lost">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card space_rocket&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = $COLLECTED_ITEMS + 1&gt;&gt;</span>
[MISSING TRANSLATION: Great! Now bring it back to its guardian #shadow:0757b30]
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_earthmoon&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-rocket"></a>

## rocket

<div class="yarn-node" data-title="rocket">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card space_rocket&gt;&gt;</span>
<span class="yarn-line">Questo è un RAZZO spaziale.</span> <span class="yarn-meta">#line:0de00ff </span>
<span class="yarn-line">Un RAZZO spinge con forza per lasciare la TERRA.</span> <span class="yarn-meta">#line:06b6d4d </span>

</code>
</pre>
</div>

<a id="ys-node-book-earthmoon"></a>

## book_earthmoon

<div class="yarn-node" data-title="book_earthmoon">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card book_earthmoon&gt;&gt;</span>
<span class="yarn-line">Jules Verne immaginò i RAZZI e i VIAGGI nello spazio molto tempo fa.</span> <span class="yarn-meta">#line:0907ccb </span>
<span class="yarn-line">La storia ha un enorme CANNONE spaziale per lanciare il RAZZO</span> <span class="yarn-meta">#line:05e298d </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-80days"></a>

## tutor_80days

<div class="yarn-node" data-title="tutor_80days">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// --------------------------------------------------------------------------- </span>
<span class="yarn-header-dim">// 80 DAYS - TRAIN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_train")&gt;&gt;</span>
<span class="yarn-line">    Hai trovato il TRENO!</span> <span class="yarn-meta">#line:01f76f3 </span>
    [MISSING TRANSLATION:     But it's BROKEN in pieces! #shadow:097c3fc]
    [MISSING TRANSLATION:     Can you please fix it? #shadow:02e7812]
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_verne_train activity_train_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("find_balloon")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card train&gt;&gt;</span>
<span class="yarn-line">    Aiuto! Abbiamo perso il TRENO!</span> <span class="yarn-meta">#line:08cfbc0 </span>
<span class="yarn-line">    Trova il VECCHIO TRENO A VAPORE</span> <span class="yarn-meta">#line:036574d</span>
    [MISSING TRANSLATION:     If you can't find it, USE THE MAP #shadow:05bc5e9]
    <span class="yarn-cmd">&lt;&lt;SetActive train_lost true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start find_train&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    [MISSING TRANSLATION:     Come back to me later. #shadow:03af9a8]
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-train-done"></a>

## activity_train_done

<div class="yarn-node" data-title="activity_train_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_80days&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive train true&gt;&gt;</span>
[MISSING TRANSLATION: You repaired it! Thank you! #shadow:0ba85d3]
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Hai trovato tutti i veicoli!</span> <span class="yarn-meta">#line:084486e </span>
<span class="yarn-line">Torna alla GUIDA DEL MUSEO!</span> <span class="yarn-meta">#line:0ee83b2 #task:back_to_museum_guide</span>
<span class="yarn-cmd">&lt;&lt;task_start back_to_museum_guide&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target museum_guide&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-train-lost"></a>

## train_lost

<div class="yarn-node" data-title="train_lost">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = $COLLECTED_ITEMS + 1&gt;&gt;</span>
[MISSING TRANSLATION: Great! Now bring it back to its guardian #shadow:0757b30]
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_80days&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-train"></a>

## train

<div class="yarn-node" data-title="train">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train&gt;&gt;</span>
<span class="yarn-line">Un TRENO viaggia su ROTAIE.</span> <span class="yarn-meta">#line:0998347 </span>
<span class="yarn-line">Questo vecchio TRENO utilizzava il VAPORE per muoversi.</span> <span class="yarn-meta">#line:0b7b198 </span>
<span class="yarn-line">I TRENI hanno reso i viaggi lunghi più rapidi.</span> <span class="yarn-meta">#line:05130c0 </span>

</code>
</pre>
</div>

<a id="ys-node-book-80days"></a>

## book_80days

<div class="yarn-node" data-title="book_80days">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card book_80days&gt;&gt;</span>
<span class="yarn-line">Questo LIBRO è "Il giro del mondo in 80 giorni".</span> <span class="yarn-meta">#line:03131e3</span>
<span class="yarn-line">Mostra molti luoghi della TERRA.</span> <span class="yarn-meta">#line:0bcc84d </span>

</code>
</pre>
</div>

<a id="ys-node-verne-painting"></a>

## verne_painting

<div class="yarn-node" data-title="verne_painting">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// ---------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// OBJECTS</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questo è Jules Verne. Era uno SCRITTORE.</span> <span class="yarn-meta">#line:096a3b3 </span>

</code>
</pre>
</div>

<a id="ys-node-verne-house"></a>

## verne_house

<div class="yarn-node" data-title="verne_house">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jules Verne è nato qui a NANTES.</span> <span class="yarn-meta">#line:003b311 </span>

</code>
</pre>
</div>

<a id="ys-node-map-nantes"></a>

## map_nantes

<div class="yarn-node" data-title="map_nantes">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questa è una MAPPA di NANTES.</span> <span class="yarn-meta">#line:09bcaba </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-visitor"></a>

## spawned_visitor

<div class="yarn-node" data-title="spawned_visitor">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: visitors </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Adoro leggere LIBRI!</span> <span class="yarn-meta">#line:00f3a57 </span>
<span class="yarn-line">Jules Verne ha inventato storie di fantascienza.</span> <span class="yarn-meta">#line:056e79e </span>
<span class="yarn-line">Jules Verne ha scritto molti LIBRI!</span> <span class="yarn-meta">#line:0caca1b </span>
<span class="yarn-line">La gente legge i suoi LIBRI in molte lingue!</span> <span class="yarn-meta">#line:0f5f36d </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-visitor"></a>

## spawned_kid_visitor

<div class="yarn-node" data-title="spawned_kid_visitor">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: visitors </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi piace la storia sul MONDO.</span> <span class="yarn-meta">#line:0e81901 #card:book_around_the_world_80_days</span>
<span class="yarn-line">Il SOTTOMARINO Nautilus è incredibile.</span> <span class="yarn-meta">#line:08669ce #card:submarine_nautilus</span>
<span class="yarn-line">Un giorno vorrei fare un giro in MONGOLFIERA.</span> <span class="yarn-meta">#line:00be1f0 #card:hot_air_balloon</span>
<span class="yarn-line">Il RAZZO sulla LUNA sembra veloce.</span> <span class="yarn-meta">#line:07ee86b #card:space_rocket</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-guide"></a>

## spawned_guide

<div class="yarn-node" data-title="spawned_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: guides </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sai chi è Jules Verne?</span> <span class="yarn-meta">#line:00b1d04 </span>
<span class="yarn-line">Sì, è uno SCRITTORE!</span> <span class="yarn-meta">#line:07f7b4b </span>
<span class="yarn-line">No, chi è?</span> <span class="yarn-meta">#line:06fb1cd </span>
<span class="yarn-line">Jules Verne era uno scrittore francese. Nacque qui vicino.</span> <span class="yarn-meta">#line:0c790cf </span>
<span class="yarn-line">Ha scritto LIBRI d'avventura molto famosi.</span> <span class="yarn-meta">#line:0f92a05</span>
<span class="yarn-line">Molte idee contenute nei suoi LIBRI sono diventate realtà.</span> <span class="yarn-meta">#line:06e1473 </span>

</code>
</pre>
</div>


