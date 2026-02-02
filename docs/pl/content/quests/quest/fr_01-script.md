---
title: Paryż! (fr_01) - Script
hide:
---

# Paryż! (fr_01) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-01-paris/23/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=755037318#gid=755037318)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_01%20Paris/FR_01%20Paris%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_01 | Paris</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $BAGUETTE_STEP = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $met_tutor = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card capital_paris&gt;&gt;</span>
<span class="yarn-line">Jesteśmy w Paryżu, stolicy Francji.</span> <span class="yarn-meta">#line:start </span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Dzisiaj zwiedzimy Wieżę Eiffla</span> <span class="yarn-meta">#line:start_1</span>
<span class="yarn-cmd">&lt;&lt;card notre_dame_de_paris&gt;&gt;</span>
<span class="yarn-line">i Notre-Dame</span> <span class="yarn-meta">#line:start_1a</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">ale najpierw zjedzmy bagietkę!</span> <span class="yarn-meta">#line:start_1b</span>
<span class="yarn-cmd">&lt;&lt;target tutor&gt;&gt;</span>
<span class="yarn-line">Jesteś gotowy? Zaczynajmy!</span> <span class="yarn-meta">#line:start_2</span>

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
<span class="yarn-line">Gratulacje!</span> <span class="yarn-meta">#line:0d11596 </span>
<span class="yarn-cmd">&lt;&lt;card capital_paris&gt;&gt;</span>
<span class="yarn-line">Czy podobał Ci się Paryż?</span> <span class="yarn-meta">#line:0d11596a</span>
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
<span class="yarn-cmd">&lt;&lt;card eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Chcesz narysować Wieżę Eiffla?</span> <span class="yarn-meta">#line:002620f</span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-tutor"></a>

## talk_tutor

<div class="yarn-node" data-title="talk_tutor">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $met_tutor == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $met_tutor = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_paris&gt;&gt;</span>
<span class="yarn-line">    Cześć! Byłeś kiedyś w Paryżu?</span> <span class="yarn-meta">#line:talk_tutor_0</span>
<span class="yarn-line">    Tak!</span> <span class="yarn-meta">#line:talk_tutor_0b</span>
<span class="yarn-line">        Świetnie! Zobaczmy, czy pamiętasz te miejsca.</span> <span class="yarn-meta">#line:talk_tutor_0c</span>
<span class="yarn-line">    NIE.</span> <span class="yarn-meta">#line:talk_tutor_0d</span>
<span class="yarn-line">        Mam nadzieję, że kiedyś tu przyjedziesz!</span> <span class="yarn-meta">#line:talk_tutor_0e</span>
<span class="yarn-line">    Widziałem, jak Antura poszedł do piekarza. Chodźmy tam!</span> <span class="yarn-meta">#line:talk_tutor</span>
    <span class="yarn-cmd">&lt;&lt;area area_bakery&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_man&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-baker"></a>

## talk_baker

<div class="yarn-node" data-title="talk_baker">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if $BAGUETTE_STEP == 1&gt;&gt;</span>
<span class="yarn-line">    Świetnie! Mamy mąkę.</span> <span class="yarn-meta">#line:baker_r1</span>
    <span class="yarn-cmd">&lt;&lt;card food_salt&gt;&gt;</span>
<span class="yarn-line">    Teraz potrzebuję soli.</span> <span class="yarn-meta">#line:06cccc0 </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_notredame&gt;&gt;</span>
<span class="yarn-line">    Idź do Notre-Dame.</span> <span class="yarn-meta">#line:baker_r2 #task:go_notredame</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_notredame&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_notredame&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 2&gt;&gt;</span>
<span class="yarn-line">    Sól! Dobra robota.</span> <span class="yarn-meta">#line:baker_r3</span>
    <span class="yarn-cmd">&lt;&lt;card louvre&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_louvre&gt;&gt;</span>
