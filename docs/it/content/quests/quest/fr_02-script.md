---
title: Il sistema scolastico (fr_02) - Script
hide:
---

# Il sistema scolastico (fr_02) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://github.com/vgwb/Antura/discussions/629)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1873232287#gid=1873232287)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_02%20Angers%20School/FR_02%20Angers%20School%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_02 | Schools (Angers)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $got_backpack = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack false&gt;&gt;</span>
<span class="yarn-line">Benvenuti ad Angers! Angers è una città nella Francia occidentale. Oggi è il primo giorno di scuola! Si torna a scuola!</span> <span class="yarn-meta">#line:014887e </span>
<span class="yarn-line">La scuola ricomincia a settembre. Hai 10 anni e frequenterai la classe CM2.</span> <span class="yarn-meta">#line:063e8e0 </span>
<span class="yarn-line">Trova la tua scuola e la tua classe!</span> <span class="yarn-meta">#line:0f65a1b #task:TASK_SCHOOL</span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_SCHOOL task_find_school_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La partita è finita! Congratulazioni!</span> <span class="yarn-meta">#line:022962d </span>
<span class="yarn-line">Si torna a scuola!</span> <span class="yarn-meta">#line:038dacc </span>
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
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Disegna la tua aula.</span> <span class="yarn-meta">#line:09ac4f1 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-find-school-done"></a>

## task_find_school_done

<div class="yarn-node" data-title="task_find_school_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai trovato la tua scuola!</span> <span class="yarn-meta">#line:03ed76a </span>

</code>
</pre>
</div>

<a id="ys-node-task-find-school-desc"></a>

## task_find_school_desc

<div class="yarn-node" data-title="task_find_school_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Trova la tua scuola!</span> <span class="yarn-meta">#line:0da284c </span>

</code>
</pre>
</div>

<a id="ys-node-school-1"></a>

## school_1

<div class="yarn-node" data-title="school_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card education_ecole_maternelle_fr&gt;&gt;</span>
<span class="yarn-line">Benvenuti! Questa è una scuola materna. È una scuola per bambini dai 3 ai 5 anni. Ci sono tre classi nella scuola materna: la classe dei più piccoli, la classe intermedia e la classe dei più grandi.</span> <span class="yarn-meta">#line:096b721 </span>
<span class="yarn-line">Sei troppo grande. La scuola elementare è proprio qui accanto. Gli alunni hanno tra i 6 e i 10 anni.</span> <span class="yarn-meta">#line:07ed07b </span>

</code>
</pre>
</div>

<a id="ys-node-school-2"></a>

## school_2

<div class="yarn-node" data-title="school_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $got_backpack&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump school_2_talk&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump school_2_welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-school-2-welcome"></a>

## school_2_welcome

<div class="yarn-node" data-title="school_2_welcome">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card education_ecole_primaire_fr&gt;&gt;</span>
<span class="yarn-line">Ciao! Benvenuto! Hai trovato la tua scuola. Sei alla scuola elementare. Ci sono 5 classi: CP, CE1, CE2, CM1 e CM2.</span> <span class="yarn-meta">#line:092b309</span>
<span class="yarn-cmd">&lt;&lt;card school_bag&gt;&gt;</span>
<span class="yarn-line">Ma hai dimenticato di portare la cartella.</span> <span class="yarn-meta">#line:0dd7977 </span>
<span class="yarn-line">Ti servono l'astuccio, le matite, i quaderni e i libri.</span> <span class="yarn-meta">#line:044385f </span>
<span class="yarn-cmd">&lt;&lt;jump task_backpack&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-school-2-talk"></a>

## school_2_talk

<div class="yarn-node" data-title="school_2_talk">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ed ecco fatto! Entrate in classe.</span> <span class="yarn-meta">#line:0624437 </span>
<span class="yarn-cmd">&lt;&lt;action door_open_2&gt;&gt;</span>
<span class="yarn-line">Entrate! La vostra aula è in fondo al corridoio.</span> <span class="yarn-meta">#line:0b91268 </span>
<span class="yarn-cmd">&lt;&lt;jump task_find_classroom &gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-school-3"></a>

