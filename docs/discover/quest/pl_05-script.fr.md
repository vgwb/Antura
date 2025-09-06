---
title: Le collier d'ambre de Gdańsk (pl_05) - Script
hide:
---

# Le collier d'ambre de Gdańsk (pl_05) - Script
[Quest Index](./index.fr.md) - Language: [english](./pl_05-script.md) - french - [polish](./pl_05-script.pl.md) - [italian](./pl_05-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-05-the-amber-necklace-of-gdansk/36/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=224592228#gid=224592228)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_05%20Baltic%20Sea/PL_05%20Baltic%20Sea%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// PL_05 - BALTIC_SEA - The Amber Necklace of Gdańsk</span>
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
<span class="yarn-header-dim">// Words used: Baltic Sea, coast, amber, lighthouse, Gdansk, fishing, maritime, beach, waves, seagull, ship, port, navigation, ecosystem, Poland</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">image: gdansk_lighthouse_off</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Welcome to GDAŃSK in POLAND. <span class="yarn-meta">#line:086baaf </span></span>
<span class="yarn-line">The LIGHTHOUSE is off. <span class="yarn-meta">#line:060d616 </span></span>
<span class="yarn-line">Let’s fix it! <span class="yarn-meta">#line:05414f7 </span></span>


</code></pre></div>

<a id="ys-node-mission-note"></a>
## MISSION_NOTE

<div class="yarn-node" data-title="MISSION_NOTE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: mission_card</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">We need a gift for NEPTUNE. <span class="yarn-meta">#line:0c699c3 </span></span>
<span class="yarn-line">Make a NECKLACE. <span class="yarn-meta">#line:05a458a </span></span>
<span class="yarn-line">5 AMBER + 5 SHELLS. <span class="yarn-meta">#line:01ce7c7 </span></span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PART 1 – BEACHCOMBING SHELLS</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-beach-intro"></a>
## BEACH_INTRO

<div class="yarn-node" data-title="BEACH_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Beach</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: baltic_beach</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the BALTIC SEA. <span class="yarn-meta">#line:050f059 </span></span>
<span class="yarn-line">Let’s find SHELLS in the SAND. <span class="yarn-meta">#line:07d772f </span></span>


</code></pre></div>

<a id="ys-node-beach-shells-hint"></a>
## BEACH_SHELLS_HINT

<div class="yarn-node" data-title="BEACH_SHELLS_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Beach</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: beach_sand_piles</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Drag to clean the SAND. <span class="yarn-meta">#line:06e5065 </span></span>
<span class="yarn-line">Find 5 SHELL piles. <span class="yarn-meta">#line:03e5d4f </span></span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas beach_shells tutorial&gt;&gt;</span>

<span class="yarn-line">-&gt; Meet the FISHERMAN <span class="yarn-meta">#line:0b3a2d8 </span></span>
<span class="yarn-cmd">&lt;&lt;jump FISHERMAN_MEET&gt;&gt;</span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PART 2 – MEET THE FISHERMAN &amp; AMBER</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-fisherman-meet"></a>
## FISHERMAN_MEET

<div class="yarn-node" data-title="FISHERMAN_MEET"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">tags: actor=Fisherman</span>
<span class="yarn-header-dim">image: fisherman_cutter_net</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Witaj! I am a FISHERMAN. <span class="yarn-meta">#line:00e94a2 </span></span>
<span class="yarn-line">This is my CUTTER and NET. <span class="yarn-meta">#line:0270483 </span></span>
<span class="yarn-line">Let’s find AMBER too. <span class="yarn-meta">#line:0b8e688 </span></span>


</code></pre></div>

<a id="ys-node-amber-hint"></a>
## AMBER_HINT

<div class="yarn-node" data-title="AMBER_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">tags: actor=Fisherman</span>
<span class="yarn-header-dim">image: amber_piles</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">AMBER is BALTIC GOLD. <span class="yarn-meta">#line:0901a09 </span></span>
<span class="yarn-line">Clean the SAND to spot 5. <span class="yarn-meta">#line:03d5567 </span></span>
<span class="yarn-cmd">&lt;&lt;activity cleancanvas beach_amber tutorial&gt;&gt;</span>

<span class="yarn-line">-&gt; Lighthouse facts <span class="yarn-meta">#line:015ddce </span></span>
    <span class="yarn-cmd">&lt;&lt;jump LIGHTHOUSE_FACT&gt;&gt;</span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// LIGHTHOUSE FACT CARD</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-lighthouse-fact"></a>
## LIGHTHOUSE_FACT

<div class="yarn-node" data-title="LIGHTHOUSE_FACT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Fisherman</span>
<span class="yarn-header-dim">tags: actor=Fisherman</span>
<span class="yarn-header-dim">image: gdansk_lighthouse</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A LIGHTHOUSE helps ships. <span class="yarn-meta">#line:049551f </span></span>
<span class="yarn-line">It shines a bright light. <span class="yarn-meta">#line:050c5f3 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle gdansk_lighthouse tutorial&gt;&gt;</span>

<span class="yarn-line">-&gt; Craft the NECKLACE <span class="yarn-meta">#line:0a800e5 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump CRAFTING_INTRO&gt;&gt;</span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PART 3 – CRAFTING THE NECKLACE</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-crafting-intro"></a>
## CRAFTING_INTRO

<div class="yarn-node" data-title="CRAFTING_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Crafting</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: crafting_table</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">We have 10 pieces. <span class="yarn-meta">#line:02129aa </span></span>
<span class="yarn-line">AMBER and SHELL. <span class="yarn-meta">#line:03e4fc6 </span></span>
<span class="yarn-line">Make a pattern. <span class="yarn-meta">#line:0ba66f7 </span></span>


</code></pre></div>

<a id="ys-node-crafting-order"></a>
## CRAFTING_ORDER

<div class="yarn-node" data-title="CRAFTING_ORDER"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Crafting</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: necklace_pattern</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Drag in order. <span class="yarn-meta">#line:064215d </span></span>
<span class="yarn-line">AMBER–SHELL–AMBER–SHELL… <span class="yarn-meta">#line:09746e2 </span></span>
<span class="yarn-cmd">&lt;&lt;activity order necklace_sequence tutorial&gt;&gt;</span>

<span class="yarn-line">-&gt; To NEPTUNE’S FOUNTAIN <span class="yarn-meta">#line:0e2f20d </span></span>
    <span class="yarn-cmd">&lt;&lt;jump NEPTUNE_FOUNTAIN&gt;&gt;</span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PART 4 – NEPTUNE &amp; LIGHTHOUSE ON</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-neptune-fountain"></a>
## NEPTUNE_FOUNTAIN

<div class="yarn-node" data-title="NEPTUNE_FOUNTAIN"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: neptune_fountain</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Place the NECKLACE on NEPTUNE. <span class="yarn-meta">#line:01b559a </span></span>


</code></pre></div>

<a id="ys-node-neptune-speak"></a>
## NEPTUNE_SPEAK

<div class="yarn-node" data-title="NEPTUNE_SPEAK"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">tags: actor=Neptune</span>
<span class="yarn-header-dim">image: neptune_glow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A fine NECKLACE! <span class="yarn-meta">#line:0ca234d </span></span>
<span class="yarn-line">You respect the SEA. <span class="yarn-meta">#line:0d0c226 </span></span>
<span class="yarn-line">I will help you. <span class="yarn-meta">#line:0a0dddf </span></span>


</code></pre></div>

<a id="ys-node-amber-room-note"></a>
## AMBER_ROOM_NOTE

<div class="yarn-node" data-title="AMBER_ROOM_NOTE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">tags: actor=Neptune</span>
<span class="yarn-header-dim">image: amber_room_card</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">AMBER is famous here. <span class="yarn-meta">#line:05ce5f4 </span></span>
<span class="yarn-line">Like the Amber Room story. <span class="yarn-meta">#line:075c0a2 </span></span>


</code></pre></div>

<a id="ys-node-lighthouse-on"></a>
## LIGHTHOUSE_ON

<div class="yarn-node" data-title="LIGHTHOUSE_ON"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Neptune</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: lighthouse_beam_on</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">FLASH! The LIGHTHOUSE shines. <span class="yarn-meta">#line:0941ab9 </span></span>
<span class="yarn-line">Great job! <span class="yarn-meta">#line:0a44f23 </span></span>

<span class="yarn-line">-&gt; Watch for SEALS <span class="yarn-meta">#line:009476a </span></span>
    <span class="yarn-cmd">&lt;&lt;jump SEALS_WARNING&gt;&gt;</span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PART 5 – SEALS &amp; ANTURA</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-seals-warning"></a>
## SEALS_WARNING

<div class="yarn-node" data-title="SEALS_WARNING"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Rescue</span>
<span class="yarn-header-dim">tags: actor=Fisherman</span>
<span class="yarn-header-dim">image: seals_on_beach</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">These are wild SEALS. <span class="yarn-meta">#line:0c0dc82 </span></span>
<span class="yarn-line">Keep a safe distance. <span class="yarn-meta">#line:06b0e4e </span></span>


</code></pre></div>

<a id="ys-node-antura-scene"></a>
## ANTURA_SCENE

<div class="yarn-node" data-title="ANTURA_SCENE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Rescue</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">image: antura_run</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Yea! The quest is done! <span class="yarn-meta">#line:0283435 </span></span>
<span class="yarn-line">-&gt; Review words <span class="yarn-meta">#line:0322b5e </span></span>
    <span class="yarn-cmd">&lt;&lt;jump RECAP_MEMORY&gt;&gt;</span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// RECAP – MEMORY CARDS</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-recap-memory"></a>
## RECAP_MEMORY

<div class="yarn-node" data-title="RECAP_MEMORY"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Recap</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: sea_vocab_cards</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Match the words. <span class="yarn-meta">#line:0372493 </span></span>
<span class="yarn-cmd">&lt;&lt;activity memory sea_vocab tutorial&gt;&gt;</span>

<span class="yarn-line">-&gt; Final Quiz <span class="yarn-meta">#line:05251b3 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ&gt;&gt;</span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// FINAL QUIZ</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-final-quiz"></a>
## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: baltic_quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Answer two questions. <span class="yarn-meta">#line:00a0665 </span></span>
<span class="yarn-line">Tap the best choice. <span class="yarn-meta">#line:07c72eb </span></span>
<span class="yarn-cmd">&lt;&lt;activity quiz baltic_basics tutorial&gt;&gt;</span>
<span class="yarn-comment">// Suggested questions in preset:</span>
<span class="yarn-comment">// 1) What sea is near GDAŃSK? a) BLACK  b) BALTIC ✅</span>
<span class="yarn-comment">// 2) What is BALTIC GOLD?     a) AMBER ✅ b) SAND</span>
<span class="yarn-comment">// 3) What does a LIGHTHOUSE do? a) Helps ships ✅ b) Catches fish</span>

<span class="yarn-line">-&gt; Quest Complete <span class="yarn-meta">#line:07016a3 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump QUEST_COMPLETE&gt;&gt;</span>


<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// END</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-quest-complete"></a>
## QUEST_COMPLETE

<div class="yarn-node" data-title="QUEST_COMPLETE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: quest_complete</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You helped GDAŃSK. <span class="yarn-meta">#line:0a7204e </span></span>
<span class="yarn-line">The LIGHTHOUSE shines again! <span class="yarn-meta">#line:0d95290 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>

</code></pre></div>


