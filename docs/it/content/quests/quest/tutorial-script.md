---
title: Tutorial (tutorial) - Script
hide:
---

# Tutorial (tutorial) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/quest-tutorial/41)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=631129787#gid=631129787)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/_TUTORIAL/Tutorial%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// tutorial | Tutorial</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Benvenuti al tutorial!</span> <span class="yarn-meta">#line:021793f </span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset keys_wasd&gt;&gt;</span>
<span class="yarn-line">    Per camminare usa i tasti WASD.</span> <span class="yarn-meta">#line:037d71d </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_move&gt;&gt;</span>
<span class="yarn-line">    Usa il dito sinistro per camminare</span> <span class="yarn-meta">#line:0e55bc4 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-tutor-1"></a>

## talk_tutor_1

<div class="yarn-node" data-title="talk_tutor_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tocca questo testo per ascoltarlo di nuovo.</span> <span class="yarn-meta">#line:03dbfa7 #native</span>
<span class="yarn-comment">// &lt;&lt;asset tutorial_goon&gt;&gt;</span>
<span class="yarn-comment">// Use this button to advance the dialog.</span> <span class="yarn-meta">#line:0f4f069 </span>

<span class="yarn-comment">//&lt;&lt;asset tutorial_image&gt;&gt;</span>
<span class="yarn-comment">//Use this button to view the photo</span> <span class="yarn-meta">#line:0784704 </span>

<span class="yarn-line">Vai a parlare con il prossimo tutor!</span> <span class="yarn-meta">#line:0eb85aa </span>
<span class="yarn-cmd">&lt;&lt;area area_medium&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-camera"></a>

## tutor_camera

<div class="yarn-node" data-title="tutor_camera">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Premere il pulsante del mouse per spostare la telecamera.</span> <span class="yarn-meta">#line:0e633a2 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_camera&gt;&gt;</span>
<span class="yarn-line">    Usa il dito destro per muovere la telecamera.</span> <span class="yarn-meta">#line:0aa47cb </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_large&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-act"></a>

## tutor_act

<div class="yarn-node" data-title="tutor_act">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_act&gt;&gt;</span>
<span class="yarn-line">Usa questo pulsante per parlare o interagire</span> <span class="yarn-meta">#line:0c14f65 </span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Oppure premere il tasto SPAZIO.</span> <span class="yarn-meta">#line:0c18f6b </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-4-jump"></a>

## tutor_4_jump

<div class="yarn-node" data-title="tutor_4_jump">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: asset=tutorial_move</span>
<span class="yarn-header-dim">---</span>
 <span class="yarn-cmd">&lt;&lt;asset tutorial_jump&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Premi il tasto SPAZIO o questo pulsante per saltare</span> <span class="yarn-meta">#line:07940cf</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Usa questo pulsante per saltare</span> <span class="yarn-meta">#line:0b9c1fa </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_large&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-6-cookies"></a>

## tutor_6_cookies

<div class="yarn-node" data-title="tutor_6_cookies">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_cookies&gt;&gt;</span>
<span class="yarn-line">Prendi tutti i biscotti che trovi. Possono essere utili</span> <span class="yarn-meta">#line:0f50a6e </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-7-map"></a>

## tutor_7_map

<div class="yarn-node" data-title="tutor_7_map">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_map&gt;&gt;</span>
<span class="yarn-line">Questo pulsante apre la mappa!</span> <span class="yarn-meta">#line:01777e4 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-8-interact"></a>

## tutor_8_interact

<div class="yarn-node" data-title="tutor_8_interact">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_actions&gt;&gt;</span>
<span class="yarn-line">Esplora tutti gli oggetti che hanno questa icona.</span> <span class="yarn-meta">#line:0139142 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-9-pushball"></a>

## tutor_9_pushball

<div class="yarn-node" data-title="tutor_9_pushball">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_ball&gt;&gt;</span>
<span class="yarn-line">Prova a spingere questa palla.</span> <span class="yarn-meta">#line:02253ae </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-10-follow"></a>

## tutor_10_follow

<div class="yarn-node" data-title="tutor_10_follow">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_target&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_10&gt;&gt;</span>
<span class="yarn-line">Questa icona indica dove andare.</span> <span class="yarn-meta">#line:06c117d </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-11-mission"></a>

## tutor_11_mission

