---
title: Odkryj Warszawę (pl_01) - Script
hide:
---

# Odkryj Warszawę (pl_01) - Script
[Quest Index](./index.pl.md) - Language: [english](./pl_01-script.md) - [french](./pl_01-script.fr.md) - polish - [italian](./pl_01-script.it.md)

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
<span class="yarn-line">Witamy w WARSZAWIE, stolicy POLSKI. <span class="yarn-meta">#line:0b126ba </span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetnie! Zwiedziłeś Warszawę. <span class="yarn-meta">#line:0f168cb </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_plaza&gt;&gt;</span>
<span class="yarn-line">Poznałeś Syrenę, Warsa i Sawę. <span class="yarn-meta">#line:07b58db </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_chopin_monument&gt;&gt;</span>
<span class="yarn-line">Słyszeliście o Chopinie i Marii Skłodowskiej‑Curie. <span class="yarn-meta">#line:0421ca8 </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_palace_culture&gt;&gt;</span>
<span class="yarn-line">Odwiedził Pan Parlament i Pałac Kultury. <span class="yarn-meta">#line:06ad318 </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">Widziałeś rzekę, stadion i wysoką kolumnę. <span class="yarn-meta">#line:07661f8 </span></span>
<span class="yarn-line">Nauczyłeś się kolorów flag i kilku słów związanych z transportem. <span class="yarn-meta">#line:03f7300 </span></span>
<span class="yarn-line">Warszawa łączy w sobie historię, naukę, muzykę i sport. <span class="yarn-meta">#line:063523b </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Narysuj polską flagę, białą na górze, czerwoną na dole. <span class="yarn-meta">#line:0a4ef13 </span></span>
<span class="yarn-line">Wskaż Warszawę na mapie Polski. <span class="yarn-meta">#line:056b063 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-guide-intro"></a>
## GUIDE_INTRO

<div class="yarn-node" data-title="GUIDE_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_city_gate&gt;&gt;</span>
<span class="yarn-line">Antura przebiegł przez miasto i narobił zamieszania. <span class="yarn-meta">#line:0f4026d </span></span>
<span class="yarn-line">Czy możesz nam pomóc to naprawić? <span class="yarn-meta">#line:0e40172 </span></span>
<span class="yarn-line">Zacznij od Syrenki Warszawskiej nad rzeką. <span class="yarn-meta">#line:0303973 </span></span>

</code></pre></div>

<a id="ys-node-mermaid-square"></a>
## MERMAID_SQUARE

<div class="yarn-node" data-title="MERMAID_SQUARE"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Mermaid</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_plaza&gt;&gt;</span>
<span class="yarn-line">Cześć. Jestem Syrenką Warszawską. <span class="yarn-meta">#line:048b274 </span></span>
<span class="yarn-line">Antura zabrał mi miecz, gdy próbowałem go powstrzymać. <span class="yarn-meta">#line:03284c1 </span></span>
<span class="yarn-line">Proszę go odnaleźć i pomóc innym osobom na jego tropie. <span class="yarn-meta">#line:0b37c7e </span></span>
<span class="yarn-line">Czy wiesz, jak poruszają się ludzie w Warszawie? <span class="yarn-meta">#line:014ebd5 </span></span>
<span class="yarn-line">Możesz pojechać tramwajem, autobusem lub pociągiem. <span class="yarn-meta">#line:0f69ca8 </span></span>
<span class="yarn-line">Można również pojechać samochodem lub rowerem. <span class="yarn-meta">#line:01b35cc </span></span>
<span class="yarn-line">Widziałem Anturę w pobliżu Pomnika Chopina. <span class="yarn-meta">#line:016885a </span></span>
<span class="yarn-line">Podążaj za muzyką. <span class="yarn-meta">#line:00591a8 </span></span>

</code></pre></div>

<a id="ys-node-chopin-monument"></a>
## CHOPIN_MONUMENT

<div class="yarn-node" data-title="CHOPIN_MONUMENT"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Chopin</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_chopin_monument&gt;&gt;</span>
<span class="yarn-line">Cześć. Jestem Fryderyk Chopin. <span class="yarn-meta">#line:05e23dd </span></span>
<span class="yarn-line">Moje nuty odleciały. <span class="yarn-meta">#line:0c2dc21 </span></span>
<span class="yarn-line">Proszę odtworzyć melodię. <span class="yarn-meta">#line:0cd9161 </span></span>
<span class="yarn-cmd">&lt;&lt;activity piano chopin_melody tutorial&gt;&gt;</span>
<span class="yarn-line">Pięknie. Dziękuję. <span class="yarn-meta">#line:0588964 </span></span>
<span class="yarn-line">Napisałem wiele utworów na fortepian. <span class="yarn-meta">#line:02f0cb6 </span></span>
<span class="yarn-line">Widziałem Anturę biegnącą w stronę Kopernika. <span class="yarn-meta">#line:0d0dfb2 </span></span>
<span class="yarn-line">Kontynuuj. Syrena potrzebuje swojego miecza. <span class="yarn-meta">#line:0f18f1a </span></span>

