---
title: La collana d'ambra di Danzica (pl_05) - Script
hide:
---

# La collana d'ambra di Danzica (pl_05) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-05-the-amber-necklace-of-gdansk/36/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=224592228#gid=224592228)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_05%20Baltic%20Sea/PL_05%20Baltic%20Sea%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_05 | The Amber Necklace of Gdańsk</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// WANTED:</span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - baltic_sea_coast ( coastal geography)</span>
<span class="yarn-header-dim">// - BalticSea (water body)</span>
<span class="yarn-header-dim">// - amber (regional treasure)</span>
<span class="yarn-header-dim">// - baltic_lighthouse (maritime navigation)</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// cleancanvas beach_shells        - Beachcombing: reveal 5 SHELL piles. (BEACH, SAND, SHELL)</span>
<span class="yarn-header-dim">// cleancanvas beach_amber         - Beachcombing: reveal 5 AMBER piles. (AMBER, BALTIC SEA)</span>
<span class="yarn-header-dim">// jigsawpuzzle gdansk_lighthouse  - Rebuild LIGHTHOUSE image; role of a LIGHTHOUSE.</span>
<span class="yarn-header-dim">// order necklace_sequence         - Craft NECKLACE: pattern of AMBER + SHELL.</span>
<span class="yarn-header-dim">// memory sea_vocab                - Memory cards: FISHERMAN, CUTTER, NET, SEAL, BEACH, AMBER</span>
<span class="yarn-header-dim">// quiz baltic_basics              - Final quiz.</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gdansk&gt;&gt;</span>
<span class="yarn-line">Benvenuti a DANZICA in POLONIA.</span> <span class="yarn-meta">#line:086baaf </span>
<span class="yarn-cmd">&lt;&lt;card gdansk_lighthouse&gt;&gt;</span>
<span class="yarn-line">Il FARO è spento.</span> <span class="yarn-meta">#line:060d616 </span>
<span class="yarn-line">Risolviamo il problema!</span> <span class="yarn-meta">#line:05414f7 </span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questa missione è completata.</span> <span class="yarn-meta">#line:04b8922 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Disegna una CONCHIGLIE.</span> <span class="yarn-meta">#line:07af5de </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-mission-note"></a>

## MISSION_NOTE

<div class="yarn-node" data-title="MISSION_NOTE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Abbiamo bisogno di un regalo per NETTUNO.</span> <span class="yarn-meta">#line:0c699c3 </span>
<span class="yarn-cmd">&lt;&lt;card necklace&gt;&gt;</span>
<span class="yarn-line">Crea una COLLANA.</span> <span class="yarn-meta">#line:05a458a </span>
<span class="yarn-line">5 AMBRA + 5 CONCHIGLIE.</span> <span class="yarn-meta">#line:01ce7c7 </span>

</code>
</pre>
</div>

<a id="ys-node-beach-intro"></a>

## BEACH_INTRO

<div class="yarn-node" data-title="BEACH_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// PART 1 – BEACHCOMBING SHELLS</span>
<span class="yarn-header-dim">group: Beach</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card baltic_sea&gt;&gt;</span>
<span class="yarn-line">Questo è il MAR BALTICO.</span> <span class="yarn-meta">#line:050f059 </span>
<span class="yarn-cmd">&lt;&lt;card baltic_sea_coast&gt;&gt;</span>
<span class="yarn-line">Cerchiamo le CONCHIGLIE nella SABBIA.</span> <span class="yarn-meta">#line:07d772f </span>


</code>
</pre>
</div>

<a id="ys-node-beach-shells-hint"></a>

## BEACH_SHELLS_HINT

<div class="yarn-node" data-title="BEACH_SHELLS_HINT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Beach</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Trascinare per pulire la SABBIA.</span> <span class="yarn-meta">#line:06e5065 </span>
<span class="yarn-cmd">&lt;&lt;card seashell&gt;&gt;</span>
<span class="yarn-line">Trova 5 pile di CONCHIGLIE.</span> <span class="yarn-meta">#line:03e5d4f </span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas beach_shells tutorial&gt;&gt;</span>
<span class="yarn-line">Ora vai e incontra il PESCATORE</span> <span class="yarn-meta">#line:0b3a2d8 </span>