## school_3

<div class="yarn-node" data-title="school_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card education_lycee_fr&gt;&gt;</span>
<span class="yarn-line">Ciao! Questa è la scuola. Gli studenti hanno un'età compresa tra gli 11 e i 14 anni.</span> <span class="yarn-meta">#line:06478e9 </span>
<span class="yarn-line">Dopo il CM2, gli studenti accedono alla scuola media. Iniziano la scuola media all'età di 11 anni, entrando in sesta elementare.</span> <span class="yarn-meta">#line:0df97f8 </span>

</code>
</pre>
</div>

<a id="ys-node-school-4"></a>

## school_4

<div class="yarn-node" data-title="school_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card education_college_fr&gt;&gt;</span>
<span class="yarn-line">Nella scuola media ci sono quattro classi: sesta, quinta, quarta e terza.</span> <span class="yarn-meta">#line:04a0e4b </span>
<span class="yarn-line">Gli studenti si chiamano studenti delle scuole superiori. Ci sono tre classi nelle scuole superiori.</span> <span class="yarn-meta">#line:027eba1 </span>
<span class="yarn-line">Sei troppo giovane per iscriverti alle scuole medie o superiori.</span> <span class="yarn-meta">#line:0630d14 </span>

</code>
</pre>
</div>

<a id="ys-node-school-4-talk-man"></a>

## school_4_talk_man

<div class="yarn-node" data-title="school_4_talk_man">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("order_schools_settings") &gt; 0&gt;&gt;
<span class="yarn-line">  Ora, trova la tua scuola!</span> <span class="yarn-meta">#line:0b22d07 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
  <span class="yarn-cmd">&lt;&lt;card school_bag&gt;&gt;</span>
<span class="yarn-line">  Ciao! Ho trovato uno zaino scolastico.</span> <span class="yarn-meta">#line:0c3b4fe </span>
<span class="yarn-line">  Questi libri sono utili per imparare a leggere. Con questi libri si impara l'alfabeto e le lettere.</span> <span class="yarn-meta">#line:04da48b </span>
<span class="yarn-line">  Forse questo è il tuo libro?</span> <span class="yarn-meta">#line:0ce9646 </span>
<span class="yarn-line">  Ma prima, devi vincerlo!</span> <span class="yarn-meta">#line:094eace </span>
<span class="yarn-line">  Rimetti le classi in ordine.</span> <span class="yarn-meta">#line:092fccb </span>
  <span class="yarn-cmd">&lt;&lt;activity order_schools_settings schools_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-schools-activity-done"></a>

## schools_activity_done

<div class="yarn-node" data-title="schools_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("order_schools_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Ben fatto!</span> <span class="yarn-meta">#line:07e8390 </span>
<span class="yarn-line">    Prendilo!</span> <span class="yarn-meta">#line:078fff9 </span>
    <span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetActivityResult("order_schools_settings") == 0&gt;&gt;</span>
<span class="yarn-line">    Spiacente! Non è la risposta corretta. Riprova.</span> <span class="yarn-meta">#line:0e1ff90 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-find-classroom"></a>

## task_find_classroom

<div class="yarn-node" data-title="task_find_classroom">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Trova la tua classe.</span> <span class="yarn-meta">#line:058ddbc #task:TASK_CLASSROOM</span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_CLASSROOM task_class_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-classroom-desc"></a>

## task_classroom_desc

<div class="yarn-node" data-title="task_classroom_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Trova la tua classe.</span> <span class="yarn-meta">#line:00e71a9 </span>

</code>
</pre>
</div>

<a id="ys-node-task-class-done"></a>

## task_class_done

<div class="yarn-node" data-title="task_class_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Puoi accomodarti. Ecco il tuo tavolo e la tua sedia.</span> <span class="yarn-meta">#line:093e91d </span>

