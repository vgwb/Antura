---
title: Scopri Varsavia (pl_01) - Script
hide:
---

# Scopri Varsavia (pl_01) - Script
[Quest Index](./index.it.md) - Language: [english](./pl_01-script.md) - [french](./pl_01-script.fr.md) - [polish](./pl_01-script.pl.md) - italian

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-01-discover-warszawa/32/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1983275331#gid=1983275331)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_01%20Warsaw/PL_01%20Warsaw%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// Quest: pl_01 | Warsaw</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// ---------------------------------------------</span>
<span class="yarn-header-dim">// WANTED:</span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - MermaidOfWarsaw (cultural symbol)</span>
<span class="yarn-header-dim">// - warsaw_chopin_monument (musical heritage)</span>
<span class="yarn-header-dim">// - FryderykChopin (historical figure)</span>
<span class="yarn-header-dim">// - warsaw_wars_statue (legendary figure)</span>
<span class="yarn-header-dim">// - WarsAndSawa (city legend)</span>
<span class="yarn-header-dim">// - warsaw_wisla_river (geographical feature)</span>
<span class="yarn-header-dim">// - RiverWisla (major river)</span>
<span class="yarn-header-dim">// - KingSigismund (historical figure)</span>
<span class="yarn-header-dim">// - warsaw_sejm (government building)</span>
<span class="yarn-header-dim">// - President (political figure)</span>
<span class="yarn-header-dim">// - warsaw_palace_culture (cultural landmark)</span>
<span class="yarn-header-dim">// - MariaCurie (scientific figure)</span>
<span class="yarn-header-dim">// - MoneyZloty (currency education)</span>
<span class="yarn-header-dim">// - warsaw_national_stadium (sports venue)</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - Return Mermaid's sword after parliament visit</span>
<span class="yarn-header-dim">// - Help Wars find Sawa by the river</span>
<span class="yarn-header-dim">// - Find King Sigismund's crown</span>
<span class="yarn-header-dim">// - Follow Maria Curie's coin trail</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - piano chopin_melody (musical activity)</span>
<span class="yarn-header-dim">// - quiz polish_flag_colors (civic education)</span>
<span class="yarn-header-dim">// - quiz warsaw_basics (comprehensive review)</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Words used: TRANSPORT (TRAIN, TRAM, BUS, CAR, BIKE), SPORT (FOOTBALL, BALL, GOAL, FIELD), FLAG, ZŁOTY, MERMAID OF WARSAW, CHOPIN, WARS &amp; SAWA, KING SIGISMUND, PRESIDENT/SEJM, PALACE OF CULTURE AND SCIENCE, MARIA SKŁODOWSKA-CURIE, NATIONAL STADIUM, COPERNICUS</span>
<span class="yarn-header-dim">tags: type=Start</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome to Warsaw!] <span class="yarn-meta">#line:07c7f43 </span></span>
[MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-the-end"></a>
## the_end

<div class="yarn-node" data-title="the_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: This quest is complete.]
<span class="yarn-cmd">&lt;&lt;jump quest_proposal&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-quest-proposal"></a>
## quest_proposal

<div class="yarn-node" data-title="quest_proposal"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: Why don't you draw...]
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-guide-intro"></a>
## GUIDE_INTRO

<div class="yarn-node" data-title="GUIDE_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">// GUIDE — WELCOME</span>
<span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">tags: actor=Guide, balloon=panel</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_city_gate&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome to WARSAW, the capital of POLAND.] <span class="yarn-meta">#line:0b126ba </span></span>
<span class="yarn-line">[MISSING TRANSLATION: ANTURA ran through the city and made a mess.] <span class="yarn-meta">#line:0f4026d </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Can you help us fix things?] <span class="yarn-meta">#line:0e40172 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Start with the MERMAID OF WARSAW by the RIVER.] <span class="yarn-meta">#line:0303973 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-mermaid-square"></a>
## MERMAID_SQUARE

<div class="yarn-node" data-title="MERMAID_SQUARE"><pre class="yarn-code"><code><span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">// MERMAID OF WARSAW — MISSING SWORD + TRANSPORT VOCAB</span>
<span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">tags: actor=Mermaid</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_plaza&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Hello. I am the MERMAID of WARSAW.] <span class="yarn-meta">#line:048b274 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: ANTURA took my SWORD while I tried to stop him.] <span class="yarn-meta">#line:03284c1 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Please find it, and help other places on his trail.] <span class="yarn-meta">#line:0b37c7e </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Do you know how people move in WARSAW?] <span class="yarn-meta">#line:014ebd5 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: You can ride a TRAM, take a BUS, or a TRAIN.] <span class="yarn-meta">#line:0f69ca8 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: You can also go by CAR or BIKE.] <span class="yarn-meta">#line:01b35cc </span></span>
<span class="yarn-line">[MISSING TRANSLATION: I saw ANTURA near the CHOPIN Monument.] <span class="yarn-meta">#line:016885a </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Follow the music.] <span class="yarn-meta">#line:00591a8 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-chopin-monument"></a>
## CHOPIN_MONUMENT

<div class="yarn-node" data-title="CHOPIN_MONUMENT"><pre class="yarn-code"><code><span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">// CHOPIN MONUMENT — PIANO ACTIVITY</span>
<span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">tags: actor=Chopin</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_chopin_monument&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Hello. I am FRYDERYK CHOPIN.] <span class="yarn-meta">#line:05e23dd </span></span>
<span class="yarn-line">[MISSING TRANSLATION: My MUSIC NOTES flew away.] <span class="yarn-meta">#line:0c2dc21 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Please recreate the MELODY.] <span class="yarn-meta">#line:0cd9161 </span></span>
<span class="yarn-cmd">&lt;&lt;activity piano chopin_melody tutorial&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Beautiful. Thank you.] <span class="yarn-meta">#line:0588964 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: I wrote many pieces for PIANO.] <span class="yarn-meta">#line:02f0cb6 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: I saw ANTURA run toward COPERNICUS.] <span class="yarn-meta">#line:0d0dfb2 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Keep going. The MERMAID needs her SWORD back.] <span class="yarn-meta">#line:0f18f1a </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-wars-and-sawa"></a>
## WARS_AND_SAWA

