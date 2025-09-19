---
title: Scopri Varsavia (pl_01) - Script
hide:
---

# Scopri Varsavia (pl_01) - Script
[Quest Index](./index.it.md) - Language: [english](./pl_01-script.md) - [french](./pl_01-script.fr.md) - [polish](./pl_01-script.pl.md) - italian

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-01-discover-warszawa/32/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1983275331#gid=1983275331)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_01%20Warsaw/PL_01%20Warsaw%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// pl_01 | Warsaw</span>
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
<span class="yarn-line">Benvenuti a VARSAVIA, la capitale della POLONIA. <span class="yarn-meta">#line:0b126ba </span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Fantastico! Hai esplorato Varsavia. <span class="yarn-meta">#line:0f168cb </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_plaza&gt;&gt;</span>
<span class="yarn-line">Hai incontrato la Sirena, Wars e Sawa. <span class="yarn-meta">#line:07b58db </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_chopin_monument&gt;&gt;</span>
<span class="yarn-line">Hai sentito parlare di Chopin e Maria Skłodowska‑Curie. <span class="yarn-meta">#line:0421ca8 </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_palace_culture&gt;&gt;</span>
<span class="yarn-line">Hai visitato il Parlamento e il Palazzo della Cultura. <span class="yarn-meta">#line:06ad318 </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">Hai visto il fiume, lo stadio e un'alta colonna. <span class="yarn-meta">#line:07661f8 </span></span>
<span class="yarn-line">Hai imparato i colori delle bandiere e alcune parole relative ai trasporti. <span class="yarn-meta">#line:03f7300 </span></span>
<span class="yarn-line">Varsavia unisce storia, scienza, musica e sport. <span class="yarn-meta">#line:063523b </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Disegna la bandiera polacca, bianca in alto e rossa in basso. <span class="yarn-meta">#line:0a4ef13 </span></span>
<span class="yarn-line">Indica Varsavia su una mappa della Polonia. <span class="yarn-meta">#line:056b063 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-guide-intro"></a>
## GUIDE_INTRO

<div class="yarn-node" data-title="GUIDE_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_city_gate&gt;&gt;</span>
<span class="yarn-line">Antura corse attraverso la città e combinò un gran pasticcio. <span class="yarn-meta">#line:0f4026d </span></span>
<span class="yarn-line">Puoi aiutarci a sistemare le cose? <span class="yarn-meta">#line:0e40172 </span></span>
<span class="yarn-line">Si inizia con la Sirena di Varsavia sul fiume. <span class="yarn-meta">#line:0303973 </span></span>

</code></pre></div>

<a id="ys-node-mermaid-square"></a>
## MERMAID_SQUARE

<div class="yarn-node" data-title="MERMAID_SQUARE"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Mermaid</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_plaza&gt;&gt;</span>
<span class="yarn-line">Ciao. Sono la Sirena di Varsavia. <span class="yarn-meta">#line:048b274 </span></span>
<span class="yarn-line">Antura mi prese la spada mentre cercavo di fermarlo. <span class="yarn-meta">#line:03284c1 </span></span>
<span class="yarn-line">Per favore, trovatelo e aiutate gli altri luoghi sulle sue tracce. <span class="yarn-meta">#line:0b37c7e </span></span>
<span class="yarn-line">Sai come si muovono le persone a Varsavia? <span class="yarn-meta">#line:014ebd5 </span></span>
<span class="yarn-line">Puoi prendere un tram, un autobus o un treno. <span class="yarn-meta">#line:0f69ca8 </span></span>
<span class="yarn-line">È possibile spostarsi anche in auto o in bicicletta. <span class="yarn-meta">#line:01b35cc </span></span>
<span class="yarn-line">Ho visto Antura vicino al monumento a Chopin. <span class="yarn-meta">#line:016885a </span></span>
<span class="yarn-line">Segui la musica. <span class="yarn-meta">#line:00591a8 </span></span>

</code></pre></div>

<a id="ys-node-chopin-monument"></a>
## CHOPIN_MONUMENT

<div class="yarn-node" data-title="CHOPIN_MONUMENT"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Chopin</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_chopin_monument&gt;&gt;</span>
<span class="yarn-line">Ciao. Sono Fryderyk Chopin. <span class="yarn-meta">#line:05e23dd </span></span>
<span class="yarn-line">Le mie note musicali volarono via. <span class="yarn-meta">#line:0c2dc21 </span></span>
<span class="yarn-line">Per favore, ricrea la melodia. <span class="yarn-meta">#line:0cd9161 </span></span>
<span class="yarn-cmd">&lt;&lt;activity piano chopin_melody tutorial&gt;&gt;</span>
<span class="yarn-line">Bellissimo. Grazie. <span class="yarn-meta">#line:0588964 </span></span>
<span class="yarn-line">Ho scritto molti pezzi per pianoforte. <span class="yarn-meta">#line:02f0cb6 </span></span>
<span class="yarn-line">Ho visto Antura correre verso Copernico. <span class="yarn-meta">#line:0d0dfb2 </span></span>
<span class="yarn-line">Continua. La Sirena ha bisogno della sua spada. <span class="yarn-meta">#line:0f18f1a </span></span>