</code></pre></div>

<a id="ys-node-wars-and-sawa"></a>
## WARS_AND_SAWA

<div class="yarn-node" data-title="WARS_AND_SAWA"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Wars</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wars_statue&gt;&gt;</span>
<span class="yarn-line">Widziałeś Sawę? Rozdzieliliśmy się. <span class="yarn-meta">#line:0f91d02 </span></span>
<span class="yarn-line">Antura sprawiła kłopoty. Sawa pobiegła w stronę rzeki. <span class="yarn-meta">#line:0d21844 </span></span>
<span class="yarn-line">Proszę, sprowadź ją z powrotem. <span class="yarn-meta">#line:0fac801 </span></span>

</code></pre></div>

<a id="ys-node-sawa-by-wisla"></a>
## SAWA_BY_WISLA

<div class="yarn-node" data-title="SAWA_BY_WISLA"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Sawa</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">Cześć. Jestem nad Wisłą. <span class="yarn-meta">#line:057d461 </span></span>
<span class="yarn-line">Wisła jest najdłuższą rzeką w Polsce. <span class="yarn-meta">#line:0345857 </span></span>
<span class="yarn-line">Wróćmy do wojen. <span class="yarn-meta">#line:071d7d4 </span></span>

</code></pre></div>

<a id="ys-node-wars-sawa-legend"></a>
## WARS_SAWA_LEGEND

<div class="yarn-node" data-title="WARS_SAWA_LEGEND"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Wars</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wars_sawa_back&gt;&gt;</span>
<span class="yarn-line">Dziękuję. Znów jesteśmy razem. <span class="yarn-meta">#line:099f978 </span></span>
<span class="yarn-line">Jesteśmy Wars i Sawa. To warszawska legenda. <span class="yarn-meta">#line:012d571 </span></span>
<span class="yarn-line">Następnie przeczytaj kolumnę króla Zygmunta. <span class="yarn-meta">#line:023e2de </span></span>

</code></pre></div>

<a id="ys-node-sigismund-column"></a>
## SIGISMUND_COLUMN

<div class="yarn-node" data-title="SIGISMUND_COLUMN"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=King_Sigismund</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sigismund_column&gt;&gt;</span>
<span class="yarn-line">Witam. Jestem król Zygmunt. <span class="yarn-meta">#line:0735d68 </span></span>
<span class="yarn-line">Moja korona spadła, gdy obok przebiegł Antura! <span class="yarn-meta">#line:06acced </span></span>
<span class="yarn-line">Powinno być gdzieś w pobliżu. Proszę, znajdź je. <span class="yarn-meta">#line:0a6c3f8 </span></span>
<span class="yarn-comment">// Task hint</span>
<span class="yarn-comment">// task=find_crown, action=HighlightCrown could be handled by scene logic if needed</span>
<span class="yarn-line">Dobra robota. Moja korona wróciła. <span class="yarn-meta">#line:0853f81 </span></span>
<span class="yarn-line">Idź do Parlamentu. Miecz Syreny jest tam. <span class="yarn-meta">#line:0fe293d </span></span>

</code></pre></div>

<a id="ys-node-president-parliament"></a>
## PRESIDENT_PARLIAMENT

<div class="yarn-node" data-title="PRESIDENT_PARLIAMENT"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=President</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sejm&gt;&gt;</span>
<span class="yarn-line">Witamy w Sejmie Rzeczypospolitej Polskiej. <span class="yarn-meta">#line:049ead5 </span></span>
<span class="yarn-line">Pracuję również w Pałacu Prezydenckim. <span class="yarn-meta">#line:0bd4bf9 </span></span>
<span class="yarn-line">Mam miecz Syreny. <span class="yarn-meta">#line:0a0d570 </span></span>
<span class="yarn-line">Ale najpierw pomóżcie mi naprawić polską flagę. <span class="yarn-meta">#line:08e2833 </span></span>
<span class="yarn-line">Wybierz odpowiednie kolory. <span class="yarn-meta">#line:0b6a50a </span></span>
<span class="yarn-cmd">&lt;&lt;activity quiz polish_flag_colors tutorial&gt;&gt;</span>
<span class="yarn-line">Dziękuję. Flaga jest biało-czerwona. <span class="yarn-meta">#line:0c1be80 </span></span>
<span class="yarn-line">Obchodzimy również Święto Konstytucji 3 Maja. <span class="yarn-meta">#line:0691d35 </span></span>
<span class="yarn-line">Oto miecz Syreny. Proszę go zwrócić. <span class="yarn-meta">#line:09a7cf5 </span></span>