</code>
</pre>
</div>

<a id="ys-node-fisherman-meet"></a>

## FISHERMAN_MEET

<div class="yarn-node" data-title="FISHERMAN_MEET">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// PART 2 – MEET THE FISHERMAN &amp; AMBER</span>
<span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card fisherman&gt;&gt;</span>
<span class="yarn-line">Ciao. Sono un PESCATORE.</span> <span class="yarn-meta">#line:00e94a2 </span>
<span class="yarn-cmd">&lt;&lt;card cutter&gt;&gt;</span>
<span class="yarn-line">Questo è il mio CUTTER e NET.</span> <span class="yarn-meta">#line:0270483 </span>
<span class="yarn-cmd">&lt;&lt;card fishing_net&gt;&gt;</span>
<span class="yarn-line">Troviamo anche AMBRA.</span> <span class="yarn-meta">#line:0b8e688 </span>


</code>
</pre>
</div>

<a id="ys-node-amber-hint"></a>

## AMBER_HINT

<div class="yarn-node" data-title="AMBER_HINT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card amber&gt;&gt;</span>
<span class="yarn-line">L'AMBRA è ORO BALTICO.</span> <span class="yarn-meta">#line:0901a09 </span>
<span class="yarn-line">Pulisci la SABBIA per trovare 5.</span> <span class="yarn-meta">#line:03d5567 </span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas_beach cleancanvas_beach_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cleancanvas-beach-done"></a>

## cleancanvas_beach_done

<div class="yarn-node" data-title="cleancanvas_beach_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo lavoro nel trovare AMBER!</span> <span class="yarn-meta">#line:07caa60 </span>
<span class="yarn-line">Ora andiamo al FARO.</span> <span class="yarn-meta">#line:0616f80 </span>

</code>
</pre>
</div>

<a id="ys-node-lighthouse-fact"></a>

## LIGHTHOUSE_FACT

<div class="yarn-node" data-title="LIGHTHOUSE_FACT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ship&gt;&gt;</span>
<span class="yarn-line">Un FARO aiuta le NAVI.</span> <span class="yarn-meta">#line:049551f </span>
<span class="yarn-cmd">&lt;&lt;card navigation&gt;&gt;</span>
<span class="yarn-line">Emette una luce intensa.</span> <span class="yarn-meta">#line:050c5f3 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_gdansk_lighthouse jigsaw_gdansk_lighthouse_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-jigsaw-gdansk-lighthouse-done"></a>

## jigsaw_gdansk_lighthouse_done

<div class="yarn-node" data-title="jigsaw_gdansk_lighthouse_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo lavoro nel riparare il FARO!</span> <span class="yarn-meta">#line:0647b3e </span>
<span class="yarn-line">Ora realizziamo la COLLANA.</span> <span class="yarn-meta">#line:09de5fd </span>

</code>
</pre>
</div>

<a id="ys-node-crafting-intro"></a>

## CRAFTING_INTRO

<div class="yarn-node" data-title="CRAFTING_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// PART 3 – CRAFTING THE NECKLACE</span>
<span class="yarn-header-dim">group: Crafting</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Abbiamo 10 pezzi.</span> <span class="yarn-meta">#line:02129aa </span>
<span class="yarn-line">AMBRA e CONCHIGLIE.</span> <span class="yarn-meta">#line:03e4fc6 </span>
<span class="yarn-line">Crea uno schema.</span> <span class="yarn-meta">#line:0ba66f7 </span>
<span class="yarn-cmd">&lt;&lt;jump CRAFTING_ORDER&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-crafting-order"></a>

## CRAFTING_ORDER

<div class="yarn-node" data-title="CRAFTING_ORDER">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Crafting</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Trascinare in ordine.</span> <span class="yarn-meta">#line:0137037 </span>
<span class="yarn-line">CONCHIGLIA D'AMBRA ripetere.</span> <span class="yarn-meta">#line:09746e2 </span>
<span class="yarn-cmd">&lt;&lt;activity order order_necklace order_necklace_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-order-necklace-done"></a>