<span class="yarn-line">    Antura wziął wodę i poszedł do Luwru.</span> <span class="yarn-meta">#line:baker_r4 #task:go_louvre</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_louvre&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_louvre&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 3&gt;&gt;</span>
<span class="yarn-line">    Świetnie! Mamy wodę.</span> <span class="yarn-meta">#line:baker_r5</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_arc&gt;&gt;</span>
<span class="yarn-line">    Być może drożdże są przy Łuku Triumfalnym.</span> <span class="yarn-meta">#line:baker_r6 #task:go_arc</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_arc&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_arc&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump baker_finish&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card person_baker&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem piekarzem. Piekę chleb codziennie.</span> <span class="yarn-meta">#line:baker_0</span>
    <span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">    Dzisiaj chcę zrobić bagietkę...</span> <span class="yarn-meta">#line:baker_1</span>
<span class="yarn-line">    Ale zgubiłem składniki!</span> <span class="yarn-meta">#line:baker_2</span>
    <span class="yarn-cmd">&lt;&lt;card food_flour&gt;&gt;</span>
<span class="yarn-line">    Duży niebieski pies ukradł mi mąkę!</span> <span class="yarn-meta">#line:baker_3</span>
<span class="yarn-line">    Czy możesz mi pomóc je znaleźć?</span> <span class="yarn-meta">#line:baker_4</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_eiffell&gt;&gt;</span>
<span class="yarn-line">    Idź do Wieży Eiffla.</span> <span class="yarn-meta">#line:baker_5 #task:go_eiffell</span>
    <span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 0&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_eiffell_ticket&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_eiffell&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-finish"></a>

## baker_finish

<div class="yarn-node" data-title="baker_finish">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">Znalazłeś wszystkie składniki!</span> <span class="yarn-meta">#line:baker_finish_0</span>
<span class="yarn-line">Teraz potrafię zrobić bagietkę.</span> <span class="yarn-meta">#line:baker_finish_1</span>
<span class="yarn-line">Zobaczmy, czy pamiętasz to, czego nauczyłeś się w Paryżu.</span> <span class="yarn-meta">#line:076b3e3 </span>
<span class="yarn-cmd">&lt;&lt;activity match_paris_final final_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-activity-done"></a>

## final_activity_done

<div class="yarn-node" data-title="final_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetnie! Rozwiązałeś zagadkę.</span> <span class="yarn-meta">#line:puzzle_done</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-flour"></a>

## baguette_flour

<div class="yarn-node" data-title="baguette_flour">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_flour&gt;&gt;</span>
<span class="yarn-line">Znalazłeś mąkę!</span> <span class="yarn-meta">#line:06022b0 </span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 1&gt;&gt;</span>
<span class="yarn-line">Wróć do piekarza.</span> <span class="yarn-meta">#line:go_back_baker #task:go_baker</span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-salt"></a>

## baguette_salt

<div class="yarn-node" data-title="baguette_salt">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_salt&gt;&gt;</span>
<span class="yarn-line">Znalazłeś sól.</span> <span class="yarn-meta">#line:00f1d2f </span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 2&gt;&gt;</span>
<span class="yarn-line">Wróć do piekarza.</span> <span class="yarn-meta">#shadow:go_back_baker #task:go_baker </span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-water"></a>

## baguette_water

<div class="yarn-node" data-title="baguette_water">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_water&gt;&gt;</span>
<span class="yarn-line">To jest woda.</span> <span class="yarn-meta">#line:0c4d1f6</span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 3&gt;&gt;</span>
<span class="yarn-line">Wróć do piekarza.</span> <span class="yarn-meta">#shadow:go_back_baker #task:go_baker</span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-yeast"></a>

## baguette_yeast

<div class="yarn-node" data-title="baguette_yeast">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_yeast&gt;&gt;</span>
<span class="yarn-line">Znalazłeś drożdże!</span> <span class="yarn-meta">#line:025865d</span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 4&gt;&gt;</span>
<span class="yarn-line">Wróć do piekarza.</span> <span class="yarn-meta">#shadow:go_back_baker #task:go_baker</span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-ticket"></a>

## talk_eiffell_ticket