</code>
</pre>
</div>

<a id="ys-node-classroom-1"></a>

## classroom_1

<div class="yarn-node" data-title="classroom_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questa è la classe prima elementare.</span> <span class="yarn-meta">#line:08b2774 </span>
<span class="yarn-line">Nel programma CP, i bambini iniziano la scuola primaria all'età di 6 anni.</span> <span class="yarn-meta">#line:09bfe7f </span>
<span class="yarn-line">Questa non è la tua classe. Hai 10 anni. Quindi sei in CM2.</span> <span class="yarn-meta">#line:068e48b </span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-classroom-2"></a>

## classroom_2

<div class="yarn-node" data-title="classroom_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questa è la classe CE1.</span> <span class="yarn-meta">#line:08f3ab2 </span>
<span class="yarn-line">La prima elementare non è la tua classe. Sei in quinta elementare. Riprova!</span> <span class="yarn-meta">#line:029fd9c </span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-classroom-3"></a>

## classroom_3

<div class="yarn-node" data-title="classroom_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ciao! Questa è la classe CE2.</span> <span class="yarn-meta">#line:0bce01a </span>
<span class="yarn-line">Gli studenti hanno 8 anni.</span> <span class="yarn-meta">#line:0b411ce </span>
<span class="yarn-line">Questa non è la tua classe!</span> <span class="yarn-meta">#line:0e4aa95 </span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_4&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-classroom-4"></a>

## classroom_4

<div class="yarn-node" data-title="classroom_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">CM1 è il corso successivo a CE2.</span> <span class="yarn-meta">#line:0532330 </span>
<span class="yarn-line">La tua aula è vicinissima!</span> <span class="yarn-meta">#line:00f6294 </span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_5&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-classroom-5"></a>

## classroom_5

<div class="yarn-node" data-title="classroom_5">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ed ecco fatto! Questa è la tua classe. Benvenuto in CM2!</span> <span class="yarn-meta">#line:02797f8 </span>
<span class="yarn-line">Prendete posto al vostro tavolo.</span> <span class="yarn-meta">#line:0f90123 </span>
<span class="yarn-line">Prendi il tuo astuccio e il tuo quaderno.</span> <span class="yarn-meta">#line:0071a0d </span>
<span class="yarn-line">L'insegnante è lì per aiutarti ad imparare.</span> <span class="yarn-meta">#line:0f8995f </span>
<span class="yarn-cmd">&lt;&lt;card concept_cursive_writing zoom&gt;&gt;</span>
<span class="yarn-line">Studierai matematica. Imparerai a contare, a calcolare.</span> <span class="yarn-meta">#line:0365921 </span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_geometry&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-geometry"></a>

## activity_match_geometry

<div class="yarn-node" data-title="activity_match_geometry">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Riordina le lettere dell'alfabeto per formare la parola SCUOLA.</span> <span class="yarn-meta">#line:052d22a </span>
<span class="yarn-cmd">&lt;&lt;activity match_shapes activity_match_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-done"></a>

## activity_match_done

<div class="yarn-node" data-title="activity_match_done">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_shapes") &gt; 0&gt;&gt;
<span class="yarn-line">    Ben fatto! Ci sei riuscito!</span> <span class="yarn-meta">#line:0e3f1fa </span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Non è del tutto esatto. Riprova!</span> <span class="yarn-meta">#line:0a5c8f1</span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_geometry&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-backpack"></a>

## task_backpack

<div class="yarn-node" data-title="task_backpack">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">tags:  task</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card school_bag&gt;&gt;</span>
<span class="yarn-line">Trova la tua cartella e poi torna a trovarmi.</span> <span class="yarn-meta">#line:0e3ad75 #task:TASK_BACKPACK</span>
<span class="yarn-line">Forse qualcuno ha preso il tuo libro.</span> <span class="yarn-meta">#line:0b43290 </span>
<span class="yarn-line">Chiedi alle persone che ti circondano.</span> <span class="yarn-meta">#line:0c282e5 </span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_BACKPACK task_backpack_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-backpack-done"></a>

