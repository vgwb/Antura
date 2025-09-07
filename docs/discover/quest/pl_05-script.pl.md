---
title: Gdański naszyjnik bursztynowy (pl_05) - Script
hide:
---

# Gdański naszyjnik bursztynowy (pl_05) - Script
[Quest Index](./index.pl.md) - Language: [english](./pl_05-script.md) - [french](./pl_05-script.fr.md) - polish - [italian](./pl_05-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-05-the-amber-necklace-of-gdansk/36/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=224592228#gid=224592228)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_05%20Baltic%20Sea/PL_05%20Baltic%20Sea%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// pl_05 | The Amber Necklace of Gdańsk</span>
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
<span class="yarn-line">Witamy w GDAŃSKU, w POLSCE. <span class="yarn-meta">#line:086baaf </span></span>
<span class="yarn-cmd">&lt;&lt;card gdansk_lighthouse&gt;&gt;</span>
<span class="yarn-line">LATARNIA MORSKA jest wyłączona. <span class="yarn-meta">#line:060d616 </span></span>
<span class="yarn-line">Naprawmy to! <span class="yarn-meta">#line:05414f7 </span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To zadanie zostało ukończone. <span class="yarn-meta">#line:04b8922 </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Narysuj MUSZKĘ. <span class="yarn-meta">#line:07af5de </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-mission-note"></a>
## MISSION_NOTE

<div class="yarn-node" data-title="MISSION_NOTE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Potrzebujemy prezentu dla NEPTUN. <span class="yarn-meta">#line:0c699c3 </span></span>
<span class="yarn-cmd">&lt;&lt;card necklace&gt;&gt;</span>
<span class="yarn-line">Zrób NASZYJNIK. <span class="yarn-meta">#line:05a458a </span></span>
<span class="yarn-line">5 BURSZTYNOWYCH + 5 MUSZLI. <span class="yarn-meta">#line:01ce7c7 </span></span>

</code></pre></div>

<a id="ys-node-beach-intro"></a>
## BEACH_INTRO

<div class="yarn-node" data-title="BEACH_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">// PART 1 – BEACHCOMBING SHELLS</span>
<span class="yarn-header-dim">group: Beach</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card baltic_sea&gt;&gt;</span>
<span class="yarn-line">To jest MORZE BAŁTYCKIE. <span class="yarn-meta">#line:050f059 </span></span>
<span class="yarn-cmd">&lt;&lt;card baltic_sea_coast&gt;&gt;</span>
<span class="yarn-line">Znajdźmy MUSZLE w PIASKU. <span class="yarn-meta">#line:07d772f </span></span>


</code></pre></div>

<a id="ys-node-beach-shells-hint"></a>
## BEACH_SHELLS_HINT

<div class="yarn-node" data-title="BEACH_SHELLS_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Beach</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przeciągnij, aby wyczyścić PIASEK. <span class="yarn-meta">#line:06e5065 </span></span>
<span class="yarn-cmd">&lt;&lt;card seashell&gt;&gt;</span>
<span class="yarn-line">Znajdź 5 stosów MUSZLI. <span class="yarn-meta">#line:03e5d4f </span></span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas beach_shells tutorial&gt;&gt;</span>
<span class="yarn-line">Teraz idź i spotkaj się z RYBAKIEM <span class="yarn-meta">#line:0b3a2d8 </span></span>

</code></pre></div>

<a id="ys-node-fisherman-meet"></a>
## FISHERMAN_MEET

<div class="yarn-node" data-title="FISHERMAN_MEET"><pre class="yarn-code"><code><span class="yarn-header-dim">// PART 2 – MEET THE FISHERMAN &amp; AMBER</span>
<span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">tags: actor=Fisherman</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card fisherman&gt;&gt;</span>
<span class="yarn-line">Cześć. Jestem RYBAKIEM. <span class="yarn-meta">#line:00e94a2 </span></span>
<span class="yarn-cmd">&lt;&lt;card cutter&gt;&gt;</span>
<span class="yarn-line">To jest mój CUTTER i NET. <span class="yarn-meta">#line:0270483 </span></span>
<span class="yarn-cmd">&lt;&lt;card fishing_net&gt;&gt;</span>
<span class="yarn-line">Znajdźmy też AMBER. <span class="yarn-meta">#line:0b8e688 </span></span>


</code></pre></div>

<a id="ys-node-amber-hint"></a>
## AMBER_HINT

<div class="yarn-node" data-title="AMBER_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">tags: actor=Fisherman</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card amber&gt;&gt;</span>
<span class="yarn-line">BURSZTYN JEST ZŁOTEM BAŁTYCKIM. <span class="yarn-meta">#line:0901a09 </span></span>
<span class="yarn-line">Wyczyść SAND, aby znaleźć 5. <span class="yarn-meta">#line:03d5567 </span></span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas_beach cleancanvas_beach_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-cleancanvas-beach-done"></a>
## cleancanvas_beach_done

<div class="yarn-node" data-title="cleancanvas_beach_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetna robota ze znalezieniem AMBER! <span class="yarn-meta">#line:07caa60 </span></span>
<span class="yarn-line">Teraz chodźmy do LATARNI MORSKIEJ. <span class="yarn-meta">#line:0616f80 </span></span>

</code></pre></div>

<a id="ys-node-lighthouse-fact"></a>
## LIGHTHOUSE_FACT

<div class="yarn-node" data-title="LIGHTHOUSE_FACT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">tags: actor=Fisherman</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ship&gt;&gt;</span>
<span class="yarn-line">LATARNIA MORSKA pomaga STATKOM. <span class="yarn-meta">#line:049551f </span></span>
<span class="yarn-cmd">&lt;&lt;card navigation&gt;&gt;</span>
<span class="yarn-line">Świeci jasnym światłem. <span class="yarn-meta">#line:050c5f3 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_gdansk_lighthouse jigsaw_gdansk_lighthouse_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-jigsaw-gdansk-lighthouse-done"></a>
## jigsaw_gdansk_lighthouse_done

<div class="yarn-node" data-title="jigsaw_gdansk_lighthouse_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetna robota z naprawą LATARNI! <span class="yarn-meta">#line:0647b3e </span></span>
<span class="yarn-line">Teraz zabierzemy się za wykonanie NASZYJNIKA. <span class="yarn-meta">#line:09de5fd </span></span>

</code></pre></div>

<a id="ys-node-crafting-intro"></a>
## CRAFTING_INTRO

<div class="yarn-node" data-title="CRAFTING_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">// PART 3 – CRAFTING THE NECKLACE</span>
<span class="yarn-header-dim">group: Crafting</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mamy 10 sztuk. <span class="yarn-meta">#line:02129aa </span></span>
<span class="yarn-line">BURSZTYN I MUSZLA. <span class="yarn-meta">#line:03e4fc6 </span></span>
<span class="yarn-line">Utwórz wzór. <span class="yarn-meta">#line:0ba66f7 </span></span>
<span class="yarn-cmd">&lt;&lt;jump CRAFTING_ORDER&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-crafting-order"></a>
## CRAFTING_ORDER

<div class="yarn-node" data-title="CRAFTING_ORDER"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Crafting</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przeciągnij w kolejności. <span class="yarn-meta">#line:0137037 </span></span>
<span class="yarn-line">Powtórzenie AMBER SHELL. <span class="yarn-meta">#line:09746e2 </span></span>
<span class="yarn-cmd">&lt;&lt;activity order order_necklace order_necklace_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-order-necklace-done"></a>
## order_necklace_done

<div class="yarn-node" data-title="order_necklace_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Crafting</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetna robota przy wykonaniu NASZYJNIKA! <span class="yarn-meta">#line:06bbad3 </span></span>
<span class="yarn-line">Teraz do FONTANNY NEPTUN <span class="yarn-meta">#line:0e2f20d </span></span>

</code></pre></div>

<a id="ys-node-neptune-fountain"></a>
## NEPTUNE_FOUNTAIN

<div class="yarn-node" data-title="NEPTUNE_FOUNTAIN"><pre class="yarn-code"><code><span class="yarn-header-dim">// PART 4 – NEPTUNE &amp; LIGHTHOUSE ON</span>
<span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card neptune_s_fountain&gt;&gt;</span>
<span class="yarn-line">Załóż NASZYJNIK na NEPTUNĘ. <span class="yarn-meta">#line:01b559a </span></span>


</code></pre></div>

<a id="ys-node-neptune-speak"></a>
## NEPTUNE_SPEAK

<div class="yarn-node" data-title="NEPTUNE_SPEAK"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">tags: actor=Neptune</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card necklace&gt;&gt;</span>
<span class="yarn-line">Piękny NASZYJNIK! <span class="yarn-meta">#line:0ca234d </span></span>
<span class="yarn-line">Szanujesz Morze Śródziemne. <span class="yarn-meta">#line:0d0c226 </span></span>
<span class="yarn-line">Pomogę ci. <span class="yarn-meta">#line:0a0dddf </span></span>


</code></pre></div>

<a id="ys-node-amber-room-note"></a>
## AMBER_ROOM_NOTE

<div class="yarn-node" data-title="AMBER_ROOM_NOTE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">tags: actor=Neptune</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">AMBER jest tutaj sławny. <span class="yarn-meta">#line:05ce5f4 </span></span>
<span class="yarn-cmd">&lt;&lt;card amber_room&gt;&gt;</span>
<span class="yarn-line">Podobnie jak historia AMBER ROOM. <span class="yarn-meta">#line:075c0a2 </span></span>


</code></pre></div>

<a id="ys-node-lighthouse-on"></a>
## LIGHTHOUSE_ON

<div class="yarn-node" data-title="LIGHTHOUSE_ON"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card waves&gt;&gt;</span>
<span class="yarn-line">BŁYSK! LATARNIA MORSKA świeci. <span class="yarn-meta">#line:0941ab9 </span></span>
<span class="yarn-line">Świetna robota! <span class="yarn-meta">#line:0a44f23 </span></span>
<span class="yarn-line">i uważaj na SEALS <span class="yarn-meta">#line:009476a </span></span>

</code></pre></div>

<a id="ys-node-seals-warning"></a>
## SEALS_WARNING

<div class="yarn-node" data-title="SEALS_WARNING"><pre class="yarn-code"><code><span class="yarn-header-dim">// PART 5 – SEALS &amp; ANTURA</span>
<span class="yarn-header-dim">group: Rescue</span>
<span class="yarn-header-dim">tags: actor=Fisherman</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card seal&gt;&gt;</span>
<span class="yarn-line">To są dzikie FOKi. <span class="yarn-meta">#line:0c0dc82 </span></span>
<span class="yarn-line">Zachowaj bezpieczną odległość. <span class="yarn-meta">#line:06b0e4e </span></span>

</code></pre></div>

<a id="ys-node-antura-scene"></a>
## ANTURA_SCENE

<div class="yarn-node" data-title="ANTURA_SCENE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Rescue</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card seagull&gt;&gt;</span>
<span class="yarn-line">Tak! Zadanie wykonane! <span class="yarn-meta">#line:0283435 </span></span>
<span class="yarn-line">Powtórz słowa <span class="yarn-meta">#line:0322b5e </span></span>
    <span class="yarn-cmd">&lt;&lt;jump RECAP_MEMORY&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-recap-memory"></a>
## RECAP_MEMORY

<div class="yarn-node" data-title="RECAP_MEMORY"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Recap</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ecosystem&gt;&gt;</span>
<span class="yarn-line">Połącz słowa. <span class="yarn-meta">#line:0372493 </span></span>
<span class="yarn-cmd">&lt;&lt;activity memory_sea_vocab memory_sea_vocab_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-memory-sea-vocab-done"></a>
## memory_sea_vocab_done

<div class="yarn-node" data-title="memory_sea_vocab_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dobrze zrobiony! <span class="yarn-meta">#line:061ca19 </span></span>
<span class="yarn-line">Teraz ostateczny quiz <span class="yarn-meta">#line:05251b3 </span></span>
<span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-final-quiz"></a>
## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Odpowiedz na trzy pytania. <span class="yarn-meta">#line:00a0665 </span></span>
<span class="yarn-line">Wybierz najlepszą opcję. <span class="yarn-meta">#line:07c72eb </span></span>
<span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q1&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-final-quiz-q1"></a>
## FINAL_QUIZ_Q1

<div class="yarn-node" data-title="FINAL_QUIZ_Q1"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card baltic_sea&gt;&gt;</span>
<span class="yarn-line">Jakie morze jest blisko GDAŃSKA? <span class="yarn-meta">#line:034eda7 </span></span>
<span class="yarn-line">MORZE BAŁTYCKIE <span class="yarn-meta">#line:04bbc7e </span></span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q2&gt;&gt;</span>
<span class="yarn-line">MORZE CZARNE <span class="yarn-meta">#line:046b853 </span></span>
<span class="yarn-line">    Nie ten. Spróbuj jeszcze raz! <span class="yarn-meta">#line:0cfee89 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q1&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-final-quiz-q2"></a>
## FINAL_QUIZ_Q2

<div class="yarn-node" data-title="FINAL_QUIZ_Q2"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card amber&gt;&gt;</span>
<span class="yarn-line">Czym jest BALTIC GOLD? <span class="yarn-meta">#line:039b452 </span></span>
<span class="yarn-line">BURSZTYN <span class="yarn-meta">#line:0bcd01f </span></span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q3&gt;&gt;</span>
<span class="yarn-line">PIASEK <span class="yarn-meta">#line:0af46f3 </span></span>
<span class="yarn-line">    Nie PIASEK. Spróbuj ponownie! <span class="yarn-meta">#line:005cc14 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q2&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-final-quiz-q3"></a>
## FINAL_QUIZ_Q3

<div class="yarn-node" data-title="FINAL_QUIZ_Q3"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gdansk_lighthouse&gt;&gt;</span>
<span class="yarn-line">Co robi LATARNIA MORSKA? <span class="yarn-meta">#line:02e4c51 </span></span>
<span class="yarn-line">Pomaga STATKOM <span class="yarn-meta">#line:0822992 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump QUEST_COMPLETE&gt;&gt;</span>
<span class="yarn-line">Łapie ryby <span class="yarn-meta">#line:072458e </span></span>
<span class="yarn-line">    Nie. On kieruje STATKAMI. Spróbuj ponownie! <span class="yarn-meta">#line:04b2bb3 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ_Q3&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-quest-complete"></a>
## QUEST_COMPLETE

<div class="yarn-node" data-title="QUEST_COMPLETE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Pomogłeś GDAŃSKOWI. <span class="yarn-meta">#line:0a7204e </span></span>
<span class="yarn-line">LATARNIA MORSKA znów świeci! <span class="yarn-meta">#line:0d95290 </span></span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spawned-child"></a>
## spawned_child

<div class="yarn-node" data-title="spawned_child"><pre class="yarn-code"><code><span class="yarn-header-dim">// Spawned NPC flavor</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Kiedyś widziałem duży STATEK. <span class="yarn-meta">#line:02579c6 </span></span>
<span class="yarn-line">Fale rozpryskują się szybko. <span class="yarn-meta">#line:04fe9c2 </span></span>

</code></pre></div>

<a id="ys-node-spawned-tourist"></a>
## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Stary ŻURAW podnosi ciężkie ładunki. <span class="yarn-meta">#line:0d2afb8 </span></span>
<span class="yarn-line">Jasne światło prowadzi łodzie. <span class="yarn-meta">#line:093119f </span></span>

</code></pre></div>

<a id="ys-node-spawned-local"></a>
## spawned_local

<div class="yarn-node" data-title="spawned_local"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tutaj rzeka spotyka się z morzem. <span class="yarn-meta">#line:0edcc57 </span></span>
<span class="yarn-line">Dzisiaj w porcie jest duży ruch. <span class="yarn-meta">#line:04f5d5e </span></span>

</code></pre></div>


