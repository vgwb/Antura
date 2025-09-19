---
title: Discover Warszawa (pl_01) - Script
hide:
---

# Discover Warszawa (pl_01) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-01-discover-warszawa/32/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1983275331#gid=1983275331)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_01%20Warsaw/PL_01%20Warsaw%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_01 | Warsaw</span>
<span class="yarn-header-dim">// </span>
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
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-line">Welcome to WARSAW, the capital of POLAND.</span> <span class="yarn-meta">#line:0b126ba </span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! You explored Warsaw.</span> <span class="yarn-meta">#line:0f168cb </span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_plaza&gt;&gt;</span>
<span class="yarn-line">You met the Mermaid, Wars and Sawa.</span> <span class="yarn-meta">#line:07b58db </span>
<span class="yarn-cmd">&lt;&lt;card warsaw_chopin_monument&gt;&gt;</span>
<span class="yarn-line">You heard about Chopin and Maria Skłodowska‑Curie.</span> <span class="yarn-meta">#line:0421ca8 </span>
<span class="yarn-cmd">&lt;&lt;card warsaw_palace_culture&gt;&gt;</span>
<span class="yarn-line">You visited the Parliament and the Palace of Culture.</span> <span class="yarn-meta">#line:06ad318 </span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">You saw the river, the stadium, and a tall column.</span> <span class="yarn-meta">#line:07661f8 </span>
<span class="yarn-line">You learned flag colors and some transport words.</span> <span class="yarn-meta">#line:03f7300 </span>
<span class="yarn-line">Warsaw mixes history, science, music, and sport.</span> <span class="yarn-meta">#line:063523b </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Draw the Polish flag, white on top, red below.</span> <span class="yarn-meta">#line:0a4ef13 </span>
<span class="yarn-line">Point to Warsaw on a map of Poland.</span> <span class="yarn-meta">#line:056b063 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-guide-intro"></a>

## GUIDE_INTRO

<div class="yarn-node" data-title="GUIDE_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_city_gate&gt;&gt;</span>
<span class="yarn-line">Antura ran through the city and made a mess.</span> <span class="yarn-meta">#line:0f4026d </span>
<span class="yarn-line">Can you help us fix things?</span> <span class="yarn-meta">#line:0e40172 </span>
<span class="yarn-line">Start with the Mermaid of Warsaw by the river.</span> <span class="yarn-meta">#line:0303973 </span>

</code>
</pre>
</div>

<a id="ys-node-mermaid-square"></a>

## MERMAID_SQUARE

<div class="yarn-node" data-title="MERMAID_SQUARE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Mermaid</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_plaza&gt;&gt;</span>
<span class="yarn-line">Hello. I am the Mermaid of Warsaw.</span> <span class="yarn-meta">#line:048b274 </span>
<span class="yarn-line">Antura took my sword while I tried to stop him.</span> <span class="yarn-meta">#line:03284c1 </span>
<span class="yarn-line">Please find it and help other places on his trail.</span> <span class="yarn-meta">#line:0b37c7e </span>
<span class="yarn-line">Do you know how people move in Warsaw?</span> <span class="yarn-meta">#line:014ebd5 </span>
<span class="yarn-line">You can ride a tram, take a bus, or a train.</span> <span class="yarn-meta">#line:0f69ca8 </span>
<span class="yarn-line">You can also go by car or bike.</span> <span class="yarn-meta">#line:01b35cc </span>
<span class="yarn-line">I saw Antura near the Chopin Monument.</span> <span class="yarn-meta">#line:016885a </span>
<span class="yarn-line">Follow the music.</span> <span class="yarn-meta">#line:00591a8 </span>

</code>
</pre>
</div>

<a id="ys-node-chopin-monument"></a>

## CHOPIN_MONUMENT

<div class="yarn-node" data-title="CHOPIN_MONUMENT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Chopin</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_chopin_monument&gt;&gt;</span>
<span class="yarn-line">Hello. I am Fryderyk Chopin.</span> <span class="yarn-meta">#line:05e23dd </span>
<span class="yarn-line">My music notes flew away.</span> <span class="yarn-meta">#line:0c2dc21 </span>
<span class="yarn-line">Please recreate the melody.</span> <span class="yarn-meta">#line:0cd9161 </span>
<span class="yarn-cmd">&lt;&lt;activity piano chopin_melody tutorial&gt;&gt;</span>
<span class="yarn-line">Beautiful. Thank you.</span> <span class="yarn-meta">#line:0588964 </span>
<span class="yarn-line">I wrote many pieces for piano.</span> <span class="yarn-meta">#line:02f0cb6 </span>
<span class="yarn-line">I saw Antura run toward Copernicus.</span> <span class="yarn-meta">#line:0d0dfb2 </span>
<span class="yarn-line">Keep going. The Mermaid needs her sword back.</span> <span class="yarn-meta">#line:0f18f1a </span>