## order_necklace_done

<div class="yarn-node" data-title="order_necklace_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Crafting</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo lavoro nel realizzare la COLLANA!</span> <span class="yarn-meta">#line:06bbad3 </span>
<span class="yarn-line">Ora alla FONTANA DI NETTUNO</span> <span class="yarn-meta">#line:0e2f20d </span>

</code>
</pre>
</div>

<a id="ys-node-neptune-fountain"></a>

## NEPTUNE_FOUNTAIN

<div class="yarn-node" data-title="NEPTUNE_FOUNTAIN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// PART 4 – NEPTUNE &amp; LIGHTHOUSE ON</span>
<span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card neptune_s_fountain&gt;&gt;</span>
<span class="yarn-line">Posiziona la COLLANA su NETTUNO.</span> <span class="yarn-meta">#line:01b559a </span>


</code>
</pre>
</div>

<a id="ys-node-neptune-speak"></a>

## NEPTUNE_SPEAK

<div class="yarn-node" data-title="NEPTUNE_SPEAK">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card necklace&gt;&gt;</span>
<span class="yarn-line">Una COLLANA meravigliosa!</span> <span class="yarn-meta">#line:0ca234d </span>
<span class="yarn-line">Rispettate il MARE.</span> <span class="yarn-meta">#line:0d0c226 </span>
<span class="yarn-line">Ti aiuterò.</span> <span class="yarn-meta">#line:0a0dddf </span>


</code>
</pre>
</div>

<a id="ys-node-amber-room-note"></a>

## AMBER_ROOM_NOTE

<div class="yarn-node" data-title="AMBER_ROOM_NOTE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Qui l'AMBRA è famosa.</span> <span class="yarn-meta">#line:05ce5f4 </span>
<span class="yarn-cmd">&lt;&lt;card amber_room&gt;&gt;</span>
<span class="yarn-line">Come la storia della CAMERA D'AMBRA.</span> <span class="yarn-meta">#line:075c0a2 </span>


</code>
</pre>
</div>

<a id="ys-node-lighthouse-on"></a>

## LIGHTHOUSE_ON

<div class="yarn-node" data-title="LIGHTHOUSE_ON">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card waves&gt;&gt;</span>
<span class="yarn-line">FLASH! Il FARO brilla.</span> <span class="yarn-meta">#line:0941ab9 </span>
<span class="yarn-line">Ottimo lavoro!</span> <span class="yarn-meta">#line:0a44f23 </span>
<span class="yarn-line">e fai attenzione ai SEAL</span> <span class="yarn-meta">#line:009476a </span>

</code>
</pre>
</div>

<a id="ys-node-seals-warning"></a>

## SEALS_WARNING

<div class="yarn-node" data-title="SEALS_WARNING">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// PART 5 – SEALS &amp; ANTURA</span>
<span class="yarn-header-dim">group: Rescue</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card seal&gt;&gt;</span>
<span class="yarn-line">Queste sono FOCHE selvatiche.</span> <span class="yarn-meta">#line:0c0dc82 </span>
<span class="yarn-line">Mantenere una distanza di sicurezza.</span> <span class="yarn-meta">#line:06b0e4e </span>

</code>
</pre>
</div>

<a id="ys-node-antura-scene"></a>

## ANTURA_SCENE

<div class="yarn-node" data-title="ANTURA_SCENE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Rescue</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card seagull&gt;&gt;</span>
<span class="yarn-line">Sì! La missione è compiuta!</span> <span class="yarn-meta">#line:0283435 </span>
<span class="yarn-line">Parole di revisione</span> <span class="yarn-meta">#line:0322b5e </span>
    <span class="yarn-cmd">&lt;&lt;jump RECAP_MEMORY&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-recap-memory"></a>

## RECAP_MEMORY

<div class="yarn-node" data-title="RECAP_MEMORY">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Recap</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ecosystem&gt;&gt;</span>
<span class="yarn-line">Abbina le parole.</span> <span class="yarn-meta">#line:0372493 </span>
<span class="yarn-cmd">&lt;&lt;activity memory_sea_vocab memory_sea_vocab_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-memory-sea-vocab-done"></a>