<div class="yarn-node" data-title="talk_eiffell_ticket">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">////////// EIFFEL TOWER: pay 5 coins -&gt; roof -&gt; chest flour</span>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-line">Dzień dobry. Czego chcesz?</span> <span class="yarn-meta">#line:09e454b </span>
<span class="yarn-line">Bilet na wejście na Wieżę Eiffla.</span> <span class="yarn-meta">#line:0141851 </span>
    <span class="yarn-cmd">&lt;&lt;if HasCompletedTask("collect_coins")&gt;&gt;</span>
<span class="yarn-line">        Wybierz kwotę do zapłaty.</span> <span class="yarn-meta">#line:0f44ea7 </span>
        <span class="yarn-cmd">&lt;&lt;activity money_elevator ticket_payment_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "collect_coins"&gt;&gt;</span>
<span class="yarn-line">        Bilet kosztuje 5 monet</span> <span class="yarn-meta">#line:069cbb3</span>
<span class="yarn-line">        Rozejrzyj się i podnieś monety.</span> <span class="yarn-meta">#shadow:0097a65</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        Bilet kosztuje 5 monet</span> <span class="yarn-meta">#shadow:069cbb3</span>
<span class="yarn-line">        Rozejrzyj się i podnieś monety.</span> <span class="yarn-meta">#line:0097a65 #task:collect_coins</span>
        <span class="yarn-cmd">&lt;&lt;task_start collect_coins coins_collected&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Bagietka.</span> <span class="yarn-meta">#line:03dc852 </span>
<span class="yarn-line">   Niedaleko jest piekarnia. Ale otwiera się później.</span> <span class="yarn-meta">#line:0cbdcce </span>
<span class="yarn-line">Po prostu rozejrzeć się.</span> <span class="yarn-meta">#line:0718e4a </span>
<span class="yarn-line">   Życzymy miłej wizyty!</span> <span class="yarn-meta">#line:006fcf2 </span>

</code>
</pre>
</div>

<a id="ys-node-ticket-payment-done"></a>

## ticket_payment_done

<div class="yarn-node" data-title="ticket_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_ticket&gt;&gt;</span>
<span class="yarn-line">Oto twój bilet.</span> <span class="yarn-meta">#line:04e74ad </span>
<span class="yarn-cmd">&lt;&lt;action activate_elevator&gt;&gt;</span>
<span class="yarn-line">Widziałem Anturę wchodzącego na szczyt wieży.</span> <span class="yarn-meta">#line:089abda</span>
<span class="yarn-cmd">&lt;&lt;target npc_eiffell_roof&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_map&gt;&gt;</span>
<span class="yarn-line">Skorzystaj z windy lub idź schodami!</span> <span class="yarn-meta">#line:0585a5e </span>

</code>
</pre>
</div>

<a id="ys-node-coins-collected"></a>

## coins_collected

<div class="yarn-node" data-title="coins_collected">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Masz teraz wystarczającą ilość monet, aby kupić bilet.</span> <span class="yarn-meta">#line:0ba42cd </span>
<span class="yarn-cmd">&lt;&lt;target npc_eiffell_ticket&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-guide"></a>

## talk_eiffell_guide

<div class="yarn-node" data-title="talk_eiffell_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć. Co chcesz wiedzieć?</span> <span class="yarn-meta">#line:0070084 </span>
<span class="yarn-line">Czym jest Wieża Eiffla?</span> <span class="yarn-meta">#line:0d91dc0 </span>
<span class="yarn-line">    Wysoka żelazna wieża, o wysokości około 300 metrów.</span> <span class="yarn-meta">#line:0f17af0 </span>
<span class="yarn-line">    To wyjątkowy symbol Paryża!</span> <span class="yarn-meta">#line:07a113f </span>
<span class="yarn-line">Gdzie jesteśmy?</span> <span class="yarn-meta">#line:09dd1da </span>
<span class="yarn-line">    Jesteśmy w Paryżu.</span> <span class="yarn-meta">#line:02b627d </span>
<span class="yarn-line">Czy to miejsce jest prawdziwe?</span> <span class="yarn-meta">#line:08bede4 </span>
<span class="yarn-line">    Tak! Dlaczego pytasz?</span> <span class="yarn-meta">#line:08654e6 </span>
<span class="yarn-line">    Cóż... wygląda to jak gra wideo.</span> <span class="yarn-meta">#line:0bc62a3 </span>
<span class="yarn-line">Nic. Pa.</span> <span class="yarn-meta">#line:0fe0732 #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-roof"></a>