</code>
</pre>
</div>

<a id="ys-node-wars-and-sawa"></a>

## WARS_AND_SAWA

<div class="yarn-node" data-title="WARS_AND_SAWA">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Wars</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wars_statue&gt;&gt;</span>
<span class="yarn-line">Have you seen Sawa? We got separated.</span> <span class="yarn-meta">#line:0f91d02 </span>
<span class="yarn-line">Antura caused trouble. Sawa ran toward the river.</span> <span class="yarn-meta">#line:0d21844 </span>
<span class="yarn-line">Please bring her back.</span> <span class="yarn-meta">#line:0fac801 </span>

</code>
</pre>
</div>

<a id="ys-node-sawa-by-wisla"></a>

## SAWA_BY_WISLA

<div class="yarn-node" data-title="SAWA_BY_WISLA">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Sawa</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">Hello. I am by the Wisła River.</span> <span class="yarn-meta">#line:057d461 </span>
<span class="yarn-line">The Wisła is the longest river in Poland.</span> <span class="yarn-meta">#line:0345857 </span>
<span class="yarn-line">Let’s go back to Wars.</span> <span class="yarn-meta">#line:071d7d4 </span>

</code>
</pre>
</div>

<a id="ys-node-wars-sawa-legend"></a>

## WARS_SAWA_LEGEND

<div class="yarn-node" data-title="WARS_SAWA_LEGEND">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Wars</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wars_sawa_back&gt;&gt;</span>
<span class="yarn-line">Thank you. We are together again.</span> <span class="yarn-meta">#line:099f978 </span>
<span class="yarn-line">We are Wars and Sawa. This is a Warsaw legend.</span> <span class="yarn-meta">#line:012d571 </span>
<span class="yarn-line">Find King Sigismund’s column next.</span> <span class="yarn-meta">#line:023e2de </span>

</code>
</pre>
</div>

<a id="ys-node-sigismund-column"></a>

## SIGISMUND_COLUMN

<div class="yarn-node" data-title="SIGISMUND_COLUMN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=King_Sigismund</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sigismund_column&gt;&gt;</span>
<span class="yarn-line">Greetings. I am King Sigismund.</span> <span class="yarn-meta">#line:0735d68 </span>
<span class="yarn-line">My crown fell when Antura ran past!</span> <span class="yarn-meta">#line:06acced </span>
<span class="yarn-line">It should be nearby. Please find it.</span> <span class="yarn-meta">#line:0a6c3f8 </span>
<span class="yarn-comment">// Task hint</span>
<span class="yarn-comment">// task=find_crown, action=HighlightCrown could be handled by scene logic if needed</span>
<span class="yarn-line">Well done. My crown is back.</span> <span class="yarn-meta">#line:0853f81 </span>
<span class="yarn-line">Go to the Parliament. The Mermaid’s sword is there.</span> <span class="yarn-meta">#line:0fe293d </span>

</code>
</pre>
</div>

<a id="ys-node-president-parliament"></a>

## PRESIDENT_PARLIAMENT

<div class="yarn-node" data-title="PRESIDENT_PARLIAMENT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=President</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sejm&gt;&gt;</span>
<span class="yarn-line">Welcome to the Polish Houses of Parliament.</span> <span class="yarn-meta">#line:049ead5 </span>
<span class="yarn-line">I also work at the Presidential Palace.</span> <span class="yarn-meta">#line:0bd4bf9 </span>
<span class="yarn-line">I have the Mermaid’s sword.</span> <span class="yarn-meta">#line:0a0d570 </span>
<span class="yarn-line">But first, help me fix the Polish flag.</span> <span class="yarn-meta">#line:08e2833 </span>
<span class="yarn-line">Choose the correct colors.</span> <span class="yarn-meta">#line:0b6a50a </span>
<span class="yarn-cmd">&lt;&lt;activity quiz polish_flag_colors tutorial&gt;&gt;</span>
<span class="yarn-line">Thank you. The flag is white and red.</span> <span class="yarn-meta">#line:0c1be80 </span>
<span class="yarn-line">We also celebrate the 3 May Constitution Day.</span> <span class="yarn-meta">#line:0691d35 </span>
<span class="yarn-line">Here is the Mermaid’s sword. Please return it.</span> <span class="yarn-meta">#line:09a7cf5 </span>