<div class="yarn-node" data-title="tutor_11_mission">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_coin&gt;&gt;</span>
<span class="yarn-line">Fammi vedere se hai imparato</span> <span class="yarn-meta">#line:0fe9efe</span>
<span class="yarn-cmd">&lt;&lt;target chest_end&gt;&gt;</span>
<span class="yarn-line">Sali le scale e apri quella cassa</span> <span class="yarn-meta">#line:07b2183 </span>
<span class="yarn-cmd">&lt;&lt;task_start open_chest task_open_chest_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-open-chest-done"></a>

## task_open_chest_done

<div class="yarn-node" data-title="task_open_chest_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Fantastico! Hai aperto il baule!</span> <span class="yarn-meta">#line:0c30eb1 </span>
<span class="yarn-cmd">&lt;&lt;action appear_tutor_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-end"></a>

## tutor_end

<div class="yarn-node" data-title="tutor_end">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: </span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
<span class="yarn-line">Questa è la fine del tutorial di base.</span> <span class="yarn-meta">#line:0dfdfc5 </span>
<span class="yarn-line">Ora puoi esplorare la zona e parlare con tutti</span> <span class="yarn-meta">#line:02fea28 </span>
<span class="yarn-line">Siete pronti a giocare?</span> <span class="yarn-meta">#line:0ac17d0 </span>
<span class="yarn-line">SÌ</span> <span class="yarn-meta">#line:0b66e60 </span>
<span class="yarn-line">    Fantastico. Ci vediamo presto in partita!</span> <span class="yarn-meta">#line:07498c0 </span>
<span class="yarn-line">    C'è molto altro da scoprire ad Antura.</span> <span class="yarn-meta">#line:0ed06b6 </span>
<span class="yarn-line">NO</span> <span class="yarn-meta">#line:01d5126 </span>
<span class="yarn-line">    Puoi rivedere il tutorial in qualsiasi momento.</span> <span class="yarn-meta">#line:06f9065 </span>
<span class="yarn-line">    Chiedi al tuo insegnante di aiutarti.</span> <span class="yarn-meta">#line:0c6bc14 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-teleport"></a>

## tutor_teleport

<div class="yarn-node" data-title="tutor_teleport">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_portal&gt;&gt;</span>
<span class="yarn-line">Usa i portali per viaggiare velocemente!</span> <span class="yarn-meta">#line:0f753b5 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-killzone"></a>

## tutor_killzone

<div class="yarn-node" data-title="tutor_killzone">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_danger&gt;&gt;</span>
<span class="yarn-line">Fate attenzione a non cadere in acqua!</span> <span class="yarn-meta">#line:0e9b5a9 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-livingletter"></a>

## tutor_livingletter

<div class="yarn-node" data-title="tutor_livingletter">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_livingletter&gt;&gt;</span>
<span class="yarn-line">Queste sono Lettere Viventi. Parla con loro per imparare nuove parole!</span> <span class="yarn-meta">#line:06e500f </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-blocky-character"></a>

## tutor_blocky_character

<div class="yarn-node" data-title="tutor_blocky_character">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_blocky_character&gt;&gt;</span>
<span class="yarn-line">Queste persone sono nostre amiche. Parla con loro per scoprire di più sul mondo!</span> <span class="yarn-meta">#line:0be283b </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-card"></a>

## tutor_card

<div class="yarn-node" data-title="tutor_card">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_card&gt;&gt;</span>
<span class="yarn-line">Questa è una CARTA. Contiene conoscenze e poteri. Collezionali tutti!</span> <span class="yarn-meta">#line:0ac4c18 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-cat"></a>

## tutor_cat

<div class="yarn-node" data-title="tutor_cat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_cat&gt;&gt;</span>
<span class="yarn-line">Sì. Sei proprio tu! Se giochi tutta la partita, puoi cambiare il tuo look!</span> <span class="yarn-meta">#line:0eb1890 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-antura"></a>

## tutor_antura

<div class="yarn-node" data-title="tutor_antura">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura&gt;&gt;</span>
<span class="yarn-line">Lui è il nostro amico Antura. Ti aiuterà nella tua avventura!</span> <span class="yarn-meta">#line:015cdc5 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-inventory"></a>

## tutor_inventory

<div class="yarn-node" data-title="tutor_inventory">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_inventory&gt;&gt;</span>
<span class="yarn-line">Questo è il tuo inventario. Clicca su un oggetto per usarlo.</span> <span class="yarn-meta">#line:02a6cfa </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-progress"></a>

## tutor_progress