## talk_eiffell_roof

<div class="yarn-node" data-title="talk_eiffell_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Witamy na szczycie Wieży Eiffla!</span> <span class="yarn-meta">#line:0da46e8 </span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_map&gt;&gt;</span>
<span class="yarn-line">Wieża Eiffla ma 300 metrów wysokości.</span> <span class="yarn-meta">#line:08c1973 </span>
<span class="yarn-cmd">&lt;&lt;card gustave_eiffel&gt;&gt;</span>
<span class="yarn-line">Zbudował go Gustave Eiffel w 1887 roku.</span> <span class="yarn-meta">#line:09e5c3b </span>
<span class="yarn-cmd">&lt;&lt;card iron_material&gt;&gt;</span>
<span class="yarn-line">Jest zrobiony z żelaza!</span> <span class="yarn-meta">#line:0d59ade </span>
<span class="yarn-cmd">&lt;&lt;card worlds_fair_1889&gt;&gt;</span>
<span class="yarn-line">Został zbudowany dawno temu na duże targi.</span> <span class="yarn-meta">#line:0d59ade_fair</span>
&lt;&lt;if GetActivityResult("memory_eiffell") &lt; 1 &gt;&gt;
<span class="yarn-line">    Aby otworzyć skrzynię, rozwiąż zagadkę!</span> <span class="yarn-meta">#line:solve_puzzle</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_eiffell eiffell_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eiffell-activity-done"></a>

## eiffell_activity_done

<div class="yarn-node" data-title="eiffell_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetnie! Rozwiązałeś zagadkę.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">Skrzynia jest teraz odblokowana.</span> <span class="yarn-meta">#line:chest_unlocked</span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_flour true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_flour&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-notre-dame"></a>

## talk_notre_dame

<div class="yarn-node" data-title="talk_notre_dame">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $BAGUETTE_STEP &lt; 1&gt;&gt;
<span class="yarn-line">    Wróć później.</span> <span class="yarn-meta">#line:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card notre_dame_de_paris&gt;&gt;</span>
<span class="yarn-line">    To jest katedra Notre-Dame.</span> <span class="yarn-meta">#line:06f3fa2 </span>
    <span class="yarn-cmd">&lt;&lt;card cathedral&gt;&gt;</span>
<span class="yarn-line">    Katedra oznacza bardzo duży kościół.</span> <span class="yarn-meta">#line:06f3fa2_cathedral</span>
    <span class="yarn-cmd">&lt;&lt;card church&gt;&gt;</span>
<span class="yarn-line">    Kościół to miejsce, do którego ludzie chodzą się modlić.</span> <span class="yarn-meta">#line:fr01_notredame_base_3</span>
<span class="yarn-line">    To bardzo stary kościół. Został zbudowany dawno temu.</span> <span class="yarn-meta">#line:0ac5a72 </span>
<span class="yarn-line">    Wejdź na dach tym portalem!</span> <span class="yarn-meta">#line:083dfcc</span>
    <span class="yarn-cmd">&lt;&lt;action activate_teleporter&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Tę część już rozwiązaliśmy.</span> <span class="yarn-meta">#line:already_solved</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-notre-dame-roof"></a>

## talk_notre_dame_roof