</code>
</pre>
</div>

<a id="ys-node-mermaid-return"></a>

## MERMAID_RETURN

<div class="yarn-node" data-title="MERMAID_RETURN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Mermaid</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_sword&gt;&gt;</span>
<span class="yarn-line">You found my sword. Thank you.</span> <span class="yarn-meta">#line:0098677 </span>
<span class="yarn-line">I am a symbol of Warsaw.</span> <span class="yarn-meta">#line:02279a9 </span>
<span class="yarn-line">Antura also reached the Palace of Culture and Science.</span> <span class="yarn-meta">#line:03d0b7a </span>
<span class="yarn-line">Please check it.</span> <span class="yarn-meta">#line:0c28f2d </span>

</code>
</pre>
</div>

<a id="ys-node-palace-culture-maria"></a>

## PALACE_CULTURE_MARIA

<div class="yarn-node" data-title="PALACE_CULTURE_MARIA">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Maria_Curie</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_palace_culture&gt;&gt;</span>
<span class="yarn-line">Hello. I am Maria Skłodowska‑Curie.</span> <span class="yarn-meta">#line:0d43ca0 </span>
<span class="yarn-line">This is the Palace of Culture and Science.</span> <span class="yarn-meta">#line:02b3d6b </span>
<span class="yarn-line">Antura misplaced my wallet.</span> <span class="yarn-meta">#line:0666af3 </span>
<span class="yarn-line">Follow the coin trail. Our currency is the złoty.</span> <span class="yarn-meta">#line:00a545d </span>
<span class="yarn-cmd">&lt;&lt;activity order curie_coin_trail tutorial&gt;&gt;</span>
<span class="yarn-line">You found it. Thank you.</span> <span class="yarn-meta">#line:0585b5a </span>
<span class="yarn-line">I heard noise at the National Stadium.</span> <span class="yarn-meta">#line:0762923 </span>

</code>
</pre>
</div>

<a id="ys-node-national-stadium"></a>

## NATIONAL_STADIUM

<div class="yarn-node" data-title="NATIONAL_STADIUM">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Robert_Lewandowski</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_national_stadium&gt;&gt;</span>
<span class="yarn-line">This is the National Stadium.</span> <span class="yarn-meta">#line:0bb0aa5 </span>
<span class="yarn-line">Can you score 5 goals?</span> <span class="yarn-meta">#line:09126e5 </span>
<span class="yarn-cmd">&lt;&lt;activity order score_5_goals tutorial&gt;&gt;</span>
<span class="yarn-line">Great shots!</span> <span class="yarn-meta">#line:0c55643 </span>
<span class="yarn-line">Sport words: football, ball, goal, field.</span> <span class="yarn-meta">#line:06ef367 </span>
<span class="yarn-line">People sing our national anthem here.</span> <span class="yarn-meta">#line:0b65eb8 </span>
<span class="yarn-line">Independence Day is on 11 November.</span> <span class="yarn-meta">#line:09c2e0c </span>

</code>
</pre>
</div>

<a id="ys-node-guide-outro"></a>

## GUIDE_OUTRO

<div class="yarn-node" data-title="GUIDE_OUTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_city_sunset&gt;&gt;</span>
<span class="yarn-line">You helped the city. Antura left Warsaw.</span> <span class="yarn-meta">#line:0afa91b </span>
<span class="yarn-line">The Mermaid, Chopin, and friends say thank you.</span> <span class="yarn-meta">#line:0be619e </span>
<span class="yarn-line">Keep exploring Poland!</span> <span class="yarn-meta">#line:0aeb8fb </span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz"></a>

## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor= Narrator</span>
<span class="yarn-header-dim">type: quiz</span>
<span class="yarn-header-dim">group: Quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_quiz&gt;&gt;</span>
<span class="yarn-line">Final questions.</span> <span class="yarn-meta">#line:01faf50 </span>
<span class="yarn-comment">// Q1: How does the POLISH FLAG look? (image choices handled in activity content)</span>
<span class="yarn-comment">// Q2: Match picture with words (TRANSPORT/SPORT vocabulary)</span>
<span class="yarn-cmd">&lt;&lt;activity quiz warsaw_basics tutorial&gt;&gt;</span>
<span class="yarn-line">Well done!</span> <span class="yarn-meta">#line:08f99a0 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-facts-transport"></a>