</code></pre></div>

<a id="ys-node-wars-and-sawa"></a>
## WARS_AND_SAWA

<div class="yarn-node" data-title="WARS_AND_SAWA"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Wars</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wars_statue&gt;&gt;</span>
<span class="yarn-line">Hai visto Sawa? Ci siamo separati. <span class="yarn-meta">#line:0f91d02 </span></span>
<span class="yarn-line">Antura creò problemi. Sawa corse verso il fiume. <span class="yarn-meta">#line:0d21844 </span></span>
<span class="yarn-line">Per favore, riportatela indietro. <span class="yarn-meta">#line:0fac801 </span></span>

</code></pre></div>

<a id="ys-node-sawa-by-wisla"></a>
## SAWA_BY_WISLA

<div class="yarn-node" data-title="SAWA_BY_WISLA"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Sawa</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">Ciao. Mi trovo vicino al fiume Vistola. <span class="yarn-meta">#line:057d461 </span></span>
<span class="yarn-line">La Vistola è il fiume più lungo della Polonia. <span class="yarn-meta">#line:0345857 </span></span>
<span class="yarn-line">Torniamo a Wars. <span class="yarn-meta">#line:071d7d4 </span></span>

</code></pre></div>

<a id="ys-node-wars-sawa-legend"></a>
## WARS_SAWA_LEGEND

<div class="yarn-node" data-title="WARS_SAWA_LEGEND"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Wars</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wars_sawa_back&gt;&gt;</span>
<span class="yarn-line">Grazie. Siamo di nuovo insieme. <span class="yarn-meta">#line:099f978 </span></span>
<span class="yarn-line">Siamo Wars e Sawa. Questa è una leggenda di Varsavia. <span class="yarn-meta">#line:012d571 </span></span>
<span class="yarn-line">Di seguito troverete la colonna di Re Sigismondo. <span class="yarn-meta">#line:023e2de </span></span>

</code></pre></div>

<a id="ys-node-sigismund-column"></a>
## SIGISMUND_COLUMN

<div class="yarn-node" data-title="SIGISMUND_COLUMN"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=King_Sigismund</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sigismund_column&gt;&gt;</span>
<span class="yarn-line">Saluti. Sono Re Sigismondo. <span class="yarn-meta">#line:0735d68 </span></span>
<span class="yarn-line">La mia corona è caduta quando Antura mi è passata di corsa! <span class="yarn-meta">#line:06acced </span></span>
<span class="yarn-line">Dovrebbe essere qui vicino. Per favore, trovalo. <span class="yarn-meta">#line:0a6c3f8 </span></span>
<span class="yarn-comment">// Task hint</span>
<span class="yarn-comment">// task=find_crown, action=HighlightCrown could be handled by scene logic if needed</span>
<span class="yarn-line">Ben fatto. La mia corona è tornata. <span class="yarn-meta">#line:0853f81 </span></span>
<span class="yarn-line">Vai al Parlamento. La spada della Sirena è lì. <span class="yarn-meta">#line:0fe293d </span></span>

</code></pre></div>

<a id="ys-node-president-parliament"></a>
## PRESIDENT_PARLIAMENT

<div class="yarn-node" data-title="PRESIDENT_PARLIAMENT"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=President</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sejm&gt;&gt;</span>
<span class="yarn-line">Benvenuti al Parlamento polacco. <span class="yarn-meta">#line:049ead5 </span></span>
<span class="yarn-line">Lavoro anche al Palazzo Presidenziale. <span class="yarn-meta">#line:0bd4bf9 </span></span>
<span class="yarn-line">Ho la spada della Sirena. <span class="yarn-meta">#line:0a0d570 </span></span>
<span class="yarn-line">Ma prima aiutami a sistemare la bandiera polacca. <span class="yarn-meta">#line:08e2833 </span></span>
<span class="yarn-line">Scegli i colori corretti. <span class="yarn-meta">#line:0b6a50a </span></span>
<span class="yarn-cmd">&lt;&lt;activity quiz polish_flag_colors tutorial&gt;&gt;</span>
<span class="yarn-line">Grazie. La bandiera è bianca e rossa. <span class="yarn-meta">#line:0c1be80 </span></span>
<span class="yarn-line">Celebriamo anche il 3 maggio, Giorno della Costituzione. <span class="yarn-meta">#line:0691d35 </span></span>
<span class="yarn-line">Ecco la spada della Sirena. Per favore, restituiscila. <span class="yarn-meta">#line:09a7cf5 </span></span>