<div class="yarn-node" data-title="talk_notre_dame_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card notre_dame_de_paris_fire&gt;&gt;</span>
<span class="yarn-line">W 2019 roku wybuchł duży pożar, ale go naprawiliśmy.</span> <span class="yarn-meta">#line:09a0ead </span>
<span class="yarn-line">Aby otworzyć skrzynię, rozwiąż zagadkę!</span> <span class="yarn-meta">#shadow:solve_puzzle</span>
<span class="yarn-cmd">&lt;&lt;activity memory_notredame notredame_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-notredame-activity-done"></a>

## notredame_activity_done

<div class="yarn-node" data-title="notredame_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetnie! Rozwiązałeś zagadkę.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">Skrzynia jest teraz odblokowana.</span> <span class="yarn-meta">#shadow:chest_unlocked</span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_salt true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_salt&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-gargoyle"></a>

## gargoyle

<div class="yarn-node" data-title="gargoyle">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gargoyle zoom&gt;&gt;</span>
<span class="yarn-line">Spójrz na tę statuę! Czyż nie jest przerażająca?</span> <span class="yarn-meta">#line:0f7f9d8 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-louvre"></a>

## talk_louvre

<div class="yarn-node" data-title="talk_louvre">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///// LOUVRE: puzzle -&gt; chest unlock -&gt; water</span>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
&lt;&lt;if $BAGUETTE_STEP &lt; 2&gt;&gt;
<span class="yarn-line">    Wróć później.</span> <span class="yarn-meta">#shadow:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card louvre&gt;&gt;</span>
<span class="yarn-line">    To jest Luwr, słynne muzeum.</span> <span class="yarn-meta">#line:louvre_0</span>
<span class="yarn-line">    Aby otworzyć skrzynię, rozwiąż zagadkę!</span> <span class="yarn-meta">#shadow:solve_puzzle</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_louvre louvre_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Tę część już rozwiązaliśmy.</span> <span class="yarn-meta">#shadow:already_solved</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-louvre-activity-done"></a>

## louvre_activity_done

<div class="yarn-node" data-title="louvre_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetnie! Rozwiązałeś zagadkę.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">Skrzynia jest teraz odblokowana.</span> <span class="yarn-meta">#shadow:chest_unlocked</span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_water true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-arc"></a>

## talk_arc

<div class="yarn-node" data-title="talk_arc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///// ARC DE TRIOMPHE: puzzle -&gt; chest unlock -&gt; yeast</span>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
&lt;&lt;if $BAGUETTE_STEP &lt; 3&gt;&gt;
<span class="yarn-line">    Wróć później.</span> <span class="yarn-meta">#shadow:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 3&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card arc_de_triomphe&gt;&gt;</span>
<span class="yarn-line">    To jest Łuk Triumfalny.</span> <span class="yarn-meta">#line:arc_0</span>
<span class="yarn-line">    Upamiętnia ludzi, którzy walczyli za Francję.</span> <span class="yarn-meta">#line:arc_0a</span>
<span class="yarn-line">    Aby otworzyć skrzynię, rozwiąż zagadkę!</span> <span class="yarn-meta">#shadow:solve_puzzle</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_arc arc_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Tę część już rozwiązaliśmy.</span> <span class="yarn-meta">#shadow:already_solved</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-arc-activity-done"></a>

## arc_activity_done

<div class="yarn-node" data-title="arc_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetnie! Rozwiązałeś zagadkę.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">Skrzynia jest teraz odblokowana.</span> <span class="yarn-meta">#shadow:chest_unlocked</span>
<span class="yarn-line">Znajduje się na dachu. Użyj teleportu, aby się tam dostać.</span> <span class="yarn-meta">#line:0d46853 </span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_yeast true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action activate_arc_teleporter&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_yeast&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-eiffell-tourist"></a>

## spawned_eiffell_tourist