## FACTS_TRANSPORT

<div class="yarn-node" data-title="FACTS_TRANSPORT">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// FACT NODES</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card tram&gt;&gt;</span>
<span class="yarn-line">Trams and buses help people move.</span> <span class="yarn-meta">#line:07888ab </span>
<span class="yarn-cmd">&lt;&lt;card bus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card bike&gt;&gt;</span>
<span class="yarn-line">You can also ride a bike.</span> <span class="yarn-meta">#line:0a01a9a </span>

</code>
</pre>
</div>

<a id="ys-node-facts-history"></a>

## FACTS_HISTORY

<div class="yarn-node" data-title="FACTS_HISTORY">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sigismund_column&gt;&gt;</span>
<span class="yarn-line">King Sigismund’s column is a city symbol.</span> <span class="yarn-meta">#line:0507774 </span>
<span class="yarn-cmd">&lt;&lt;card constitution_of_3_may&gt;&gt;</span>
<span class="yarn-line">May 3 is a special day.</span> <span class="yarn-meta">#line:0d4bfcd </span>

</code>
</pre>
</div>

<a id="ys-node-facts-science"></a>

## FACTS_SCIENCE

<div class="yarn-node" data-title="FACTS_SCIENCE">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card maria_skodowskacurie&gt;&gt;</span>
<span class="yarn-line">Maria studied science and won prizes.</span> <span class="yarn-meta">#line:072ab93 </span>
<span class="yarn-cmd">&lt;&lt;card nicolaus_copernicus_monument_warsaw&gt;&gt;</span>
<span class="yarn-line">Copernicus studied the sky.</span> <span class="yarn-meta">#line:057e76e </span>

</code>
</pre>
</div>

<a id="ys-node-facts-symbols"></a>

## FACTS_SYMBOLS

<div class="yarn-node" data-title="FACTS_SYMBOLS">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_plaza&gt;&gt;</span>
<span class="yarn-line">The Mermaid protects the city.</span> <span class="yarn-meta">#line:014ff09 </span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">The Wisła River crosses Warsaw.</span> <span class="yarn-meta">#line:057e3a3 </span>
<span class="yarn-cmd">&lt;&lt;card warsaw_palace_culture&gt;&gt;</span>
<span class="yarn-line">This tall building has museums.</span> <span class="yarn-meta">#line:034f607 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-warsaw-local"></a>

## spawned_warsaw_local

<div class="yarn-node" data-title="spawned_warsaw_local">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: warsaw_locals</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Warsaw has the Wisła River.</span> <span class="yarn-meta">#line:004922a </span>
<span class="yarn-line">The flag is white and red.</span> <span class="yarn-meta">#line:01536e4 </span>
<span class="yarn-line">I like to ride the tram.</span> <span class="yarn-meta">#line:028722e </span>
<span class="yarn-line">The Palace of Culture is very tall.</span> <span class="yarn-meta">#line:06fa7a6 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-warsaw-guide"></a>

## spawned_warsaw_guide

<div class="yarn-node" data-title="spawned_warsaw_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: warsaw_guides</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Chopin wrote piano music.</span> <span class="yarn-meta">#line:0016e94 </span>
<span class="yarn-line">Maria Curie studied science.</span> <span class="yarn-meta">#line:0c899ce </span>
<span class="yarn-line">King Sigismund stands on a column.</span> <span class="yarn-meta">#line:0582b47 </span>
<span class="yarn-line">The Mermaid is on the coat of arms.</span> <span class="yarn-meta">#line:0a5a892 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-warsaw-student"></a>

## spawned_warsaw_student

<div class="yarn-node" data-title="spawned_warsaw_student">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: warsaw_students</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I learn about Copernicus in school.</span> <span class="yarn-meta">#line:08a617b </span>
<span class="yarn-line">We play football at the stadium.</span> <span class="yarn-meta">#line:0eeafaf </span>
<span class="yarn-line">Independence Day is in November.</span> <span class="yarn-meta">#line:0f85b89 </span>
<span class="yarn-line">I can name all tram colors!</span> <span class="yarn-meta">#line:05fa279 </span>

</code>
</pre>
</div>