## memory_sea_vocab_done

<div class="yarn-node" data-title="memory_sea_vocab_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ben fatto!</span> <span class="yarn-meta">#line:061ca19 </span>
<span class="yarn-line">Ora quiz finale</span> <span class="yarn-meta">#line:05251b3 </span>
<span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz"></a>

## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Rispondi a tre domande.</span> <span class="yarn-meta">#line:00a0665 </span>
<span class="yarn-line">Tocca la scelta migliore.</span> <span class="yarn-meta">#line:07c72eb </span>
<span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz-q1"></a>

## FINAL_QUIZ_Q1

<div class="yarn-node" data-title="FINAL_QUIZ_Q1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card baltic_sea&gt;&gt;</span>
<span class="yarn-line">Quale mare si trova vicino a DANZICA?</span> <span class="yarn-meta">#line:034eda7 </span>
<span class="yarn-line">MAR BALTICO</span> <span class="yarn-meta">#line:04bbc7e </span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q2&gt;&gt;</span>
<span class="yarn-line">MAR NERO</span> <span class="yarn-meta">#line:046b853 </span>
<span class="yarn-line">    Non quello. Riprova!</span> <span class="yarn-meta">#line:0cfee89 </span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz-q2"></a>

## FINAL_QUIZ_Q2

<div class="yarn-node" data-title="FINAL_QUIZ_Q2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card amber&gt;&gt;</span>
<span class="yarn-line">Cos'è BALTIC GOLD?</span> <span class="yarn-meta">#line:039b452 </span>
<span class="yarn-line">AMBRA</span> <span class="yarn-meta">#line:0bcd01f </span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q3&gt;&gt;</span>
<span class="yarn-line">SABBIA</span> <span class="yarn-meta">#line:0af46f3 </span>
<span class="yarn-line">    Non SABBIA. Riprova!</span> <span class="yarn-meta">#line:005cc14 </span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz-q3"></a>

## FINAL_QUIZ_Q3

<div class="yarn-node" data-title="FINAL_QUIZ_Q3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gdansk_lighthouse&gt;&gt;</span>
<span class="yarn-line">Cosa fa un FARO?</span> <span class="yarn-meta">#line:02e4c51 </span>
<span class="yarn-line">Aiuta le NAVI</span> <span class="yarn-meta">#line:0822992 </span>
    <span class="yarn-cmd">&lt;&lt;jump QUEST_COMPLETE&gt;&gt;</span>
<span class="yarn-line">Cattura pesci</span> <span class="yarn-meta">#line:072458e </span>
<span class="yarn-line">    No. Guida le NAVI. Riprova!</span> <span class="yarn-meta">#line:04b2bb3 </span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-complete"></a>

## QUEST_COMPLETE

<div class="yarn-node" data-title="QUEST_COMPLETE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai aiutato DANZICA.</span> <span class="yarn-meta">#line:0a7204e </span>
<span class="yarn-line">Il FARO torna a splendere!</span> <span class="yarn-meta">#line:0d95290 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-child"></a>

## spawned_child

<div class="yarn-node" data-title="spawned_child">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// Spawned NPC flavor</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Una volta ho visto una grande NAVE.</span> <span class="yarn-meta">#line:02579c6 </span>
<span class="yarn-line">Le onde si infrangono veloci.</span> <span class="yarn-meta">#line:04fe9c2 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La vecchia GRU solleva carichi pesanti.</span> <span class="yarn-meta">#line:0d2afb8 </span>
<span class="yarn-line">La luce intensa guida le barche.</span> <span class="yarn-meta">#line:093119f </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-local"></a>

## spawned_local

<div class="yarn-node" data-title="spawned_local">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Qui il fiume incontra il MARE.</span> <span class="yarn-meta">#line:0edcc57 </span>
<span class="yarn-line">Oggi il porto è molto trafficato.</span> <span class="yarn-meta">#line:04f5d5e </span>

</code>
</pre>
</div>