<div class="yarn-node" data-title="spawned_eiffell_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: eiffel_tower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Chciałbym wejść na Wieżę Eiffla.</span> <span class="yarn-meta">#line:0aee9bb </span>
<span class="yarn-line">Aby wejść na górę potrzebny jest bilet.</span> <span class="yarn-meta">#line:09be864 </span>
<span class="yarn-line">W 1889 roku w Paryżu odbył się wielki jarmark.</span> <span class="yarn-meta">#line:0a3f4e1 </span>
<span class="yarn-line">    Miało to na celu uczczenie wielkich urodzin Francji.</span> <span class="yarn-meta">#line:01fa210 </span>
<span class="yarn-line">    Wieża Eiffla została zbudowana specjalnie na tę wielką imprezę.</span> <span class="yarn-meta">#line:0d6f3c4 </span>
<span class="yarn-line">Uwielbiam Paryż!</span> <span class="yarn-meta">#line:0bda18a </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-man"></a>

## spawned_man

<div class="yarn-node" data-title="spawned_man">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Masz pytania?</span> <span class="yarn-meta">#line:07b94e9 </span>
<span class="yarn-line">Widziałeś Anturę?</span> <span class="yarn-meta">#line:0f18ad3 </span>
<span class="yarn-line">    Nie. Kim jest Antura?</span> <span class="yarn-meta">#line:0f9dd62 </span>
<span class="yarn-line">Co robisz?</span> <span class="yarn-meta">#line:002796f </span>
<span class="yarn-line">    Zamierzam kupić chleb w piekarni.</span> <span class="yarn-meta">#line:05a38a8 </span>
<span class="yarn-line">Skąd pochodzisz?</span> <span class="yarn-meta">#line:05eabcf </span>
<span class="yarn-line">    Nie urodziłem się w tym kraju.</span> <span class="yarn-meta">#line:0635a6a </span>
<span class="yarn-line">Do widzenia</span> <span class="yarn-meta">#line:0ee51fc #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-f"></a>

## spawned_kid_f

<div class="yarn-node" data-title="spawned_kid_f">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: kids</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:041403d </span>
<span class="yarn-line">Ca va?</span> <span class="yarn-meta">#line:04986a3 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-notredame-roof"></a>

## npc_notredame_roof

<div class="yarn-node" data-title="npc_notredame_roof">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: notredame_roof</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Z dachu można zobaczyć większą część Paryża.</span> <span class="yarn-meta">#line:fr01_notredame_roof_1</span>
<span class="yarn-line">Siedzą tu kamienne stworzenia zwane gargulcami.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2</span>
<span class="yarn-line">Te kamienne ramiona pomagają podtrzymywać mury.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2b #card:flying_buttress</span>
<span class="yarn-line">Duże, okrągłe okno nazywane jest rozetą.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2c #card:rose_window</span>

</code>
</pre>
</div>

<a id="ys-node-npc-eiffell-elevator"></a>

## npc_eiffell_elevator

<div class="yarn-node" data-title="npc_eiffell_elevator">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: eiffel_tower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_elevators&gt;&gt;</span>
<span class="yarn-line">Winda pomaga ludziom dostać się na szczyt wieży.</span> <span class="yarn-meta">#line:fr01_eiffel_elevator_1</span>

</code>
</pre>
</div>

<a id="ys-node-npc-paris-region"></a>

## npc_paris_region

<div class="yarn-node" data-title="npc_paris_region">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ile_de_france&gt;&gt;</span>
<span class="yarn-line">Paryż leży w regionie Île-de-France.</span> <span class="yarn-meta">#line:fr01_region_1</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bakery"></a>

## npc_bakery

<div class="yarn-node" data-title="npc_bakery">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: bakery</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Zapach świeżego chleba uszczęśliwia ludzi.</span> <span class="yarn-meta">#line:fr01_bakery_1</span>
<span class="yarn-line">Do przygotowania bagietki używamy mąki, wody, drożdży i soli.</span> <span class="yarn-meta">#line:fr01_bakery_2</span>
<span class="yarn-line">Drożdże pomagają wyrosnąć chlebowi.</span> <span class="yarn-meta">#line:fr01_bakery_2a</span>
<span class="yarn-line">Piekarze wstają bardzo wcześnie rano, aby zacząć wypiekać chleb.</span> <span class="yarn-meta">#line:fr01_bakery_3</span>

</code>
</pre>
</div>