<div class="yarn-node" data-title="tutor_progress">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_progress&gt;&gt;</span>
<span class="yarn-line">Ecco i progressi del gioco. Gioca bene e ottieni 3 stelle!</span> <span class="yarn-meta">#line:0b606b8 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-tasks"></a>

## tutor_tasks

<div class="yarn-node" data-title="tutor_tasks">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_tasks&gt;&gt;</span>
<span class="yarn-line">Questo pannello ti dice cosa devi fare.</span> <span class="yarn-meta">#line:012d967 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-target"></a>

## tutor_target

<div class="yarn-node" data-title="tutor_target">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_target&gt;&gt;</span>
<span class="yarn-line">Questo simbolo ti mostra dove andare.</span> <span class="yarn-meta">#line:0864faf </span>

</code>
</pre>
</div>

<a id="ys-node-activity-memory"></a>

## activity_memory

<div class="yarn-node" data-title="activity_memory">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Giochiamo ad Activity MEMORY!</span> <span class="yarn-meta">#line:05e100e </span>
<span class="yarn-line">Trova le coppie di carte.</span> <span class="yarn-meta">#line:0ef88ae </span>
<span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-memory-result"></a>

## activity_memory_result

<div class="yarn-node" data-title="activity_memory_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">È bene esercitare la memoria!</span> <span class="yarn-meta">#line:00e6a04 </span>
<span class="yarn-line">Vuoi giocare di nuovo?</span> <span class="yarn-meta">#line:0c78a9e </span>
<span class="yarn-line">Facile</span> <span class="yarn-meta">#line:0cd0316 </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result easy&gt;&gt;</span>
<span class="yarn-line">Normale</span> <span class="yarn-meta">#line:07cbfdd </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result normal&gt;&gt;</span>
<span class="yarn-line">Esperto</span> <span class="yarn-meta">#line:0e3251c </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result expert&gt;&gt;</span>
<span class="yarn-line">NO</span> <span class="yarn-meta">#line:010515b </span>

</code>
</pre>
</div>

<a id="ys-node-activity-canvas"></a>

## activity_canvas

<div class="yarn-node" data-title="activity_canvas">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Giochiamo ad Activity CANVAS</span> <span class="yarn-meta">#line:07041b3 </span>
<span class="yarn-line">Devi pulire tutto lo schermo senza toccare Antura!</span> <span class="yarn-meta">#line:0c80adc </span>
<span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result tutorial&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-activity-canvas-result"></a>

## activity_canvas_result