</code></pre></div>

<a id="ys-node-mermaid-return"></a>
## MERMAID_RETURN

<div class="yarn-node" data-title="MERMAID_RETURN"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Mermaid</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_sword&gt;&gt;</span>
<span class="yarn-line">Znalazłeś mój miecz. Dziękuję. <span class="yarn-meta">#line:0098677 </span></span>
<span class="yarn-line">Jestem symbolem Warszawy. <span class="yarn-meta">#line:02279a9 </span></span>
<span class="yarn-line">Antura dotarła także do Pałacu Kultury i Nauki. <span class="yarn-meta">#line:03d0b7a </span></span>
<span class="yarn-line">Proszę to sprawdzić. <span class="yarn-meta">#line:0c28f2d </span></span>

</code></pre></div>

<a id="ys-node-palace-culture-maria"></a>
## PALACE_CULTURE_MARIA

<div class="yarn-node" data-title="PALACE_CULTURE_MARIA"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Maria_Curie</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_palace_culture&gt;&gt;</span>
<span class="yarn-line">Cześć. Jestem Maria Skłodowska-Curie. <span class="yarn-meta">#line:0d43ca0 </span></span>
<span class="yarn-line">To jest Pałac Kultury i Nauki. <span class="yarn-meta">#line:02b3d6b </span></span>
<span class="yarn-line">Antura zgubiła mój portfel. <span class="yarn-meta">#line:0666af3 </span></span>
<span class="yarn-line">Podążaj śladem monet. Naszą walutą jest złoty. <span class="yarn-meta">#line:00a545d </span></span>
<span class="yarn-cmd">&lt;&lt;activity order curie_coin_trail tutorial&gt;&gt;</span>
<span class="yarn-line">Znalazłeś to. Dziękuję. <span class="yarn-meta">#line:0585b5a </span></span>
<span class="yarn-line">Słyszałem hałas na Stadionie Narodowym. <span class="yarn-meta">#line:0762923 </span></span>

</code></pre></div>

<a id="ys-node-national-stadium"></a>
## NATIONAL_STADIUM

<div class="yarn-node" data-title="NATIONAL_STADIUM"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Robert_Lewandowski</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_national_stadium&gt;&gt;</span>
<span class="yarn-line">To jest Stadion Narodowy. <span class="yarn-meta">#line:0bb0aa5 </span></span>
<span class="yarn-line">Czy potrafisz strzelić 5 goli? <span class="yarn-meta">#line:09126e5 </span></span>
<span class="yarn-cmd">&lt;&lt;activity order score_5_goals tutorial&gt;&gt;</span>
<span class="yarn-line">Świetne zdjęcia! <span class="yarn-meta">#line:0c55643 </span></span>
<span class="yarn-line">Słowa związane ze sportem: piłka nożna, piłka, bramka, boisko. <span class="yarn-meta">#line:06ef367 </span></span>
<span class="yarn-line">Ludzie śpiewają tutaj nasz hymn narodowy. <span class="yarn-meta">#line:0b65eb8 </span></span>
<span class="yarn-line">Dzień Niepodległości przypada 11 listopada. <span class="yarn-meta">#line:09c2e0c </span></span>

</code></pre></div>

<a id="ys-node-guide-outro"></a>
## GUIDE_OUTRO

<div class="yarn-node" data-title="GUIDE_OUTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Guide</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_city_sunset&gt;&gt;</span>
<span class="yarn-line">Pomogłeś miastu. Antura opuściła Warszawę. <span class="yarn-meta">#line:0afa91b </span></span>
<span class="yarn-line">Syrenka, Chopin i przyjaciele składają podziękowania. <span class="yarn-meta">#line:0be619e </span></span>
<span class="yarn-line">Kontynuuj odkrywanie Polski! <span class="yarn-meta">#line:0aeb8fb </span></span>

</code></pre></div>