## task_backpack_done

<div class="yarn-node" data-title="task_backpack_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bravo! Hai scritto la parola SCUOLA. Puoi entrare nella scuola.</span> <span class="yarn-meta">#line:063f354  </span>
<span class="yarn-cmd">&lt;&lt;set $got_backpack = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-backpack-desc"></a>

## task_backpack_desc

<div class="yarn-node" data-title="task_backpack_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Trova il tuo zaino così potrai andare a scuola.</span> <span class="yarn-meta">#line:00faf2f </span>

</code>
</pre>
</div>

<a id="ys-node-school-canteen"></a>

## school_canteen

<div class="yarn-node" data-title="school_canteen">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags:  </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ciao! All'ora di pranzo puoi mangiare in mensa o a casa.</span> <span class="yarn-meta">#line:021132d </span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu&gt;&gt;</span>
<span class="yarn-line">Dai un'occhiata al menù della mensa.</span> <span class="yarn-meta">#line:0978619 </span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu zoom&gt;&gt;</span>
<span class="yarn-line">Buon appetito!</span> <span class="yarn-meta">#line:0671097 </span>

</code>
</pre>
</div>

<a id="ys-node-school-charte"></a>

## school_charte

<div class="yarn-node" data-title="school_charte">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card concept_charter_of_secularism&gt;&gt;</span>
<span class="yarn-line">A scuola tutti sono benvenuti: ragazze e ragazzi,</span> <span class="yarn-meta">#line:02977c9 </span>
<span class="yarn-line">indipendentemente dalle loro convinzioni.</span> <span class="yarn-meta">#line:09c12ac </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-f"></a>

## spawned_kid_f

<div class="yarn-node" data-title="spawned_kid_f">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Al mattino e al pomeriggio c'è la ricreazione. Durante la ricreazione, i bambini giocano nel cortile.</span> <span class="yarn-meta">#line:027d036 </span>
<span class="yarn-line">Mi piace disegnare e colorare.</span> <span class="yarn-meta">#line:0879a44 </span>
<span class="yarn-line">Adoro la musica e cantare con la mia classe.</span> <span class="yarn-meta">#line:0cee587 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-m"></a>

## spawned_kid_m

<div class="yarn-node" data-title="spawned_kid_m">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi piace la matematica perché adoro fare calcoli.</span> <span class="yarn-meta">#line:01b23d9 </span>
<span class="yarn-line">Gioco a calcio con i miei amici nel parco giochi.</span> <span class="yarn-meta">#line:0b82cb5 </span>
<span class="yarn-line">Mi piace fare sport con la mia classe. A volte andiamo in piscina.</span> <span class="yarn-meta">#line:0aeea98 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-adult"></a>

## spawned_adult

<div class="yarn-node" data-title="spawned_adult">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: adults </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">In Francia, la scuola inizia a settembre e termina a giugno.</span> <span class="yarn-meta">#line:037e288 </span>
<span class="yarn-line">Durante l'anno scolastico ci sono delle vacanze.</span> <span class="yarn-meta">#line:028f2bf </span>
<span class="yarn-line">A scuola impariamo a leggere, scrivere, contare e parlare nuove lingue.</span> <span class="yarn-meta">#line:0d8d4aa </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-teacher"></a>

## spawned_teacher

<div class="yarn-node" data-title="spawned_teacher">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: teachers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Le lezioni si tengono dalle 8:00 alle 16:00.</span> <span class="yarn-meta">#line:0f6b039 </span>
<span class="yarn-line">All'ora di pranzo, puoi mangiare in mensa oppure tornare a casa per mangiare.</span> <span class="yarn-meta">#line:008ffb5 </span>
<span class="yarn-line">A scuola impariamo a leggere e scrivere.</span> <span class="yarn-meta">#line:0d88d5b #card:concept_cursive_writing</span>

</code>
</pre>
</div>