</code></pre></div>

<a id="ys-node-mermaid-return"></a>
## MERMAID_RETURN

<div class="yarn-node" data-title="MERMAID_RETURN"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Mermaid</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_sword&gt;&gt;</span>
<span class="yarn-line">Hai trovato la mia spada. Grazie. <span class="yarn-meta">#line:0098677 </span></span>
<span class="yarn-line">Sono un simbolo di Varsavia. <span class="yarn-meta">#line:02279a9 </span></span>
<span class="yarn-line">Antura raggiunse anche il Palazzo della Cultura e della Scienza. <span class="yarn-meta">#line:03d0b7a </span></span>
<span class="yarn-line">Per favore, controllalo. <span class="yarn-meta">#line:0c28f2d </span></span>

</code></pre></div>

<a id="ys-node-palace-culture-maria"></a>
## PALACE_CULTURE_MARIA

<div class="yarn-node" data-title="PALACE_CULTURE_MARIA"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Maria_Curie</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_palace_culture&gt;&gt;</span>
<span class="yarn-line">Ciao. Sono Maria Skłodowska‑Curie. <span class="yarn-meta">#line:0d43ca0 </span></span>
<span class="yarn-line">Questo è il Palazzo della Cultura e della Scienza. <span class="yarn-meta">#line:02b3d6b </span></span>
<span class="yarn-line">Antura ha smarrito il mio portafoglio. <span class="yarn-meta">#line:0666af3 </span></span>
<span class="yarn-line">Segui il percorso delle monete. La nostra valuta è lo złoty. <span class="yarn-meta">#line:00a545d </span></span>
<span class="yarn-cmd">&lt;&lt;activity order curie_coin_trail tutorial&gt;&gt;</span>
<span class="yarn-line">L'hai trovato. Grazie. <span class="yarn-meta">#line:0585b5a </span></span>
<span class="yarn-line">Ho sentito del rumore allo Stadio Nazionale. <span class="yarn-meta">#line:0762923 </span></span>

</code></pre></div>

<a id="ys-node-national-stadium"></a>
## NATIONAL_STADIUM

<div class="yarn-node" data-title="NATIONAL_STADIUM"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Robert_Lewandowski</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_national_stadium&gt;&gt;</span>
<span class="yarn-line">Questo è lo Stadio Nazionale. <span class="yarn-meta">#line:0bb0aa5 </span></span>
<span class="yarn-line">Riesci a segnare 5 gol? <span class="yarn-meta">#line:09126e5 </span></span>
<span class="yarn-cmd">&lt;&lt;activity order score_5_goals tutorial&gt;&gt;</span>
<span class="yarn-line">Ottimi scatti! <span class="yarn-meta">#line:0c55643 </span></span>
<span class="yarn-line">Parole sportive: calcio, palla, porta, campo. <span class="yarn-meta">#line:06ef367 </span></span>
<span class="yarn-line">Qui la gente canta il nostro inno nazionale. <span class="yarn-meta">#line:0b65eb8 </span></span>
<span class="yarn-line">Il giorno dell'Indipendenza è l'11 novembre. <span class="yarn-meta">#line:09c2e0c </span></span>

</code></pre></div>

<a id="ys-node-guide-outro"></a>
## GUIDE_OUTRO

<div class="yarn-node" data-title="GUIDE_OUTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_city_sunset&gt;&gt;</span>
<span class="yarn-line">Hai aiutato la città. Antura ha lasciato Varsavia. <span class="yarn-meta">#line:0afa91b </span></span>
<span class="yarn-line">La Sirena, Chopin e gli amici ringraziano. <span class="yarn-meta">#line:0be619e </span></span>
<span class="yarn-line">Continua ad esplorare la Polonia! <span class="yarn-meta">#line:0aeb8fb </span></span>

</code></pre></div>

<a id="ys-node-final-quiz"></a>
## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor= Narrator</span>
<span class="yarn-header-dim">type: quiz</span>
<span class="yarn-header-dim">group: Quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_quiz&gt;&gt;</span>
<span class="yarn-line">Domande finali. <span class="yarn-meta">#line:01faf50 </span></span>
<span class="yarn-comment">// Q1: How does the POLISH FLAG look? (image choices handled in activity content)</span>
<span class="yarn-comment">// Q2: Match picture with words (TRANSPORT/SPORT vocabulary)</span>
<span class="yarn-cmd">&lt;&lt;activity quiz warsaw_basics tutorial&gt;&gt;</span>
<span class="yarn-line">Ben fatto! <span class="yarn-meta">#line:08f99a0 </span></span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-facts-transport"></a>
## FACTS_TRANSPORT