<div class="yarn-node" data-title="WARS_AND_SAWA"><pre class="yarn-code"><code><span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">// WARS AND SAWA — FIND SAWA AT THE WISŁA</span>
<span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">tags: actor=Wars</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wars_statue&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Have you seen SAWA? We got separated.] <span class="yarn-meta">#line:0f91d02 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: ANTURA caused trouble. SAWA ran toward the RIVER.] <span class="yarn-meta">#line:0d21844 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Please bring her back.] <span class="yarn-meta">#line:0fac801 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-sawa-by-wisla"></a>
## SAWA_BY_WISLA

<div class="yarn-node" data-title="SAWA_BY_WISLA"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Sawa</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Hello. I am by the WISŁA RIVER.] <span class="yarn-meta">#line:057d461 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: The WISŁA is the longest river in POLAND.] <span class="yarn-meta">#line:0345857 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Let’s go back to WARS.] <span class="yarn-meta">#line:071d7d4 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-wars-sawa-legend"></a>
## WARS_SAWA_LEGEND

<div class="yarn-node" data-title="WARS_SAWA_LEGEND"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Wars</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wars_sawa_back&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Thank you. We are together again.] <span class="yarn-meta">#line:099f978 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: We are WARS and SAWA. This is a WARSAW legend.] <span class="yarn-meta">#line:012d571 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Find KING SIGISMUND’s COLUMN next.] <span class="yarn-meta">#line:023e2de </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-sigismund-column"></a>
## SIGISMUND_COLUMN

<div class="yarn-node" data-title="SIGISMUND_COLUMN"><pre class="yarn-code"><code><span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">// KING SIGISMUND — FIND THE CROWN (SMALL TASK)</span>
<span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">tags: actor=King_Sigismund</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sigismund_column&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Greetings. I am KING SIGISMUND.] <span class="yarn-meta">#line:0735d68 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: My CROWN fell when ANTURA ran past!] <span class="yarn-meta">#line:06acced </span></span>
<span class="yarn-line">[MISSING TRANSLATION: It should be nearby. Please find it.] <span class="yarn-meta">#line:0a6c3f8 </span></span>
<span class="yarn-comment">// Task hint</span>
<span class="yarn-comment">// task=find_crown, action=HighlightCrown could be handled by scene logic if needed</span>
<span class="yarn-line">[MISSING TRANSLATION: Well done. My CROWN is back.] <span class="yarn-meta">#line:0853f81 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Go to the PARLIAMENT. The MERMAID’s SWORD is there.] <span class="yarn-meta">#line:0fe293d </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-president-parliament"></a>
## PRESIDENT_PARLIAMENT

<div class="yarn-node" data-title="PRESIDENT_PARLIAMENT"><pre class="yarn-code"><code><span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">// PRESIDENT &amp; PARLIAMENT — FLAG COLORS QUIZ (+ PALACE INFO)</span>
<span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">tags: actor=President</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sejm&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome to the POLISH HOUSES OF PARLIAMENT.] <span class="yarn-meta">#line:049ead5 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: I also work at the PRESIDENTIAL PALACE.] <span class="yarn-meta">#line:0bd4bf9 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: I have the MERMAID’s SWORD.] <span class="yarn-meta">#line:0a0d570 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: But first, help me fix the POLISH FLAG.] <span class="yarn-meta">#line:08e2833 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Choose the correct COLORS.] <span class="yarn-meta">#line:0b6a50a </span></span>
<span class="yarn-cmd">&lt;&lt;activity quiz polish_flag_colors tutorial&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Thank you. The FLAG is WHITE and RED.] <span class="yarn-meta">#line:0c1be80 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: We also celebrate the 3 MAY CONSTITUTION DAY.] <span class="yarn-meta">#line:0691d35 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Here is the MERMAID’s SWORD. Please return it.] <span class="yarn-meta">#line:09a7cf5 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-mermaid-return"></a>
## MERMAID_RETURN