<a id="ys-node-final-quiz"></a>
## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor= Narrator</span>
<span class="yarn-header-dim">type: quiz</span>
<span class="yarn-header-dim">group: Quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_quiz&gt;&gt;</span>
<span class="yarn-line">Pytania końcowe. <span class="yarn-meta">#line:01faf50 </span></span>
<span class="yarn-comment">// Q1: How does the POLISH FLAG look? (image choices handled in activity content)</span>
<span class="yarn-comment">// Q2: Match picture with words (TRANSPORT/SPORT vocabulary)</span>
<span class="yarn-cmd">&lt;&lt;activity quiz warsaw_basics tutorial&gt;&gt;</span>
<span class="yarn-line">Dobrze zrobiony! <span class="yarn-meta">#line:08f99a0 </span></span>
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
<span class="yarn-line">Tramwaje i autobusy pomagają ludziom się przemieszczać. <span class="yarn-meta">#line:07888ab </span></span>
<span class="yarn-cmd">&lt;&lt;card bus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card bike&gt;&gt;</span>
<span class="yarn-line">Można również jeździć na rowerze. <span class="yarn-meta">#line:0a01a9a </span></span>

</code></pre></div>

<a id="ys-node-facts-history"></a>
## FACTS_HISTORY

<div class="yarn-node" data-title="FACTS_HISTORY"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_sigismund_column&gt;&gt;</span>
<span class="yarn-line">Kolumna króla Zygmunta jest symbolem miasta. <span class="yarn-meta">#line:0507774 </span></span>
<span class="yarn-cmd">&lt;&lt;card constitution_of_3_may&gt;&gt;</span>
<span class="yarn-line">3 maja to dzień szczególny. <span class="yarn-meta">#line:0d4bfcd </span></span>

</code></pre></div>

<a id="ys-node-facts-science"></a>
## FACTS_SCIENCE

<div class="yarn-node" data-title="FACTS_SCIENCE"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card maria_skodowskacurie&gt;&gt;</span>
<span class="yarn-line">Maria studiowała nauki ścisłe i zdobywała nagrody. <span class="yarn-meta">#line:072ab93 </span></span>
<span class="yarn-cmd">&lt;&lt;card nicolaus_copernicus_monument_warsaw&gt;&gt;</span>
<span class="yarn-line">Kopernik badał niebo. <span class="yarn-meta">#line:057e76e </span></span>

</code></pre></div>

<a id="ys-node-facts-symbols"></a>
## FACTS_SYMBOLS

<div class="yarn-node" data-title="FACTS_SYMBOLS"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: Guide</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_mermaid_plaza&gt;&gt;</span>
<span class="yarn-line">Syrena chroni miasto. <span class="yarn-meta">#line:014ff09 </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">Rzeka Wisła przepływa przez Warszawę. <span class="yarn-meta">#line:057e3a3 </span></span>
<span class="yarn-cmd">&lt;&lt;card warsaw_palace_culture&gt;&gt;</span>
<span class="yarn-line">W tym wysokim budynku znajdują się muzea. <span class="yarn-meta">#line:034f607 </span></span>

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
<span class="yarn-line">Warszawa ma rzekę Wisłę. <span class="yarn-meta">#line:004922a </span></span>
<span class="yarn-line">Flaga jest biało-czerwona. <span class="yarn-meta">#line:01536e4 </span></span>
<span class="yarn-line">Lubię jeździć tramwajem. <span class="yarn-meta">#line:028722e </span></span>
<span class="yarn-line">Pałac Kultury jest bardzo wysoki. <span class="yarn-meta">#line:06fa7a6 </span></span>

</code></pre></div>

<a id="ys-node-spawned-warsaw-guide"></a>
## spawned_warsaw_guide

<div class="yarn-node" data-title="spawned_warsaw_guide"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: warsaw_guides</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Chopin pisał muzykę fortepianową. <span class="yarn-meta">#line:0016e94 </span></span>
<span class="yarn-line">Maria Curie studiowała nauki ścisłe. <span class="yarn-meta">#line:0c899ce </span></span>
<span class="yarn-line">Król Zygmunt stoi na kolumnie. <span class="yarn-meta">#line:0582b47 </span></span>
<span class="yarn-line">Syrena znajduje się w herbie. <span class="yarn-meta">#line:0a5a892 </span></span>

</code></pre></div>

<a id="ys-node-spawned-warsaw-student"></a>
## spawned_warsaw_student

<div class="yarn-node" data-title="spawned_warsaw_student"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: warsaw_students</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">O Koperniku dowiedziałem się w szkole. <span class="yarn-meta">#line:08a617b </span></span>
<span class="yarn-line">Gramy w piłkę nożną na stadionie. <span class="yarn-meta">#line:0eeafaf </span></span>
<span class="yarn-line">Dzień Niepodległości przypada w listopadzie. <span class="yarn-meta">#line:0f85b89 </span></span>
<span class="yarn-line">Potrafię nazwać wszystkie kolory tramwajów! <span class="yarn-meta">#line:05fa279 </span></span>

</code></pre></div>