<div class="yarn-node" data-title="FACTS_TRANSPORT"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">// FACT NODES</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card tram&gt;&gt;</span>
<span class="yarn-line">Tram e autobus aiutano le persone a spostarsi. <span class="yarn-meta">#line:07888ab </span></span>
<span class="yarn-cmd">&lt;&lt;card bus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card bike&gt;&gt;</span>
<span class="yarn-line">Si può anche andare in bicicletta. <span class="yarn-meta">#line:0a01a9a </span></span>

</code></pre></div>

<a id="ys-node-facts-history"></a>
## FACTS_HISTORY

<div class="yarn-node" data-title="FACTS_HISTORY"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sigismund_column&gt;&gt;</span>
<span class="yarn-line">La colonna di re Sigismondo è un simbolo della città. <span class="yarn-meta">#line:0507774 </span></span>
<span class="yarn-cmd">&lt;&lt;card constitution_of_3_may&gt;&gt;</span>
<span class="yarn-line">Il 3 maggio è un giorno speciale. <span class="yarn-meta">#line:0d4bfcd </span></span>

</code></pre></div>

<a id="ys-node-facts-science"></a>
## FACTS_SCIENCE

<div class="yarn-node" data-title="FACTS_SCIENCE"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card maria_skodowskacurie&gt;&gt;</span>
<span class="yarn-line">Maria ha studiato scienze e ha vinto dei premi. <span class="yarn-meta">#line:072ab93 </span></span>
<span class="yarn-cmd">&lt;&lt;card nicolaus_copernicus_monument_warsaw&gt;&gt;</span>
<span class="yarn-line">Copernico studiò il cielo. <span class="yarn-meta">#line:057e76e </span></span>

</code></pre></div>

<a id="ys-node-facts-symbols"></a>
## FACTS_SYMBOLS

<div class="yarn-node" data-title="FACTS_SYMBOLS"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_plaza&gt;&gt;</span>
<span class="yarn-line">La Sirena protegge la città. <span class="yarn-meta">#line:014ff09 </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">Il fiume Vistola attraversa Varsavia. <span class="yarn-meta">#line:057e3a3 </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_palace_culture&gt;&gt;</span>
<span class="yarn-line">Questo alto edificio ospita dei musei. <span class="yarn-meta">#line:034f607 </span></span>

</code></pre></div>

<a id="ys-node-spawned-warsaw-local"></a>
## spawned_warsaw_local

<div class="yarn-node" data-title="spawned_warsaw_local"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: warsaw_locals</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Varsavia ha il fiume Vistola. <span class="yarn-meta">#line:004922a </span></span>
<span class="yarn-line">La bandiera è bianca e rossa. <span class="yarn-meta">#line:01536e4 </span></span>
<span class="yarn-line">Mi piace prendere il tram. <span class="yarn-meta">#line:028722e </span></span>
<span class="yarn-line">Il Palazzo della Cultura è molto alto. <span class="yarn-meta">#line:06fa7a6 </span></span>

</code></pre></div>

<a id="ys-node-spawned-warsaw-guide"></a>
## spawned_warsaw_guide

<div class="yarn-node" data-title="spawned_warsaw_guide"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: warsaw_guides</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Chopin scrisse musica per pianoforte. <span class="yarn-meta">#line:0016e94 </span></span>
<span class="yarn-line">Maria Curie studiò scienze. <span class="yarn-meta">#line:0c899ce </span></span>
<span class="yarn-line">Re Sigismondo è in piedi su una colonna. <span class="yarn-meta">#line:0582b47 </span></span>
<span class="yarn-line">La sirena è raffigurata sullo stemma. <span class="yarn-meta">#line:0a5a892 </span></span>

</code></pre></div>

<a id="ys-node-spawned-warsaw-student"></a>
## spawned_warsaw_student

<div class="yarn-node" data-title="spawned_warsaw_student"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: warsaw_students</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ho imparato a conoscere Copernico a scuola. <span class="yarn-meta">#line:08a617b </span></span>
<span class="yarn-line">Giochiamo a calcio allo stadio. <span class="yarn-meta">#line:0eeafaf </span></span>
<span class="yarn-line">Il giorno dell'Indipendenza cade a novembre. <span class="yarn-meta">#line:0f85b89 </span></span>
<span class="yarn-line">So nominare tutti i colori dei tram! <span class="yarn-meta">#line:05fa279 </span></span>

</code></pre></div>