<div class="yarn-node" data-title="MERMAID_RETURN"><pre class="yarn-code"><code><span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">// MERMAID — RETURN THE SWORD</span>
<span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">tags: actor=Mermaid</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_sword&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: You found my SWORD. Thank you.] <span class="yarn-meta">#line:0098677 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: I am a symbol of WARSAW.] <span class="yarn-meta">#line:02279a9 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: ANTURA also reached the PALACE OF CULTURE AND SCIENCE.] <span class="yarn-meta">#line:03d0b7a </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Please check it.] <span class="yarn-meta">#line:0c28f2d </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-palace-culture-maria"></a>
## PALACE_CULTURE_MARIA

<div class="yarn-node" data-title="PALACE_CULTURE_MARIA"><pre class="yarn-code"><code><span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">// PALACE OF CULTURE AND SCIENCE — MEET MARIA, FOLLOW COIN TRAIL</span>
<span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">tags: actor=Maria_Curie</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_palace_culture&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Hello. I am MARIA SKŁODOWSKA‑CURIE.] <span class="yarn-meta">#line:0d43ca0 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: This is the PALACE OF CULTURE AND SCIENCE.] <span class="yarn-meta">#line:02b3d6b </span></span>
<span class="yarn-line">[MISSING TRANSLATION: ANTURA misplaced my WALLET.] <span class="yarn-meta">#line:0666af3 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Follow the COIN TRAIL. Our currency is the ZŁOTY.] <span class="yarn-meta">#line:00a545d </span></span>
<span class="yarn-cmd">&lt;&lt;activity order curie_coin_trail tutorial&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: You found it. Thank you.] <span class="yarn-meta">#line:0585b5a </span></span>
<span class="yarn-line">[MISSING TRANSLATION: I heard noise at the NATIONAL STADIUM.] <span class="yarn-meta">#line:0762923 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-national-stadium"></a>
## NATIONAL_STADIUM

<div class="yarn-node" data-title="NATIONAL_STADIUM"><pre class="yarn-code"><code><span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">// NATIONAL STADIUM </span>
<span class="yarn-header-dim">// ————————————————————————————————————————————————————————————————————</span>
<span class="yarn-header-dim">tags: actor=Robert_Lewandowski</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_national_stadium&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This is the NATIONAL STADIUM.] <span class="yarn-meta">#line:0bb0aa5 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Can you score 5 GOALS?] <span class="yarn-meta">#line:09126e5 </span></span>
<span class="yarn-cmd">&lt;&lt;activity order score_5_goals tutorial&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Great shots!] <span class="yarn-meta">#line:0c55643 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: SPORT words: FOOTBALL, BALL, GOAL, FIELD.] <span class="yarn-meta">#line:06ef367 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: People sing our NATIONAL ANTHEM here.] <span class="yarn-meta">#line:0b65eb8 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Independence Day is on 11 NOVEMBER.] <span class="yarn-meta">#line:09c2e0c </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-guide-outro"></a>
## GUIDE_OUTRO

<div class="yarn-node" data-title="GUIDE_OUTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_city_sunset&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: You helped the city. ANTURA left WARSAW.] <span class="yarn-meta">#line:0afa91b </span></span>
<span class="yarn-line">[MISSING TRANSLATION: The MERMAID, CHOPIN, and friends say thank you.] <span class="yarn-meta">#line:0be619e </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Keep exploring POLAND!] <span class="yarn-meta">#line:0aeb8fb </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-final-quiz"></a>
## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor= Narrator</span>
<span class="yarn-header-dim">type: quiz</span>
<span class="yarn-header-dim">group: Quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_quiz&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Final questions.] <span class="yarn-meta">#line:01faf50 </span></span>
<span class="yarn-comment">// Q1: How does the POLISH FLAG look? (image choices handled in activity content)</span>
<span class="yarn-comment">// Q2: Match picture with words (TRANSPORT/SPORT vocabulary)</span>
<span class="yarn-cmd">&lt;&lt;activity quiz warsaw_basics tutorial&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Well done!] <span class="yarn-meta">#line:08f99a0 </span></span>
<span class="yarn-cmd">&lt;&lt;jump the_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>