<div class="yarn-node" data-title="activity_canvas_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">È bello pulire, non è vero?</span> <span class="yarn-meta">#line:059323a</span>
[MISSING TRANSLATION: Do you want to play again? #shadow:0c78a9e]
[MISSING TRANSLATION: -&gt; Easy #shadow:0cd0316]
    <span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result easy&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; Normal #shadow:07cbfdd]
    <span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result normal&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; Expert #shadow:0e3251c]
    <span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result expert&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; No #highlight #shadow:010515b]

</code>
</pre>
</div>

<a id="ys-node-activity-jigsaw"></a>

## activity_jigsaw

<div class="yarn-node" data-title="activity_jigsaw">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Giochiamo al PUZZLE ATTIVITÀ!</span> <span class="yarn-meta">#line:0fe648a </span>
<span class="yarn-line">Completa l'immagine.</span> <span class="yarn-meta">#line:0bc50ca </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-jigsaw-result"></a>

## activity_jigsaw_result

<div class="yarn-node" data-title="activity_jigsaw_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Adoro i puzzle!</span> <span class="yarn-meta">#line:0fc42d8 </span>
[MISSING TRANSLATION: Do you want to play again? #shadow:0c78a9e]
[MISSING TRANSLATION: -&gt; Easy #shadow:0cd0316]
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result easy&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; Normal #shadow:07cbfdd]
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result normal&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; Expert #shadow:0e3251c]
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result expert&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; No #highlight #shadow:010515b]

</code>
</pre>
</div>

<a id="ys-node-activity-match"></a>

## activity_match

<div class="yarn-node" data-title="activity_match">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Giochiamo ad Activity MATCH!</span> <span class="yarn-meta">#line:07c8447 </span>
<span class="yarn-line">Abbina le carte simili.</span> <span class="yarn-meta">#line:02aec90 </span>
<span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-result"></a>

## activity_match_result

<div class="yarn-node" data-title="activity_match_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bell'abbinamento!</span> <span class="yarn-meta">#line:0f0ee2e</span>
[MISSING TRANSLATION: Do you want to play again? #shadow:0c78a9e]
[MISSING TRANSLATION: -&gt; Easy #shadow:0cd0316]
    <span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result easy&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; Normal #shadow:07cbfdd]
    <span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result normal&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; Expert #shadow:0e3251c]
    <span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result expert&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; No #highlight #shadow:010515b]

</code>
</pre>
</div>

<a id="ys-node-activity-money"></a>

## activity_money

<div class="yarn-node" data-title="activity_money">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Giochiamo all'attività CONTA I SOLDI!</span> <span class="yarn-meta">#line:06b295f </span>
<span class="yarn-line">Devi dare la giusta quantità di monete.</span> <span class="yarn-meta">#line:0f00e5d </span>
<span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-money-result"></a>

## activity_money_result

<div class="yarn-node" data-title="activity_money_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">È importante imparare a usare il denaro!</span> <span class="yarn-meta">#line:06136c1</span>
[MISSING TRANSLATION: Do you want to play again? #shadow:0c78a9e]
[MISSING TRANSLATION: -&gt; Easy #shadow:0cd0316]
    <span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result easy&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; Normal #shadow:07cbfdd]
    <span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result normal&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; Expert #shadow:0e3251c]
    <span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result expert&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; No #highlight #shadow:010515b]

</code>
</pre>
</div>

<a id="ys-node-activity-order"></a>

## activity_order

<div class="yarn-node" data-title="activity_order">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Giochiamo all'ORDINE delle attività!</span> <span class="yarn-meta">#line:015a3ea </span>
<span class="yarn-line">Metti gli elementi nell'ordine corretto.</span> <span class="yarn-meta">#line:0ed152d </span>
<span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-order-result"></a>

## activity_order_result

<div class="yarn-node" data-title="activity_order_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buona attività.</span> <span class="yarn-meta">#line:0838f7f </span>
[MISSING TRANSLATION: Do you want to play again? #shadow:0c78a9e]
[MISSING TRANSLATION: -&gt; Easy #shadow:0cd0316]
    <span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result easy&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; Normal #shadow:07cbfdd]
    <span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result normal&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; Expert #shadow:0e3251c]
    <span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result expert&gt;&gt;</span>
[MISSING TRANSLATION: -&gt; No #highlight #shadow:010515b]

</code>
</pre>
</div>

<a id="ys-node-ll-skyscraper"></a>

## ll_skyscraper

<div class="yarn-node" data-title="ll_skyscraper">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">È meraviglioso vedere la città da quassù!</span> <span class="yarn-meta">#line:0969b2f </span>
<span class="yarn-line">Vorrei viaggiare in tutto il mondo!</span> <span class="yarn-meta">#line:060db75 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-secrets"></a>

## npc_secrets

<div class="yarn-node" data-title="npc_secrets">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ho sentito che ci sono dei segreti in questo livello...</span> <span class="yarn-meta">#line:0b8d755 </span>
<span class="yarn-line">E un portale per viaggiare su quel grattacielo!</span> <span class="yarn-meta">#line:032af93 </span>
<span class="yarn-cmd">&lt;&lt;jump global_init&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-hallo"></a>

## npc_hallo

<div class="yarn-node" data-title="npc_hallo">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ciao!</span> <span class="yarn-meta">#line:022bd3f</span>
<span class="yarn-line">hai visto Antura?</span> <span class="yarn-meta">#line:00ad419</span>
<span class="yarn-line">Ho perso il mio biscotto!</span> <span class="yarn-meta">#line:0a94666 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-kid"></a>

## npc_kid

<div class="yarn-node" data-title="npc_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I biscotti sono nascosti ovunque! Trovali!</span> <span class="yarn-meta">#line:0c8d2fa </span>
<span class="yarn-line">Adoro usare i portali per viaggiare!</span> <span class="yarn-meta">#line:0fb6855 </span>
<span class="yarn-line">Parla con tutti quelli che incontri!</span> <span class="yarn-meta">#line:0812c45 </span>
<span class="yarn-line">Esplora il mondo!</span> <span class="yarn-meta">#line:041d845 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-arcade"></a>

## npc_arcade

<div class="yarn-node" data-title="npc_arcade">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Adoro i videogiochi!</span> <span class="yarn-meta">#line:0479595 </span>
<span class="yarn-line">Vuoi giocare con me?</span> <span class="yarn-meta">#line:0d3fdc7 </span>

</code>
</pre>
</div>


